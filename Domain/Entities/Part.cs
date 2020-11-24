using LegoAccounting.Domain.Enums;
using MongoDB.Bson;

namespace LegoAccounting.Domain.Entities
{
	public class Part : IEntity
	{
		private const string StickerCode = "stk";

		public bool IsSticker => Number.Contains(StickerCode);

		public ObjectId Id { get; set; }

		public string Number { get; set; }

		public Color Color { get; set; }

		public string Name { get; set; }

		public string ImageUrl => $"//img.bricklink.com/ItemImage/PN/{(int) Color}/{Number}.png";
	}
}