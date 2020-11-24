using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;

namespace LegoAccounting.DAL.Repositories
{
	public interface ICollectionItemRepository : IEntityRepository<CollectionItem>
	{
		public Task<IEnumerable<CollectionItem>> GetAllItems();
	}
}