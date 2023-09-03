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
                    
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        connection.Close();
                        throw new InvalidOperationException("User was not created successfully.");
                    }
                }
                connection.Close();
            }
            return t.Id;
        }

        public async Task DeleteAsync(string id)
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
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        connection.Close();
                        throw new InvalidOperationException($"User with Id {id} not found for deletion");
                    }
                }
                connection.Close();
            }
        }

        
        public async Task<User> GetAsync(string Id)
        {
            var sql = "SELECT * FROM [User] WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User
                            {
                                Id = Convert.ToString(reader["Id"]),
                                FirstName = Convert.ToString(reader["FirstName"]),
                                LastName = Convert.ToString(reader["LastName"]),
                                Photo = reader["Photo"] as byte[]
                            };
                            connection.Close();
                            return user;
                        }
                        else
                        {
                            connection.Close();
                            throw new InvalidOperationException($"User with Id {Id} not found");
                        }
                    }
                }
            }
        }
        
        
        public async Task<Task> ReplaceAsync(User t)
        {
            {
                var sql = "UPDATE [User] SET FirstName = @FirstName, LastName = @LastName, Photo = @Photo WHERE Id = @Id";
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
                        
                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            connection.Close();
                            throw new InvalidOperationException("User was not updated successfully.");
                        }
                    }
                    connection.Close();
                }
                return Task.CompletedTask;
            }
        }
    }
}
