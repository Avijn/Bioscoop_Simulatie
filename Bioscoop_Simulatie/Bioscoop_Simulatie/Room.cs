using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Bioscoop_Simulatie
{
    public class Room : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public Movie Movie { get; set; }
        public RoomStatus Status { get; set; }
        public int Seats { set; get; }
        public int TakenSeats { get; set; }
        public int CleanDuration { get; set; }
        public Thread Thread { get; set; }
        public BitmapImage Img { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage Waiting { get; set; }
		public BitmapImage Playing { get; set; }
		public BitmapImage Cleaning { get; set; }


		public Room(string name, int seats, int cleanDuration)
		{
			Waiting = CreateBitMapImage(@"status_waiting.png");
			Playing = CreateBitMapImage(@"status_playing.png");
			Cleaning = CreateBitMapImage(@"status_cleaning.png");

			Name = name;
            Seats = seats;
            Status = RoomStatus.Open;
            CleanDuration = cleanDuration;
			Img = Waiting;
		}

        /// <summary>
        /// Adds a customer to the Room, this adds a takenseat unless the Takenseats equal the total amount of seats
        /// </summary>
        /// <returns>
        /// Returns TRUE if the customer can get a seat.<br /> Returns false if the room is full
        /// </returns>
        public bool AddReservation()
        {
            if (Seats != TakenSeats)
            {
                TakenSeats = TakenSeats++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// "Plays" the movie, waits the given duration of the movie <br />
        /// Sets the TakenSeats to 0
        /// </summary>
        public void Play()
        {
            TakenSeats = 0;
            Thread.Sleep(Movie.Duration);
            Status = RoomStatus.FinishedPlaying;
		}

        /// <summary>
        /// Starts cleaning the room, waits the given duration for cleaning
        /// </summary>
        public void Clean()
		{
			Thread.Sleep(CleanDuration);
            Status = RoomStatus.FinishedCleaning;
		}

        /// <summary>
        /// Opens the room for customers
        /// </summary>
        public void Open()
        {
			Status = RoomStatus.Open;
        }

        public void Sleep(int amount)
        {
            Thread.Sleep(amount);
        }

        public BitmapImage CreateBitMapImage(string path)
		{
			string assetsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
			return new BitmapImage(new Uri(Path.Combine(assetsFolderPath, path)));
		}

		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}