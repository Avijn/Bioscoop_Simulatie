using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Bioscoop_Simulatie
{
    /// <summary>
    /// Class to hold UI components for the 2 registers
    /// </summary>
    public class UICheckout
    {
        private TextBlock Status;

        public UICheckout(TextBlock status)
        {
            Status = status;
        }

        /// <summary>
        /// Set the status of a register, can by waiting, busy or closed
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(CheckoutStatus status)
        {
            this.Status.Text = "Status: " + status.ToString();
        }
    }
}
