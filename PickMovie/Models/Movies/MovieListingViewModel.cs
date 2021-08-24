﻿namespace PickMovie.Models.Movies
{
    using PickMovie.Models.Comments;
    using System.Collections.Generic;
    public class MovieListingViewModel
    {
        public string Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Director { get; init; }
        public string ImageUrl { get; init; }
        public int Year { get; init; }
        public string Category { get; init; }
        public string CommentContent { get; init; }
        public List<CommentViewModel> Comments { get; init; }
    }
}
