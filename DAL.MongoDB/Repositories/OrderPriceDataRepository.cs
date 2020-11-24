using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Repositories
{
	public class OrderPriceDataRepository : RepositoryBase<OrderPriceData>, IOrderPriceDataRepository
	{
		public OrderPriceDataRepository(MongoContext mongoContext) : base(mongoContext)
		{
		}

		public async Task<List<OrderPriceData>> GetPartPrices(Part part, StateType state)
		{
			return await Collection
				.Find(data => data.PartId.Equals(part.Id) && data.State.Equals(state))
				.ToListAsync();
		}

		public Task<List<OrderPriceData>> UpdateAndGetPartPrices(Part part, StateType state)
		{
			throw new System.NotImplementedException();
		}
	}
}