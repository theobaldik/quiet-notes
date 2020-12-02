/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuietNotes
{
    /// <summary>
    /// Interaction logic for FrameButton.xaml
    /// </summary>
    public partial class FrameButton : Button
    {
        public FrameButton()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Colors.Transparent);
        }

        public ImageSource Image
        {
            get { return image.Source; }
            set
            {
                image.Source = value;
                image.Width = ((BitmapImage)value).PixelWidth;
                image.Height = ((BitmapImage)value).PixelHeight;
            }
        }

        public void SetRotation(double angle)
        {
            if (angle == 0)
                image.RenderTransform = null;
            else
                image.RenderTransform =
                    new RotateTransform(angle,
                    ((BitmapImage)image.Source).PixelWidth / 2.0,
                    ((BitmapImage)image.Source).PixelHeight / 2.0);
        }
    }
}
