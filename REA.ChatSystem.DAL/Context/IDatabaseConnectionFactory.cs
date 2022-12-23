using System.Data;

namespace REA.ChatSystem.DAL.Context;

public interface IDatabaseConnectionFactory
{
    IDbConnection GetConnection();
}