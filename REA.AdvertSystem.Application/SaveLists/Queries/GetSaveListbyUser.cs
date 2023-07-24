using AutoMapper;
using MediatR;
using MongoDB.Driver;
using REA.AdvertSystem.Application.Common.DTO.SaveListDTO;
using REA.AdvertSystem.Application.Common.Exceptions;
using REA.AdvertSystem.Application.Common.Interfaces;
using REA.AdvertSystem.Domain.Entities;

namespace REA.AdvertSystem.Application.SaveLists.Queries
{
    public record GetSaveListbyUser : IRequest<List<SaveListResponse>>
    {
        public string Id { get; set; }
    }

    public class GetAdvertPhotoListHandler : IRequestHandler<GetSaveListbyUser, List<SaveListResponse>>
    {
        private IMongoCollection<SaveList> SaveList { get; }

        private IMapper Mapper { get; }

        public GetAdvertPhotoListHandler(IAgencyDbConnection context, IMapper mapper)
        {
            SaveList = context.ConnectToMongo<SaveList>("SaveList");
            Mapper = mapper;
        }

        public async Task<List<SaveListResponse>> Handle(GetSaveListbyUser query, CancellationToken cancellationToken)
        {
            var result = await SaveList.Find(x => x.UserID == query.Id).ToListAsync();

            if (result.Count == 0) throw new NotFoundException("SaveList", query.Id);

            return Mapper.Map<List<SaveList>, List<SaveListResponse>>(result);
        }
    }
}
