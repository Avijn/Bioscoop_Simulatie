using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
