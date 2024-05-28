namespace REA.ChatSystem.DAL.Models
{
    public class Chat
    {
        public string ChatId { get; set; }
        public string UserId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime Created { get; set; }
        

        public User User { get; set; }
        public List<Message> Messages { get; set; }
    }
}