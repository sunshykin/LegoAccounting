using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Infrastructure
{
	public interface IMongoDatabaseProvider
	{
		public IMongoDatabase GetDatabase();
	}
}