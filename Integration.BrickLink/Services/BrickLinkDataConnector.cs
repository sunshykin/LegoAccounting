using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LegoAccounting.Domain.Configuration;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Domain.Extensions;
using LegoAccounting.Domain.SubEntities;
using LegoAccounting.Integration.BrickLink.Enums;
using LegoAccounting.Integration.BrickLink.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace LegoAccounting.Integration.BrickLink.Services
{
	public class BrickLinkDataConnector : IBrickLinkDataConnector
	{
		private readonly RestClient client;


		public BrickLinkDataConnector(
			IOptions<BrickLinkAuthConfiguration> settings
		)
		{

			client = new RestClient("https://api.bricklink.com/api/store/v1/")
			{
				Authenticator = OAuth1Authenticator.ForAccessToken(
					settings.Value.ConsumerKey,
					settings.Value.ConsumerSecret,
					settings.Value.Token,
					settings.Value.TokenSecret
				)
			};
		}
		
		public async Task UpdatePartsOfSet(Set set)
		{
			throw new NotImplementedException();
			/*
			var request = new RestRequest($"items/set/{set.Number}-1/subsets", Method.GET);
			var response = await client.ExecuteAsync(request);

			if (!response.IsSuccessful)
				throw new Exception("Something wrong with BrickLink Part Price request");

			var json = JsonConvert.DeserializeObject<BrickLinkResponse<ItemsData[]>>(response.Content);

			var entries = json.Data
				.SelectMany(data => data.Entries)
				.ToArray();

			var setParts = await partOfSetRepository.GetSetParts(set);

			foreach (var entry in entries)
			{
				if (entry.Item.Type != ItemType.Part)
				{
					continue;
				}

				var part = await partRepository.GetByNumberAndColor(entry.Item.Number, entry.Color);

				// ToDo: replace this part with request to BrickLink someday
				if (part == null)
				{
					part = new Part
					{
						Name = entry.Item.Name,
						Number = entry.Item.Number,
						Color = entry.Color
					};

					await partRepository.Save(part);
				}

				var partOfSet = setParts.FirstOrDefault(p => p.PartId == part.Id && p.SetId == set.Id);

				if (partOfSet == null || partOfSet.Quantity != entry.Quantity)
				{
					await partOfSetRepository.Save(new PartOfSet
					{
						PartId = part.Id,
						SetId = set.Id,
						Quantity = entry.Quantity,
						IsCounterpart = entry.IsCounterpart,
						IsAlternate = entry.IsAlternate
					});
				}
			}*/
		}

		public async Task UpdatePartPrices(Part part, PriceFormationType priceFormation)
		{
			throw new NotImplementedException();
			/*
			// ToDo: think about deleting old data

			var guide = BrickLinkUtils.GetPriceGuide(priceFormation);


			switch (priceFormation)
			{
				case PriceFormationType.AverageRecentlySoldLimited:

					var soldUsedData = await ExecutePartPriceRequest<PriceSoldDetail>(part, StateType.Used, guide);
					var soldNewData = await ExecutePartPriceRequest<PriceSoldDetail>(part, StateType.New, guide);

					var usedOrderPriceData = soldUsedData.Data.PriceDetail
						.OrderByDescending(data => data.DateOrdered)
						.Take(10)
						.Select(data => new OrderPriceData
						{
							PartId = part.Id,
							State = StateType.Used,
							Quantity = data.Quantity,
							Price = new Price
							{
								Currency = soldUsedData.Data.Currency,
								Value = Math.Round(data.UnitPrice, 4)
							},
							Date = data.DateOrdered
						});
					var newOrderPriceData = soldNewData.Data.PriceDetail
						.OrderByDescending(data => data.DateOrdered)
						.Take(10)
						.Select(data => new OrderPriceData
						{
							PartId = part.Id,
							State = StateType.New,
							Quantity = data.Quantity,
							Price = new Price
							{
								Currency = soldNewData.Data.Currency,
								Value = Math.Round(data.UnitPrice, 4)
							},
							Date = data.DateOrdered
						});

					await orderPriceDataRepository.InsertMany(usedOrderPriceData.Union(newOrderPriceData));

					break;

				case PriceFormationType.AverageInStockLimited:

					var stockUsedData = await ExecutePartPriceRequest<PriceStockDetail>(part, StateType.Used, guide);
					var stockNewData = await ExecutePartPriceRequest<PriceStockDetail>(part, StateType.New, guide);

					var usedStockPriceData = stockUsedData.Data.PriceDetail
						.OrderBy(data => data.UnitPrice)
						.Take(10)
						.Select(data => new StockPriceData
						{
							PartId = part.Id,
							State = StateType.Used,
							Quantity = data.Quantity,
							Price = new Price
							{
								Currency = stockUsedData.Data.Currency,
								Value = Math.Round(data.UnitPrice, 4)
							},
							DateCollected = DateTime.Today
						});
					var newStockPriceData = stockNewData.Data.PriceDetail
						.OrderBy(data => data.UnitPrice)
						.Take(10)
						.Select(data => new StockPriceData
						{
							PartId = part.Id,
							State = StateType.New,
							Quantity = data.Quantity,
							Price = new Price
							{
								Currency = stockNewData.Data.Currency,
								Value = Math.Round(data.UnitPrice, 4)
							},
							DateCollected = DateTime.Today
						});


					await stockPriceDataRepository.InsertMany(usedStockPriceData.Union(newStockPriceData));

					break;

				default:
					throw new NotImplementedException(priceFormation.ToString());
			}*/
		}

		public async Task<Set> GetSet(string number)
		{
			var response = await ExecuteItemRequest($"{number}-1", ItemType.Set);

			return new Set
			{
				Number = number,
				Name = response.Data.Name
			};
		}

		public async Task<Entry[]> GetSetParts(string number)
		{
			var response = await ExecuteItemSubsetsRequest($"{number}-1", ItemType.Set);

			return response.Data.SelectMany(data => data.Entries).ToArray();
			
			/*foreach (var entry in response.Data.Entries)
			{
				var part = await partRepository.GetByNumberAndColor(entry.Item.Number, entry.Color);

				if (part == null)
				{
					part = await GetPart(entry.Item.Number, entry.Color);

					await partRepository.Save(part);
				}
			}*/
		}

		public async Task<Part> GetPart(string number, Color color)
		{
			var response = await ExecuteItemRequest(number, ItemType.Part);

			return new Part
			{
				Number = response.Data.Number,
				Name = response.Data.Name,
				Color = color
			};
		}

		public async Task<List<StockPriceData>> GetPartStockPrices(Part part, StateType state)
		{
			var response = await ExecutePartPriceRequest<PriceStockDetail>(part, state, PriceGuideType.StockItems);

			return response.Data.PriceDetail
				.OrderBy(data => data.UnitPrice)
				.Take(10)
				.Select(data => new StockPriceData
				{
					PartId = part.Id,
					State = state,
					Quantity = data.Quantity,
					Price = new Price
					{
						Currency = response.Data.Currency,
						Value = Math.Round(data.UnitPrice, 4)
					},
					DateCollected = DateTime.Today
				})
				.ToList();
		}

		public async Task<List<OrderPriceData>> GetPartOrderPrices(Part part, StateType state)
		{
			var response = await ExecutePartPriceRequest<PriceSoldDetail>(part, state, PriceGuideType.SoldItems);

			return response.Data.PriceDetail
				.OrderByDescending(data => data.DateOrdered)
				.Take(10)
				.Select(data => new OrderPriceData
				{
					PartId = part.Id,
					State = StateType.Used,
					Quantity = data.Quantity,
					Price = new Price
					{
						Currency = response.Data.Currency,
						Value = Math.Round(data.UnitPrice, 4)
					},
					Date = data.DateOrdered
				})
				.ToList();
		}

		private async Task<BrickLinkResponse<Item>> ExecuteItemRequest(string number, ItemType type)
		{
			var request = new RestRequest($"items/{type.GetEnumMemberValue().ToLower()}/{number}");

			var response = await client.ExecuteAsync(request);

			if (!response.IsSuccessful)
				throw new Exception("Something wrong with BrickLink Item request");

			return JsonConvert.DeserializeObject<BrickLinkResponse<Item>>(response.Content);
		}

		private async Task<BrickLinkResponse<ItemsData[]>> ExecuteItemSubsetsRequest(string number, ItemType type)
		{
			var request = new RestRequest($"items/{type.GetEnumMemberValue()}/{number}/subsets");

			var response = await client.ExecuteAsync(request);

			if (!response.IsSuccessful)
				throw new Exception("Something wrong with BrickLink Item Subsets request");

			return JsonConvert.DeserializeObject<BrickLinkResponse<ItemsData[]>>(response.Content);
		}

		private async Task<BrickLinkResponse<PriceData<T>>> ExecutePartPriceRequest<T>(
			Part part,
			StateType state,
			PriceGuideType priceGuide
		) where T : IPriceData
		{
			var request = new RestRequest($"items/part/{part.Number}/price");

			request.AddQueryParameter("new_or_used", state.GetEnumMemberValue());
			request.AddQueryParameter("guide_type", priceGuide.GetEnumMemberValue());
			request.AddQueryParameter("color_id", ((int)part.Color).ToString());

			var response = await client.ExecuteAsync(request);

			if (!response.IsSuccessful)
				throw new Exception("Something wrong with BrickLink Part Price request");

			return JsonConvert.DeserializeObject<BrickLinkResponse<PriceData<T>>>(response.Content);
		}
	}
}