namespace REA.ChatSystem.BLL.DTO.Request
{
    public class ChatRequest
    {
        public string ChatId { get; set; }
        
        public string UserId { get; set; }
        public string RecieverId { get; set; }
        public DateTime Created { get; set; }
    }
}
