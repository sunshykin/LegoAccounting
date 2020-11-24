using System;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Domain.SubEntities;
using MongoDB.Bson;

namespace LegoAccounting.Domain.Entities
{
	public class OrderPriceData : IEntity
	{
		public ObjectId Id { get; set; }

		public ObjectId PartId { get; set; }

		public StateType State { get; set; }

		public Price Price { get; set; }

		public int Quantity { get; set; }

		//public SellerCountryType SellerCountry { get; set; }

		//public BuyerCountryType BuyerCountry { get; set; }
		public DateTime Date { get; set; }
	}
}