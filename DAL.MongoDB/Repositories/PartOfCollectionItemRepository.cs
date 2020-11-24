using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Repositories
{
	public class PartOfCollectionItemRepository : RepositoryBase<PartOfCollectionItem>, IPartOfCollectionItemRepository
	{
		public PartOfCollectionItemRepository(MongoContext mongoContext) : base(mongoContext)
		{
		}

		public async Task<List<PartOfCollectionItem>> GetItemParts(ObjectId itemId)
		{
			return await Collection
				.Find(part => part.CollectionItemId.Equals(itemId))
				.ToListAsync();
		}

		public async Task DeleteItemParts(ObjectId itemId)
		{
			await Collection.DeleteManyAsync(part => part.CollectionItemId.Equals(itemId));
		}
	}
}