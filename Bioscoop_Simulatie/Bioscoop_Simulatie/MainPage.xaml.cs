using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
        public UIRoom[] UIRooms { get; set; }
        public UICheckout Checkout_1 { get; set; }
        public UICheckout Checkout_2 { get; set; }
        public UIStation Queue { get; set; }
        public UIStation Lobby { get; set; }
        public Cinema Cinema { get; set; }


		public List<Room> Rooms { get; set; }
		public List<Movie> Movies { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            //Creates Cinema
			Cinema = new Cinema();
			//DataContext = Cinema;

			//Create movies
			CreateMovies();

			//Create cinema rooms
			UIRooms = new UIRoom[3];
            CreateRooms();

            //Bind movies to rooms
            BindMovieToRoom();

            //Bind rooms to cinema
            BindRoomToCinema();

			//Set cinema registers
			Checkout_1 = new UICheckout(registerStatus_1); 
            Checkout_2 = new UICheckout(registerStatus_2);

            //Set Queue
            Queue = new UIStation(queueStatus);

            //Set Lobby
            Lobby = new UIStation(lobbyStatus);

            Cinema.Checkouts.Add(new Checkout());
            Cinema.AddCustomerToQueue(new Customer(25));
            Cinema.AddCustomerToQueue(new Customer(25));
            Cinema.OpenCheckouts();
        }

        /// <summary>
        /// Creates all the room classes in the cinema
        /// All room classes hold the UI components for the room
        /// </summary>
        private void CreateRooms()
        {
            //Create room 1
            UIRoom room1 = new UIRoom(movieTitle_1, movieScene_1, movieStatus_1, seats_room_1);
			UIRooms[0] = room1;

            //Create room 2
            UIRoom room2 = new UIRoom(movieTitle_2, movieScene_2, movieStatus_2, seats_room_2);
			UIRooms[1] = room2;

            //Create room 3
            UIRoom room3 = new UIRoom(movieTitle_3, movieScene_3, movieStatus_3, seats_room_3);
			UIRooms[2] = room3;

			Rooms.Add(new Room("Room 1", 15, 5000));
			Rooms.Add(new Room("Room 2", 15, 6000));
			Rooms.Add(new Room("Room 3", 45, 3000));
		}

        private void CreateMovies()
        {
			Movies.Add(new Movie("Shrek 4", 12000, 13));
			Movies.Add(new Movie("Shrek 5", 10000, 21));
			Movies.Add(new Movie("The lord of the rings: fellowship of the ring", 15000, 16));
		}

        private void BindMovieToRoom()
        {
			Rooms[0].Movie = Movies[0];
            Rooms[1].Movie = Movies[1];
            Rooms[2].Movie = Movies[2];
		}

        private void BindRoomToCinema()
        {
            Cinema.Rooms = Rooms;
        }

		/// <summary>
		/// Gets the room by given nr
		/// </summary>
		/// <param name="nr"></param>
		/// <returns></returns>
		private UIRoom GetRoomByNr(int nr)
        {
            return UIRooms[nr - 1];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Thread thread = new Thread(Cinema.HandleCheckouts);
            Cinema.HandleCheckouts();
        }
    }
}
