using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using REA.ChatSystem.DAL.Interfaces;

namespace REA.ChatSystem.DAL.Context;

public class DatabaseConnectionFactory : IDatabaseConnectionFactory
{
    
    private IDbConnection _connection;

    public DatabaseConnectionFactory( IDbConnection connection)
    {
        _connection = connection;
    }
    public IDbConnection GetConnection()
    {
        return _connection;
    }
}