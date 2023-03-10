using MediatR;
using MongoDB.Driver;
using AutoMapper;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Adverts.Queries
{
    public record GetAdvertsById : IRequest<AdvertResponse>
    {
        public string Id { get; set; }
    }

    public class GetAdvertsByIdHandler : IRequestHandler<GetAdvertsById, AdvertResponse>
    {
        private IMongoCollection<Advert> _advert { get; }

        private IMapper _mapper { get; }

        public GetAdvertsByIdHandler(IAgencyDbConnection context, IMapper mapper)
        {
            _advert = context.ConnectToMongo<Advert>("Advert");
            _mapper = mapper;
        }

        public async Task<AdvertResponse> Handle(GetAdvertsById query, CancellationToken cancellationToken)   
        {
            var result = await _advert.Find(x => x.AdvertID == query.Id).ToListAsync();

            if(result.Count == 0) throw new NotFoundException("Advert", query.Id);

            return _mapper.Map<Advert,AdvertResponse>(result.FirstOrDefault());

        }
    }
}
