using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace REA.ChatSystem.DAL.Context;

public class DatabaseConnectionFactory : IDatabaseConnectionFactory
{
    private IConfiguration _configuration;

    private IDbConnection _connection;

    public DatabaseConnectionFactory(IConfiguration configuration, IDbConnection connection)
    {
        _configuration = configuration;
        _connection = connection;
    }
    public IDbConnection GetConnection()
    {
        return _connection;
    }
}