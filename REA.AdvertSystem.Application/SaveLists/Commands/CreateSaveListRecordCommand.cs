using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.SaveLists.Commands
{
    public class CreateSaveListCommandHandler : IRequestHandler<CreateSaveListRecordCommand, string>
    {
        private IMongoCollection<SaveList> SaveList { get; }
        private readonly ILogger<CreateSaveListCommandHandler> _logger;

        public CreateSaveListCommandHandler(IAgencyDbConnection connection, ILogger<CreateSaveListCommandHandler> logger)
        {
            _logger = logger;
            SaveList = connection.ConnectToMongo<SaveList>("SaveList");
        }

        public async Task<string> Handle(CreateSaveListRecordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new SaveList
                {
                    Id = Guid.NewGuid().ToString(),
                    AdvertId = request.AdvertId,
                    UserId = request.UserId
                };

                await SaveList.InsertOneAsync(entity, cancellationToken: cancellationToken);

                return entity.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while inserting record in SaveList");
                throw;
            }
        }
    }

    public record CreateSaveListRecordCommand : IRequest<string>
    {
        public string AdvertId { get; set; }

        public string UserId { get; set; }
    }
}
