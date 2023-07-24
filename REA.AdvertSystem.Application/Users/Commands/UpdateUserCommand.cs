using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
{
    private IMongoCollection<User> User { get; }

    public UpdateUserCommandHandler(IAgencyDbConnection context)
    {
        User = context.ConnectToMongo<User>("Users");
    }

    public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Patronymic = request.Patronymic,
            Photo = request.Photo
        };

        await User.ReplaceOneAsync(Builders<User>.Filter.Eq("_id", request.Id), newUser);

        return newUser.Id;
    }
}
public record UpdateUserCommand : IRequest<string>
{
    public string Id { get; set; }

    public string FirstName { get; set; }
        
    public string LastName { get; set; }
        
    public string? Patronymic { get; set; }
        
    public byte[]? Photo { get; set; }
}