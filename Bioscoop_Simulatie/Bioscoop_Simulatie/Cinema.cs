using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Bioscoop_Simulatie
{
    class Cinema
    {
        public Queue<Customer> Queue { get; set; }
        public List<Checkout> Checkouts { get; set; }
        public List<Customer> Lobby { get; set; }
        public List<Room> Rooms { get; set; }
        public Dictionary<Movie, int> SoldTickets { get; set; }

        public Cinema()
        {
            Queue = new Queue<Customer>();
            Checkouts = new List<Checkout>();
            Lobby = new List<Customer>();
            Rooms = new List<Room>();
            SoldTickets = new Dictionary<Movie, int>();
        }

        public void OpenCheckouts()
        {
            foreach (var checkout in Checkouts)
            {
                checkout.CheckoutOpen();
            }
        }

        public void HandleCheckouts()
        {
            while (Queue.Count != 0)
            {
                foreach (var checkout in Checkouts)
                {
                    if (checkout.Status == CheckoutStatus.Open)
                    {
                        checkout.CheckoutInProgress();
                        ThreadPool.QueueUserWorkItem(HandleSingleCheckout, checkout);
                    }
                }
            }
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

            if (stopwatch.ElapsedMilliseconds < 1500)
            {
                int sleepTime = (int) (1500 - stopwatch.ElapsedMilliseconds);
                Thread.Sleep(sleepTime);
            }

            checkout.CheckoutOpen();
        }

        /// <summary>
        /// Tries to sell a ticket to first customer in the queue
        /// </summary>
        /// <param name="checkout">The checkout the customer buys the ticket from</param>
        private void SellToCustomer(Checkout checkout)
        {
            Customer customer = GetNextCustomer();
            if (customer == null)
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
    }
}
