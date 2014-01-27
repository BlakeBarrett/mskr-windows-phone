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
        String selectedMask = "resources/crclmsk.png";

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

        private void AddLayerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            ImageSaver.SaveImage(PreviewImage);
            ImageSaver.SaveToAlbum = false;
            WriteableBitmap savedImage = ImageSaver.SaveImage(ContentPanel);

            SaveImageGrid.Visibility = Visibility.Collapsed;
            SelectImageGrid.Visibility = Visibility.Visible;

            BackroundImage.Source = savedImage;
        }

        private void ListPicker_SelectionChanged(object sender, EventArgs e)
        {
            String selectedMaskString = ((ListPickerItem)((ListPicker)sender).SelectedItem).Content.ToString();
            this.selectedMask = "resources/" + selectedMaskString.ToLower() + "msk.png";
            if (mskdBmpImg != null)
            {
                mskdBmpImg.ChangeMask(this.selectedMask);
                PreviewImage.OpacityMask = mskdBmpImg.GetMask();
            }
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                Stream selected = e.ChosenPhoto;

                //String maskString = "resources/" + ((ListPickerItem)MaskListPicker.SelectedItem).Content.ToString().ToLower() + "msk.png";

                // Code to display the photo on the page in an image control named myImage.
                mskdBmpImg = new MaskedBitmapImage(e.ChosenPhoto, this.selectedMask);
                WriteableBitmap source = mskdBmpImg.ImageSource();

                PreviewImage.Source = source;
                PreviewImage.OpacityMask = mskdBmpImg.GetMask();

                SelectImageGrid.Visibility = Visibility.Collapsed;
                SaveImageGrid.Visibility = Visibility.Visible;

                // save every step of the way
                //BackroundImage.Source = ImageSaver.SaveImage(ContentPanel);

                // TODO: Offer the ability to "stretch" or "constrain" the mask mode.
                // it appears as though masks are stretched to fit their parent images by default.
                // Perhaps the parent image needs to be constrained to the aspect ratio of the mask image at some point.
            }
        }
    }
}