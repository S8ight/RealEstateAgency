using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Adverts.Commands
{
    public class UpdateAdvertCommandHandler : IRequestHandler<UpdateAdvertCommand, string>
    {
        private IMongoCollection<Advert> _advert { get; }

        public UpdateAdvertCommandHandler(IAgencyDbConnection context)
        {
            _advert = context.ConnectToMongo<Advert>("Advert");
        }

        public async Task<string> Handle(UpdateAdvertCommand request, CancellationToken cancellationToken)
        {
            var newAdvert = new Advert
            {
                AdvertID = request.AdvertID,
                Name = request.Name,
                Description = request.Description,
                Adress = request.Adress,
                Square = request.Square,
                Price = request.Price,
                Created = request.Created,
                UserID = request.UserID,
                
            };

            await _advert.ReplaceOneAsync(Builders<Advert>.Filter.Eq("_id", request.AdvertID), newAdvert);

            return newAdvert.AdvertID;
        }
    }
    public record UpdateAdvertCommand : IRequest<string>
    {
        public string AdvertID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Adress { get; set; }

        public float Square { get; set; }

        public float Price { get; set; }

        public DateTime Created { get; set; }

        public string UserID { get; set; }

        public string AdvertType { get; set; }
    }
}
