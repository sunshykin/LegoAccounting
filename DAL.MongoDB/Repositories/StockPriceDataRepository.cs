using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Integration.BrickLink.Services;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Repositories
{
	public class StockPriceDataRepository : RepositoryBase<StockPriceData>, IStockPriceDataRepository
	{
		private readonly IBrickLinkDataConnector brickLinkDataConnector;

		public StockPriceDataRepository(MongoContext mongoContext, IBrickLinkDataConnector brickLinkDataConnector) : base(mongoContext)
		{
			this.brickLinkDataConnector = brickLinkDataConnector;
		}
		
		public async Task<List<StockPriceData>> GetPartPrices(Part part, StateType state)
		{
			var toDate = DateTime.Today.AddDays(-3);

			return await Collection
				.Find(data => data.PartId.Equals(part.Id) && data.State.Equals(state) && data.DateCollected > toDate)
				.ToListAsync();
		}

		public async Task<List<StockPriceData>> UpdateAndGetPartPrices(Part part, StateType state)
		{
			var priceData = await GetPartPrices(part, state);

			if (priceData.Count == 0)
			{
				priceData = await brickLinkDataConnector.GetPartStockPrices(part, state);

				await InsertMany(priceData);
			}

			return priceData;
		}
	}
}