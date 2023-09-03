using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Application.Common.Models;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Adverts.Queries
{
    public record GetAdvertsPaginationList : IRequest<PaginatedList<AdvertListResponse>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetAdvertsPaginationListHandler : IRequestHandler<GetAdvertsPaginationList, PaginatedList<AdvertListResponse>>
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

        public async Task<PaginatedList<AdvertListResponse>> Handle(GetAdvertsPaginationList request, CancellationToken cancellationToken)
        {
            try
            {
                var paginatedAdvertList = await PaginatedList<Advert>.GetPagerResultAsync(request.PageNumber, request.PageSize, Advert);
                
                var mappedList = Mapper.Map<PaginatedList<Advert>, PaginatedList<AdvertListResponse>>(paginatedAdvertList);

                return mappedList;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating the paginated list of adverts");
                throw;
            }
        }
    }


}
