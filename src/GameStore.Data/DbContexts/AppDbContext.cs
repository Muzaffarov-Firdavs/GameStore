using GameStore.Domain.Entities.Files;
using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Orders;
using GameStore.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<ContactInformation> ContactInformations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
