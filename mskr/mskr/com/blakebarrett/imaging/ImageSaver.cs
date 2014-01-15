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

        public static void SaveImage(Image image)
        {
            String filename = DateTime.Now.Ticks.ToString() + "_mskr.jpg";
            SaveImage(image, filename);
        }

        public static void SaveImage(Image image, String filename)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(image, null);
            using (System.IO.MemoryStream s = new System.IO.MemoryStream())
            {
                writeableBitmap.SaveJpeg(s, (int)image.ActualWidth, (int)image.ActualHeight, 0, 100);
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
