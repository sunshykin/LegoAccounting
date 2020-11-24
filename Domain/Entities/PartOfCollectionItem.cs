using LegoAccounting.Domain.Enums;
using MongoDB.Bson;

namespace LegoAccounting.Domain.Entities
{
	public class PartOfCollectionItem : IEntity
	{
		public ObjectId Id { get; set; }

		public ObjectId CollectionItemId { get; set; }

		public ObjectId PartId { get; set; }

		public int Quantity { get; set; }

		public ConditionType Condition { get; set; }

		public StateType State { get; set; }
	}
}