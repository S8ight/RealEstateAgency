using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Commands
{
    public class CreatePhotoListCommandHandler : IRequestHandler<CreatePhotoListCommand, string>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }

        private readonly ILogger<CreatePhotoListCommandHandler> _logger;

        public CreatePhotoListCommandHandler(IAgencyDbConnection connection, ILogger<CreatePhotoListCommandHandler> logger)
        {
            _logger = logger;
            PhotoList = connection.ConnectToMongo<PhotoList>("PhotoList");
        }

        public async Task<string> Handle(CreatePhotoListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var photoEntities = request.PhotoLinks.Select(photoLink => new PhotoList
                {
                    Id = Guid.NewGuid().ToString(),
                    AdvertId = request.AdvertId,
                    PhotoLink = photoLink
                }).ToList();
                
                await PhotoList.InsertManyAsync(photoEntities, cancellationToken: cancellationToken);
                
                return string.Join(",", photoEntities.Select(entity => entity.Id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while inserting PhotoList");
                throw;
            }
        }
    }

    public record CreatePhotoListCommand : IRequest<string>
    {
        public string AdvertId { get; set; }
        public List<string> PhotoLinks { get; set; }
    }

}
