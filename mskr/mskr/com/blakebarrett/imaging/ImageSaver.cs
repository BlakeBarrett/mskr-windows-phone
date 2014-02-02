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
        public static Boolean SaveToCameraRoll = false;
        public static Boolean SaveToAlbum = false;

        private static String GetNowString()
        {
            DateTime now = DateTime.Now;
            return now.Ticks.ToString();
        }

        public static WriteableBitmap SaveImage(FrameworkElement image)
        {
            String filename =  GetNowString() + "_mskr";
            return SaveImage(image, filename);
        }

        public static WriteableBitmap SaveImage(WriteableBitmap image)
        {
            String filename = GetNowString() + "_mskr";
            return SaveImage(image, filename);
        }

        public static WriteableBitmap SaveImage(FrameworkElement element, String filename)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(element, null);
            return SaveImage(writeableBitmap, filename);
        }

        public static WriteableBitmap SaveImage(WriteableBitmap writeableBitmap, String filename) 
        {
            using (System.IO.MemoryStream s = new System.IO.MemoryStream())
            {
                // save the longest edge.
                int width = writeableBitmap.PixelWidth;
                int height = writeableBitmap.PixelHeight;
                writeableBitmap.SaveJpeg(s, width, height, 0, 100);
                WriteStreamToFile(s, filename);
            }
            return writeableBitmap;
        }

        public static void WriteStreamToFile(System.IO.Stream stream, String filename)
        {
            Microsoft.Xna.Framework.Media.MediaLibrary library = new Microsoft.Xna.Framework.Media.MediaLibrary();

            // write to "Saved Pictures"
            if (SaveToAlbum)
            {
                stream.Position = 0;
                library.SavePicture(filename, stream);
                Console.WriteLine("Saved Image: " + filename + " to 'Saved Pictures' album");
            }

            // write to "Camera Roll"
            if (SaveToCameraRoll)
            {
                stream.Position = 0;
                library.SavePictureToCameraRoll(filename, stream);
                Console.WriteLine("Saved Image: " + filename + " to Camera Roll");
            }

            stream.Dispose();
        }
    }
}
