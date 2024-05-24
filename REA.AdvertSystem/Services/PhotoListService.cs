using AutoMapper;
using REA.AdvertSystem.DataAccess.Entities;
using REA.AdvertSystem.DTOs.Request;
using REA.AdvertSystem.Interfaces.Repositories;
using REA.AdvertSystem.Interfaces.Services;

namespace REA.AdvertSystem.Services;

public class PhotoListService : IPhotoListService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PhotoListService> _logger;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public PhotoListService(IUnitOfWork unitOfWork, ILogger<PhotoListService> logger, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _configuration = configuration;
    }
    
    public async Task UploadImagesAsync(UploadPhotoListRequest request)
    {
        if (request.PhotoList.Any())
        {
            var path = Path.Combine(_configuration["StoredFilesPath"]!, request.AdvertId);
            var filePaths = await UploadImagesToStorage(request.PhotoList, path);
            var images = new List<PhotoList>();

            for (int i = 0; i < request.PhotoList.Count; i++)
            {
                images.Add(new PhotoList
                {
                    AdvertId = request.AdvertId,
                    PhotoUrl = filePaths[i]
                });
            }

            await _unitOfWork.PhotoListRepository.AddRangeOfPhotoListsAsync(images);
        }
    }
    
    public async Task DeletePhotoAsync(string id)
    {

            var photo = await _unitOfWork.PhotoListRepository.GetPhotoListByIdAsync(id);
            DeleteImageFromStorage(photo);
            await _unitOfWork.PhotoListRepository.DeletePhotoListAsync(id);
    }
    
    private async Task<List<string>> UploadImagesToStorage(List<IFormFile> images, string imagesPath)
    {
        List<string> fileNames = new List<string>();
        var allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
        const long maxFileSize = 10 * 1024 * 1024;
        
        Directory.CreateDirectory(Path.Combine("wwwroot",imagesPath));
        
        foreach (var image in images)
        {
            if (image.Length > 0)
            {
                if (image.Length > maxFileSize)
                {
                    throw new ArgumentException($"File size exceeds the limit of 10 MB: {image.FileName}");
                }
                
                var extension = Path.GetExtension(image.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Invalid file extension: {extension}");
                }
                    
                var filePath = Path.Combine(imagesPath, fileName);

                await using var stream = File.Create(Path.Combine("wwwroot", filePath));
                await image.CopyToAsync(stream);

                fileNames.Add(filePath);
            }
        }

        return fileNames;
    }

    public void DeleteImageFromStorage(PhotoList image)
    {
        var folderPath = Path.Combine("wwwroot", _configuration["StoredFilesPath"]!);
        var albumPath = Path.Combine(folderPath, image.Id);
        var imagePath = Path.Combine("wwwroot", image.PhotoUrl);
        
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }
        
        if (Directory.Exists(albumPath) && !Directory.EnumerateFileSystemEntries(albumPath).Any())
        {
            Directory.Delete(albumPath);
        }
    }
}