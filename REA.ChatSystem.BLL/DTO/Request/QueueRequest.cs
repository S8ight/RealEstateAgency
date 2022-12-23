namespace REA.ChatSystem.BLL.DTO.Request;

public class QueueRequest
{
    public string Id { get; set; }
        
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public byte[] Photo { get; set; }
}