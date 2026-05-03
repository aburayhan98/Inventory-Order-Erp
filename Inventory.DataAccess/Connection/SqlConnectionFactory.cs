using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Inventory.DataAccess.Connection;

public class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
	private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

	public IDbConnection CreateConnection()
	{
		return new SqlConnection(_connectionString);
	}
}