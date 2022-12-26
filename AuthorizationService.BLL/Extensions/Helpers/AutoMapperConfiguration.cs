using AuthorizationService.BLL.DTO.Request;
using AuthorizationService.BLL.DTO.Response;
using AuthorizationService.DAL.Entities;
using AutoMapper;

namespace AuthorizationService.BLL.Extensions.Helpers;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<User, UserResponse>();
        
        CreateMap<User, AuthenticateResponse>();
        
        CreateMap<RegisterRequest, User>();

        CreateMap<CreateRequest, User>();
        
        CreateMap<User, QueueRequest>();
    }
}