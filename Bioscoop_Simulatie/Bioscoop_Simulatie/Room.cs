using System.Diagnostics;
using System.Threading;

namespace Bioscoop_Simulatie
{
    class Room
    {
        public string Name { get; set; }
        public Movie Movie { get; set; }
        public RoomStatus Status { get; set; }
        public int Seats { set; get; }
        public int TakenSeats { get; set; }
        public int CleanDuration { get; set; }
        public Thread Thread { get; set; }

        public Room(string name, int seats, int cleanDuration)
        {
            Name = name;
            Seats = seats;
            Status = RoomStatus.Open;
            CleanDuration = cleanDuration;
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
            Debug.WriteLine("Start playing movie");
            Thread.Sleep(Movie.Duration);
			Debug.WriteLine("Finished playing movie");
            Status = RoomStatus.FinishedPlaying;
		}

        /// <summary>
        /// Starts cleaning the room, waits the given duration for cleaning
        /// </summary>
        public void Clean()
        {
			Debug.WriteLine("Start cleaning room");
			Thread.Sleep(CleanDuration);
			Debug.WriteLine("Finished cleaning room");
            Status = RoomStatus.FinishedCleaning;
		}

        /// <summary>
        /// Opens the room for customers
        /// </summary>
        public void Open()
        {
            Status = RoomStatus.Open;
        }

    }
}