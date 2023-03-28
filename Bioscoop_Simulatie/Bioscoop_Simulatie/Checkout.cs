﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioscoop_Simulatie
{
    public class Checkout : INotifyPropertyChanged
    {
        public CheckoutStatus Status { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

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
            OnPropertyChanged("Status");
            //update UI with new status
        }

        public void CheckoutInProgress()
        {
            Status = CheckoutStatus.InProgress;
            OnPropertyChanged("Status");
            //update UI with new status
        }

        public void CheckoutFinished() 
        {
            Status = CheckoutStatus.Finished;
        }

        public void CheckoutClosed()
        {
            Status = CheckoutStatus.Closed;
            OnPropertyChanged("Status");
            //update UI with new status
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
