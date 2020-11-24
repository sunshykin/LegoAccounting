using System.Runtime.Serialization;

namespace LegoAccounting.Integration.BrickLink.Enums
{
	public enum PriceGuideType
	{
		/// <summary>
		/// Last 6 Month Sales
		/// </summary>
		[EnumMember(Value = "sold")]
		SoldItems,

		/// <summary>
		/// Current Items For Sale
		/// </summary>
		[EnumMember(Value = "stock")]
		StockItems
	}
}