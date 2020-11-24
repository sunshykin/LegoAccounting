using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Context
{
	public partial class MongoContext
	{
		private readonly IMongoDatabase mongoDatabase;

		public MongoContext(IMongoDatabase mongoDatabase)
		{
			this.mongoDatabase = mongoDatabase;
		}

		public IMongoCollection<TEntity> GetCollection<TEntity>()
		{
			return mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name);
		}
	}
}