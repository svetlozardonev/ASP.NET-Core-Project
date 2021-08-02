namespace PickMovie.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using PickMovie.Data;
    using PickMovie.Data.Models;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDataBase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<PickMovieDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            return app;
        }

        private static void SeedCategories(PickMovieDbContext data)
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Action" },
                new Category { Name = "Animation" },
                new Category { Name = "Comedy" },
                new Category { Name = "Fantasy" },
                new Category { Name = "Horror" },
                new Category { Name = "Mystery" },
                new Category { Name = "Thriller" },
            });

            data.SaveChanges();
        }

    }
}
