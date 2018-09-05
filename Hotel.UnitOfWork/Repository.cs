using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Hotel.UnitOfWork
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public Repository(DbContext context)
        {
            dbContext = context;
            dbSet = context.Set<TEntity>();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null ? 
                    dbSet.Any(predicate) 
                    : dbSet.Any();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null ?
                    dbSet.Count(predicate)
                    : dbSet.Count();
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = GetAll(predicate)
                            .ToList();

            Delete(entities);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null ?
                    dbSet.Where(predicate)
                    : dbSet;
        }

        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
        }




        public TEntity Find(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }
            else
            {
                return query.FirstOrDefault();
            }
        }

        public TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(selector).FirstOrDefault();
            }
            else
            {
                return query.Select(selector).FirstOrDefault();
            }
        }

        public PagedListResult<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 10000, bool disableTracking = true) where TResult : class
        {
            IQueryable<TEntity> query = dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return new PagedListResult<TResult>(orderBy(query).Select(selector).ToList()); //.ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return new PagedListResult<TResult>(query.Select(selector).ToList()); //.ToPagedList(pageIndex, pageSize);
            }
        }

        public PagedListResult<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 10000, bool disableTracking = true)
        {
            IQueryable<TEntity> query = dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return new PagedListResult<TEntity>(orderBy(query).ToList()); //.ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return new PagedListResult<TEntity>(query.ToList()); //.ToPagedList(pageIndex, pageSize);
            }
        }

        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            return dbSet.FromSql(sql, parameters);
        }
    }
}