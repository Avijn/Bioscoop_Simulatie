using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioscoop_Simulatie
{
    internal class Ticket
    {
        public Movie Movie { get; }
        public string RoomName { get; }

        public Ticket(Movie movie, string name)
        {
            Movie = movie;
            RoomName = name;
        }
    }
}
