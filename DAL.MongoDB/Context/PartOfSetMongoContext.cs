using LegoAccounting.Domain.Entities;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Context
{
	public partial class MongoContext
	{
		private IMongoCollection<PartOfSet> partsOfSet;

		public IMongoCollection<PartOfSet> PartsOfSet => partsOfSet ??= GetCollection<PartOfSet>();
	}
}