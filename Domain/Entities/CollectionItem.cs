using System;
using LegoAccounting.Domain.Enums;
using MongoDB.Bson;

namespace LegoAccounting.Domain.Entities
{
	public class CollectionItem : IEntity
	{
		public ObjectId Id { get; set; }

		public ProcessType Type { get; set; }

		public string Number { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public DateTime DateCreated { get; set; }

		//public string DepartureCity
		//public SellingPlatformType Platform
	}
}