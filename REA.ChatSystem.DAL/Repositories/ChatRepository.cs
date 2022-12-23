using System.Data;
using Microsoft.Data.SqlClient;
using REA.ChatSystem.DAL.Context;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(IDatabaseConnectionFactory dataContext) : base(dataContext)
        {
        }
    }
}
