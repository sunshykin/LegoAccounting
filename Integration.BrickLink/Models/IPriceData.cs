namespace LegoAccounting.Integration.BrickLink.Models
{
	public interface IPriceData
	{
		int Quantity { get; set; }

		decimal UnitPrice { get; set; }
	}
}