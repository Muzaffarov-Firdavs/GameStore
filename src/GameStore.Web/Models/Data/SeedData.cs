using GameStore.Data.DbContexts;
using GameStore.Domain.Entities.Games;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Web.Models.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            if (!context.Genres.Any())
            {
                var genres = new List<Genre>()
                {
                    new Genre{ Name = "Strategy", CreatedAt = DateTime.UtcNow },
                    new Genre{ Name = "RPG", CreatedAt = DateTime.UtcNow },
                    new Genre{ Name = "Sports", CreatedAt = DateTime.UtcNow },
                    new Genre{ Name = "Races", CreatedAt = DateTime.UtcNow },
                    new Genre{ Name = "Action", CreatedAt = DateTime.UtcNow },
                    new Genre{ Name = "Adventure", CreatedAt = DateTime.UtcNow },
                    new Genre{ Name = "Puzzle & skill", CreatedAt = DateTime.UtcNow }
                };

                context.Genres.AddRange(genres);
                context.SaveChanges();
            }
        }
    }
}
