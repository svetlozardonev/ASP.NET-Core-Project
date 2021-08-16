namespace TestProject.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using TestProject.Data;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using TestProject.Data.Models;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetRequiredService<PickMovieDbContext>();

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
                new Category { Name = "Action"},
                new Category { Name = "Adventure"},
                new Category { Name = "Animation"},
                new Category { Name = "Comedy"},
                new Category { Name = "Crime"},
                new Category { Name = "Drama"},
                new Category { Name = "Fantasy"},
                new Category { Name = "Horror"},
                new Category { Name = "Mystery"},
                new Category { Name = "Thriller"}
            });

            data.SaveChanges();
        }
    }
}
