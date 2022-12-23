namespace REA.ChatSystem.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IChatRepository ChatRepository { get; set; }

        IMessageRepository MessageRepository { get; set; }

        IUserRepository UserRepository { get; set; }

        void Commit();

        void Dispose();
    }
}
