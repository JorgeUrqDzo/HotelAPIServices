using Hotel.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }
        
        protected void ConfigureChangeTrackingEntity<T>(ModelBuilder modelBuilder) where T : ChangeTrackingEntity
        {
            modelBuilder.Entity<T>(builder =>
            {
                builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(u => u.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(u => u.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureChangeTrackingEntity<User>(modelBuilder);
        }
        
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}