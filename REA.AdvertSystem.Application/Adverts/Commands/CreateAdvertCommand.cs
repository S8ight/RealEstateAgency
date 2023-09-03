using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Adverts.Commands
{

    public class CreateAdvertCommandHandler : IRequestHandler<CreateAdvertCommand, string>
    {
        private IMongoCollection<Advert> Advert { get; }
        
        private readonly ILogger<CreateAdvertCommandHandler> _logger;

        public CreateAdvertCommandHandler(IAgencyDbConnection context, ILogger<CreateAdvertCommandHandler> logger)
        {
            _logger = logger;
            Advert = context.ConnectToMongo<Advert>("Advert");
        }

        public async Task<string> Handle(CreateAdvertCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new Advert
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Description = request.Description,
                    Adress = request.Adress,
                    Country = request.Country,
                    Settlement = request.Settlement,
                    Square = request.Square,
                    Price = request.Price,
                    Created = request.Created,
                    UserId = request.UserId,
                    EstateType = request.EstateType,
                    IsForSale = request.IsForSale,
                    IsForRent = request.IsForRent
                };

                await Advert.InsertOneAsync(entity, cancellationToken: cancellationToken);

                return entity.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error occurred while inserting advert");
                throw;
            }
        }
    }
    public record CreateAdvertCommand : IRequest<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Adress { get; set; }
        
        public string Country { get; set; }
        
        public string Settlement { get; set; }

        public float Square { get; set; }

        public float Price { get; set; }

        public DateTime Created { get; set; }

        public string UserId { get; set; }

        public string EstateType { get; set; }

        public bool IsForSale { get; set; }

        public bool IsForRent { get; set; }
    }
}
