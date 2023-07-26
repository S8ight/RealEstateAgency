namespace REA.ChatSystem.BLL.DTO.Request
{
    public class MessageRequest
    {
        public string ChatId { get; set; }
        
        public string SenderId { get; set; }
        
        public string ReceiverId { get; set; }
        public string MessageBody { get; set; }
        public DateTime Created { get; set; }
    }
}
