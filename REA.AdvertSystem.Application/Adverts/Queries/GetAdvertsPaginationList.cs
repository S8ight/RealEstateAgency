using System.Text.Json;
using AutoMapper;
using Gridify;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Application.Common.Models;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Adverts.Queries
{
    public record GetAdvertsPaginationList : IRequest<PaginationResponse<AdvertListResponse>>
    {
        public SearchParams SearchParams { get; set; }
    }

    public class GetAdvertsPaginationListHandler : IRequestHandler<GetAdvertsPaginationList, PaginationResponse<AdvertListResponse>>
    {
        private IMongoCollection<Advert> Advert { get; }

        private IMapper Mapper { get; }
        
        private readonly ILogger<GetAdvertsPaginationListHandler> _logger;

        public GetAdvertsPaginationListHandler(IAgencyDbConnection context, IMapper mapper, ILogger<GetAdvertsPaginationListHandler> logger)
        {
            Advert = context.ConnectToMongo<Advert>("Advert");
            Mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginationResponse<AdvertListResponse>> Handle(GetAdvertsPaginationList request, CancellationToken cancellationToken)
        {
            try
            {
                IQueryable<Advert> adverts = Advert.AsQueryable();
                
                if (request.SearchParams.ColumnFilters != null)
                {
                    var columnFilters = new List<ColumnFilter>();
                
                    columnFilters.AddRange(JsonSerializer.Deserialize<List<ColumnFilter>>(request.SearchParams.ColumnFilters)!);
                    
                    string filterString = string.Join(", ", columnFilters.Select(filter => $"{filter.id} = {filter.value}"));
                    adverts = adverts.ApplyFiltering(filterString);
                }
                
                if (request.SearchParams.SearchTerm != null)
                {
                    adverts = adverts.ApplyFiltering($"Name =* {request.SearchParams.SearchTerm}/i | Adress =* {request.SearchParams.SearchTerm}/i | Country =* {request.SearchParams.SearchTerm}/i | EstateType =* {request.SearchParams.SearchTerm}/i");
                }

                if (request.SearchParams.ColumnSorting != null)
                {
                    adverts = adverts.ApplyOrdering($"{request.SearchParams.ColumnSorting.ColumnName} {request.SearchParams.ColumnSorting.Desc}");
                }

                var pagedAdverts = adverts.ApplyPaging(request.SearchParams.PageNumber, request.SearchParams.PageSize);

                var mappedAdverts = Mapper.Map<List<Advert>, List<AdvertListResponse>>(pagedAdverts.ToList());

                return new PaginationResponse<AdvertListResponse>
                {
                    PagesCount = (int)Math.Ceiling((double)adverts.Count() / request.SearchParams.PageSize),
                    Items = mappedAdverts
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating the paginated list of adverts");
                throw;
            }
        }
    }


}
