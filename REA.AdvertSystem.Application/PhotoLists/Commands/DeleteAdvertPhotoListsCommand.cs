using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Commands
{
    public class DeleteAdvertPhotoListsCommandHandler : IRequestHandler<DeleteAdvertPhotoListsCommand, long>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }

        public DeleteAdvertPhotoListsCommandHandler(IAgencyDbConnection connection)
        {
            PhotoList = connection.ConnectToMongo<PhotoList>("PhotoList");
        }

        public async Task<long> Handle(DeleteAdvertPhotoListsCommand request, CancellationToken cancellationToken)
        {
            var deleteResult = await PhotoList.DeleteManyAsync(x => x.AdvertId == request.AdvertId, cancellationToken: cancellationToken);

            if (deleteResult.DeletedCount == 0)
            {
                throw new ArgumentException($"Advert({request.AdvertId}) has no photos");
            }

            return deleteResult.DeletedCount;
        }
    }

    public record DeleteAdvertPhotoListsCommand : IRequest<long>
    {
        public string AdvertId { get; set; }

    }
}