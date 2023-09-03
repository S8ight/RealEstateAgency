using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using REA.ChatSystem.DAL.Context;
using REA.ChatSystem.DAL.Interfaces;

namespace REA.ChatSystem.DAL.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        private readonly string _tableName = typeof(T).Name;

        protected GenericRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _connectionFactory.GetConnection().QueryAsync<T>($"SELECT * FROM {_tableName}");
        }

        public async Task<T> GetAsync(string id)
        {
            var properties = GenerateListOfProperties(GetProperties);
            var result = await _connectionFactory.GetConnection().QuerySingleOrDefaultAsync<T>(
                $"SELECT * FROM {_tableName} WHERE {properties.First()}=@Id",
                param: new { Id = id });
            if (result == null)
                throw new KeyNotFoundException($"{_tableName} with id [{id}] could not be found.");
            return result;
        }

        public async Task DeleteAsync(string id)
        {
            var properties = GenerateListOfProperties(GetProperties);
            var deleted = await _connectionFactory.GetConnection().ExecuteAsync($"DELETE FROM {_tableName} WHERE {properties.First()}=@Id", 
                param: new { Id = id });
            if (deleted == 0) 
                throw new ArgumentException("Chat with that Id was not found");
        }


        public async Task<string> AddAsync(T model)
        {
            var insertQuery = GenerateInsertQuery();
            var newId = await _connectionFactory.GetConnection().ExecuteScalarAsync<string>(insertQuery,
                param: model);
            return newId;
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = GenerateInsertQuery();
            inserted += await _connectionFactory.GetConnection().ExecuteAsync(query, 
                param: list);
            return inserted;
        }


        public async Task ReplaceAsync(T model)
        {
            var updateQuery = GenerateUpdateQuery();
            await _connectionFactory.GetConnection().ExecuteAsync(updateQuery, 
                param: model);
        }
        
        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }
        
        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(property =>
            {
                if (!property.Equals($"{properties.First()}"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });
            updateQuery.Remove(updateQuery.Length - 1, 1);
            updateQuery.Append($" WHERE {properties.First()}=@{properties.First()}");
            return updateQuery.ToString();
        }
        
        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");
            insertQuery.Append("(");
            var properties = GenerateListOfProperties(GetProperties);
            
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });
            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append((string?)$"@{prop},"); });
            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");
            insertQuery.Append($";");
            return insertQuery.ToString();
        }
    }
}
