using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for FrameButton.xaml
    /// </summary>
    public partial class FrameButton : Button
    {
        private BitmapImage image;
        public FrameButton()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Colors.White) { Opacity = 0 };            
        }

        private string icon;
        
        public string Icon
        {
            get { return icon; } 
            set
            {
                image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri($"\\Images\\{value}", UriKind.Relative);
                image.EndInit();
                IconImage.Source = image;
                IconImage.Width = image.PixelWidth;
                IconImage.Height = image.PixelHeight;
                icon = value;
            }
        }

        public void SetIconRotation(double angle)
        {
            if (angle == 0)
                IconImage.RenderTransform = null;
            else
                IconImage.RenderTransform = new RotateTransform(angle, image.PixelWidth / 2.0, image.PixelHeight / 2.0);
        }      
    }
}
