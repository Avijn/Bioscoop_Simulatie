namespace Bioscoop_Simulatie
{
    public class Movie
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
