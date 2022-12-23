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
        private IMongoCollection<Advert> _advert { get; }

        private IMapper _mapper { get; }

        public GetAdvertsPaginationListHandler(IAgencyDbConnection context, IMapper mapper)
        {
            _advert = context.ConnectToMongo<Advert>("Advert");
            _mapper = mapper;
        }

        public async Task<PaginatedList<AdvertResponse>> Handle(GetAdvertsPaginationList request, CancellationToken cancellationToken)
        {
            var results = await PaginatedList<Advert>.GetPagerResultAsync(request.PageNumber, request.PageSize, _advert);
            return _mapper.Map<PaginatedList<Advert>, PaginatedList<AdvertResponse>>(results);
        }
    }


}
