using REA.AdvertSystem.DataAccess.Entities.Enums;

namespace REA.AdvertSystem.DTOs.Request;

public class AdvertRequest
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Address { get; set; }
        
    public string Province { get; set; }
        
    public string Settlement { get; set; }
    
    public string? Infrastructure { get; set; }
    
    public string? HouseholdAppliances { get; set; }
    
    public float Square { get; set; }
    
    public int FloorsNumber { get; set; }
    
    public int RoomsNumber { get; set; }
    
    public string UserId { get; set; }
    
    public float Price { get; set; }
    
    public EstateType EstateType { get; set; }
    public ActionType ActionType { get; set; }
    public ConditionType ConditionType { get; set; }
}