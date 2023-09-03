namespace ApiGateWay.Models;

public class UserUpdateQueueModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public byte[]? Photo { get; set; }
}