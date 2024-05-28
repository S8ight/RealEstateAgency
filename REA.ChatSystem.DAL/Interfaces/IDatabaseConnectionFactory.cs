using System.Data;

namespace REA.ChatSystem.DAL.Interfaces;

public interface IDatabaseConnectionFactory
{
    IDbConnection GetConnection();
}