using AutoMapper;
using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.DTO.PhotoListDTO;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Queries
{
    public record GetPhotoListById : IRequest<PhotoResponse>
    {
        public string Id { get; set; }
    }

    public class GetPhotoListByIdHandler : IRequestHandler<GetPhotoListById, PhotoResponse>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }

        private IMapper Mapper { get; }

        public GetPhotoListByIdHandler(IAgencyDbConnection context, IMapper mapper)
        {
            PhotoList = context.ConnectToMongo<PhotoList>("PhotoList");
            Mapper = mapper;
        }

        public async Task<PhotoResponse> Handle(GetPhotoListById query, CancellationToken cancellationToken)
        {
            var result = await PhotoList.Find(x => x.PhotoID == query.Id).ToListAsync();

            if (result.Count == 0) throw new NotFoundException("PhotoList", query.Id);

            return Mapper.Map<PhotoList, PhotoResponse>(result.FirstOrDefault());

        }
    }
}
