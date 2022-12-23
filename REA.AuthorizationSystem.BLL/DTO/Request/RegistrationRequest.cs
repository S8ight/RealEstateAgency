namespace REA.AuthorizationSystem.BLL.DTO.Request;

public class RegistrationRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
        
    public string? Patronymic { get; set; }
        
    public DateTime DateOfBirthd { get; set; }
        
    public DateTime Created { get; set; }
        
    public byte[]? Photo { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}