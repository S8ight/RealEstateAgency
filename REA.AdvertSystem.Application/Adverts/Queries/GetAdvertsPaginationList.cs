using AutoMapper;
using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Application.Common.Models;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Adverts.Queries
{
    public record GetAdvertsPaginationList : IRequest<PaginatedList<AdvertResponse>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 9;
    }

    public class GetAdvertsPaginationListHandler : IRequestHandler<GetAdvertsPaginationList, PaginatedList<AdvertResponse>>
    {
        private IMongoCollection<Advert> Advert { get; }

        private IMapper Mapper { get; }

        public GetAdvertsPaginationListHandler(IAgencyDbConnection context, IMapper mapper)
        {
            Advert = context.ConnectToMongo<Advert>("Advert");
            Mapper = mapper;
        }

        public async Task<PaginatedList<AdvertResponse>> Handle(GetAdvertsPaginationList request, CancellationToken cancellationToken)
        {
            var results = await PaginatedList<Advert>.GetPagerResultAsync(request.PageNumber, request.PageSize, Advert);
            return Mapper.Map<PaginatedList<Advert>, PaginatedList<AdvertResponse>>(results);
        }
    }


}
