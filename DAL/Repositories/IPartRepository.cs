using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;

namespace LegoAccounting.DAL.Repositories
{
	public interface IPartRepository : IEntityRepository<Part>
	{
		public Task<Part> GetByNumberAndColor(string number, Color color);
		public Task<Part> UpdateAndGetByNumberAndColor(string number, Color color);
	}
}