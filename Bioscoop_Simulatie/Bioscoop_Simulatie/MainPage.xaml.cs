﻿using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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

			//Create movies
            Movies = new List<Movie>();
			CreateMovies();

			//Create cinema rooms
			UIRooms = new UIRoom[3];
            Rooms= new List<Room>();

            Rooms.Add(new Room("Room 1", 15, 5000));
            Rooms.Add(new Room("Room 2", 15, 6000));
            Rooms.Add(new Room("Room 3", 45, 3000));

            CreateRooms();

            //Bind movies to rooms
            BindMovieToRoom();

			//Set cinema registers
			Checkout_1 = new UICheckout(registerStatus_1); 
            Checkout_2 = new UICheckout(registerStatus_2);

            //Set Queue
            Queue = new UIStation(queueStatus);

            //Set Lobby
            Lobby = new UIStation(lobbyStatus);


            //Set hover on button to transparent
            //CinemaControlBtn.PointerOverBackground = new SolidColorBrush();
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

        /// <summary>
        /// Gets the room by given nr
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        private UIRoom GetRoomByNr(int nr)
        {
            return UIRooms[nr - 1];
        }

        private void Open_Close_Checkouts(object sender, RoutedEventArgs e)
        {
            if (Cinema.Checkouts[0].Status == CheckoutStatus.Closed)
            {
                Cinema.IsCheckoutsOpen = true;
                UpdateCheckoutBtnUI(Cinema.IsCheckoutsOpen);
                Cinema.OpenCheckouts();
                return;
            }

            Cinema.IsCheckoutsOpen = false;
            UpdateCheckoutBtnUI(Cinema.IsCheckoutsOpen);
            Cinema.CloseCheckouts();
        }

        private void Open_Close_Cinema(object sender, RoutedEventArgs e)
        {
            if (Cinema.RunCinemaFlag)
            {
                Debug.WriteLine("Cinema is not running");
                Cinema.RunCinemaFlag = false;
                UpdateCinemaBtnUI(Cinema.RunCinemaFlag);

                return;
            }

            Cinema.RunCinemaFlag = true;
            UpdateCinemaBtnUI(Cinema.RunCinemaFlag);
            UpdateCheckoutBtnUI(Cinema.IsCheckoutsOpen);

            if (Cinema.IsThreadRunning)
                return;

            Cinema.IsThreadRunning = true;
            Cinema.Run.Start();
        }

        private void UpdateCinemaBtnUI(bool closed)
        {
            Windows.UI.Color color;
            string content;
            if(closed)
            {
                color = Windows.UI.Color.FromArgb(255, 253, 73, 73);
                content = "Close Cinema";
            }
            else
            {
                color = Windows.UI.Color.FromArgb(255, 104, 184, 102);
                content = "Open Cinema";
            }

            CinemaControlBtn.Background = new SolidColorBrush(color);
            CinemaControlBtn.Content = content;
        }


        private void UpdateCheckoutBtnUI(bool closed)
        {
            Windows.UI.Color color;
            string content;
            if (!closed)
            {
                color = Windows.UI.Color.FromArgb(255, 253, 73, 73);
                content = "Close Checkouts";
            }
            else
            {
                color = Windows.UI.Color.FromArgb(255, 104, 184, 102);
                content = "Open Checkouts";
            }

            CheckoutControlBtn.Background = new SolidColorBrush(color);
            CheckoutControlBtn.Content = content;
        }
    }
}
