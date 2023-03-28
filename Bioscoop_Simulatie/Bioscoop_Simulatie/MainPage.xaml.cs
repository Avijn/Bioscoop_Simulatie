using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Bioscoop_Simulatie
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Cinema Cinema = new Cinema();

        public MainPage()
        {
            this.InitializeComponent();
            PopulateCinema();
        }

        private void PopulateCinema()
        {
            Random random = new Random();
            Queue<Customer> customers = new Queue<Customer>();

            for (int i = 0; i < 15; ++i)
            {
                Customer customer = new Customer(random.Next(0, 51));
                Debug.WriteLine($"customer::{0}", customer);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"cinema.Queue.Count::{0}", Cinema.Queue.Count);
        }
    }
}
