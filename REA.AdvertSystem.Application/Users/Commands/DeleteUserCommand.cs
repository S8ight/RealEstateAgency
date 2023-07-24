using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Users.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, string>
{
    private IMongoCollection<User> User { get; }

    public DeleteUserCommandHandler(IAgencyDbConnection context)
    {
        User= context.ConnectToMongo<User>("Users");
    }

    public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await User.Find(x => x.Id == request.Id).ToListAsync();

        if (user == null) throw new NotFoundException("User", request.Id);

        await User.DeleteOneAsync(x => x.Id == request.Id);

        return request.Id;
    }
}
public record DeleteUserCommand : IRequest<string>
{
    public string Id { get; set;}
}