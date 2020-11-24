using LegoAccounting.Domain.Entities;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Context
{
	public partial class MongoContext
	{
		private IMongoCollection<Part> parts;

		public IMongoCollection<Part> Parts => parts ??= GetCollection<Part>();
	}
}