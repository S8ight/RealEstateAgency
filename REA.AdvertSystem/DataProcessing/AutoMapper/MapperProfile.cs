using ApiGateWay.Models;
using AutoMapper;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.DTO.SaveListDTO;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.DTOs.Response;
using AdvertRequest = REA.AdvertSystem.DTOs.Request.AdvertRequest;
using AdvertResponse = REA.AdvertSystem.DTOs.Response.AdvertResponse;

namespace REA.AdvertSystem.DataProcessing.AutoMapper;

public class MapperProfile :  Profile
{
    public MapperProfile()
    {
        CreateMap<Advert, AdvertsListResponse>()
            .ForMember(dest => dest.PhotoPreview,
                opt => opt.MapFrom(src => src.PhotoList.Count == 0 ? "" : src.PhotoList.ToArray()[0].PhotoUrl))
            .ForMember(dest => dest.EstateType,
                opt => opt.MapFrom(src => src.EstateType.ToString()));
        
        CreateMap<Advert, AdvertResponse>()
            .ForMember(dest => dest.EstateType,
                opt => opt.MapFrom(src => src.EstateType.ToString()))
            .ForMember(dest => dest.PhotoList,
                opt => opt.MapFrom(src => src.PhotoList!.Select(x => x.PhotoUrl).ToList()))
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.ActionType,
                opt => opt.MapFrom(src => src.ActionType.ToString()))
            .ForMember(dest => dest.ConditionType,
                opt => opt.MapFrom(src => src.ConditionType.ToString()));

        CreateMap<AdvertRequest, Advert>();
        
        CreateMap<User, UserResponse>();
        CreateMap<UserRequest, User>();

        CreateMap<SaveList, SaveListResponse>();
        CreateMap<SaveListRequest, SaveList>();
        CreateMap<UserRegistrationQueueModel, User>();
        CreateMap<UserUpdateQueueModel, User>();
    }
}