﻿using System.Collections;
using System.Collections.Generic;

namespace PickMovie.Models.Home
{
    public class IndexViewModel
    {
        public int TotalMovies { get; init; }

        public int TotalUsers { get; init; }

        public List<MovieIndexViewModel> Movies { get; init; }
    }
}
