using LegoAccounting.Domain.Enums;

namespace LegoAccounting.Domain.SubEntities
{
	public class Price
	{
		public Currency Currency { get; set; }

		public decimal Value { get; set; }
	}
}