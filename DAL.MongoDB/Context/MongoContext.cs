using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Context
{
	/// <summary>
	/// Context is partial cause I add new entity using VS template.
	/// It adds a file for entity, repository and context.
	/// </summary>
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