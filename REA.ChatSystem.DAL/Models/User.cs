namespace REA.ChatSystem.DAL.Models
{
    public class User
    {
        public string Id { get; set; }
        
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public byte[]? Photo { get; set; }
        
        //public List<Chat> Chat { get; set; }
    }
}