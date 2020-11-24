using Newtonsoft.Json;

namespace LegoAccounting.Integration.BrickLink.Models
{
	public class PriceStockDetail : IPriceData
	{
		[JsonProperty("quantity")]
		public int Quantity { get; set; }

		[JsonProperty("unit_price")]
		public decimal UnitPrice { get; set; }

		[JsonProperty("shipping_available")]
		public bool ShippingAvailable { get; set; }
	}
}