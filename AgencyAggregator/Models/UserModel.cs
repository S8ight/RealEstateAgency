namespace AgencyAggregator.Models;

public class UserModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirthd { get; set; }
    public DateTime Created { get; set; }
    public byte[]? Photo { get; set; }
    public bool IsVerified { get; set; }
}