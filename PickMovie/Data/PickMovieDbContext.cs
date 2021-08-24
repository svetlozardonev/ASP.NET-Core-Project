namespace PickMovie.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PickMovie.Data.Models;

    public class PickMovieDbContext : IdentityDbContext<User>
    {
        public PickMovieDbContext(DbContextOptions<PickMovieDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<Comment> Comments { get; init; }
        public DbSet<UserComment> UserComments { get; init; }
        public DbSet<UserMovie> UserMovies { get; init; }
        public DbSet<Critic> Critics { get; init; }
        public DbSet<Review> Reviews { get; init; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Movie>()
                .HasOne(m => m.Category)
                .WithMany(m => m.Movies)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .Entity<Movie>()
                .HasOne(m => m.User)
                .WithMany(m => m.Movies)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .Entity<Comment>()
                .HasOne(m => m.Author)
                .WithMany(m => m.Comments)
                .HasForeignKey(m => m.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Comment>()
                .HasOne(m => m.Movie)
                .WithMany(m => m.Comments)
                .HasForeignKey(m => m.MovieId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<UserMovie>()
                .HasKey(um => new { um.UserId, um.MovieId });

            builder
                .Entity<UserMovie>()
                .HasOne(um => um.User)
                .WithMany(um => um.UserMovies)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<UserMovie>()
                .HasOne(um => um.Movie)
                .WithMany(um => um.UserMovies)
                .HasForeignKey(um => um.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserComment>()
                .HasKey(uc => new { uc.UserId, uc.CommentId });

            builder
                .Entity<UserComment>()
                .HasOne(uc => uc.User)
                .WithMany(uc => uc.UserComments)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<UserComment>()
                .HasOne(uc => uc.Comment)
                .WithMany(uc => uc.UserComments)
                .HasForeignKey(uc => uc.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Critic>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Critic>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Review>()
                .HasOne<Critic>()
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.CriticId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Review>()
                .HasOne<Movie>()
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
