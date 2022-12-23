using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Adverts.Commands
{
    public class DeleteAdvertCommandHandler : IRequestHandler<DeleteAdvertCommand, string>
    {
        private IMongoCollection<Advert> _advert { get; }

        public DeleteAdvertCommandHandler(IAgencyDbConnection context)
        {
            _advert = context.ConnectToMongo<Advert>("Advert");
        }

        public async Task<string> Handle(DeleteAdvertCommand request, CancellationToken cancellationToken)
        {
            var advert = await _advert.Find(x => x.AdvertID == request.Id).ToListAsync();

            if (advert == null) throw new NotFoundException("Advert", request.Id);

            await _advert.DeleteOneAsync(x => x.AdvertID == request.Id);

            return request.Id;
        }
    }
    public record DeleteAdvertCommand : IRequest<string>
    {
        public string Id { get; set;}
    }
}
