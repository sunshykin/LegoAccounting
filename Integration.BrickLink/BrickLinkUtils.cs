using System;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Integration.BrickLink.Enums;

namespace LegoAccounting.Integration.BrickLink
{
	public static class BrickLinkUtils
	{
		public static PriceGuideType GetPriceGuide(PriceFormationType type)
		{
			switch (type)
			{
				case PriceFormationType.AverageRecentlySoldLimited:
					return PriceGuideType.SoldItems;

				case PriceFormationType.AverageInStockLimited:
					return PriceGuideType.StockItems;

				default:
					throw new NotImplementedException();
			}
		}
	}
}
