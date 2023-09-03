using Microsoft.AspNetCore.Mvc;

namespace REA.ChatSystem.BLL.DTO.Response
{
    public class UserResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public FileContentResult? Photo { get; set; }
    }
}
