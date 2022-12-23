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
        private IMongoCollection<SaveList> _saveList { get; }

        private IMapper _mapper { get; }

        public GetAdvertPhotoListHandler(IAgencyDbConnection context, IMapper mapper)
        {
            _saveList = context.ConnectToMongo<SaveList>("SaveList");
            _mapper = mapper;
        }

        public async Task<List<SaveListResponse>> Handle(GetSaveListbyUser query, CancellationToken cancellationToken)
        {
            var result = await _saveList.Find(x => x.UserID == query.Id).ToListAsync();

            if (result.Count == 0) throw new NotFoundException("SaveList", query.Id);

            return _mapper.Map<List<SaveList>, List<SaveListResponse>>(result);
        }
    }
}
