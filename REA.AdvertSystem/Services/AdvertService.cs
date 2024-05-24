using AutoMapper;
using Gridify;
using Microsoft.EntityFrameworkCore;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.DataProcessing.Specifications;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.DTOs.Response;
using REA.AdvertSystem.Interfaces.Repositories;
using REA.AdvertSystem.Interfaces.Services;

namespace REA.AdvertSystem.Services;

public class AdvertService : IAdvertService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdvertService> _logger;
    private readonly IMapper _mapper;

    public AdvertService(IUnitOfWork unitOfWork, ILogger<AdvertService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<PaginationResponse<AdvertsListResponse>> GetFilteredAdvertsAsync(AdvertsFilterRequest request)
    {
        var specification = new AdvertSpecification(request);
        var items = await _unitOfWork.AdvertRepository.ListAsync(specification);
        var totalItems = await _unitOfWork.AdvertRepository.CountAsync(specification);

        var result = new PaginationResponse<AdvertsListResponse>
        {
            PagesCount = (int)Math.Ceiling((double)totalItems / request.PageSize),
            Items = _mapper.Map<IEnumerable<AdvertsListResponse>>(items)
        };

        return result;
    }
    
    public async Task<PaginationResponse<AdvertsListResponse>> GetUserAdvertsAsync(UserSaveListRequest request)
    {
        var adversQuery = _unitOfWork.AdvertRepository.GetUserAdverts(request.UserId);

        var totalAlbumsCount = await adversQuery.CountAsync();

        if (totalAlbumsCount <= 0)
        {
            return new PaginationResponse<AdvertsListResponse>();
        }
            
        var paginatedAlbums = await adversQuery.ApplyPaging(request.PageNumber, request.PageSize).ToListAsync();
            
        var listResponse = _mapper.Map<List<Advert>, List<AdvertsListResponse>>(paginatedAlbums);

        return new PaginationResponse<AdvertsListResponse>
        {
            PagesCount = (int)Math.Ceiling((double)totalAlbumsCount / request.PageSize),
            Items = listResponse
        };
    }
    
    public async Task<PaginationResponse<AdvertsListResponse>> SearchAdvertsAsync(SearchAdvertsRequest request)
    {
        var specification = new AdvertSearchSpecification(request.Keywords, request.PageNumber, request.PageSize);
        var items = await _unitOfWork.AdvertRepository.ListAsync(specification);
        var totalItems = await _unitOfWork.AdvertRepository.CountAsync(specification);

        return new PaginationResponse<AdvertsListResponse>
        {
            PagesCount = (int)Math.Ceiling((double)totalItems / request.PageSize),
            Items = _mapper.Map<IEnumerable<AdvertsListResponse>>(items)
        };
    }


    public async Task<AdvertResponse> GetAdvertByIdAsync(string advertId)
    {
        var advert = await _unitOfWork.AdvertRepository.GetAvertByIdAsync(advertId);
        var response = _mapper.Map<AdvertResponse>(advert);

        return response;
    }
    
    public async Task AddAdvertAsync(AdvertRequest request)
    {
        var advert = _mapper.Map<Advert>(request);
        await _unitOfWork.AdvertRepository.AddAdvertAsync(advert);
    }
    
    public async Task UpdateAdvertAsync(string id, AdvertRequest request)
    {
        var existingAdvert = await _unitOfWork.AdvertRepository.GetAvertByIdAsync(id);
            
        if (existingAdvert == null)
        {
            throw new KeyNotFoundException($"Advert with Id: {id} not found.");
        }
            
        _mapper.Map(request, existingAdvert);
        await _unitOfWork.AdvertRepository.UpdateAdvertAsync(existingAdvert);
    }
    
    public async Task DeleteAdvertAsync(string id)
    {
        await _unitOfWork.AdvertRepository.DeleteAdvertAsync(id);
    }
}