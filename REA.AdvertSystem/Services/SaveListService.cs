using AutoMapper;
using Microsoft.EntityFrameworkCore;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.DTOs.Response;
using REA.AdvertSystem.Interfaces.Repositories;
using REA.AdvertSystem.Interfaces.Services;

namespace REA.AdvertSystem.Services;

public class SaveListService : ISaveListService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SaveListService> _logger;
    private readonly IMapper _mapper;

    public SaveListService(IUnitOfWork unitOfWork, ILogger<SaveListService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<PaginationResponse<AdvertsListResponse>> GetUserSaveList(UserSaveListRequest request)
    {
        var items = _unitOfWork.SaveListRepository.GetUserSaveList(request.UserId);
        var totalItems = await items.CountAsync();
        var adverts = await items.Select(sl => sl.Advert).ToListAsync();
        var result = new PaginationResponse<AdvertsListResponse>
        {
            PagesCount = (int)Math.Ceiling((double)totalItems / request.PageSize),
            Items = _mapper.Map<IEnumerable<AdvertsListResponse>>(adverts)
        };

        return result;
    }
    
    public async Task AddSaveListAsync(SaveListRequest request)
    {
        var saveList = _mapper.Map<SaveList>(request);
        await _unitOfWork.SaveListRepository.AddSaveListAsync(saveList);
    }
    
    public async Task DeleteSaveListAsync(string userId, string advertId)
    {
        await _unitOfWork.SaveListRepository.DeleteSaveListAsync(userId, advertId);
    }
}