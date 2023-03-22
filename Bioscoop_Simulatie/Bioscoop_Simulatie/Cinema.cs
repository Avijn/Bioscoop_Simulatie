using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioscoop_Simulatie
{
    internal class Cinema
    {
        public List<Customer> Queue { get; set; }
        public List<Checkout> Checkouts { get; set; }
        public List<Customer> Lobby { get; set; }
        public List<Room> Rooms { get; set; }
        public Dictionary<Movie, int> SoldTickets { get; set; }


    }
}
