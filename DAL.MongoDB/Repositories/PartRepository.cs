using System.Threading.Tasks;
using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Domain.Enums;
using LegoAccounting.Integration.BrickLink.Services;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Repositories
{
	public class PartRepository : RepositoryBase<Part>, IPartRepository
	{
		private readonly IBrickLinkDataConnector brickLinkDataConnector;

		public PartRepository(MongoContext mongoContext, IBrickLinkDataConnector brickLinkDataConnector) : base(mongoContext)
		{
			this.brickLinkDataConnector = brickLinkDataConnector;
		}

		public async Task<Part> GetByNumberAndColor(string number, Color color)
		{
			return await Collection
				.Find(p => p.Number.Equals(number) && p.Color.Equals(color))
				.SingleOrDefaultAsync();
		}

		public async Task<Part> UpdateAndGetByNumberAndColor(string number, Color color)
		{
			var part = await GetByNumberAndColor(number, color);

			if (part == null)
			{
				part = await brickLinkDataConnector.GetPart(number, color);

				await Save(part);
			}

			return part;
		}
	}
}
