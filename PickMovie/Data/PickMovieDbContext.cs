namespace PickMovie.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PickMovie.Data.Models;

    public class PickMovieDbContext : IdentityDbContext
    {
        public PickMovieDbContext(DbContextOptions<PickMovieDbContext> options)
            : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Movie>()
                .HasOne(m => m.Category)
                .WithMany(m => m.Movies)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
        public DbSet<Movie> Movies { get; init; }
        public DbSet<Category> Categories { get; init; }
    }
}
