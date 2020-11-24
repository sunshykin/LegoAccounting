using MongoDB.Bson;

namespace LegoAccounting.Domain.Entities
{
	public class PartOfSet : IEntity
	{
		public ObjectId Id { get; set; }

		public ObjectId SetId { get; set; }

		public ObjectId PartId { get; set; }

		public int Quantity { get; set; }

		public bool IsCounterpart { get; set; }

		public bool IsAlternate { get; set; }
	}
}