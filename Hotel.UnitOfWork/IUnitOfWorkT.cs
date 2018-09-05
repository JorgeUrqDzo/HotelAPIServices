using Microsoft.EntityFrameworkCore;

namespace Hotel.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        TContext Context { get; set; }
    }
}