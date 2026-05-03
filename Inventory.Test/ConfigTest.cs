using Inventory.DataAccess.Connection;
using Microsoft.Extensions.Configuration;

namespace Inventory.Test;

public class ConfigTest
{
	private readonly IConfiguration _configuration;

	public ConfigTest()
	{
		_configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false)
				.AddUserSecrets<ConfigTest>(optional: true)
				.Build();
	}

	public string DbConnection =>
			_configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string not found.");

	public DbConfig DbConfig => new(_configuration);
}