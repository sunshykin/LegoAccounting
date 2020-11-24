using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;

namespace LegoAccounting.DAL.Repositories
{
	public interface ISetRepository : IEntityRepository<Set>
	{
		public Task<Set> GetByNumber(string number);

		public Task<Set> UpdateAndGetByNumber(string number);
	}
}