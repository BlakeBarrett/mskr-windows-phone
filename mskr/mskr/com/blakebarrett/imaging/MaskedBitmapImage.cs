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

        public MaskedBitmapImage(WriteableBitmap writableBitmap, String relativeUrl)
        {
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    writableBitmap.SaveJpeg(ms, (int)writableBitmap.PixelWidth, (int)writableBitmap.PixelHeight, 0, 100);
            //    this.source = new BitmapImage();
            //    this.source.SetSource(ms);
            //}
            ChangeImage(writableBitmap);
            ChangeMask(relativeUrl);
        }

        private void Render()
        {
            container = new Grid();
            container.Background = new SolidColorBrush(Colors.White);

            // See link for docs on `Image` class: http://msdn.microsoft.com/en-us/library/system.windows.controls.image(v=vs.110).aspx
            image = new Image();
            //System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
            //rectangle.Margin = new System.Windows.Thickness(0);
            //rectangle.Fill = new SolidColorBrush(Colors.White);

            // See link for more on `Stretch` enum: http://msdn.microsoft.com/en-us/library/system.windows.media.stretch(v=vs.110).aspx
            image.Stretch = Stretch.UniformToFill;
            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            image.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            image.OpacityMask = GetMask();
            image.Source = source;

            container.Children.Add(image);
        }

        void image_ImageOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            Image img = (Image)sender;
            img.OpacityMask = GetMask();
        }

        public void ChangeMask(String relativeUrl) 
        {
            this.mask = GetBrushMaskForUrl(relativeUrl);
            Render();
        }

        public void ChangeImage(WriteableBitmap image)
        {
            this.source = ToBitmapImage(image);
            Render();
        }

        public static BitmapImage ToBitmapImage(WriteableBitmap wbm)
        {
            BitmapImage bmp = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                wbm.SaveJpeg(ms, wbm.PixelWidth, wbm.PixelHeight, 0, 100);
                bmp.SetSource(ms);
            }
            return bmp;
        }

        public static ImageBrush GetBrushMaskForUrl(String relativeUrl)
        {
            BitmapImage mask = new BitmapImage(new Uri(relativeUrl, UriKind.Relative));
            return GetMaskForBitmap(mask);
        }

        public static ImageBrush GetMaskForBitmap(BitmapImage mask) {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = mask;
            brush.Stretch = Stretch.Uniform;
            return brush;
        }

        public ImageBrush GetMask()
        {
            return this.mask;
        }

        public Image GetImage()
        {
            return this.image;
        }

        public WriteableBitmap ImageSource()
        {
            return new WriteableBitmap(this.container, null);
        }
        /*
        public void WriteToFile()
        {
            this.container.Width = source.PixelWidth;
            this.container.Height = source.PixelHeight;
            ImageSaver.SaveImage(container);
        }
        */
    }
}
