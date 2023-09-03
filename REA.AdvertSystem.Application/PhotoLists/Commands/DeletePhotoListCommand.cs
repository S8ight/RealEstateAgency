using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Commands
{
    public class DeletePhotoListCommandHandler : IRequestHandler<DeletePhotoListCommand, string>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }

        public DeletePhotoListCommandHandler(IAgencyDbConnection connection)
        {
            PhotoList = connection.ConnectToMongo<PhotoList>("PhotoList");
        }

        public async Task<string> Handle(DeletePhotoListCommand request, CancellationToken cancellationToken)
        {
            var advert = await PhotoList.Find(x => x.Id == request.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            if (advert == null)
            {
                throw new NotFoundException("PhotoList", request.Id);
            }

            var deleteResult = await PhotoList.DeleteOneAsync(x => x.Id == request.Id, 
                cancellationToken: cancellationToken);
            
            if (deleteResult.DeletedCount == 0)
            {
                throw new ArgumentException($"Failed to delete the PhotoList with id: {request.Id}.");
            }

            return request.Id;
        }
    }

    public record DeletePhotoListCommand : IRequest<string>
    {
        public string Id { get; set; }

    }
}
