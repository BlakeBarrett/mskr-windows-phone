using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace mskr.com.blakebarrett.imaging
{
    class MaskedBitmapImage
    {
        private BitmapImage source;
        private BitmapImage mask;
        private Image image;
        private Grid container;

        public MaskedBitmapImage(Stream selectedImage, String relativeUrl)
        {
            BitmapImage source = new BitmapImage();
            source.SetSource(selectedImage);

            Uri uri = new Uri("ms-appx:///" + relativeUrl);
            BitmapImage mask = new BitmapImage(uri);

            this.source = source;
            this.mask = mask;
            Render();
        }

        private void Render()
        {
            container = new Grid();
            image = new Image();
            image.Source = source;
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = mask;
            image.OpacityMask = brush;

            image.Width = source.PixelWidth;
            image.Height = source.PixelHeight;

            container.Children.Add(image);
        }

        public WriteableBitmap ImageSource()
        {
            return new WriteableBitmap(this.container, null);
        }

        public void WriteToFile()
        {
            this.container.Width = source.PixelWidth;
            this.container.Height = source.PixelHeight;
            ImageSaver.SaveImage(container);
        }
    }
}
