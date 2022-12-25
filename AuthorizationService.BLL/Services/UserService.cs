using System.Security.Cryptography;
using AuthorizationService.BLL.DTO.Request;
using AuthorizationService.BLL.DTO.Response;
using AuthorizationService.BLL.Extensions.Helpers;
using AuthorizationService.BLL.Interfaces;
using AuthorizationService.DAL.Context;
using AuthorizationService.DAL.Data.Interfaces;
using AuthorizationService.DAL.Entities;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace AuthorizationService.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    private readonly AuthorizationContext _context;
    private readonly AppSettings _appSettings;
    private readonly IEmailService _emailService;

    public UserService(IUserRepository userRepository, IJwtUtils jwtUtils, IMapper mapper, IOptions<AppSettings> appSettings,
        IEmailService emailService, AuthorizationContext context)
    {
        _userRepository = userRepository;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
        _appSettings = appSettings.Value;
        _emailService = emailService;
        _context = context;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
    {
        var user = await _userRepository.GetByEmail(model.Email);

        if (user == null || !user.IsVerified || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            throw new AppException("Email or password is incorrect");
        }
        
        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(user);
        var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        if(user.RefreshTokens == null) user.RefreshTokens = new List<RefreshToken>();
        
        user.RefreshTokens.Add(refreshToken);
        RemoveOldRefreshTokens(user);

        // save changes to db
        _context.Update(user);
        _context.SaveChanges();

        var response = _mapper.Map<AuthenticateResponse>(user);
        response.JwtToken = jwtToken;
        response.RefreshToken = refreshToken.Token;
        
        return response;
    }
    
    public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
    {
        var user = _userRepository.GetByToken(token).Result;
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (refreshToken!.IsRevoked)
        {
            RevokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        if (!refreshToken.IsActive)
            throw new AppException("Invalid token");
        
        var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
        user.RefreshTokens.Add(newRefreshToken);

        // remove old refresh tokens from account
        RemoveOldRefreshTokens(user);

        // save changes to db
        _context.Update(user);
        await _context.SaveChangesAsync();

        // generate new jwt
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        // return data in authenticate response object
        var response = _mapper.Map<AuthenticateResponse>(user);
        response.JwtToken = jwtToken;
        response.RefreshToken = newRefreshToken.Token;
        
        return response;
    }
    
    public void RevokeToken(string token, string ipAddress)
    {
        var user = _userRepository.GetByToken(token).Result;
        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
            throw new AppException("Invalid token");
        
        RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
        _context.Update(user);
        _context.SaveChanges();
    }
    
    public async Task Register(RegisterRequest model, string origin)
    {
        if (_context.User.Any(x => x.Email == model.Email))
        {
            SendAlreadyRegisteredEmail(model.Email, origin);
            return;
        }
        
        var user = _mapper.Map<User>(model);

        // first registered account is an admin
        var isFirstAccount = !_context.User.Any();
        user.Role = isFirstAccount ? Role.Admin : Role.User;
        user.Id = Guid.NewGuid().ToString();
        user.Created = DateTime.UtcNow;
        user.VerificationToken = GenerateVerificationToken();
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        await _userRepository.Create(user);
        //_context.User.Add(user);
        //await _context.SaveChangesAsync();

        // send email
        SendVerificationEmail(user, origin);
    }
    
    public void VerifyEmail(string token)
    {
        var user = _userRepository.GetByToken(token).Result;

        if (user == null) 
            throw new AppException("Verification failed");

        user.Verified = DateTime.UtcNow;
        user.VerificationToken = "Verified";

        _context.User.Update(user);
        _context.SaveChanges();
    }
    
    public void ForgotPassword(ForgotPasswordRequest model, string origin)
    {
        var user = _userRepository.GetByEmail(model.Email).Result;
        
        if (user == null) return;

        // create reset token that expires after 1 day
        user.ResetToken = GenerateResetToken();
        user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

        _context.User.Update(user);
        _context.SaveChanges();

        // send email
        SendPasswordResetEmail(user, origin);
    }
    
    public void ValidateResetToken(ValidateResetTokenRequest model)
    {
        GetUserByResetToken(model.Token);
    }
    
    public void ResetPassword(ResetPasswordRequest model)
    {
        var user = GetUserByResetToken(model.Token);

        // update password and remove reset token
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
        user.PasswordReset = DateTime.UtcNow;
        user.ResetToken = null;
        user.ResetTokenExpires = null;

        _context.User.Update(user);
        _context.SaveChanges();
    }

    public async Task<IEnumerable<UserResponse>> GetAll()
    {
        var users = await _userRepository.GetAll();

        return _mapper.Map<IEnumerable<UserResponse>>(users);
    }
    
    public async Task<UserResponse> GetById(string id)
    {
        var user = await _userRepository.GetById(id);
        return _mapper.Map<UserResponse>(user);
    }

    public async Task Delete(string id)
    {
        await _userRepository.Delete(id);
    }
    
    private User GetUserByResetToken(string token)
    {
        var user = _context.User.SingleOrDefault(x =>
            x.ResetToken == token && x.ResetTokenExpires > DateTime.UtcNow);
        if (user == null) throw new AppException("Invalid token");
        return user;
    }
    
    private string GenerateResetToken()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

        var tokenIsUnique = !_context.User.Any(x => x.ResetToken == token);
        if (!tokenIsUnique)
            return GenerateResetToken();
        
        return token;
    }
    
    private string GenerateVerificationToken()
    {
        // token is a cryptographically strong random sequence of values
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        
        var tokenIsUnique = !_context.User.Any(x => x.VerificationToken == token);
        if (!tokenIsUnique)
            return GenerateVerificationToken();
        
        return token;
    }

    private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
        RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }
    
    private void RemoveOldRefreshTokens(User user)
    {
        user.RefreshTokens.RemoveAll(x => 
            !x.IsActive && 
            x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
            if (childToken.IsActive)
                RevokeRefreshToken(childToken, ipAddress, reason);
            else
                RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
        }
    }
    
    private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason, string replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }
    
    private void SendVerificationEmail(User user, string origin)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
        {
            // origin exists if request sent from browser single page app (e.g. Angular or React)
            // so send link to verify via single page app
            var verifyUrl = $"{origin}/user/verify-email?token={user.VerificationToken}";
            message = $@"<p>Please click the below link to verify your email address:</p>
                            <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
        }
        else
        {
            // origin missing if request sent directly to api (e.g. from Postman)
            // so send instructions to verify directly with api
            message = $@"<p>Please use the below token to verify your email address with the <code>/users/verify-email</code> api route:</p>
                            <p><code>{user.VerificationToken}</code></p>";
        }

        _emailService.Send(
            to: user.Email,
            subject: "Sign-up Verification API - Verify Email",
            html: $@"<h4>Verify Email</h4>
                        <p>Thanks for registering!</p>
                        {message}"
        );
    }

    private void SendAlreadyRegisteredEmail(string email, string origin)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
            message = $@"<p>If you don't know your password please visit the <a href=""{origin}/user/forgot-password"">forgot password</a> page.</p>";
        else
            message = "<p>If you don't know your password you can reset it via the <code>/users/forgot-password</code> api route.</p>";

        _emailService.Send(
            to: email,
            subject: "Sign-up Verification API - Email Already Registered",
            html: $@"<h4>Email Already Registered</h4>
                        <p>Your email <strong>{email}</strong> is already registered.</p>
                        {message}"
        );
    }

    private void SendPasswordResetEmail(User user, string origin)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
        {
            var resetUrl = $"{origin}/user/reset-password?token={user.ResetToken}";
            message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                            <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
        }
        else
        {
            message = $@"<p>Please use the below token to reset your password with the <code>/users/reset-password</code> api route:</p>
                            <p><code>{user.ResetToken}</code></p>";
        }

        _emailService.Send(
            to: user.Email,
            subject: "Sign-up Verification API - Reset Password",
            html: $@"<h4>Reset Password Email</h4>
                        {message}"
        );
    }
}