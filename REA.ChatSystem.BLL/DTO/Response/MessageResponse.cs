namespace REA.ChatSystem.BLL.DTO.Response
{
    public class MessageResponse
    {
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public string MessageBody { get; set; }
        public DateTime Created { get; set; }
    }
}
