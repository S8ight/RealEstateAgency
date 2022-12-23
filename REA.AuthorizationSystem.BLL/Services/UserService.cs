using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using REA.AuthorizationSystem.BLL.Authorization.Helpers;
using REA.AuthorizationSystem.BLL.DTO.Request;
using REA.AuthorizationSystem.BLL.DTO.Response;
using REA.AuthorizationSystem.BLL.Interfaces;
using REA.AuthorizationSystem.BLL.Validation;
using REA.AuthorizationSystem.DAL.Context;
using REA.AuthorizationSystem.DAL.Entities;
using REA.AuthorizationSystem.DAL.Interfaces;

namespace REA.AuthorizationSystem.BLL.Services;

public class UserService : IUserService
{
    private AgencyContext _context;
    private IJwtConfiguration _jwtConfiguration;
    private readonly AppSettings _appSettings;
    private IUserRepository _repository;
    private IMapper _mapper;
    private IEmailService _emailService;
    private readonly IBusControl _bus;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public UserService(
        AgencyContext context,
        IJwtConfiguration jwtUtils,
        IUserRepository repository,
        UserManager<User> userManager,
        IMapper mapper,
        IEmailService emailService,
        IBusControl bus,
        RoleManager<IdentityRole> roleManager,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _jwtConfiguration = jwtUtils;
        _repository = repository;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _bus = bus;
        _emailService = emailService;
        _appSettings = appSettings.Value;
    }
    
    public AuthenticationResponse Authenticate(AuthenticationRequest model, string ipAddress)
    {
        var user = _context.Users.SingleOrDefault(x => x.UserName == model.Username);
        
        if (user == null || !_userManager.CheckPasswordAsync(user, model.Password).Result) 
            throw new Exception("Username or password is incorrect");

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtConfiguration.GenerateJwtToken(user);
        var refreshToken = _jwtConfiguration.GenerateRefreshToken(ipAddress);
        user.RefreshTokens.Add(refreshToken);

        // remove old refresh tokens from user
        removeOldRefreshTokens(user);

        // save changes to db
        _context.Update(user);
        _context.SaveChanges();
        
        return new AuthenticationResponse(user, jwtToken, refreshToken.Token);
    }

    public async Task<string> Register(RegistrationRequest model, string origin)
    {
        var userExists = _userManager.FindByEmailAsync(model.Email);
        if (userExists.Result == null)
        {
            UserValidation validator = new UserValidation();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return validationResult.ToString();
            }
        }
        else
        {
            return "User with this email already exist";
        }

        var user = _mapper.Map<User>(model);

        await _userManager.CreateAsync(user, model.Password);

        var emailToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

        // if (_context.Users.Count() == 0 && !_roleManager.RoleExistsAsync(AplicationRoles.Ad))
        // {
        //     await _userManager.AddToRoleAsync(user, "Admin");
        // }
        // else
        // {
        //     await _userManager.AddToRoleAsync(user, "User");
        // }

        sendVerificationEmail(emailToken, user.Email!, origin);

        //return string.Empty;
        return emailToken;
    }
    
    public AuthenticationResponse RefreshToken(string token, string ipAddress)
    {
        var user = getUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (refreshToken.IsRevoked)
        {
            revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
            _context.Update(user);
            _context.SaveChanges();
        }

        if (!refreshToken.IsActive)
            throw new Exception("Invalid token");

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
        user.RefreshTokens.Add(newRefreshToken);

        // remove old refresh tokens from user
        removeOldRefreshTokens(user);

        // save changes to db
        _context.Update(user);
        _context.SaveChanges();

        // generate new jwt
        var jwtToken = _jwtConfiguration.GenerateJwtToken(user);

        return new AuthenticationResponse(user, jwtToken, newRefreshToken.Token);
    }

    public void RevokeToken(string token, string ipAddress)
    {
        var user = getUserByRefreshToken(token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
            throw new Exception("Invalid token");

        // revoke token and save
        revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
        _context.Update(user);
        _context.SaveChanges();
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _repository.GetAllUsers();
    }

    public async Task<User> GetById(string id)
    {
        var user = await _repository.GetUserById(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    // helper methods

    private User getUserByRefreshToken(string token)
    {
        var user = _repository.GetUserByRefreshToken(token);

        if (user == null)
            throw new Exception("Invalid token");

        return user;
    }

    private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = _jwtConfiguration.GenerateRefreshToken(ipAddress);
        revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
    
    public async Task<string> VerifyEmail(string token, string email)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == email);
        if (user == null)
            return "User with this email is not registered.";
        
        var verified = _userManager.ConfirmEmailAsync(user, token);
        if (!verified.Result.Succeeded)
            return "Verification failed.";

        return "Verification successful, you can now login";
    }

    public async Task AddUserToQueue(string email)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == email);
        var queueUser = _mapper.Map<QueueRequest>(user);
        Console.WriteLine("Published...");
        await _bus.Publish(queueUser);
        // var endPoint = await _bus.GetSendEndpoint(new Uri("exchange:chat-system-queue"));
        // await endPoint.Send(queueUser);
    }


    private void removeOldRefreshTokens(User user)
    {
        user.RefreshTokens.RemoveAll(x => 
            !x.IsActive && 
            x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if(!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
            if (childToken.IsActive)
                revokeRefreshToken(childToken, ipAddress, reason);
            else
                revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
        }
    }

    private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }
    
    private void sendVerificationEmail(string token, string email, string origin)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
        {
            // origin exists if request sent from browser single page app (e.g. Angular or React)
            // so send link to verify via single page app
            var verifyUrl = $"{origin}/User/verify-email?token={token}";
            message = $@"<p>Please click the below link to verify your email address:</p>
                            <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
        }
        else
        {
            // origin missing if request sent directly to api (e.g. from Postman)
            // so send instructions to verify directly with api
            message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                            <p><code>{token}</code></p>";
        }

        _emailService.Send(
            to: email,
            subject: "Sign-up Verification API - Verify Email",
            html: $@"<h4>Verify Email</h4>
                        <p>Thanks for registering!</p>
                        {message}"
        );
    }
}