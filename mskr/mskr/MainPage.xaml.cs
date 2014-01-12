using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace mskr
{
    using Microsoft.Phone.Tasks;

    public partial class MainPage : PhoneApplicationPage   
    {
        PhotoChooserTask photoChooserTask;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }

        // Simple button Click event handler to take us to the second page
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            photoChooserTask.Show();
        }

        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            RasterizeStack(PreviewImage);
            SaveImageGrid.Visibility = Visibility.Collapsed;
            SelectImageGrid.Visibility = Visibility.Visible;
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                System.IO.Stream selected = e.ChosenPhoto;

                //Code to display the photo on the page in an image control named myImage.
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(selected);
                PreviewImage.Source = bmp;

                SelectImageGrid.Visibility = Visibility.Collapsed;
                SaveImageGrid.Visibility = Visibility.Visible;
                // TODO: Render full-sized masked image.
                // Also, offer the ability to "stretch" or "constrain" the mask mode.
                // it appears as though masks are stretched to fit their parent images by default.
                // Perhaps the parent image needs to be constrained to the aspect ratio of the mask image at some point.
            }
        }

        void RasterizeStack(UIElement stack)
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(stack, null);
            using (System.IO.MemoryStream s = new System.IO.MemoryStream())
            {
                writeableBitmap.SaveJpeg(s, (int)PreviewImage.ActualWidth, (int)PreviewImage.ActualHeight, 0, 100);
                String filename = DateTime.Now.Ticks.ToString() + ".jpg";
                WriteStreamToFile(s, filename);
            }
        }

        void WriteStreamToFile(System.IO.Stream stream, String filename)
        {
            Microsoft.Xna.Framework.Media.MediaLibrary library = new Microsoft.Xna.Framework.Media.MediaLibrary();

            // write to "Saved Pictures"
            stream.Position = 0;
            library.SavePicture(filename, stream);

            // write to "Camera Roll"
            //stream.Position = 0;
            //library.SavePictureToCameraRoll(filename, stream);

            stream.Dispose();
        }
    }
}