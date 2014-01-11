using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
            photoChooserTask.Show();
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //MessageBox.Show(e.ChosenPhoto.Length.ToString());

                //Code to display the photo on the page in an image control named myImage.
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                PreviewImage.Source = bmp;

                PageTitle.Visibility = Visibility.Collapsed;
                // TODO: Render full-sized masked image and save that.
                // Also, offer the ability to "stretch" or "constrain" the mask mode.
                // it appears as though masks are stretched to fit their parent images by default.
                // Perhaps the parent image needs to be constrained to the aspect ratio of the mask image at some point.
            }
        }
    }
}