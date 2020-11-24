using LegoAccounting.Domain.Enums;
using Newtonsoft.Json;

namespace LegoAccounting.Integration.BrickLink.Models
{
	public class PriceData<T> where T : IPriceData
	{
		[JsonProperty("item")]
		public Item Item { get; set; }

		[JsonProperty("new_or_used")]
		public string NewOrUsed { get; set; }

		[JsonProperty("currency_code")]
		public Currency Currency { get; set; }

		[JsonProperty("min_price")]
		public string MinPrice { get; set; }

		[JsonProperty("max_price")]
		public string MaxPrice { get; set; }

		[JsonProperty("avg_price")]
		public string AvgPrice { get; set; }

		[JsonProperty("qty_avg_price")]
		public string QtyAvgPrice { get; set; }

		[JsonProperty("unit_quantity")]
		public long UnitQuantity { get; set; }

		[JsonProperty("total_quantity")]
		public long TotalQuantity { get; set; }

		[JsonProperty("price_detail")]
		public T[] PriceDetail { get; set; }
	}
}