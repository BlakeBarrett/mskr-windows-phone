using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public MaskedBitmapImage(BitmapImage source, BitmapImage mask)
        {
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

        public void WriteToFile(String filename)
        {

        }
    }
}
