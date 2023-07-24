using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Commands
{
    public class UpdatePhotoListCommandHandler : IRequestHandler<UpdatePhotoListCommand, string>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }

        public UpdatePhotoListCommandHandler(IAgencyDbConnection connection)
        {
            PhotoList = connection.ConnectToMongo<PhotoList>("PhotoList");
        }

        public async Task<string> Handle(UpdatePhotoListCommand request, CancellationToken cancellationToken)
        {
            var newList = new PhotoList
            {
                PhotoID = request.PhotoID,
                AdvertID = request.AdvertID,
                PhotoLink = request.PhotoLink
            };

            await PhotoList.ReplaceOneAsync(Builders<PhotoList>.Filter.Eq("_id", request.PhotoID), newList);

            return newList.PhotoID;
        }
    }

    public record UpdatePhotoListCommand : IRequest<string>
    {
        public string PhotoID { get; set; }

        public string AdvertID { get; set; }

        public string PhotoLink { get; set; }
    }
}
