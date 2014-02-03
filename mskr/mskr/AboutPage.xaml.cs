using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace mskr
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
            AboutTextBlock.Text = ABOUT_MSKR + "\n\n" + ABOUT_BLAKE + "\n\n" + ABOUT_TIFFANY;
        }

        private static String ABOUT_MSKR = "mskr lets you shape layer masks over images and save them. Small app making small claims. If you don't like it, use Photoshop. \nmskr was written by Blake Barrett with design support from Tiffany Taylor. \nMore info can be found at http://mskr.co";
        private static String ABOUT_BLAKE = "About Blake: \"I am a software engineer, photographer, traveler, surfer, yogi, motorcyclist. I have been spotted drinking iced tea in various cafes across the world.\"";
        private static String ABOUT_TIFFANY = "About Tiffany: \"I am a designer and illustrator currently living in San Francisco. My passions are concept art, illustration, photography, and design for mobile and web. I have also studied Japanese for several years and am an avid corgi admirer.\"";
    }
}