using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.SaveLists.Commands
{
    public class CreateSaveListCommandHandler : IRequestHandler<CreateSaveListCommand, string>
    {
        private IMongoCollection<SaveList> SaveList { get; }

        public CreateSaveListCommandHandler(IAgencyDbConnection connection)
        {
            SaveList = connection.ConnectToMongo<SaveList>("SaveList");
        }

        public async Task<string> Handle(CreateSaveListCommand request, CancellationToken cancellationToken)
        {
            var entity = new SaveList
            {
                ListID = request.ListID,
                AdvertID = request.AdvertID,
                UserID = request.UserID
            };

            await SaveList.InsertOneAsync(entity);


            return entity.ListID;
        }
    }

    public record CreateSaveListCommand : IRequest<string>
    {
        public string ListID { get; set; }

        public string AdvertID { get; set; }

        public string UserID { get; set; }
    }
}
