using System.Data;

namespace Inventory.DataAccess.Connection;

public interface IDbConnectionFactory
{
	IDbConnection CreateConnection();
}