using Microsoft.EntityFrameworkCore;

using SWP391.Project.Entities;

namespace SWP391.Project.DbContexts
{
    public class ProjectDbContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; } = null!;
        public DbSet<AddressEntity> Addresses { get; set; } = null!;
        public DbSet<BrandEntity> Brands { get; set; } = null!;
        public DbSet<CartEntity> Carts { get; set; } = null!;
        public DbSet<ColorEntity> Colors { get; set; } = null!;
        public DbSet<FeedbackEntity> Feedbacks { get; set; } = null!;
        public DbSet<OrderEntity> Orders { get; set; } = null!;
        public DbSet<ProductEntity> Products { get; set; } = null!;
        public DbSet<SizeEntity> Sizes { get; set; } = null!;
        public DbSet<ImageEntity> Images { get; set; } = null!;
        public DbSet<ProductInStockEntity> InStock { get; set; } = null!;

        public ProjectDbContext(DbContextOptions<ProjectDbContext> opts) : base(options: opts)
        {
            // Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.Entity<AccountEntity>().Navigation(navigationExpression: exp => exp.Avatar).AutoInclude();
            _ = modelBuilder.Entity<AddressEntity>().Navigation(navigationExpression: exp => exp.Account).AutoInclude();
            _ = modelBuilder.Entity<BrandEntity>().Navigation(navigationExpression: exp => exp.Logo).AutoInclude();
            _ = modelBuilder.Entity<CartEntity>().Navigation(navigationExpression: exp => exp.Account).AutoInclude();
            _ = modelBuilder.Entity<CartEntity>().Navigation(navigationExpression: exp => exp.Product).AutoInclude();
            // _ = modelBuilder.Entity<ColorEntity>().Navigation(navigationExpression: exp => exp.Product).AutoInclude();
            _ = modelBuilder.Entity<FeedbackEntity>().Navigation(navigationExpression: exp => exp.Order).AutoInclude();
            _ = modelBuilder.Entity<OrderEntity>().Navigation(navigationExpression: exp => exp.Product).AutoInclude();
            _ = modelBuilder.Entity<OrderEntity>().Navigation(navigationExpression: exp => exp.ShipmentAddress).AutoInclude();
            _ = modelBuilder.Entity<ProductEntity>().Navigation(navigationExpression: exp => exp.Brand).AutoInclude();
            _ = modelBuilder.Entity<ProductEntity>().Navigation(navigationExpression: exp => exp.Images).AutoInclude();
            _ = modelBuilder.Entity<ProductEntity>().Navigation(navigationExpression: exp => exp.Brand).AutoInclude();
            _ = modelBuilder.Entity<ProductInStockEntity>().Navigation(navigationExpression: exp => exp.Color).AutoInclude();
            _ = modelBuilder.Entity<ProductInStockEntity>().Navigation(navigationExpression: exp => exp.Size).AutoInclude();
            _ = modelBuilder.Entity<ProductInStockEntity>().Navigation(navigationExpression: exp => exp.Product).AutoInclude();
            // _ = modelBuilder.Entity<SizeEntity>().Navigation(navigationExpression: exp => exp.Product).AutoInclude();
        }
    }
}