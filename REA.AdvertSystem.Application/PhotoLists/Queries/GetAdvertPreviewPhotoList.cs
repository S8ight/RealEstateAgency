using AutoMapper;
using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.DTO.PhotoListDTO;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Queries
{
    public record GetAdvertPreviewPhotoList : IRequest<string>
    {
        public string Id { get; set; }
    }

    public class GetPhotoListByIdHandler : IRequestHandler<GetAdvertPreviewPhotoList, string>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }

        private IMapper Mapper { get; }

        public GetPhotoListByIdHandler(IAgencyDbConnection context, IMapper mapper)
        {
            PhotoList = context.ConnectToMongo<PhotoList>("PhotoList");
            Mapper = mapper;
        }

        public async Task<string> Handle(GetAdvertPreviewPhotoList query, CancellationToken cancellationToken)
        {
            var photoList = await PhotoList.Find(x => x.AdvertId == query.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (photoList == null)
            {
                throw new ArgumentException("Advert has no photos");
            }

            return photoList.PhotoLink;
        }
    }
}
