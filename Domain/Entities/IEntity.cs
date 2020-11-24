using MongoDB.Bson;

namespace LegoAccounting.Domain.Entities
{
	public interface IEntity
	{
		public ObjectId Id { get; set; }
	}
}