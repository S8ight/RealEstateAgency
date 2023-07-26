namespace REA.ChatSystem.BLL.DTO.Request;

public class MessageUpdateRequest
{
    public string MessageId { get; set; }
    
    public string ChatId { get; set; }
    
    public string MessageBody { get; set; }
}