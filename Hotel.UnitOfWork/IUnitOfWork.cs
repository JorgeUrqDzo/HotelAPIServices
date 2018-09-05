using System;

namespace Hotel.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}