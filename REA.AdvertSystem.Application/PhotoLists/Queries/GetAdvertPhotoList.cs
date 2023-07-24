using AutoMapper;
using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.DTO.PhotoListDTO;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Queries
{
    public record GetAdvertPhotoList : IRequest<List<PhotoResponse>>
    {
        public string Id { get; set; }
    }

    public class GetAdvertPhotoListHandler : IRequestHandler<GetAdvertPhotoList, List<PhotoResponse>>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }

        private IMapper Mapper { get; }

        public GetAdvertPhotoListHandler(IAgencyDbConnection context, IMapper mapper)
        {
            PhotoList = context.ConnectToMongo<PhotoList>("PhotoList");
            Mapper = mapper;
        }

        public async Task<List<PhotoResponse>> Handle(GetAdvertPhotoList query, CancellationToken cancellationToken)
        {
            var result = await PhotoList.Find(x => x.AdvertID == query.Id).ToListAsync();

            if (result.Count == 0) throw new NotFoundException("PhotoList", query.Id);

            return Mapper.Map<List<PhotoList>, List<PhotoResponse>>(result); 
        }
    }
}
