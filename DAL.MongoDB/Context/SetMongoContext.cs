using LegoAccounting.Domain.Entities;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Context
{
	public partial class MongoContext
	{
		private IMongoCollection<Set> sets;

		public IMongoCollection<Set> Sets => sets ??= GetCollection<Set>();
	}
}