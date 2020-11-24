using LegoAccounting.Domain.Enums;
using Newtonsoft.Json;

namespace LegoAccounting.Integration.BrickLink.Models
{
	public class Entry
	{
		[JsonProperty("item")]
		public Item Item { get; set; }

		[JsonProperty("color_id")]
		public Color Color { get; set; }

		[JsonProperty("quantity")]
		public int Quantity { get; set; }

		[JsonProperty("extra_quantity")]
		public long ExtraQuantity { get; set; }

		[JsonProperty("is_alternate")]
		public bool IsAlternate { get; set; }

		[JsonProperty("is_counterpart")]
		public bool IsCounterpart { get; set; }
	}
}