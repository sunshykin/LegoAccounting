using Newtonsoft.Json;

namespace LegoAccounting.Integration.BrickLink.Models
{
	public class BrickLinkResponse<T>
	{
		[JsonProperty("meta")]
		public ResponseMeta Meta { get; set; }

		[JsonProperty("data")]
		public T Data { get; set; }
	}
}