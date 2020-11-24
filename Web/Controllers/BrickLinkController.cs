using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Integration.BrickLink.Models;
using LegoAccounting.Integration.BrickLink.Services;
using Microsoft.AspNetCore.Mvc;

namespace LegoAccounting.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BrickLinkController : ControllerBase
	{
		private readonly IPartRepository partRepository;
		private readonly ISetRepository setRepository;
		private readonly IPartOfSetRepository partOfSetRepository;
		private readonly IOrderPriceDataRepository orderPriceDataRepository;
		private readonly IStockPriceDataRepository stockPriceDataRepository;
		private readonly IBrickLinkDataConnector brickLinkDataConnector;

		public BrickLinkController(
			IPartRepository partRepository,
			ISetRepository setRepository,
			IPartOfSetRepository partOfSetRepository,
			IOrderPriceDataRepository orderPriceDataRepository,
			IStockPriceDataRepository stockPriceDataRepository,
			IBrickLinkDataConnector brickLinkDataConnector
		)
		{
			this.partRepository = partRepository;
			this.setRepository = setRepository;
			this.partOfSetRepository = partOfSetRepository;
			this.orderPriceDataRepository = orderPriceDataRepository;
			this.stockPriceDataRepository = stockPriceDataRepository;
			this.brickLinkDataConnector = brickLinkDataConnector;
		}


		[HttpGet("multi-set-price")]
		public async Task<IActionResult> GetMultiSetPriceByParts(
			[FromQuery(Name = "sets")] string setNumberString,
			[FromQuery(Name = "state")] StateType stateType,
			[FromQuery(Name = "price")] PriceFormationType priceFormationType
		)
		{
			return Ok();
		}

		[HttpGet("by-parts-price/{setNumber}")]
		public async Task<IActionResult> GetSetPriceByParts(
			[FromRoute] string setNumber,
			[FromQuery(Name = "state")] StateType stateType,
			[FromQuery(Name = "price")] PriceFormationType priceFormationType
		)
		{
			var set = await setRepository.UpdateAndGetByNumber(setNumber);

			#region Process Set Parts

			var timer = new Stopwatch();
			timer.Start();
			var partsOfSet = await partOfSetRepository.UpdateAndGetSetParts(set);
			var parallelPartsTime = timer.Elapsed;
			timer.Stop();


			decimal sum = 0;
			var priceList = new List<(string Number, Color Color, decimal Price)>();


			timer.Restart();
			var selectedParts = partsOfSet.Where(p => !p.IsCounterpart && !p.IsAlternate);
			var parallelTasks = selectedParts.Select(p => ProcessPartPrice(p, stateType, priceFormationType));

			var taskResults = (await Task.WhenAll(parallelTasks))
				.Where(i => i.Item2 != default)
				.ToArray();
			priceList.AddRange(taskResults.Select(r => r.Item2));
			sum = taskResults.Sum(i => i.Item1);

			var parallelPricesTime = timer.Elapsed;
			timer.Stop();

			var temp = 0;

			/*foreach (var setPart in partsOfSet.Where(p => !p.IsCounterpart && !p.IsAlternate))
			{
				var part = await partRepository.Get(setPart.PartId);

				// Skip Used Stickers
				if (part.IsSticker && stateType == StateType.Used)
				{
					continue;
				}

				decimal usedAvg = -1,
					newAvg;

				switch (priceFormationType)
				{
					case PriceFormationType.AverageRecentlySoldLimited:

						newAvg = (await orderPriceDataRepository.UpdateAndGetPartPrices(part, StateType.New))
							.OrderByDescending(d => d.Date)
							.Take(10)
							.Average(d => d.Price.Value);

						if (stateType == StateType.Used)
							usedAvg = (await orderPriceDataRepository.UpdateAndGetPartPrices(part, StateType.Used))
								.OrderByDescending(d => d.Date)
								.Take(10)
								.Average(d => d.Price.Value);

						break;

					case PriceFormationType.AverageInStockLimited:

						newAvg = (await stockPriceDataRepository.UpdateAndGetPartPrices(part, StateType.New))
							.OrderBy(d => d.Price.Value)
							.Take(10)
							.Average(d => d.Price.Value);

						if (stateType == StateType.Used)
							usedAvg = (await stockPriceDataRepository.UpdateAndGetPartPrices(part, StateType.Used))
								.OrderBy(d => d.Price.Value)
								.Take(10)
								.Average(d => d.Price.Value);

						break;

					default:
						throw new NotImplementedException(priceFormationType.ToString());
				}


				if (newAvg >= 0 && usedAvg >= 0)
				{
					var min = Math.Min(newAvg, usedAvg);

					sum += min * setPart.Quantity;
					priceList.Add((part.Number, part.Color, min * setPart.Quantity));
				}
				else if (newAvg >= 0)
				{
					sum += newAvg * setPart.Quantity;
					priceList.Add((part.Number, part.Color, newAvg * setPart.Quantity));
				}
				else if (usedAvg >= 0)
				{
					sum += usedAvg * setPart.Quantity;
					priceList.Add((part.Number, part.Color, usedAvg * setPart.Quantity));
				}
			}*/

			#endregion


			return Ok(new
			{
				SetNumber = setNumber,
				PriceFormation = priceFormationType.ToString(),
				Sum = sum,
				Detailed = priceList
					.OrderByDescending(item => item.Price)
					.Select(p => new { p.Number, p.Color, Price = Math.Round(p.Price, 4) })
					.ToArray()
			});
		}

		private async Task<(decimal, (string Number, Color Color, decimal Price))> ProcessPartPrice(PartOfSet setPart, StateType stateType, PriceFormationType priceFormationType)
		{
			var part = await partRepository.Get(setPart.PartId);

			// Skip Used Stickers
			if (part.IsSticker && stateType == StateType.Used)
			{
				return (0m, default);
			}

			decimal usedAvg = -1,
				newAvg = -1;

			switch (priceFormationType)
			{
				case PriceFormationType.AverageRecentlySoldLimited:

					newAvg = (await orderPriceDataRepository.UpdateAndGetPartPrices(part, StateType.New))
						.OrderByDescending(d => d.Date)
						.Take(10)
						.Average(d => d.Price.Value);

					if (stateType == StateType.Used)
						usedAvg = (await orderPriceDataRepository.UpdateAndGetPartPrices(part, StateType.Used))
							.OrderByDescending(d => d.Date)
							.Take(10)
							.Average(d => d.Price.Value);

					break;

				case PriceFormationType.AverageInStockLimited:

					var newPartInStock = await stockPriceDataRepository.UpdateAndGetPartPrices(part, StateType.New);
					if (newPartInStock.Count > 0)
					{
						newAvg = newPartInStock
							.OrderBy(d => d.Price.Value)
							.Take(10)
							.Average(d => d.Price.Value);
					}


					if (stateType == StateType.Used)
					{
						var usedPartsInStock = await stockPriceDataRepository.UpdateAndGetPartPrices(part, StateType.Used);

						if (usedPartsInStock.Count > 0)
						{
							usedAvg = usedPartsInStock
								.OrderBy(d => d.Price.Value)
								.Take(10)
								.Average(d => d.Price.Value);
						}
					}

					break;

				default:
					throw new NotImplementedException(priceFormationType.ToString());
			}


			if (newAvg >= 0 && usedAvg >= 0)
			{
				var min = Math.Min(newAvg, usedAvg);

				return (min * setPart.Quantity, (part.Number, part.Color, min * setPart.Quantity));
			}
			else if (newAvg >= 0)
			{
				return (newAvg * setPart.Quantity, (part.Number, part.Color, newAvg * setPart.Quantity));
			}
			else
			{
				return (usedAvg * setPart.Quantity, (part.Number, part.Color, usedAvg * setPart.Quantity));
			}
		}


		#region Deprecated

		public async Task<IActionResult> OldGetMultiSetPriceByParts(
			[FromQuery(Name = "sets")] string setNumberString,
			[FromQuery(Name = "state")] StateType stateType,
			[FromQuery(Name = "price")] PriceFormationType priceFormationType
		)
		{
			var setNumbers = setNumberString.Split(',')
				.Select(s => s.Trim())
				.OrderBy(s => s)
				.ToArray();


			var setNumberDict = setNumbers
				.GroupBy(s => s, s => 1)
				.ToDictionary(g => g.Key, g => g.Count());


			decimal sum = 0;
			var dict = new Dictionary<(string Number, Color Color), decimal>();

			var sets = await setRepository.Filter(s => setNumbers.Contains(s.Number));
			foreach (var entity in setNumberDict)
			{
				var set = sets.FirstOrDefault(s => s.Number.Equals(entity.Key));

				if (set == null)
				{
					//await brickLinkDataService.UpdateSetInformation(entity.Key);

					set = await setRepository.GetByNumber(entity.Key);
				}

				var data = await ProcessSetParts(/*set.Parts*/null, stateType, priceFormationType);

				sum += data.Sum * entity.Value;

				foreach (var item in data.List)
				{
					var key = (item.Number, item.Color);

					if (dict.ContainsKey(key))
					{
						dict[key] += item.Price * entity.Value;
					}
					else
					{
						dict.Add(key, item.Price * entity.Value);
					}
				}
			}

			return Ok(new
			{
				Sets = setNumbers,
				PriceFormation = priceFormationType.ToString(),
				Sum = sum,
				Detailed = dict.OrderByDescending(item => item.Value)
					.ToDictionary(item => item.Key, item => item.Value)
			});
		}


		public async Task<IActionResult> OldGetSetPriceByParts(
			[FromRoute] string setNumber,
			[FromQuery(Name = "state")] StateType stateType,
			[FromQuery(Name = "price")] PriceFormationType priceFormationType
		)
		{
			if (!await setRepository.Exists(s => s.Number.Equals(setNumber)))
			{
				//wait brickLinkDataService.UpdateSetInformation(setNumber);
			}

			var set = await setRepository.GetByNumber(setNumber);

			var priceList = await ProcessSetParts(/*set.Parts*/null, stateType, priceFormationType);

			return Ok(new
			{
				SetNumber = setNumber,
				PriceFormation = priceFormationType.ToString(),
				Sum = priceList.Sum,
				Detailed = priceList.List.Select(p => new { p.Number, p.Color, Price = Math.Round(p.Price, 4) }).ToArray()
			});
		}

		private async Task<(decimal Sum, List<(string Number, Color Color, decimal Price)> List)> ProcessSetParts(List<PartOfSet> setParts, StateType state, PriceFormationType priceFormation)
		{
			decimal sum = 0;
			var priceList = new List<(string Number, Color Color, decimal Price)>();

			foreach (var setPart in setParts.Where(p => !p.IsCounterpart && !p.IsAlternate))
			{
				var part = await partRepository.Get(setPart.PartId);

				// Skip Used Stickers
				if (part.IsSticker && state == StateType.Used)
				{
					continue;
				}

				decimal usedAvg = -1,
					newAvg;

				switch (priceFormation)
				{
					case PriceFormationType.AverageRecentlySoldLimited:

						newAvg = await GetPartOrderPrices(part, StateType.New);
						if (state == StateType.Used)
							usedAvg = await GetPartOrderPrices(part, StateType.Used);

						break;

					case PriceFormationType.AverageInStockLimited:

						newAvg = await GetPartStockPrices(part, StateType.New);
						if (state == StateType.Used)
							usedAvg = await GetPartStockPrices(part, StateType.Used);

						break;

					default:
						throw new NotImplementedException(priceFormation.ToString());
				}


				if (newAvg >= 0 && usedAvg >= 0)
				{
					var min = Math.Min(newAvg, usedAvg);

					sum += min * setPart.Quantity;
					priceList.Add((part.Number, part.Color, min * setPart.Quantity));
				}
				else if (newAvg >= 0)
				{
					sum += newAvg * setPart.Quantity;
					priceList.Add((part.Number, part.Color, newAvg * setPart.Quantity));
				}
				else if (usedAvg >= 0)
				{
					sum += usedAvg * setPart.Quantity;
					priceList.Add((part.Number, part.Color, usedAvg * setPart.Quantity));
				}
			}

			return (sum, priceList.OrderByDescending(item => item.Price).ToList());
		}

		private async Task<decimal> GetPartOrderPrices(Part part, StateType state)
		{
			// ToDo: make limit(10) to DB
			var orderData = await orderPriceDataRepository.GetPartPrices(part, state);

			// ToDo: make condition on data obsolete
			if (!orderData.Any())
			{
				await brickLinkDataConnector.UpdatePartPrices(part, PriceFormationType.AverageRecentlySoldLimited);

				orderData = await orderPriceDataRepository.GetPartPrices(part, state);
			}

			return orderData
				.OrderByDescending(d => d.Date)
				.Take(10)
				.Average(d => d.Price.Value);
		}

		private async Task<decimal> GetPartStockPrices(Part part, StateType state)
		{
			// ToDo: make limit(10) to DB
			var stockData = await stockPriceDataRepository.GetPartPrices(part, state);
			// ToDo: make condition on data obsolete
			if (!stockData.Any())
			{
				await brickLinkDataConnector.UpdatePartPrices(part, PriceFormationType.AverageInStockLimited);

				stockData = await stockPriceDataRepository.GetPartPrices(part, state);
			}

			// If there is no data even at BrickLink - set -1
			if (stockData.Count == 0)
				return -1;

			return stockData
				.OrderBy(d => d.Price.Value)
				.Take(10)
				.Average(d => d.Price.Value);
		}

		#endregion

		[HttpPost("test")]
		public IActionResult TestAction([FromBody] Entry entry, [FromQuery] StateType state)
		{

			return Ok();
		}
	}
}
