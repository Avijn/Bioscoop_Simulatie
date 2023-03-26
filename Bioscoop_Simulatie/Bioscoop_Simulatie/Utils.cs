using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Bioscoop_Simulatie
{
    internal static class Utils
    {
        /// <summary>
        /// Creates a bitmap image from a path
        /// Is used for creating an image in the UI
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage CreateImage(String fileName)
        {
            string assetsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");

            return new BitmapImage(new Uri(Path.Combine(assetsFolderPath, fileName)));
        }
    }
}
