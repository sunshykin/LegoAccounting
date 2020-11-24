using LegoAccounting.Domain.Entities;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Context
{
	public partial class MongoContext
	{
		private IMongoCollection<PartOfCollectionItem> partsOfCollectionItem;

		public IMongoCollection<PartOfCollectionItem> PartsOfCollectionItem => partsOfCollectionItem ??= GetCollection<PartOfCollectionItem>();
	}
}