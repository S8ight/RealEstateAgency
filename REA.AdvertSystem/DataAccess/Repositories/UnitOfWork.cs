using REA.AdvertSystem.Interfaces.Repositories;

namespace REA.AdvertSystem.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IAdvertRepository AdvertRepository { get; }
    public IPhotoListRepository PhotoListRepository { get; }
    public ISaveListRepository SaveListRepository { get; }
    public IUserRepository UserRepository { get; }
    
    public UnitOfWork(IAdvertRepository advertRepository, IPhotoListRepository photoListRepository, ISaveListRepository saveListRepository, IUserRepository userRepository)
    {
        AdvertRepository = advertRepository;
        PhotoListRepository = photoListRepository;
        SaveListRepository = saveListRepository;
        UserRepository = userRepository;
    }
}