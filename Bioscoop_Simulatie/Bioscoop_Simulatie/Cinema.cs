using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace Bioscoop_Simulatie
{
    public class Cinema : INotifyPropertyChanged
    {
        public Queue<Customer> Queue { get; set; }
        public List<Checkout> Checkouts { get; set; }
        public List<Customer> Lobby { get; set; }
        public List<Room> Rooms { get; set; }
        public Dictionary<Movie, int> SoldTickets { get; set; }
        public bool RunRoomsFlag { get; set; }
        public bool RunCheckoutsFlag { get; set; }
        public bool IsCheckoutsOpen { get; set; }
        public bool IsThreadRunning { get; set; }
        public Thread Run { get; }
        public bool RunCinemaFlag { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Cinema()
        {
            Queue = new Queue<Customer>();
            Checkouts = new List<Checkout>();
            Lobby = new List<Customer>();
            Rooms = new List<Room>();
            SoldTickets = new Dictionary<Movie, int>();
            Run = new Thread(RunCinema);

            RunRoomsFlag = false;
            RunCheckoutsFlag = false;
            RunCinemaFlag = false;
            IsCheckoutsOpen = false;
            IsThreadRunning = false;

            PopulateCinema();
        }

        private void PopulateCinema()
        {
            AddRegisters();
            PopulateRooms();
            //PopulateQueue();
        }

        private void PopulateQueue()
        {
            if (Lobby.Count >= TotalAmountOfSeats() || Queue.Count > 0)
                return;

            Random random = new Random();

            for (int i = Queue.Count; i < 50; i++)
            {
                AddCustomerToQueue(new Customer(random.Next(3, 100)));
            }
        }

        private int TotalAmountOfSeats()
        {
            int total = 0;

            foreach (var room in Rooms)
            {
                total += room.Seats;
            }

            return total;
        }

        private void AddRegisters()
        {
            Checkouts.Add(new Checkout());
            Checkouts.Add(new Checkout());
        }

        private void PopulateRooms()
        {
            Rooms.Add(new Room("Room 1", 9, 5000) { Movie = new Movie("Shrek 4", 12000, 13) });
            Rooms.Add(new Room("Room 2", 9, 6000) { Movie = new Movie("Shrek 5", 10000, 21) });
            Rooms.Add(new Room("Room 3", 9, 3000) { Movie = new Movie("LOTR: Fellowship of the Ring", 15000, 16) });
        }

        public void RunCinema()
        {
            while (RunCinemaFlag)
            {
                RunRoomsFlag = true;

                if (!IsCheckoutsOpen)
                {
                    OpenCheckouts();
                    IsCheckoutsOpen = true;
                }

                PopulateQueue();

                while ((Queue.Count > 0 || RunCheckoutsFlag) && (Lobby.Count < TotalAmountOfSeats()))
                {
                    HandleCheckouts();
                }

                while (RunRoomsFlag)
                {
                    RunRooms();
                }
            }
        }

        public async void OpenCheckouts()
        {
            foreach (var checkout in Checkouts)
            {
                await ExecuteOnUIThread(() => checkout.CheckoutOpen());
            }
        }

        public async void CloseCheckouts()
        {
            foreach (var checkout in Checkouts)
            {
                await ExecuteOnUIThread(() => checkout.CheckoutClosed());
            }
        }

        private async void HandleCheckouts()
        {
            foreach (var checkout in Checkouts)
            {
                if (checkout.Status == CheckoutStatus.Open && Queue.Count > 0)
                {
                    RunCheckoutsFlag = true;
                    await ExecuteOnUIThread(() => checkout.CheckoutInProgress());
                    ThreadPool.QueueUserWorkItem(HandleSingleCheckout, checkout);
                    continue;
                }

                if (!CheckoutsInProgress())
                {
                    RunCheckoutsFlag = false;
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
        private async void HandleSingleCheckout(Object state)
        {
            Checkout checkout = (Checkout) state;

            Stopwatch stopwatch = Stopwatch.StartNew();
            SellToCustomer(checkout);
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds < 1500)
            {
                int sleepTime = (int)(1500 - stopwatch.ElapsedMilliseconds);
                Thread.Sleep(sleepTime);
            }

            await ExecuteOnUIThread(() => checkout.CheckoutOpen());
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
                ExecuteOnUIThread(() => OnPropertyChanged("Lobby"));
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

                ExecuteOnUIThread(() => OnPropertyChanged("Queue"));
                return customer;
            }
        }

        private async void AddCustomerToQueue(Customer customer)
        {
            lock(Queue)
            {
                Queue.Enqueue(customer);
            }

            await ExecuteOnUIThread(() => OnPropertyChanged("Queue"));
        }

        private void PlayRoom(Room room) 
        {
            room.Thread = new Thread(room.Play);
            room.Thread.Start();

			// We keep this to show that we know how it normally works,
			// our solution seems more fun in our case tho

			//Thread thread= new Thread(room.Play);
			//thread.Start();
		}

        private void CleanRoom(Room room)
        {
            room.Thread = new Thread(room.Clean);
            room.Thread.Start();

            // We keep this to show that we know how it normally works,
            // our solution seems more fun in our case tho

            //Thread cleanRoom = new Thread(room.Clean);
            //cleanRoom.Start();
        }

        private async void RunRooms()
        {
            foreach (Room room in Rooms)
            {
                switch(room.Status)
                {
                    case RoomStatus.Open:
                        room.Status = RoomStatus.SeatingCustomers;
                        await AddCustomersToRoom(room);
                        break;

                    case RoomStatus.ReadyToPlay:
						room.Status = RoomStatus.Playing;
                        room.Img = room.Playing;
						await ExecuteOnUIThread(() => room.OnPropertyChanged("Img"));
                        PlayRoom(room);
						break;

                    case RoomStatus.FinishedPlaying:
                        RemoveCustomersFromRoom(room);
						room.Status = RoomStatus.Cleaning;
                        room.Img = room.Cleaning;
						await ExecuteOnUIThread(() => room.OnPropertyChanged("Img"));
                        CleanRoom(room);
						break;

                    case RoomStatus.FinishedCleaning:
                        room.Status = RoomStatus.WaitingToOpen;
                        room.Img = room.Waiting;
						await ExecuteOnUIThread(() => room.OnPropertyChanged("Img"));
                        break;

                    case RoomStatus.WaitingToOpen:
                        room.Status = RoomStatus.SeatCustomers;
                        room.Sleep(1500);
                        break;

                    default:
                        break;
                }
			}

            if (AllCustomersSeated())
            {
                RunRoomsFlag = false;

                foreach (var room in Rooms)
                {
                    room.Status = RoomStatus.Open;
                }
            }
        }

        private bool AllCustomersSeated()
        {
            List<bool> bools = new List<bool>();

            foreach (var room in Rooms)
            {
                bools.Add(room.Status == RoomStatus.SeatCustomers);
            }

            return !bools.Contains(false);
        }

        private async Task<bool> AddCustomersToRoom(Room room)
        {
            for (int i = 0; i < Lobby.Count; i++)
            {
                Customer customer = Lobby[i];

                if (room.Movie == customer.Ticket.Movie)
                {
                    Lobby.Remove(customer);
                    await ExecuteOnUIThread(() => OnPropertyChanged("Lobby"));
                    room.TakenSeats++;
                    await ExecuteOnUIThread(() => room.OnPropertyChanged("GetSeatImage"));
                }
            }

            room.Status = RoomStatus.ReadyToPlay;

            return true;
        }

        private async void RemoveCustomersFromRoom(Room room)
        {
            room.TakenSeats = 0;
            await ExecuteOnUIThread(() => room.OnPropertyChanged("GetSeatImage"));
            //update seats UI to empty seats
        }

        private IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }

        public SolidColorBrush GetCheckoutColor()
        {
            if(IsCheckoutsOpen)
            {
                return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 104, 184, 102));
            } else
            {
                return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 253, 73, 73));
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
