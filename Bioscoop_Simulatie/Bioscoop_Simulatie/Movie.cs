using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioscoop_Simulatie
{
    class Movie
    {
        public string Title { get; }
        public int Duration { get; }
        public int AgeRestriction { get; }

        public Movie(string title, int duration, int ageRestriction)
        {
            Title = title;
            Duration = duration;
            AgeRestriction = ageRestriction;
        }
    }
}
