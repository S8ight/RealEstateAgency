using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
{
    private IMongoCollection<User> User { get; }
    
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IAgencyDbConnection context, ILogger<UpdateUserCommandHandler> logger)
    {
        _logger = logger;
        User = context.ConnectToMongo<User>("Users");
    }

    public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newUser = new User
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Patronymic = request.Patronymic,
                Photo = request.Photo
            };

            await User.ReplaceOneAsync(Builders<User>.Filter.Eq("_id", request.Id), newUser, cancellationToken: cancellationToken);

            return newUser.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error occurred while replacing user: {Id}", request.Id);
            throw;
        }
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