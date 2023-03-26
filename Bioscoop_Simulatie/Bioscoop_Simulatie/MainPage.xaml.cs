using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using static Bioscoop_Simulatie.MainPage;

namespace Bioscoop_Simulatie
{
    public sealed partial class MainPage : Page
    {
        internal class UIRoom
        {
            /// <summary>
            /// Class to hold all the UI elements for a room
            /// On creating the object it will take all of the seats from the seats grid and put them into a list
            /// </summary>
            /// <param name="title"></param>
            /// <param name="screen"></param>
            /// <param name="status"></param>
            /// <param name="seats"></param

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
                if(this.PeopleInRoom + 1 <= this.MaxSpace)
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

            public void SetStatus(MovieStatus status)
            {
                if (status == MovieStatus.Playing)
                {
                    this.Status.Source = Utils.CreateImage("status_playing.PNG");
                }
                else if (status == MovieStatus.Cleaning)
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

        /// <summary>
        /// Class to hold UI components for the 2 registers
        /// </summary>
        internal class UIRegister
        {
            private TextBlock Status;

            public UIRegister(TextBlock status)
            {
                Status = status;
            }

            /// <summary>
            /// Set the status of a register, can by waiting, busy or closed
            /// </summary>
            /// <param name="status"></param>
            public void SetStatus(RegisterStatus status)
            {
                this.Status.Text = "Status: " + status.ToString();
            }
        }

        /// <summary>
        /// Class to hold UI components for the queue and lobby
        /// </summary>
        internal class UIStation
        {
            private TextBlock PeopleWaiting;

            public UIStation(TextBlock status)
            {
                this.PeopleWaiting = status;
            }

            /// <summary>
            /// Set the number of people waiting in a station (Queue or Lobby)
            /// </summary>
            /// <param name="nrOfPeople"></param>
            public void SetPeopleWaiting(int nrOfPeople)
            {
                this.PeopleWaiting.Text = "People waiting: " + nrOfPeople.ToString();
            }
        }

        private UIRoom[] Rooms;
        private UIRegister Register_1;
        private UIRegister Register_2;
        private UIStation Queue;
        private UIStation Lobby;

        public MainPage()
        {
            this.InitializeComponent();

            //Create cinema rooms
            this.Rooms = new UIRoom[3];
            CreateRooms();

            //Set cinema registers
            this.Register_1 = new UIRegister(registerStatus_1); 
            this.Register_2 = new UIRegister(registerStatus_2);

            //Set Queue
            this.Queue = new UIStation(queueStatus);

            //Set Lobby
            this.Lobby = new UIStation(lobbyStatus);
        }

        /// <summary>
        /// Creates all the room classes in the cinema
        /// All room classes hold the UI components for the room
        /// </summary>
        private void CreateRooms()
        {
            //Create room 1
            UIRoom room1 = new UIRoom(movieTitle_1, movieScene_1, movieStatus_1, seats_room_1);
            Rooms[0] = room1;

            //Create room 2
            UIRoom room2 = new UIRoom(movieTitle_2, movieScene_2, movieStatus_2, seats_room_2);
            Rooms[1] = room2;

            //Create room 3
            UIRoom room3 = new UIRoom(movieTitle_3, movieScene_3, movieStatus_3, seats_room_3);
            Rooms[2] = room3;
        }

        /// <summary>
        /// Gets the room by given nr
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        private UIRoom GetRoomByNr(int nr)
        {
            return this.Rooms[nr - 1];
        }
    }
}
