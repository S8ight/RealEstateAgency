using System.Data;
using REA.ChatSystem.DAL.Interfaces;

namespace REA.ChatSystem.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbTransaction _dbTransaction;

        public IChatRepository ChatRepository { get; set; }

        public IMessageRepository MessageRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public UnitOfWork(
            IDbTransaction dbTransaction,
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IUserRepository userRepository)
        {
            _dbTransaction = dbTransaction;
            ChatRepository = chatRepository;
            MessageRepository = messageRepository;
            UserRepository = userRepository;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _dbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
