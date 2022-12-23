namespace REA.ChatSystem.BLL.DTO.Request
{
    public class MessageRequest
    {
        public string MessageId { get; set; }
        public string ChatId { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string MessageBody { get; set; }
        public DateTime Created { get; set; }
    }
}
