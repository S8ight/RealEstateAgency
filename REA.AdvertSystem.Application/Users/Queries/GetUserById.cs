using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.DTO.UserDTO;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Users.Queries;

public record GetUserById : IRequest<UserResponse>
{
    public string Id { get; set; }
}

public class GetUserByIdHandler : IRequestHandler<GetUserById, UserResponse>
{
    private IMongoCollection<User> User { get; }

    private IMapper Mapper { get; }

    public GetUserByIdHandler(IAgencyDbConnection context, IMapper mapper)
    {
        User = context.ConnectToMongo<User>("Users");
        Mapper = mapper;
    }

    public async Task<UserResponse> Handle(GetUserById query, CancellationToken cancellationToken)   
    {
        var user = await User.Find(x => x.Id == query.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User", query.Id);
        }
        var userResponse = Mapper.Map<User,UserResponse>(user);
        
        if (user.Photo != null)
        {
            userResponse.Photo = new FileContentResult(user.Photo, "image/jpeg");
        }

        return userResponse;

    }
}