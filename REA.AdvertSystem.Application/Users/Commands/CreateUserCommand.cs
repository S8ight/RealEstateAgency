using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private IMongoCollection<User> User { get; }
    
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(IAgencyDbConnection context, ILogger<CreateUserCommandHandler> logger)
    {
        _logger = logger;
        User = context.ConnectToMongo<User>("Users");
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
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
        catch (Exception e)
        {
            _logger.LogError(e,"Error occurred while inserting User");
            throw;
        }
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