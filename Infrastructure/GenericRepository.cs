using Dapper;
using Domain;
using Microsoft.Extensions.Caching.Memory; // Add this namespace
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure
{
    public class GenericRepository<TEntity> : InterfaceGeneric<TEntity>
    {
        private readonly string connectionString;
        private readonly IMemoryCache _cache;

        // Add IMemoryCache to the constructor 
        public GenericRepository(IConfiguration configuration, IMemoryCache cache)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _cache = cache;
        }

        public void Add(TEntity entity)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(GetInsertQuery(), entity);

                // Clear the cache when an entity is added
                _cache.Remove(GetCacheKey());
            }
        }

        public bool Delete(TEntity entity)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int rowsDeleted = connection.Execute(GetDeleteQuery(), entity);

                if (rowsDeleted > 0)
                {
                    // Clear the cache when an entity is deleted
                    _cache.Remove(GetCacheKey());
                }

                return rowsDeleted > 0;
            }
        }

        public bool Update(TEntity entity)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int rowsAffected = connection.Execute(GetUpdateQuery(), entity);

                if (rowsAffected > 0)
                {
                    // Clear the cache when an entity is updated
                    _cache.Remove(GetCacheKey());
                }

                return rowsAffected > 0;
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            // Try to get the data from cache
            if (!_cache.TryGetValue(GetCacheKey(), out IEnumerable<TEntity> entities))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    entities = connection.Query<TEntity>($"SELECT * FROM {typeof(TEntity).Name}").ToList();

                    // Set cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Adjust cache duration as needed
                    };

                    // Save data in cache
                    _cache.Set(GetCacheKey(), entities, cacheEntryOptions);
                }
            }

            return entities;
        }

        private string GetInsertQuery()
        {
            var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id" && p.Name != "Image");
            var columns = string.Join(",", properties.Select(p => p.Name));
            var parameters = string.Join(",", properties.Select(p => "@" + p.Name));
            return $"INSERT INTO {typeof(TEntity).Name} ({columns}) VALUES ({parameters})";
        }

        private string GetDeleteQuery()
        {
            return $"DELETE FROM {typeof(TEntity).Name} WHERE Id = @Id";
        }

        private string GetUpdateQuery()
        {
            var properties = typeof(TEntity).GetProperties().Where(x => x.Name != "Id" && x.Name != "Image");
            var setClause = string.Join(",", properties.Select(a => $"{a.Name}=@{a.Name}"));
            return $"UPDATE {typeof(TEntity).Name} SET {setClause} WHERE Id=@Id";
        }

        // Generate a cache key based on the entity type
        private string GetCacheKey()
        {
            return $"EntityCache_{typeof(TEntity).Name}";
        }
    }
}

        //public class GenericRepository <TEntity>:Interface1 <TEntity>
        //{
        //    private readonly string connectionString;
        //    public GenericRepository(string connectionString)
        //    {
        //        this.connectionString = connectionString;
        //    }
        //    public void Add(TEntity entity)
        //    {
        //        //get table
        //        var tableName = typeof(TEntity).Name;
        //        //get column names 
        //        var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id" && p.Name!="Image"); // sari lines get ho jae lekin id get na ho

        //        // now get prop in term of , separated 

        //        var columnName = string.Join(",", properties.Select(p => p.Name));

        //        // make paramters
        //        var parameterNames = string.Join(",", properties.Select(y => "@" + y.Name));

        //        // now make query 

        //        var query = $"insert into {tableName} ({columnName}) values ({parameterNames})";

        //        using (var connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            var sqlCommand = new SqlCommand(query, connection);
        //            foreach (var property in properties)
        //            {
        //                sqlCommand.Parameters.AddWithValue("@" + property.Name, property.GetValue(entity));
        //            }
        //            sqlCommand.ExecuteNonQuery();
        //        }
        //    }
        //    public bool Delete(TEntity entity)
        //    {
        //        // Get table name
        //        var tableName = typeof(TEntity).Name;
        //        var property = typeof(TEntity).GetProperty("Id");
        //        var query = $"DELETE FROM {tableName} WHERE Id = @Id";

        //        using (var connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            var sqlCommand = new SqlCommand(query, connection);
        //            sqlCommand.Parameters.AddWithValue("@" + property.Name, property.GetValue(entity));

        //            int rowsDeleted=sqlCommand.ExecuteNonQuery();
        //            if(rowsDeleted > 0)
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //    public bool Update(TEntity entity)
        //    {
        //        // Get table name
        //        var tableName = typeof(TEntity).Name;
        //        var primaryKey = "Id";

        //        // Get column names excluding the primary key
        //        var properties = typeof(TEntity).GetProperties().Where(x => x.Name != primaryKey && x.Name != "Image");

        //        // Create the SET clause
        //        var setClause = string.Join(",", properties.Select(a => $"{a.Name}=@{a.Name}"));

        //        // Construct the query
        //        var query = $"UPDATE {tableName} SET {setClause} WHERE {primaryKey}=@{primaryKey}";

        //        using (var connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            var sqlCommand = new SqlCommand(query, connection);

        //            // Add parameters for all properties except the primary key
        //            foreach (var property in properties)
        //            {
        //                var propertyName = property.Name;
        //                var propertyValue = property.GetValue(entity) ?? DBNull.Value;

        //                // Check if the property is being correctly added
        //                sqlCommand.Parameters.AddWithValue("@" + propertyName, propertyValue);
        //            }

        //            // Add parameter for the primary key
        //            var primaryKeyValue = typeof(TEntity).GetProperty(primaryKey).GetValue(entity);
        //            sqlCommand.Parameters.AddWithValue("@" + primaryKey, primaryKeyValue);

        //            // Debugging output
        //            //Debug.WriteLine("SQL Command Text: " + sqlCommand.CommandText);
        //            foreach (SqlParameter param in sqlCommand.Parameters)
        //            {
        //                Debug.WriteLine($"Parameter: {param.ParameterName}, Value: {param.Value}");
        //            }

        //            // Execute the query
        //            int rowsAffected = sqlCommand.ExecuteNonQuery();
        //            if (rowsAffected > 0)
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //    public IEnumerable<TEntity> GetAll( )
        //    {
        //        var tableName = typeof(TEntity).Name;
        //        var query = $"SELECT * FROM {tableName}";

        //        var entities = new List<TEntity>();

        //        using (var connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            var sqlCommand = new SqlCommand(query, connection);

        //            using (var reader = sqlCommand.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    var entity = Activator.CreateInstance<TEntity>();

        //                    foreach (var property in typeof(TEntity).GetProperties().Where( p=>p.Name!="Image"))// here exclude the properties agiant which there is no column in database
        //                    {
        //                        var value = reader[property.Name];// == DBNull.Value ? null : reader[property.Name];
        //                        property.SetValue(entity, value);
        //                    }

        //                    entities.Add(entity);
        //                }
        //            }
        //        }
        //        return entities;
        //    }
    