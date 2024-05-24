namespace REA.AdvertSystem.Interfaces.Repositories;

public interface IUnitOfWork
{
    public IAdvertRepository AdvertRepository { get; }
    public IPhotoListRepository PhotoListRepository { get; }
    public ISaveListRepository SaveListRepository { get; }
    public IUserRepository UserRepository { get; }
}