using System.Threading.Tasks;
using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using LegoAccounting.Integration.BrickLink.Services;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Repositories
{
	public class SetRepository : RepositoryBase<Set>, ISetRepository
	{
		private readonly IBrickLinkDataConnector brickLinkDataConnector;

		public SetRepository(MongoContext mongoContext, IBrickLinkDataConnector brickLinkDataConnector) : base(mongoContext)
		{
			this.brickLinkDataConnector = brickLinkDataConnector;
		}

		public async Task<Set> GetByNumber(string number)
		{
			return await Collection.Find(s => s.Number.Equals(number)).SingleOrDefaultAsync();
		}

		public async Task<Set> UpdateAndGetByNumber(string number)
		{
			var set = await GetByNumber(number);

			if (set == null)
			{
				set = await brickLinkDataConnector.GetSet(number);

				await Save(set);
			}

			return set;
		}
	}
}