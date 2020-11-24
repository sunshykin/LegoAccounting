using System;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Domain.SubEntities;
using MongoDB.Bson;

namespace LegoAccounting.Domain.Entities
{
	public class StockPriceData : IEntity
	{
		public ObjectId Id { get; set; }

		public ObjectId PartId { get; set; }

		public StateType State { get; set; }

		public Price Price { get; set; }

		public int Quantity { get; set; }

		public DateTime DateCollected { get; set; }
		
		//public bool ShippingAvailable { get; set; }
	}
}