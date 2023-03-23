using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioscoop_Simulatie
{
    internal class Checkout
    {
        public CheckoutStatus Status { get; set; }

        public Checkout()
        {
            Status = CheckoutStatus.Closed;
        }

        public Ticket SellTicket(Room room)
        {
            
        }


        public void OpenCheckout()
        {
            Status = CheckoutStatus.Open;
        }

        public void ProgressedCheckout()
        {
            Status = CheckoutStatus.InProgress;
        }

        public void CloseCheckout()
        {
            Status = CheckoutStatus.Closed;
        }
    }

    enum CheckoutStatus
    {
        Open,
        InProgress,
        Closed
    }
}
