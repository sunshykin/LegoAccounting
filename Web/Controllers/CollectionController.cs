using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace LegoAccounting.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CollectionController : ControllerBase
	{
		private readonly ICollectionItemRepository collectionItemRepository;
		private readonly ISetRepository setRepository;
		private readonly IPartOfCollectionItemRepository partOfCollectionItemRepository;
		private readonly IPartOfSetRepository partOfSetRepository;
		private readonly IPartRepository partRepository;

		public CollectionController(
			ICollectionItemRepository collectionItemRepository,
			ISetRepository setRepository,
			IPartOfCollectionItemRepository partOfCollectionItemRepository,
			IPartOfSetRepository partOfSetRepository,
			IPartRepository partRepository
		)
		{
			this.collectionItemRepository = collectionItemRepository;
			this.setRepository = setRepository;
			this.partOfCollectionItemRepository = partOfCollectionItemRepository;
			this.partOfSetRepository = partOfSetRepository;
			this.partRepository = partRepository;
		}

		[HttpGet("list")]
		public async Task<ActionResult<IEnumerable<CollectionItem>>> GetList()
		{
			var collection = await collectionItemRepository.GetAllItems();

			return Ok(collection);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CollectionItem>> GetItem(ObjectId id)
		{
			var collectionItem = await collectionItemRepository.Get(id);

			return Ok(collectionItem);
		}

		[HttpGet("{id}/parts")]
		public async Task<ActionResult<IEnumerable<PartOfCollectionItem>>> GetItemParts(ObjectId id)
		{
			var collectionItemParts = await partOfCollectionItemRepository.GetItemParts(id);
			var partIds = collectionItemParts.Select(p => p.PartId).ToArray();
			var parts = await partRepository.Filter(p => partIds.Contains(p.Id));

			var results = collectionItemParts.Select(p =>
			{
				var part = parts.First(pa => pa.Id.Equals(p.PartId));

				return new
				{
					Id = p.Id,
					Number = part.Number,
					Name = System.Net.WebUtility.HtmlDecode(part.Name),
					Image = part.ImageUrl,
					Color = part.Color.ToString(),
					Quantity = p.Quantity,
					Condition = p.Condition.ToString()
				};
			});

			return Ok(results);
		}

		#region POST Requests

		[HttpPost("")]
		public async Task<IActionResult> Create(CollectionItemViewModel model)
		{
			var set = await setRepository.UpdateAndGetByNumber(model.SetNumber);
			var setParts = await partOfSetRepository.UpdateAndGetSetParts(set);

			var collectionItem = new CollectionItem
			{
				Name = set.Name,
				Number = set.Number,
				DateCreated = DateTime.Now,
				Type = ProcessType.Created
			};
			await collectionItemRepository.Save(collectionItem);

			//var partIds = setParts.Select(p => p.PartId).ToArray();
			//var parts = await partRepository.Filter(p => partIds.Contains(p.Id));
			var partsOfCollection = setParts
				.Where(p => !p.IsCounterpart && !p.IsAlternate)
				.Select(p => new PartOfCollectionItem
				{
					PartId = p.PartId,
					CollectionItemId = collectionItem.Id,
					Condition = ConditionType.Good,
					Quantity = p.Quantity,
					//ToDo: do smth with state which could be only taken from PartRepository
					State = StateType.Used
				})
				.ToList();
			await partOfCollectionItemRepository.InsertMany(partsOfCollection);

			return Ok();
		}

		#endregion

		#region PUT Requests

		[HttpPut("{id}")]
		public async Task<IActionResult> Edit([FromRoute]ObjectId id, [FromBody] CollectionItemPartViewModel model)
		{
			var part = await partOfCollectionItemRepository.Get(ObjectId.Parse(model.Id));

			part.Quantity = model.Quantity;
			part.Condition = model.Condition;

			await partOfCollectionItemRepository.Save(part);

			return Ok();
		}

		#endregion

		#region DELETE Requests

		[HttpDelete("{id}")]
		public async Task<ActionResult<IEnumerable<PartOfCollectionItem>>> DeleteItem(ObjectId id)
		{
			await partOfCollectionItemRepository.DeleteItemParts(id);
			await collectionItemRepository.Delete(id);

			return Ok();
		}

		#endregion
	}
}
