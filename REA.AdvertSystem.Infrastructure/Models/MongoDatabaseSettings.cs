namespace REA.AdvertSystem.Infrastructure.Models;

public class MongoDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
    

    public string AdvertsCollectionName { get; set; } = null!;
    
    public string PhotoListsCollectionName { get; set; } = null!;
    
    public string SaveListsCollectionName { get; set; } = null!;
    
    public string UsersCollectionName { get; set; } = null!;

}