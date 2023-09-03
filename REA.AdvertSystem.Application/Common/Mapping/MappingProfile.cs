using ApiGateWay.Models;
using AutoMapper;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.DTO.PhotoListDTO;
using REA.AdvertSystem.Application.Common.DTO.SaveListDTO;
using REA.AdvertSystem.Application.Common.DTO.UserDTO;
using REA.AdvertSystem.Application.Common.Models;
using REA.AdvertSystem.Application.Users.Commands;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            DtoMapping();
        }

        private void DtoMapping()
        {
            CreateMap<Advert, AdvertResponse>();
            CreateMap<Advert, AdvertListResponse>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.Ignore());
            CreateMap<PaginatedList<Advert>, PaginatedList<AdvertListResponse>>();
            CreateMap<PhotoList, PhotoResponse>();
            CreateMap<SaveList, SaveListResponse>();
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore());;
            CreateMap<User, CreateUserCommand>();
            CreateMap<CreateUserCommand, UserRegistrationQueueModel>();
            CreateMap<UserRegistrationQueueModel, CreateUserCommand>();
            CreateMap<UserUpdateQueueModel, UpdateUserCommand>();
            CreateMap<UserDeleteQueueModel, DeleteUserCommand>();
        }
    }
}
