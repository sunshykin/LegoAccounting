using LegoAccounting.Domain.Enums;

namespace LegoAccounting.Web.ViewModel
{
	public class CollectionItemPartViewModel
	{
		public string Id { get; set; }

		public string Number { get; set; }

		public string Name { get; set; }

		public string ImageUrl { get; set; }

		public Color Color { get; set; }

		public int Quantity { get; set; }

		public ConditionType Condition { get; set; }
	}
}