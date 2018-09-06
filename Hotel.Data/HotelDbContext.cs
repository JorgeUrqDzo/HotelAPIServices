using Hotel.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPasswordHistoryEntry> UserPasswordHistory { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuRole> MenuRoles { get; set; }
        public DbSet<ApplicationEvent> ApplicationEvents { get; set; }
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }

        private void ConfigureChangeTrackingEntity<T>(ModelBuilder modelBuilder) where T : ChangeTrackingEntity
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
            ConfigureChangeTrackingEntity<ApplicationSetting>(modelBuilder);

            modelBuilder.Entity<UserPasswordHistoryEntry>(builder =>
            {
                builder.HasOne(h => h.User)
                    .WithMany()
                    .HasForeignKey(h => h.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MenuRole>(builder => { builder.HasKey(i => new {i.MenuId, i.RoleId}); });

            modelBuilder.Entity<ApplicationSetting>().HasAlternateKey(s => new {s.Type, s.Name});
        }
    }
}