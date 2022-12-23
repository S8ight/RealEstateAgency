using AutoMapper;
using REA.AuthorizationSystem.BLL.DTO.Request;
using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.BLL.Authorization.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegistrationRequest, User>();
        CreateMap<User, QueueRequest>();
    }
}