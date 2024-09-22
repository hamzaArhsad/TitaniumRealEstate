using Dapper;
using Domain;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Infrastructure
{
    public class PropertyRepository:InterfaceProperty
    {
        private readonly string connectionString;

        public PropertyRepository(IConfiguration configuration)
        {
            connectionString= configuration.GetConnectionString("DefaultConnection");
        }
        private SqlConnection connection;

        //public List<Property> searchProperty(string propertyLocation)
        //{
        //    List<Property> searchedProperties = new List<Property>();

        //    string selecteQuery = "SELECT * FROM PROPERTY WHERE LOCATION=@LOC";
        //    connection = new SqlConnection(connectionString);
        //    connection.Open();

        //    SqlCommand selectCommand = new SqlCommand(selecteQuery, connection);
        //    selectCommand.Parameters.Add(new SqlParameter("LOC", SqlDbType.NChar, 50) { Value = propertyLocation });

        //    SqlDataReader reader = selectCommand.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        Property property = new Property();
        //        property.Location = reader["Location"].ToString();
        //        property.Type = reader["Type"].ToString();
        //        property.Area = reader["Area"].ToString();
        //        property.UploadDate = (DateTime)reader[4];
        //        property.Path = reader["Path"].ToString();
        //        searchedProperties.Add(property);
        //    }
        //    connection.Close();
        //    return searchedProperties;
        //}
        public List<Property> searchProperty(string propertyLocation)
        {
            List<Property> searchedProperties = new List<Property>();

            string selectQuery = "SELECT * FROM PROPERTY WHERE LOCATION LIKE @LOC";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                selectCommand.Parameters.Add(new SqlParameter("@LOC", SqlDbType.NVarChar, 50) { Value = "%" + propertyLocation + "%" });

                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Property property = new Property();
                    property.Location = reader["Location"].ToString();
                    property.Type = reader["Type"].ToString();
                    property.Area = reader["Area"].ToString();
                    property.UploadDate = (DateTime)reader["UploadDate"]; // Assuming "UploadDate" is the column name
                    property.Path = reader["Path"].ToString();
                    searchedProperties.Add(property);
                }
            }
            return searchedProperties;
        }
        public List<Property> GetPropertiesAddedAfter(DateTime lastCheckedTime)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM PROPERTY WHERE UploadDate > @LastCheckedTime";
                var parameters = new { LastCheckedTime = lastCheckedTime };
                return connection.Query<Property>(selectQuery, parameters).ToList();
            }
        }
    }
}
