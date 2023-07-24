using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.SaveLists.Commands
{
    public class UpdateSaveListCommandHandler : IRequestHandler<UpdateSaveListCommand, string>
    {
        private IMongoCollection<SaveList> SaveList { get; }

        public UpdateSaveListCommandHandler(IAgencyDbConnection connection)
        {
            SaveList = connection.ConnectToMongo<SaveList>("SaveList");
        }

        public async Task<string> Handle(UpdateSaveListCommand request, CancellationToken cancellationToken)
        {
            var newList = new SaveList
            {
                ListID = request.ListID,
                AdvertID = request.AdvertID,
                UserID = request.UserID
            };

            await SaveList.ReplaceOneAsync(Builders<SaveList>.Filter.Eq("_id", request.ListID), newList);

            return newList.ListID;
        }
    }

    public record UpdateSaveListCommand : IRequest<string>
    {
        public string ListID { get; set; }

        public string AdvertID { get; set; }

        public string UserID { get; set; }
    }
}
