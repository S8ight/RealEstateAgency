namespace AuthorizationService.DAL.Entities;

public class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirthd { get; set; }
    public DateTime Created { get; set; }
    public byte[]? Photo { get; set; }
    
    public string PasswordHash { get; set; }
    public string VerificationToken { get; set; }
    public DateTime? Verified { get; set; }
    public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public DateTime? PasswordReset { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; }
    public Role Role { get; set; }
    
    public bool OwnsToken(string token) 
    {
        return RefreshTokens.Find(x => x.Token == token) != null;
    }
}