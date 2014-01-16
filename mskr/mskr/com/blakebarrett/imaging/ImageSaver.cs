using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace mskr.com.blakebarrett.imaging
{
    class ImageSaver
    {
        public static Boolean SaveToCameraRoll = true;
        public static Boolean SaveToAlbum = true;

        public static void SaveImage(FrameworkElement image)
        {
            DateTime now = DateTime.Now;
            String filename = now.Ticks.ToString() + "_mskr";
            SaveImage(image, filename);
        }

        public static void SaveImage(FrameworkElement element, String filename)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(element, null);
            using (System.IO.MemoryStream s = new System.IO.MemoryStream())
            {
                // save the longest edge.
                int width = 0;
                if (element.Width != double.NaN)
                {
                    width = (int)element.Width;
                }
                if (element.ActualWidth != double.NaN)
                {
                    width = Math.Max(width, (int)element.ActualWidth);
                }

                int height = 0;
                if (element.Height != double.NaN)
                {
                    height = (int)element.Height;
                }
                if (element.ActualHeight != double.NaN)
                {
                    height = Math.Max(height, (int)element.ActualHeight);
                }

                writeableBitmap.SaveJpeg(s, width, height, 0, 100);
                WriteStreamToFile(s, filename);
            }
        }

        public static void WriteStreamToFile(System.IO.Stream stream, String filename)
        {
            Microsoft.Xna.Framework.Media.MediaLibrary library = new Microsoft.Xna.Framework.Media.MediaLibrary();

            // write to "Saved Pictures"
            if (SaveToAlbum)
            {
                stream.Position = 0;
                library.SavePicture(filename, stream);
            }

            // write to "Camera Roll"
            if (SaveToCameraRoll)
            {
                stream.Position = 0;
                library.SavePictureToCameraRoll(filename, stream);
            }

            stream.Dispose();
        }
    }
}
