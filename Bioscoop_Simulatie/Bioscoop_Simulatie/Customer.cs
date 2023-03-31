using System;

namespace Bioscoop_Simulatie
{
    public class Customer
    {
        public int Age { get; }
        public Ticket Ticket { get; set; }

        public Customer(int age)
        {
            Age = age;
        }

        public int PickRandomMovie(int max)
        {
            Random random = new Random();
            return random.Next(max);
        }
    }
}
