using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Inventory.DataAccess.Connection;

public sealed class DbConfig(IConfiguration configuration)
{
	private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
				?? throw new InvalidOperationException("DefaultConnection is missing.");

	public SqlConnection Connection => new(_connectionString);
}