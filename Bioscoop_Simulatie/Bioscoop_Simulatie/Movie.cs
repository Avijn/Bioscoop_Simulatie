using System.ComponentModel;

namespace Bioscoop_Simulatie
{
    public class Movie : INotifyPropertyChanged
	{
        public string Title { get; }
        public int Duration { get; }
        public int AgeRestriction { get; }
		public event PropertyChangedEventHandler PropertyChanged;

		public Movie(string title, int duration, int ageRestriction)
        {
            Title = title;
            Duration = duration;
            AgeRestriction = ageRestriction;
		}

		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
