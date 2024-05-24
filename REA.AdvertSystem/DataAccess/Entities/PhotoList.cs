namespace REA.AdvertSystem.DataAccess.Entities;

public class PhotoList
{
    public string Id { get; set; }

    public string AdvertId { get; set;}

    public string PhotoUrl{ get; set; }
    
    public virtual Advert Advert { get; set; }
}