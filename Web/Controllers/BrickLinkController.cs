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
	}
}
