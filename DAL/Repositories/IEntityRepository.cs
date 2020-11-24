using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LegoAccounting.Domain.Entities;
using MongoDB.Bson;

namespace LegoAccounting.DAL.Repositories
{
	public interface IEntityRepository<TEntity> where TEntity : class, IEntity
	{
		public Task<bool> Exists(Expression<Func<TEntity, bool>> filter);

		public Task<TEntity> Get(ObjectId id);

		public Task Save(TEntity entity);

		public Task InsertMany(IList<TEntity> data);

		public Task<List<TEntity>> Filter(Expression<Func<TEntity, bool>> filter);

		public Task Delete(ObjectId id);
	}
}