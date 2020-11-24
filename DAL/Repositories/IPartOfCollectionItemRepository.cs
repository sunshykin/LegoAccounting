using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;
using MongoDB.Bson;

namespace LegoAccounting.DAL.Repositories
{
	public interface IPartOfCollectionItemRepository : IEntityRepository<PartOfCollectionItem>
	{
		public Task<List<PartOfCollectionItem>> GetItemParts(ObjectId itemId);

		public Task DeleteItemParts(ObjectId itemId);
	}
}