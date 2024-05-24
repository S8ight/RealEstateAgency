using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.DataProcessing.Specifications;

public class AdvertSearchSpecification : Specification<Advert>
{
    public AdvertSearchSpecification(string keywords, int pageNumber, int pageSize)
    {
        if (!string.IsNullOrEmpty(keywords))
        {
            var keywordTerms = keywords.Trim().ToLower();
            Query.Where(a => EF.Functions.ILike(a.Name, $"%{keywordTerms}%") ||
                             EF.Functions.ILike(a.Description, $"%{keywordTerms}%") ||
                             EF.Functions.ILike(a.Province, $"%{keywordTerms}%") ||
                             EF.Functions.ILike(a.Settlement, $"%{keywordTerms}%") ||
                             EF.Functions.ILike(a.Address, $"%{keywordTerms}%"));
        }

        Query
            .Include(a => a.PhotoList)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
