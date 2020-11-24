using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Repositories
{
	public class CollectionItemRepository : RepositoryBase<CollectionItem>, ICollectionItemRepository
	{
		public CollectionItemRepository(MongoContext mongoContext) : base(mongoContext)
		{
		}

		public async Task<IEnumerable<CollectionItem>> GetAllItems()
		{
			return await Collection.AsQueryable().ToListAsync();
		}
	}
}