using System;
using Newtonsoft.Json;

namespace LegoAccounting.Integration.BrickLink.Models
{
	public class PriceSoldDetail : IPriceData
	{
		[JsonProperty("quantity")]
		public int Quantity { get; set; }

		[JsonProperty("unit_price")]
		public decimal UnitPrice { get; set; }

		[JsonProperty("seller_country_code")]
		public string SellerCountryCode { get; set; }

		[JsonProperty("buyer_country_code")]
		public string BuyerCountryCode { get; set; }

		[JsonProperty("date_ordered")]
		public DateTime DateOrdered { get; set; }
	}
}