﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bioscoop_Simulatie
{
    public class Cinema/* : INotifyPropertyChanged*/
    {
        public Queue<Customer> Queue { get; set; }
        public List<Checkout> Checkouts { get; set; }
        public List<Customer> Lobby { get; set; }
        public List<Room> Rooms { get; set; }
        public Dictionary<Movie, int> SoldTickets { get; set; }
        public bool RunRoomsFlag { get; set; }
        public bool RunCheckoutsFlag { get; set; }

        public Cinema()
        {
            Queue = new Queue<Customer>();
            Checkouts = new List<Checkout>();
            Lobby = new List<Customer>();
            Rooms = new List<Room>();
            SoldTickets = new Dictionary<Movie, int>();

            //// For "Testing" purposes
            //Rooms.Add(new Room("Room 1", 15, 5000));
            //Rooms.Add(new Room("Room 2", 15, 6000));
            //Rooms.Add(new Room("Room 3", 45, 3000));

            //// For ""Testing"" purposes
            //Rooms[0].Movie = new Movie("Shrek 4", 12000, 13);
            //Rooms[1].Movie = new Movie("Shrek 5", 10000, 21);
            //Rooms[2].Movie = new Movie("The lord of the rings: fellowship of the ring", 15000, 16);

            // For """Testing""" purposes
            RunRoomsFlag = false;
            RunCheckoutsFlag = false;
        }

        //public event PropertyChangedEventHandler PropertyChanged;


        public void OpenCheckouts()
        {
            foreach (var checkout in Checkouts)
            {
                checkout.CheckoutOpen();
            }
        }

        public async void HandleCheckouts()
        {
            while (Queue.Count > 0 || RunCheckoutsFlag)
            {
                foreach (var checkout in Checkouts)
                {
                    if (checkout.Status == CheckoutStatus.Open && Queue.Count != 0)
                    {
                        RunCheckoutsFlag = true;
                        await ExecuteOnUIThread(() =>
                        {
                            checkout.CheckoutInProgress();
                        });

                        ThreadPool.QueueUserWorkItem(HandleSingleCheckout, checkout);
                        continue;
                    }

                    if (checkout.Status == CheckoutStatus.Finished)
                    {
                        await ExecuteOnUIThread(() =>
                        {
                            checkout.CheckoutOpen();
                        });

                        if (!CheckoutsInProgress())
                        {
                            RunCheckoutsFlag = false;
                        }
                    }
                }
            }
        }

        private bool CheckoutsInProgress()
        {
            List<bool> bools = new List<bool>();

            foreach (var checkout in Checkouts)
            {
                bools.Add(checkout.Status == CheckoutStatus.InProgress);
            }

            return bools.Contains(true);
        }

        /// <summary>
        /// Delegate used in the threadpool <br />
        /// Handles the full transaction between the checkout and the customer
        /// </summary>
        /// <param name="state">The checkout object</param>
        private void HandleSingleCheckout(Object state)
        {
            Checkout checkout = (Checkout) state;

            Stopwatch stopwatch = Stopwatch.StartNew();
            SellToCustomer(checkout);
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds < 5000)
            {
                int sleepTime = (int) (5000 - stopwatch.ElapsedMilliseconds);
                Thread.Sleep(sleepTime);
            }

            checkout.CheckoutFinished();
        }

        /// <summary>
        /// Tries to sell a ticket to first customer in the queue
        /// </summary>
        /// <param name="checkout">The checkout the customer buys the ticket from</param>
        private void SellToCustomer(Checkout checkout)
        {
            Customer customer = GetNextCustomer();
            if (customer == null || Rooms.Count == 0)
                return;

            Room room = Rooms[customer.PickRandomMovie(Rooms.Count)];
            Ticket ticket = checkout.SellTicket(room, customer);
            if (ticket == null)
                return;

            customer.Ticket = ticket;
            AddToTotalSoldTickets(room.Movie);
            SendCustomerToLobby(customer);
        }

        /// <summary>
        /// Updates the total sold tickets amount to include the newly sold ticket
        /// </summary>
        /// <param name="movie">The movie a ticket was sold for</param>
        private void AddToTotalSoldTickets(Movie movie)
        {
            lock(SoldTickets)
            {
                if (SoldTickets.TryGetValue(movie, out int soldAmount))
                {
                    int newAmount = soldAmount +1;
                    SoldTickets[movie] = newAmount;
                    return;
                }

                SoldTickets.Add(movie, 1);
            }
        }

        /// <summary>
        /// Adds a new customer to the lobby list
        /// </summary>
        /// <param name="customer">Customer to add</param>
        private void SendCustomerToLobby(Customer customer)
        {
            lock(Lobby)
            {
                Lobby.Add(customer);
            }
        }

        /// <summary>
        /// Grabs the next customer in the queue
        /// </summary>
        /// <returns>The customer object when the queue is not empty, otherwise null</returns>
        private Customer GetNextCustomer()
        {
            lock(Queue)
            {
                if (Queue.Count == 0)
                    return null;

                Customer customer = Queue.Dequeue();
                //update UI with new amount of customers in queue
                return customer;
            }
        }

        public void AddCustomerToQueue(Customer customer)
        {
            lock(Queue)
            {
                Queue.Enqueue(customer);
            }
        }

        public void PlayRoom(Room room) 
        {
            room.Thread = new Thread(room.Play);
            room.Thread.Start();

			// We keep this to show that we know how it normally works,
			// our solution seems more fun in our case tho

			//Thread thread= new Thread(room.Play);
			//thread.Start();
		}

		public void CleanRoom(Room room)
        {
            room.Thread = new Thread(room.Clean);
            room.Thread.Start();

            // We keep this to show that we know how it normally works,
            // our solution seems more fun in our case tho

            //Thread cleanRoom = new Thread(room.Clean);
            //cleanRoom.Start();
        }

        public void UnnamedWhileLoop()
        {
            RunRoomsFlag = !RunRoomsFlag;

            while(RunRoomsFlag)//TODO Becomes open/close button or something
			{
				RunRooms();
            }
        }

		public void RunRooms()
        {
            foreach (Room room in Rooms)
            {
                switch(room.Status)
                {
                    case RoomStatus.Open:
						room.Status = RoomStatus.ReadyToPlay;
                        //Todo Insert logic to add people to room here
						break;
                    case RoomStatus.ReadyToPlay:
						room.Status = RoomStatus.Playing;
						PlayRoom(room);
						break;
                    case RoomStatus.FinishedPlaying:
						//Todo Throw out the people in the room
						room.Status = RoomStatus.Cleaning;
						CleanRoom(room);
						break;
                    case RoomStatus.FinishedCleaning:
                        //Insert logic to change movies etc.
						room.Status = RoomStatus.Open;
						break;
                }
			}
		}

        private IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }
    }
}
