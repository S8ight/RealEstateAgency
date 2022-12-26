using System.Reflection;
using AuthorizationService.BLL.DTO.Request;
using AutoMapper;
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
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            CreateMap<Advert, AdvertResponse>();
            CreateMap<PaginatedList<Advert>, PaginatedList<AdvertResponse>>();
            CreateMap<PhotoList, PhotoResponse>();
            CreateMap<SaveList, SaveListResponse>();
            CreateMap<User, CreateUserCommand>();
            CreateMap<CreateUserCommand, QueueRequest>();
            CreateMap<QueueRequest, CreateUserCommand>();
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {

            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
