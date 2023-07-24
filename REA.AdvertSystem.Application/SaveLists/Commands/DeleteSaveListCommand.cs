using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.SaveLists.Commands
{
    public class DeleteSaveListCommandHandler : IRequestHandler<DeleteSaveListCommand, string>
    {
        private IMongoCollection<SaveList> SaveList { get; }

        public DeleteSaveListCommandHandler(IAgencyDbConnection connection)
        {
            SaveList = connection.ConnectToMongo<SaveList>("SaveList");
        }

        public async Task<string> Handle(DeleteSaveListCommand request, CancellationToken cancellationToken)
        {
            var saveList = await SaveList.Find(x => x.ListID == request.Id).ToListAsync();

            if (saveList.Count == 0) throw new NotFoundException("SaveList", request.Id);

            await SaveList.DeleteOneAsync(x => x.ListID == request.Id);

            return request.Id;
        }
    }

    public record DeleteSaveListCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
}
