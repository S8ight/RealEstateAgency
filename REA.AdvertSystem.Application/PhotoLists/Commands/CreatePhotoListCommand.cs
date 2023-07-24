using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Commands
{
    public class CreatePhotoListCommandHandler : IRequestHandler<CreatePhotoListCommand, string>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }
        
        public CreatePhotoListCommandHandler(IAgencyDbConnection connection)
        {
            PhotoList = connection.ConnectToMongo<PhotoList>("PhotoList");
        }

        public async Task<string> Handle(CreatePhotoListCommand request, CancellationToken cancellationToken)
        {
            var entity = new PhotoList
            {
                PhotoID = request.PhotoID,
                AdvertID = request.AdvertID,
                PhotoLink = request.PhotoLink
            };

            await PhotoList.InsertOneAsync(entity);


            return entity.PhotoID;
        }
    }

    public record CreatePhotoListCommand : IRequest<string>
    {
        public string PhotoID { get; set; }

        public string AdvertID { get; set; }

        public string PhotoLink { get; set; }
    }
}
