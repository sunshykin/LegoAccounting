using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;

namespace LegoAccounting.DAL.Repositories
{
	public interface IStockPriceDataRepository : IEntityRepository<StockPriceData>
	{
		public Task<List<StockPriceData>> GetPartPrices(Part part, StateType state);

		public Task<List<StockPriceData>> UpdateAndGetPartPrices(Part part, StateType state);
	}
}