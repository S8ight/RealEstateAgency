using AuthorizationService.BLL.DTO.Request;
using AuthorizationService.BLL.DTO.Response;

namespace AuthorizationService.BLL.Interfaces;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
    Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
    void RevokeToken(string token, string ipAddress);
    Task Register(RegisterRequest model, string origin);
    Task VerifyEmail(string token);
    void ForgotPassword(ForgotPasswordRequest model, string origin);
    void ValidateResetToken(ValidateResetTokenRequest model);
    void ResetPassword(ResetPasswordRequest model);
    Task<IEnumerable<UserResponse>> GetAll();
    Task<UserResponse> GetById(string id);
    //UserResponse Create(CreateRequest model);
    Task Delete(string id);
}