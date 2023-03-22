using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioscoop_Simulatie
{
    internal class Customer
    {
        public int Age { get; }
        public Ticket Ticket { get; set; }

        public Customer(int age)
        {
            Age = age;
        }
    }
}
