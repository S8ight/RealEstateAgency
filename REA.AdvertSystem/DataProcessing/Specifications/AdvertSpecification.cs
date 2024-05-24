using Ardalis.Specification;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.DTOs.Request;

public class AdvertSpecification : Specification<Advert>
{
    public AdvertSpecification(AdvertsFilterRequest filters)
    {
        Query
            .Where(a => string.IsNullOrEmpty(filters.Province) || a.Province == filters.Province)
            .Where(a => string.IsNullOrEmpty(filters.Settlement) || a.Settlement == filters.Settlement)
            .Where(a => !filters.Square.HasValue || a.Square >= filters.Square)
            .Where(a => !filters.Square.HasValue || a.Square <= filters.Square)
            .Where(a => !filters.FloorsNumber.HasValue || a.FloorsNumber >= filters.FloorsNumber)
            .Where(a => !filters.FloorsNumber.HasValue || a.FloorsNumber <= filters.FloorsNumber)
            .Where(a => !filters.RoomsNumber.HasValue || a.RoomsNumber >= filters.RoomsNumber)
            .Where(a => !filters.RoomsNumber.HasValue || a.RoomsNumber <= filters.RoomsNumber)
            .Where(a => !filters.MinPrice.HasValue || a.Price >= filters.MinPrice)
            .Where(a => !filters.MaxPrice.HasValue || a.Price <= filters.MaxPrice)
            .Where(a => !filters.EstateType.HasValue || a.EstateType == filters.EstateType)
            .Include(a => a.User)
            .Include(a => a.PhotoList.OrderBy(pl => pl.Id));
        
        if (filters.PageNumber > 0 && filters.PageSize > 0)
        {
            Query
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize);
        }
    }
}