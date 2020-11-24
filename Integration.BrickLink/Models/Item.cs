using LegoAccounting.Integration.BrickLink.Enums;
using Newtonsoft.Json;

namespace LegoAccounting.Integration.BrickLink.Models
{
	public class Item
	{
		[JsonProperty("no")]
		public string Number { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public ItemType Type { get; set; }

		[JsonProperty("category_id")]
		public long CategoryId { get; set; }
	}
}