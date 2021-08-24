namespace PickMovie.Data.Models
{
    using System;

    public class Review
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public string Title { get; set; }

        public string Content { get; set; }

        public string CriticId { get; set; }

        public Critic Critic { get; set; }

        public string MovieId { get; set; }

        public Movie Movie { get; set; }
    }
}
