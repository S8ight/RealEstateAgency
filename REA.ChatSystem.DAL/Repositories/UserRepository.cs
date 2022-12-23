using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using REA.ChatSystem.DAL.Interfaces;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;
        
        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public async Task<string> AddAsync(User t)
        {
            var sql = "Insert into [User](Id,FirstName,LastName,Photo) VALUES (@Id,@FirstName,@LastName,@Photo) ";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandText = sql;
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Id", t.Id);
                    cmd.Parameters.AddWithValue("@FirstName", t.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", t.LastName);
                    cmd.Parameters.AddWithValue("@Photo", t.Photo);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            return t.Id;
        }
        
        public async Task<Task> DeleteAsync(string id)
        {
            {
                var sql = "DELETE FROM [User] WHERE Id = @Id";
                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandText = sql;
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return Task.CompletedTask;
            }
        }
        
        public async Task<User> GetAsync(string Id)
        {
            User user = new User();
            var sql = "SELECT * FROM [User] WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql,connection))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = Convert.ToString(reader["Id"]);
                            user.FirstName = Convert.ToString(reader["FirstName"]);
                            user.LastName = Convert.ToString(reader["LastName"]);
                            //user.Photo = Convert.ToByte(reader["Photo"]);
                        }
                    }
                }
                connection.Close();
                return user;
            }
        }
        
        
        public async Task<Task> ReplaceAsync(User t)
        {
            {
                var sql = "UPDATE User SET Id = @Id,FirstName = @FirstName,LastName = @LastName,Photo = @Photo";
                using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandText = sql;
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@Id", t.Id);
                        cmd.Parameters.AddWithValue("@Name", t.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", t.LastName);
                        cmd.Parameters.AddWithValue("@Photo", t.Photo);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return Task.CompletedTask;
            }
        }
    }
}
