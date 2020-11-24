using Newtonsoft.Json;

namespace LegoAccounting.Integration.BrickLink.Models
{
	public class ResponseMeta
	{
		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("code")]
		public long Code { get; set; }
	}
}