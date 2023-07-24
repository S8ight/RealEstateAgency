﻿using System.Reflection;
using AuthorizationService.BLL.DTO.Request;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using REA.AdvertSystem.Application.Common.DTO.AdvertDTO;
using REA.AdvertSystem.Application.Common.DTO.PhotoListDTO;
using REA.AdvertSystem.Application.Common.DTO.SaveListDTO;
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
            CreateMap<PaginatedList<Advert>, PaginatedList<AdvertResponse>>();
            CreateMap<PhotoList, PhotoResponse>();
            CreateMap<SaveList, SaveListResponse>();
            CreateMap<User, CreateUserCommand>();
            CreateMap<CreateUserCommand, QueueRequest>();
            CreateMap<QueueRequest, CreateUserCommand>();
        }
    }
}
