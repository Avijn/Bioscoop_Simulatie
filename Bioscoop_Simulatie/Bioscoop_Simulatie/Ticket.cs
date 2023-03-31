namespace Bioscoop_Simulatie
{
    public class Ticket
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
