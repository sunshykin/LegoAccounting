using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Repositories
{
	public abstract class RepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity
	{
		protected readonly MongoContext Context;
		protected readonly IMongoCollection<TEntity> Collection;

		protected RepositoryBase(MongoContext context)
		{
			Context = context;
			Collection = Context.GetCollection<TEntity>();
		}

		public async Task<bool> Exists(Expression<Func<TEntity, bool>> filter)
		{
			return await Collection
				.Find(filter)
				.AnyAsync();
		}

		public async Task<TEntity> Get(ObjectId id)
		{
			//ToDo: think about default result throwing exception
			return await Collection
				.Find(entity => entity.Id.Equals(id))
				.SingleOrDefaultAsync();
		}

		public async Task Save(TEntity entity)
		{
			if (entity.Id == ObjectId.Empty)
			{
				// ToDo: think bout this implementation. InsertOne returns entity and can create own ObjectId
				//entity.Id = ObjectId.GenerateNewId();

				await Collection.InsertOneAsync(entity);
			}
			else
			{
				await Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
			}
		}

		public async Task InsertMany(IList<TEntity> data)
		{
			if (data.Count > 0)
				await Collection.InsertManyAsync(data);
		}

		public async Task<List<TEntity>> Filter(Expression<Func<TEntity, bool>> filter)
		{
			return await Collection
				.Find(filter)
				.ToListAsync();
		}

		public async Task Delete(ObjectId id)
		{
			await Collection.DeleteOneAsync(e => e.Id.Equals(id));
		}
	}
}
