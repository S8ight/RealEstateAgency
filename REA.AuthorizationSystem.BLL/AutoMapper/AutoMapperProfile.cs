using ApiGateWay.Models;
using AutoMapper;
using REA.AuthorizationSystem.BLL.DTO.Request;
using REA.AuthorizationSystem.BLL.DTO.Response;
using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.BLL.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        DtoMapping();
    }

    private void DtoMapping()
    {
        CreateMap<User, UserResponseByToken>();
        CreateMap<User, UserResponse>();
        CreateMap<UserUpdateRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.Condition(src => src.FirstName != null))
            .ForMember(dest => dest.LastName, opt => opt.Condition(src => src.LastName != null))
            .ForMember(dest => dest.Patronymic, opt => opt.Condition(src => src.Patronymic != null))
            .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth != null))
            .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => src.PhoneNumber != null))
            .ForMember(dest => dest.Photo, opt => opt.Ignore());
            
        CreateMap<User, UserLoginResponse>();
        CreateMap<User, UserRegistrationQueueModel>();
        CreateMap<User, UserUpdateQueueModel>();
    }
    
}