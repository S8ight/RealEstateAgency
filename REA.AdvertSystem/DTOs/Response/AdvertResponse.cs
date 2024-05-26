using REA.AdvertSystem.Application.Common.DTO.UserDTO;

namespace REA.AdvertSystem.DTOs.Response;

public class AdvertResponse
{
    public string Id { get; set; }

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

    public float Price { get; set; }

    public DateTime Created { get; set; }

    public string EstateType { get; set; }
    public string ConditionType { get; set; }
    public string ActionType { get; set; }

    public List<string> PhotoList { get; set; }

    public UserResponse Owner { get; set; }

}