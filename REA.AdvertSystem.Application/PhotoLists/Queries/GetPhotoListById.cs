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
        private IMongoCollection<PhotoList> _photoList { get; }

        private IMapper _mapper { get; }

        public GetPhotoListByIdHandler(IAgencyDbConnection context, IMapper mapper)
        {
            _photoList = context.ConnectToMongo<PhotoList>("PhotoList");
            _mapper = mapper;
        }

        public async Task<PhotoResponse> Handle(GetPhotoListById query, CancellationToken cancellationToken)
        {
            var result = await _photoList.Find(x => x.PhotoID == query.Id).ToListAsync();

            if (result.Count == 0) throw new NotFoundException("PhotoList", query.Id);

            return _mapper.Map<PhotoList, PhotoResponse>(result.FirstOrDefault());

        }
    }
}
