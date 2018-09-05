using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Hotel.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext
    {
        private Dictionary<Type, object> repositories;


        public TContext Context { get; set; }

        public UnitOfWork(TContext context)
        {
            Context = context;
            repositories = new Dictionary<Type, object>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (!repositories.Keys.Contains(typeof(TEntity)))
            {
                Repository<TEntity> repository = new Repository<TEntity>(Context);

                repositories.Add(typeof(TEntity), repository);
            }

            return (Repository<TEntity>) repositories[typeof(TEntity)];
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context?.Dispose();
                }

                if (repositories != null)
                {
                    repositories.Clear();
                    repositories = null;
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}