using System.Runtime.Serialization;

namespace LegoAccounting.Domain.Enums
{
	public enum PriceFormationType
	{
		/// <summary>
		/// Uses average price of last 10 sales of item
		/// </summary>
		[EnumMember(Value = "avg_sold")]
		AverageRecentlySoldLimited = 0,

		/// <summary>
		/// Uses average price of 10 lowest price items in stock
		/// </summary>
		[EnumMember(Value = "avg_stock")]
		AverageInStockLimited = 1
	}
}