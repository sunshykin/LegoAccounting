using Newtonsoft.Json;

namespace LegoAccounting.Integration.BrickLink.Models
{
	public class ItemsData
	{
		[JsonProperty("match_no")]
		public long MatchNo { get; set; }

		[JsonProperty("entries")]
		public Entry[] Entries { get; set; }
	}
}