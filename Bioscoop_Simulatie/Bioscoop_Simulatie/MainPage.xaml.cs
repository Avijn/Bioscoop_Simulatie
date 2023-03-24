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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Bioscoop_Simulatie
{
    public sealed partial class MainPage : Page
    {
        internal class Room
        {
            /// <summary>
            /// Class to hold all the UI elements for a room
            /// On creating the object it will take all of the seats from the seats grid and put them into a list
            /// </summary>
            /// <param name="title"></param>
            /// <param name="screen"></param>
            /// <param name="status"></param>
            /// <param name="seats"></param>
            public Room(TextBlock title, Rectangle screen, Image status, Grid seats) 
            {
                this.Title = title;
                this.Screen = screen;
                this.Status = status;
                this.Seats = new List<Image>();

                //Get all seats from provided Grid
                foreach (var seat in seats.Children)
                {
                    this.Seats.Add(seat as Image);
                }
            }
            public TextBlock Title { get; set; }
            public Rectangle Screen { get; set; }
            public Image Status { get; set; }
            public List<Image> Seats { get; set; }
        }

        private Room[] Rooms;

        public MainPage()
        {
            this.InitializeComponent();
            this.Rooms = new Room[3];

            CreateRooms();
        }

        /// <summary>
        /// Creates all the room classes in the cinema
        /// All room classes hold the UI components for the room
        /// </summary>
        private void CreateRooms()
        {
            //Create room 1
            Room room1 = new Room(movieTitle_1, movieScene_1, movieStatus_1, seats_room_1);
            Rooms[0] = room1;

            //Create room 2
            Room room2 = new Room(movieTitle_2, movieScene_2, movieStatus_2, seats_room_2);
            Rooms[1] = room2;

            //Create room 3
            Room room3 = new Room(movieTitle_3, movieScene_3, movieStatus_3, seats_room_3);
            Rooms[2] = room3;
        }

        /// <summary>
        /// Gets the room by given nr
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        private Room GetRoomByNr(int nr)
        {
            return this.Rooms[nr - 1];
        }

        /// <summary>
        /// Sets the title of the given room
        /// </summary>
        /// <param name="roomNr"></param>
        /// <param name="title"></param>
        public void SetTitleByRoomNr(int roomNr, String title)
        {
            Room room = GetRoomByNr(roomNr);
            room.Title.Text = title;
        }

        /// <summary>
        /// Sets the status of the given room
        /// </summary>
        /// <param name="roomNr"></param>
        /// <param name="status"></param>
        public void SetStatusByRoomNr(int roomNr, MovieStatus status)
        {
            Room room = GetRoomByNr(roomNr);
            if(status == MovieStatus.Playing)
            {
                room.Status.Source = CreateImage("/Assets/status_playing.PNG");
            } else if(status == MovieStatus.Cleaning)
            {
                room.Status.Source = CreateImage("/Assets/status_cleaning.PNG");
            } else
            {
                room.Status.Source = CreateImage("/Assets/status_waiting.PNG");
            }
        }

        /// <summary>
        /// Creates a bitmap image from a path
        /// Is used for creating an image in the UI
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private BitmapImage CreateImage(String path)
        {
            return new BitmapImage(new Uri(path));
        }
    }
}
