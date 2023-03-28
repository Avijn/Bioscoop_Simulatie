using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Bioscoop_Simulatie
{
    /// <summary>
    /// Class to hold UI components for the queue and lobby
    /// </summary>
    internal class UIStation
    {
        private TextBlock PeopleWaiting;

        public UIStation(TextBlock status)
        {
            this.PeopleWaiting = status;
        }

        /// <summary>
        /// Set the number of people waiting in a station (Queue or Lobby)
        /// </summary>
        /// <param name="nrOfPeople"></param>
        public void SetPeopleWaiting(int nrOfPeople)
        {
            this.PeopleWaiting.Text = "People waiting: " + nrOfPeople.ToString();
        }
    }
}
