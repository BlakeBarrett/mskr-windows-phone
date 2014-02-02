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
using Microsoft.Phone.Shell;

namespace mskr
{
    using System.IO;
    using Microsoft.Phone.Tasks;
    using com.blakebarrett.imaging;

    public partial class MainPage : PhoneApplicationPage   
    {
        public const String SELECT_IMAGE = "Select Image";
        public const String ADD_LAYER = "Add Layer";
        PhotoChooserTask photoChooserTask;
        MaskedBitmapImage mskdBmpImg;
        String selectedMask = "resources/crclmsk.png";
        
        // Constructor
        public MainPage()
        {
            InitializeComponent(); 
            CreateNew();
        }

        // Simple button Click event handler to take us to the second page
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            switch (ActionLabel.Text)
            {
                case SELECT_IMAGE:
                    photoChooserTask.Show();
                    break;
                case ADD_LAYER:
                    AddLayer();
                    break;
            };
        }

        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void AddLayerButton_Click(object sender, EventArgs e)
        {
            AddLayer();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            SetActionLabel(SELECT_IMAGE);
            BackgroundImage = new Image();
            PreviewImage = new Image();
            CreateNew();
        }

        private void ListPicker_SelectionChanged(object sender, EventArgs e)
        {
            String selectedMaskString = ((ListPickerItem)((ListPicker)sender).SelectedItem).Content.ToString();
            this.selectedMask = "resources/" + selectedMaskString.ToLower() + "msk.png";
            if (mskdBmpImg != null)
            {
                ChangeMask(this.selectedMask);
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
                SetImages(mskdBmpImg.ImageSource());

                WriteableBitmap source = mskdBmpImg.ImageSource();
                PreviewImage.Source = source;
                PreviewImage.OpacityMask = mskdBmpImg.GetMask();

                // save every step of the way
                //BackroundImage.Source = ImageSaver.SaveImage(ContentPanel);

                // TODO: Offer the ability to "stretch" or "constrain" the mask mode.
                // it appears as though masks are stretched to fit their parent images by default.
                // Perhaps the parent image needs to be constrained to the aspect ratio of the mask image at some point.

                MakeAppActive();
                AddLayer();
                SetActionLabel(ADD_LAYER);
            }
        }

        private void AddLayer()
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(ContentPanel, null);
            mskdBmpImg = new MaskedBitmapImage(writeableBitmap, this.selectedMask);
            //WriteableBitmap writeableBitmap = mskdBmpImg.ImageSource();
            //SetImages(writeableBitmap);
            SetImages(mskdBmpImg.ImageSource());
        }

        private void MakeAppActive() 
        {
            MaskListPicker.IsEnabled = true;
            ApplicationBar.IsVisible = true;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true; // save
            (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = true; // add layer
            (ApplicationBar.Buttons[2] as ApplicationBarIconButton).IsEnabled = true; // delete/new
        }

        private void SaveImage()
        {
            ImageSaver.SaveToAlbum = false;
            ImageSaver.SaveToCameraRoll = true;
            WriteableBitmap savedImage = ImageSaver.SaveImage(new WriteableBitmap(ContentPanel, null));
            //WriteableBitmap savedImage = ImageSaver.SaveImage(mskdBmpImg.ImageSource());
            SetImages(savedImage);
        }

        private void ChangeMask(String selectedMask)
        {
            mskdBmpImg.ChangeMask(selectedMask);
            PreviewImage.OpacityMask = mskdBmpImg.GetMask();
            //SetImages(mskdBmpImg.ImageSource());
        }

        private void SetImages(WriteableBitmap writeableBitmap)
        {
            BitmapImage bitmap = MaskedBitmapImage.ToBitmapImage(writeableBitmap);
            BackgroundImage.Source = bitmap;
            PreviewImage.Source = bitmap;
        }

        private void CreateNew()
        {
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }

        private void SetActionLabel(String label) {
            ActionLabel.Text = label;
        }
    }
}