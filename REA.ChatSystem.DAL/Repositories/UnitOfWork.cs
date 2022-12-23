using System.Data;
using REA.ChatSystem.DAL.Interfaces;

namespace REA.ChatSystem.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbTransaction DbTransaction;

        public IChatRepository ChatRepository { get; set; }

        public IMessageRepository MessageRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public UnitOfWork(
            IDbTransaction dbTransaction,
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IUserRepository userRepository)
        {
            DbTransaction = dbTransaction;
            ChatRepository = chatRepository;
            MessageRepository = messageRepository;
            UserRepository = userRepository;
        }

        public void Commit()
        {
            try
            {
                DbTransaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            DbTransaction.Connection?.Close();
            DbTransaction.Connection?.Dispose();
            DbTransaction.Dispose();
        }
    }
}
