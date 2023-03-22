using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioscoop_Simulatie
{
    internal class Checkout
    {
        public Status Status { get; set; }
    }

    enum Status
    {
        Open,
        Closed,
        InProgress
    }
}
