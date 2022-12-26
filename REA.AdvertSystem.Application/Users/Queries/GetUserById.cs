using AutoMapper;
using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Users.Queries;

public record GetUserById : IRequest<User>
{
    public string Id { get; set; }
}

public class GetUserByIdHandler : IRequestHandler<GetUserById, User>
{
    private IMongoCollection<User> _user { get; }

    private IMapper _mapper { get; }

    public GetUserByIdHandler(IAgencyDbConnection context, IMapper mapper)
    {
        _user = context.ConnectToMongo<User>("Users");
        _mapper = mapper;
    }

    public async Task<User> Handle(GetUserById query, CancellationToken cancellationToken)   
    {
        var result = await _user.Find(x => x.Id == query.Id).ToListAsync();

        if(result.Count == 0) throw new NotFoundException("User", query.Id);

        return result.FirstOrDefault()!;

    }
}