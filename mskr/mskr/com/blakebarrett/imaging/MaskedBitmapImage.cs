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
            image = new Image();
            image.Source = source;
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = mask;
            image.OpacityMask = brush;
        }

        public WriteableBitmap ImageSource()
        {
            return new WriteableBitmap(this.image, null);
        }

        public void WriteToFile(String filename)
        {
            ImageSaver.SaveImage(image, filename);
        }
    }
}
