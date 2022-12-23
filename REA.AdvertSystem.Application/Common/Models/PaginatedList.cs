using MongoDB.Driver;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Common.Models
{
    public class PaginatedList<T>
    {  
        public int Count { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public IEnumerable<Advert> Items { get; set; }

        public static async Task<PaginatedList<Advert>> GetPagerResultAsync(int page, int pageSize, IMongoCollection<Advert> collection)
        {
            var countFacet = AggregateFacet.Create("countFacet",
                PipelineDefinition<Advert, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<Advert>()
                }));

            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<Advert, Advert>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(Builders<Advert>.Sort.Descending(x => x.Created)),
                    PipelineStageDefinitionBuilder.Skip<Advert>((page - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<Advert>(pageSize),
                }));

            var filter = Builders<Advert>.Filter.Empty;
            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet<Advert>(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "countFacet")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "dataFacet")
                .Output<Advert>();

            return new PaginatedList<Advert>
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Items = data
            };
        }
    }
}
