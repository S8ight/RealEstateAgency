using AuthorizationService.BLL.DTO.Request;
using AutoMapper;
using REA.ChatSystem.BLL.DTO.Request;
using REA.ChatSystem.BLL.DTO.Response;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.BLL.Mapper
{
    public class MapperCfg : Profile
    {
        public MapperCfg()
        {
            CreateMap<ChatRequest, Chat>();
            CreateMap<Chat, ChatResponse>();
            CreateMap<MessageRequest, Message>();
            CreateMap<Message, MessageResponse>();
            CreateMap<User, UserResponse>();
            CreateMap<UserRequest, User>();
            CreateMap<QueueRequest, User>();
        }
    }
}
