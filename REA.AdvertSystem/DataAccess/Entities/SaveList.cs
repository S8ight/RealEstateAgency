namespace REA.AdvertSystem.DataAccess.Entities;

public class SaveList
{
    public string Id { get; set; }

    public string AdvertId { get; set; }

    public string UserId { get; set; }
    
    public virtual Advert Advert { get; set; }
}