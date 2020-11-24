using LegoAccounting.Domain.Entities;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Context
{
	public partial class MongoContext
	{
		private IMongoCollection<CollectionItem> collectionItems;

		public IMongoCollection<CollectionItem> CollectionItems => collectionItems ??= GetCollection<CollectionItem>();
	}
}