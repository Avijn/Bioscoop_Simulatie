using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Bioscoop_Simulatie
{
    public class UIRoom
    {
        /// <summary>
        /// Class to hold all the UI elements for a room
        /// On creating the object it will take all of the seats from the seats grid and put them into a list
        /// </summary>
        /// <param name="title"></param>
        /// <param name="screen"></param>
        /// <param name="status"></param>
        /// <param name="seats"></param>

        private TextBlock Title;
        private Rectangle Screen;
        private Image Status;
        private List<Image> Seats { get; set; }
        private int PeopleInRoom;
        private int MaxSpace;

        public UIRoom(TextBlock title, Rectangle screen, Image status, Grid seats)
        {
            this.Title = title;
            this.Screen = screen;
            this.Status = status;
            this.Seats = new List<Image>();
            this.PeopleInRoom = 0;
            this.MaxSpace = 9;

            //Get all seats from provided Grid
            foreach (var seat in seats.Children)
            {
                this.Seats.Add(seat as Image);
            }
        }

        /// <summary>
        /// Add a person to the room and change a seat icon to taken
        /// </summary>
        public void AddPerson()
        {
            if (this.PeopleInRoom + 1 <= this.MaxSpace)
            {
                //Room still has space
                this.PeopleInRoom++;

                Image seat = this.Seats[this.PeopleInRoom - 1];
                Console.WriteLine(seat);
                seat.Source = Utils.CreateImage("seat_taken.PNG");
            }
        }

        public void ClearRoom()
        {
            this.Seats.ForEach(seat => seat.Source = Utils.CreateImage("seat_free.png"));
            this.PeopleInRoom = 0;
        }

        public void SetStatus(RoomStatus status)
        {
            if (status == RoomStatus.Playing)
            {
                this.Status.Source = Utils.CreateImage("status_playing.PNG");
            }
            else if (status == RoomStatus.Cleaning)
            {
                this.Status.Source = Utils.CreateImage("status_cleaning.PNG");
            }
            else
            {
                this.Status.Source = Utils.CreateImage("status_waiting.PNG");
            }
        }

        public void SetTitle(string title)
        {
            this.Title.Text = title;
        }
    }
}
