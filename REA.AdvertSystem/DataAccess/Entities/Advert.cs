using REA.AdvertSystem.DataAccess.Entities.Enums;

namespace REA.AdvertSystem.DataAccess.Entities;

public class Advert
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Address { get; set; }
        
    public string Province { get; set; }
        
    public string Settlement { get; set; }
    
    public string? Infrastructure { get; set; }
    
    public string? HouseholdAppliances { get; set; }
    
    public double  Square { get; set; }
    
    public int FloorsNumber { get; set; }
    
    public int RoomsNumber { get; set; }

    public double  Price { get; set; }

    public DateTime Created { get; set; }

    public string UserId { get; set; }

    public EstateType EstateType { get; set; }
    
    public virtual ICollection<PhotoList>? PhotoList { get; set; }
    public virtual ICollection<SaveList>? SaveLists { get; set; }
    public virtual User? User { get; set; }
}