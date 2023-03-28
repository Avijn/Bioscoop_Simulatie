using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioscoop_Simulatie
{
    class Checkout
    {
        public CheckoutStatus Status { get; set; }

        public Checkout()
        {
            Status = CheckoutStatus.Closed;
        }

        /// <summary>
        /// Creates a new ticket to "sell" to the customer
        /// </summary>
        /// <param name="room">The room the movie is shown</param>
        /// <param name="customer">The customer that wants a ticket</param>
        /// <returns>A ticket object for the movie if possible, otherwise null</returns>
        public Ticket SellTicket(Room room, Customer customer)
        {
            if (customer.Age < room.Movie.AgeRestriction)
                return null;

            if (room.AddReservation())
                return new Ticket(room.Movie, room.Name);

            return null;
        }

        public void CheckoutOpen()
        {
            Status = CheckoutStatus.Open;
            //update UI with new status
        }

        public void CheckoutInProgress()
        {
            Status = CheckoutStatus.InProgress;
            //update UI with new status
        }

        public void CheckoutClosed()
        {
            Status = CheckoutStatus.Closed;
            //update UI with new status
        }
    }
}
