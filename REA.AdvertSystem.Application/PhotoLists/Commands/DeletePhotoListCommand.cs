using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.PhotoLists.Commands
{
    public class DeletePhotoListCommandHandler : IRequestHandler<DeletePhotoListCommand, string>
    {
        private IMongoCollection<PhotoList> _photoList { get; }

        public DeletePhotoListCommandHandler(IAgencyDbConnection connection)
        {
            _photoList = connection.ConnectToMongo<PhotoList>("PhotoList");
        }

        public async Task<string> Handle(DeletePhotoListCommand request, CancellationToken cancellationToken)
        {
            var advert = await _photoList.Find(x => x.PhotoID == request.Id).ToListAsync();

            if (advert == null) throw new NotFoundException("PhotoList", request.Id);

            await _photoList.DeleteOneAsync(x => x.PhotoID == request.Id);

            return request.Id;
        }
    }

    public record DeletePhotoListCommand : IRequest<string>
    {
        public string Id { get; set; }

    }
}
