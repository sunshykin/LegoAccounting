using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Integration.BrickLink.Enums;
using LegoAccounting.Integration.BrickLink.Services;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Repositories
{
	public class PartOfSetRepository : RepositoryBase<PartOfSet>, IPartOfSetRepository
	{
		private readonly IBrickLinkDataConnector brickLinkDataConnector;

		public PartOfSetRepository(
			MongoContext mongoContext,
			IBrickLinkDataConnector brickLinkDataConnector
		) : base(mongoContext)
		{
			this.brickLinkDataConnector = brickLinkDataConnector;
		}
		public async Task<List<PartOfSet>> GetSetParts(Set set)
		{
			return await Collection.Find(p => p.SetId.Equals(set.Id))
				.ToListAsync();
		}

		public async Task<List<PartOfSet>> UpdateAndGetSetParts(Set set)
		{
			var parts = await GetSetParts(set);
			
			if (parts.Count == 0)
			{
				var entries = await brickLinkDataConnector.GetSetParts(set.Number);
				var tasks = entries
					.Where(entry => entry.Item.Type == ItemType.Part)
					.Select(entry => UpdateAndGetPartByNumberAndColor(entry.Item.Number, entry.Color));

				parts.AddRange((await Task.WhenAll(tasks)).Select(part =>
				{
					try
					{
						var entry = entries.First(e =>
							e.Item.Number.Equals(part.Number) &&
							e.Color.Equals(part.Color)
						);

						return new PartOfSet
						{
							PartId = part.Id,
							SetId = set.Id,
							Quantity = entry.Quantity,
							IsCounterpart = entry.IsCounterpart,
							IsAlternate = entry.IsAlternate
						};
					}
					catch (Exception ex)
					{
						throw;
					}
				}));

				await InsertMany(parts);
			}

			return parts;
		}

		private async Task<Part> UpdateAndGetPartByNumberAndColor(string number, Color color)
		{
			var part = await Context.Parts
				.Find(p => p.Number.Equals(number) && p.Color.Equals(color))
				.SingleOrDefaultAsync();

			if (part == null)
			{
				part = await brickLinkDataConnector.GetPart(number, color);

				await Context.Parts.InsertOneAsync(part);
			}

			return part;
		}
	}
}