using System.Collections.Generic;
using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Domain.Interfaces;
using LegoAccounting.Integration.BrickLink.Models;

namespace LegoAccounting.Integration.BrickLink.Services
{
	public interface IBrickLinkDataConnector : IDataConnector
	{
		public Task UpdatePartsOfSet(Set set);
		public Task UpdatePartPrices(Part part, PriceFormationType priceFormation);

		public Task<Set> GetSet(string number);

		public Task<Entry[]> GetSetParts(string number);

		public Task<Part> GetPart(string number, Color color);

		public Task<List<StockPriceData>> GetPartStockPrices(Part part, StateType state);

		public Task<List<OrderPriceData>> GetPartOrderPrices(Part part, StateType state);
	}
}