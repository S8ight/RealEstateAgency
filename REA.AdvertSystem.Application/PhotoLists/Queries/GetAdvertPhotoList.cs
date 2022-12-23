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
        private IMongoCollection<PhotoList> _photoList { get; }

        private IMapper _mapper { get; }

        public GetAdvertPhotoListHandler(IAgencyDbConnection context, IMapper mapper)
        {
            _photoList = context.ConnectToMongo<PhotoList>("PhotoList");
            _mapper = mapper;
        }

        public async Task<List<PhotoResponse>> Handle(GetAdvertPhotoList query, CancellationToken cancellationToken)
        {
            var result = await _photoList.Find(x => x.AdvertID == query.Id).ToListAsync();

            if (result.Count == 0) throw new NotFoundException("PhotoList", query.Id);

            return _mapper.Map<List<PhotoList>, List<PhotoResponse>>(result); 
        }
    }
}
