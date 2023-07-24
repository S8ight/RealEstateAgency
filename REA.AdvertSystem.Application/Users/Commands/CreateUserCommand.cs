using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private IMongoCollection<User> User { get; }

    public CreateUserCommandHandler(IAgencyDbConnection context)
    {
        User = context.ConnectToMongo<User>("Users");
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Patronymic = request.Patronymic,
            Photo = request.Photo
        };

        await User.InsertOneAsync(entity, cancellationToken: cancellationToken);


        return entity.Id;
    }
}
public record CreateUserCommand : IRequest<string>
{
    public string Id { get; set; }

    public string FirstName { get; set; }
        
    public string LastName { get; set; }
        
    public string? Patronymic { get; set; }
        
    public byte[]? Photo { get; set; }
}