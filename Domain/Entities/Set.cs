using MongoDB.Bson;

namespace LegoAccounting.Domain.Entities
{
	public class Set : IEntity
	{
		public ObjectId Id { get; set; }

		public string Number { get; set; }

		public string Name { get; set; }
	}
}