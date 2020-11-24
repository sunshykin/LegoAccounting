using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;

namespace LegoAccounting.DAL.Repositories
{
	public interface IOrderPriceDataRepository : IEntityRepository<OrderPriceData>
	{
		public Task<List<OrderPriceData>> GetPartPrices(Part part, StateType state);

		public Task<List<OrderPriceData>> UpdateAndGetPartPrices(Part part, StateType state);
	}
}