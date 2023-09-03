using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.DTO.SaveListDTO;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.SaveLists.Queries
{
    public class GetAdvertPhotoListHandler : IRequestHandler<GetSaveListbyUser, List<SaveListResponse>>
    {
        private IMongoCollection<SaveList> SaveList { get; }

        private IMapper Mapper { get; }
        
        private readonly ILogger<GetAdvertPhotoListHandler> _logger;

        public GetAdvertPhotoListHandler(IAgencyDbConnection context, IMapper mapper, ILogger<GetAdvertPhotoListHandler> logger)
        {
            SaveList = context.ConnectToMongo<SaveList>("SaveList");
            Mapper = mapper;
            _logger = logger;
        }

        public async Task<List<SaveListResponse>> Handle(GetSaveListbyUser query, CancellationToken cancellationToken)
        {
            try
            {
                var userSaveList = await SaveList.Find(x => x.UserId == query.Id)
                    .ToListAsync(cancellationToken: cancellationToken);
                
                var mappedList = Mapper.Map<List<SaveList>, List<SaveListResponse>>(userSaveList);
                
                return mappedList;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while retrieving user SaveList");
                throw;
            }
        }
    }
    
    public record GetSaveListbyUser : IRequest<List<SaveListResponse>>
    {
        public string Id { get; set; }
    }
}
