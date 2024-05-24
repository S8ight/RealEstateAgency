using System.Collections;

namespace REA.AdvertSystem.DataAccess.Entities;

public class User
{
    public string Id { get; set; }

    public string FirstName { get; set; }
        
    public string LastName { get; set; }
        
    public string? Patronymic { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
        
    public byte[]? Photo { get; set; }
    
    public virtual ICollection<Advert>? Adverts { get; set; }
}