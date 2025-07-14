using OldLeaf.Services;

/ OldLeaf.Services / SqlDataServices.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Com.OldLeaf.Shared.Customer;
using Com.OldLeaf.Shared.Person;
using Com.OldLeaf.Shared.Order;
using Com.OldLeaf.Shared.Delivery;

namespace OldLeaf.Services
{
    public class SqlDataServices
    {
        private readonly string _connectionString;

        // Default constructor
        public SqlDataServices()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? "Data Source=localhost;Initial Catalog=OldLeafDB;Integrated Security=true";
        }

        // Constructor with connection string
        public SqlDataServices(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Generic Get method
        public T Get<T>(int id) where T : class
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name;
                string sql = $"SELECT * FROM {tableName} WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapToObject<T>(reader);
                        }
                    }
                }
            }
            return null;
        }

        // Get with custom parameters
        public T Get<T>(int id, string additionalCriteria) where T : class
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name;
                string sql = $"SELECT * FROM {tableName} WHERE Id = @Id AND {additionalCriteria}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapToObject<T>(reader);
                        }
                    }
                }
            }
            return null;
        }

        // Generic Save method
        public void Save<T>(T entity) where T : class
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name;
                // This is a simplified example - in real implementation, you'd use reflection
                // or a proper ORM to generate the SQL
                string sql = $"INSERT INTO {tableName} VALUES (@param1, @param2, @param3)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Add parameters based on entity properties
                    MapObjectToCommand(entity, command);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Save with return value
        public int Save<T>(T entity, bool returnId) where T : class
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name;
                string sql = $"INSERT INTO {tableName} VALUES (@param1, @param2, @param3); SELECT SCOPE_IDENTITY()";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    MapObjectToCommand(entity, command);
                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        // Generic Search method
        public List<T> Search<T>(string criteria) where T : class
        {
            List<T> results = new List<T>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name;
                string sql = $"SELECT * FROM {tableName} WHERE {criteria}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(MapToObject<T>(reader));
                        }
                    }
                }
            }
            return results;
        }

        // Search with parameters
        public List<T> Search<T>(int id, string criteria) where T : class
        {
            List<T> results = new List<T>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name;
                string sql = $"SELECT * FROM {tableName} WHERE Id = @Id AND {criteria}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(MapToObject<T>(reader));
                        }
                    }
                }
            }
            return results;
        }

        // Generic Remove method
        public void Remove<T>(int id) where T : class
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name;
                string sql = $"DELETE FROM {tableName} WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Remove with additional criteria
        public void Remove<T>(int id, string additionalCriteria) where T : class
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name;
                string sql = $"DELETE FROM {tableName} WHERE Id = @Id AND {additionalCriteria}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Helper methods for mapping (simplified - in real implementation use reflection or ORM)
        private T MapToObject<T>(SqlDataReader reader) where T : class
        {
            // This is a simplified mapping - in real implementation, use reflection
            // or a proper ORM like Entity Framework or Dapper
            return Activator.CreateInstance<T>();
        }

        private void MapObjectToCommand<T>(T entity, SqlCommand command) where T : class
        {
            // This is a simplified mapping - in real implementation, use reflection
            // to map entity properties to command parameters
        }
    }
}