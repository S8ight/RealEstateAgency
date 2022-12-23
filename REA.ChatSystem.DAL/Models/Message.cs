namespace REA.ChatSystem.DAL.Models
{
    public class Message
    {
        public string MessageId { get; set; }
        
        public string ChatId { get; set; }
        
        public string SenderId { get; set; }
        
        public string RecieverId { get; set; }
        public string MessageBody { get; set; }
        
        public DateTime Created { get; set; }
        
        public bool Checked { get; set; }
        
        //public Chat Chat { get; set; }
    }
}