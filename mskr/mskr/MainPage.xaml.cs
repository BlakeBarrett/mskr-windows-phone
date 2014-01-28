﻿using System;
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

        private void NewCompositionButton_Click(object sender, EventArgs e)
        {
            SetActionLabel(SELECT_IMAGE);
            PreviewImage.Source = null;
            BackgroundImage.Source = null;
            CreateNew();
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

                // save every step of the way
                //BackroundImage.Source = ImageSaver.SaveImage(ContentPanel);

                // TODO: Offer the ability to "stretch" or "constrain" the mask mode.
                // it appears as though masks are stretched to fit their parent images by default.
                // Perhaps the parent image needs to be constrained to the aspect ratio of the mask image at some point.

                AddLayer();
                SetActionLabel(ADD_LAYER);
            }
        }

        private void AddLayer()
        {
            BackgroundImage.Source = new WriteableBitmap(ContentPanel, null);
        }

        private void SaveImage()
        {
            ImageSaver.SaveToAlbum = false;
            ImageSaver.SaveToCameraRoll = true;
            WriteableBitmap savedImage = ImageSaver.SaveImage(ContentPanel);

            BackgroundImage.Source = savedImage;
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