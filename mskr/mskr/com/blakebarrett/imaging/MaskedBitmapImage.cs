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
        private ImageBrush mask;
        private Image image;
        private Grid container;

        public MaskedBitmapImage(Stream selectedImage, String relativeUrl)
        {
            BitmapImage source = new BitmapImage();
            source.SetSource(selectedImage);
            this.source = source;
            ChangeMask(relativeUrl);
        }

        private void Render()
        {
            container = new Grid();
            image = new Image();
            image.Source = source;

            image.Stretch = Stretch.UniformToFill;
            image.OpacityMask = GetMask();

            container.Children.Add(image);
        }

        public void ChangeMask(String relativeUrl) 
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource =
            new BitmapImage(
                new Uri(relativeUrl, UriKind.Relative)
            );
            brush.Stretch = Stretch.Uniform;
            this.mask = brush;
            Render();
        }

        public ImageBrush GetMask()
        {
            return this.mask;
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
