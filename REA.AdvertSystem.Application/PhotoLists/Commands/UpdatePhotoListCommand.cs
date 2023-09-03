using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Commands
{
    public class UpdatePhotoListCommandHandler : IRequestHandler<UpdatePhotoListCommand, string>
    {
        private IMongoCollection<PhotoList> PhotoList { get; }
        
        private readonly ILogger<UpdatePhotoListCommandHandler> _logger;

        public UpdatePhotoListCommandHandler(IAgencyDbConnection connection, ILogger<UpdatePhotoListCommandHandler> logger)
        {
            _logger = logger;
            PhotoList = connection.ConnectToMongo<PhotoList>("PhotoList");
        }

        public async Task<string> Handle(UpdatePhotoListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newList = new PhotoList
                {
                    Id = request.Id,
                    AdvertId = request.AdvertId,
                    PhotoLink = request.PhotoLink
                };

                await PhotoList.ReplaceOneAsync(Builders<PhotoList>.Filter.Eq("_id", request.Id), newList, cancellationToken: cancellationToken);

                return newList.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error occurred while replacing PhotoList: {Id}", request.Id);
                throw;
            }
        }
    }

    public record UpdatePhotoListCommand : IRequest<string>
    {
        public string Id { get; set; }

        public string AdvertId { get; set; }

        public string PhotoLink { get; set; }
    }
}
