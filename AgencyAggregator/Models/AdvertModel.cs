namespace AgencyAggregator.Models;

public class AdvertModel
{
    public string AdvertID { get; set; }

    public string Name { get; set; }

    public string Town { get; set; }

    public float Square { get; set; }

    public float Price { get; set; }

    public DateTime Created { get; set; }
}