using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;

namespace LegoAccounting.DAL.Repositories
{
	public interface IPartOfSetRepository : IEntityRepository<PartOfSet>
	{
		public Task<List<PartOfSet>> GetSetParts(Set set);

		public Task<List<PartOfSet>> UpdateAndGetSetParts(Set set);
	}
}