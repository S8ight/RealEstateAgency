using System.Security.Claims;
using ApiGateWay.Models;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using REA.AuthorizationSystem.BLL.DTO.Request;
using REA.AuthorizationSystem.BLL.DTO.Response;
using REA.AuthorizationSystem.BLL.Helpers;
using REA.AuthorizationSystem.BLL.Interfaces;
using REA.AuthorizationSystem.BLL.Validation;
using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly IBusControl _bus;
    private readonly ILogger<UserController> _logger;

    public UserController(IBusControl bus,
        UserManager<User> userManager,
        IEmailService emailService,
        ITokenService tokenService,
        IMapper mapper, ILogger<UserController> logger)
    {
         _bus = bus;
        _userManager = userManager;
        _emailService = emailService;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        try
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists != null)
            {
                return BadRequest("User with such email already exist.");
            }

            var userValidation = new UserValidation();
            var validationResult = await userValidation.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString());
            }

            var userCreationResult = await _userManager.CreateAsync(
                new User()
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Patronymic = request.Patronymic,
                    DateOfBirth = request.DateOfBirth,
                    Created = DateTime.Now
                }, request.Password);

            if (!userCreationResult.Succeeded)
            {
                return BadRequest(userCreationResult.Errors);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action(
                nameof(ConfirmEmail),
                nameof(User),
                new { emailToken, email = user.Email },
                Request.Scheme
            );

            await _emailService.SendEmailAsync(
                request.Email,
                "RealEstateAgency Email Confirmation",
                $"Click the link below to confirm email: \n{confirmationLink}"
            );

            var queueUser = _mapper.Map<UserRegistrationQueueModel>(user);
            await _bus.Publish(queueUser);
            
            _logger.LogInformation("User registered successfully: {Email}", request.Email);
            return Ok(
                "Email confirmation link has been sent. Please check your email and complete the registration process.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "User registration failed: {Email}", request.Email);
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string emailToken, string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("User does not exist");
            }

            var result = await _userManager.ConfirmEmailAsync(user, emailToken);
            if (!result.Succeeded)
            {
                return BadRequest("Unable to confirm email");
            }
            
            _logger.LogInformation("User ({Email}) verified email address", email);
            return Ok("Email confirmed");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Email confirmation failed: {Email}", email);
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("LoginWithCredentials")]
    public async Task<ActionResult<UserLoginResponse>> LoginWithCredentials([FromBody] AuthenticationRequest request)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return BadRequest("User does not exist");
            }
        
            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return BadRequest("Wrong Credentials!");
            }

            var userResponse = _mapper.Map<UserLoginResponse>(user);
            userResponse.AccessToken = _tokenService.CreateAccessToken(user);
            userResponse.RefreshToken = _tokenService.CreateRefreshToken(user);
            
            _logger.LogInformation("User logged in successfully: {Username}", request.Username);
            return Ok(userResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "User login failed: {Username}", request.Username);
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("RefreshAccessToken")]
    public async Task<ActionResult<string>> RefreshAccessToken()
    {
        try
        {
            string refreshToken = Request.Headers["Authorization"]!;
            
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is missing.");
            }
            
            var principal = _tokenService.GetPrincipalFromToken(refreshToken, false);

            var unixTimestamp = long.Parse(principal.FindFirst(JwtRegisteredClaimNames.Exp)!.Value);
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
            
            if (expirationTime <= DateTime.UtcNow)
            {
                return BadRequest("Refresh token has expired.");
            }

            var user = await _userManager.FindByNameAsync(
                principal.FindFirst(ClaimTypes.GivenName)?.Value);
            
            var accessToken = _tokenService.CreateAccessToken(user);
            
            _logger.LogInformation("Refreshed access token for user ({Username})", user.UserName);
            return Ok(accessToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed refreshing access token");
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("LoginWithAccessToken")]
    public async Task<ActionResult<UserResponseByToken>> LoginWithAccessToken()
    {
        try
        {
            string accessToken = Request.Headers["Authorization"]!;
            
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Access token is missing.");
            }
            
            var principal = _tokenService.GetPrincipalFromToken(accessToken, true);
            
            var unixTimestamp = long.Parse(principal.FindFirst(JwtRegisteredClaimNames.Exp)!.Value);
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
            
            if (expirationTime <= DateTime.UtcNow)
            {
                return BadRequest("Access token has expired.");
            }
            
            var user = await _userManager.FindByNameAsync(
                principal.FindFirst(ClaimTypes.GivenName)?.Value);
            
            _logger.LogInformation("User logged in successfully: {Username}", user.UserName);
            return Ok(_mapper.Map<UserResponseByToken>(user));

        }
        catch (Exception e)
        {
            _logger.LogError(e, "User login by token failed");
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest("Invalid email or email not confirmed.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var callbackUrl = Url.Action(nameof(PasswordReset), "User", 
                new { resetToken, email = user.Email }, Request.Scheme);
            
            await _emailService.SendEmailAsync(user.Email, 
                "Real Estate Password Reset",
                $"Click the link to reset your password:\n{callbackUrl}");
            
            _logger.LogInformation("Confirmation email has been sent to the user ({Email})", request.Email);
            return Ok("Password reset link has been sent to your email.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed sending letter to the user: {Email}", request.Email);
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("PasswordReset")]
    public async Task<IActionResult> PasswordReset([FromBody] ResetPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        
            if (!resetResult.Succeeded)
            {
                return BadRequest($"Unable to reset password.");
            }
            
            _logger.LogInformation("User ({Email}) successfully changed password", request.Email);
            return Ok("Password reset successfully!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed password reset for user ({Email})", request.Email);
            return BadRequest(e.Message);
        }

    }
    [AllowAnonymous]
    [HttpGet("GetUser/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            var userResponse = _mapper.Map<UserResponse>(user);

            return Ok(userResponse);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while getting user ({Id})", id);
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("GetUserPhoto/{id}")]
    public async Task<IActionResult> GetUserPhoto(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            if (user.Photo != null)
            {
                return new FileContentResult(user.Photo, "image/jpeg");
            }
            
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error while getting user photo ({Id})", id);
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromForm] UserUpdateRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }
            
            var updatedUser = _mapper.Map(request, user);

            if (request.Photo != null)
            {
                updatedUser.Photo = await FileConverter.FileToBytes(request.Photo);
            }

            var result = await _userManager.UpdateAsync(updatedUser);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            var queueUser = _mapper.Map<UserUpdateQueueModel>(updatedUser);
            await _bus.Publish(queueUser);

            _logger.LogInformation("User ({UserId}) updated successfully", request.UserId);
            return Ok("User information has been updated successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "User update failed: {UserId}", request.UserId);
            return BadRequest(e.Message);
        }
    }
}

