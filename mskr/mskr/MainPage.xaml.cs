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
    using System.IO;
    using Microsoft.Phone.Tasks;
    using com.blakebarrett.imaging;

    public partial class MainPage : PhoneApplicationPage   
    {
        PhotoChooserTask photoChooserTask;
        MaskedBitmapImage mskdBmpImg;

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
            // mskdBmpImg.WriteToFile();
            // ImageSaver.SaveImage(PreviewImage);
            ImageSaver.SaveToAlbum = false;
            ImageSaver.SaveImage(ContentPanel);

            SaveImageGrid.Visibility = Visibility.Collapsed;
            SelectImageGrid.Visibility = Visibility.Visible;
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                Stream selected = e.ChosenPhoto;

                // Code to display the photo on the page in an image control named myImage.
                mskdBmpImg = new MaskedBitmapImage(e.ChosenPhoto, "resources/powmsk.png");
                // mskdBmpImg = new MaskedBitmapImage(e.ChosenPhoto, "resources/crclmsk.png");
                WriteableBitmap source = mskdBmpImg.ImageSource();

                PreviewImage.Source = source;
                BackroundImage.Source = source;

                SelectImageGrid.Visibility = Visibility.Collapsed;
                SaveImageGrid.Visibility = Visibility.Visible;
                // TODO: Offer the ability to "stretch" or "constrain" the mask mode.
                // it appears as though masks are stretched to fit their parent images by default.
                // Perhaps the parent image needs to be constrained to the aspect ratio of the mask image at some point.
            }
        }
    }
}