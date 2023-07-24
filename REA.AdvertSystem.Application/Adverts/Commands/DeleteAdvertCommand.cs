using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Adverts.Commands
{
    public class DeleteAdvertCommandHandler : IRequestHandler<DeleteAdvertCommand, string>
    {
        private IMongoCollection<Advert> Advert { get; }

        public DeleteAdvertCommandHandler(IAgencyDbConnection context)
        {
            Advert = context.ConnectToMongo<Advert>("Advert");
        }

        public async Task<string> Handle(DeleteAdvertCommand request, CancellationToken cancellationToken)
        {
            var advert = await Advert.Find(x => x.AdvertID == request.Id).FirstOrDefaultAsync();

            if (advert == null) throw new NotFoundException("Advert", request.Id);

            var deleteResult = await Advert.DeleteOneAsync(x => x.AdvertID == request.Id);

            if (deleteResult.DeletedCount == 0)
                throw new Exception($"Failed to delete the advert with id: {request.Id}.");

            return request.Id;
        }
    }
    public record DeleteAdvertCommand : IRequest<string>
    {
        public string Id { get; set;}
    }
}
