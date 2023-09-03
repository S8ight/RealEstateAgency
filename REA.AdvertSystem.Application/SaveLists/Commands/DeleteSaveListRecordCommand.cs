using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.SaveLists.Commands
{
    public class DeleteSaveListCommandHandler : IRequestHandler<DeleteSaveListRecordCommand, string>
    {
        private IMongoCollection<SaveList> SaveList { get; }

        public DeleteSaveListCommandHandler(IAgencyDbConnection connection)
        {
            SaveList = connection.ConnectToMongo<SaveList>("SaveList");
        }

        public async Task<string> Handle(DeleteSaveListRecordCommand request, CancellationToken cancellationToken)
        {
            var saveList = await SaveList.Find(x => x.Id == request.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            if (saveList.Count == 0)
            {
                throw new NotFoundException("SaveList", request.Id);
            }

            var deleteResult = await SaveList.DeleteOneAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            
            if (deleteResult.DeletedCount == 0)
            {
                throw new ArgumentException($"Failed to delete record({request.Id}) from SaveList");
            }
            
            return request.Id;
        }
    }

    public record DeleteSaveListRecordCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
}
