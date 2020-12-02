/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QuietNotes
{
    /// <summary>
    /// Interaction logic for ColorPickRectangle.xaml
    /// </summary>
    public partial class ColorPickRectangle : WrapPanel
    {
        private SolidColorBrush HoverBrush { get; set; }
        private SolidColorBrush DefaultBrush { get; set; }

        public ColorPickRectangle()
        {
            InitializeComponent();
            HoverBrush = new SolidColorBrush(Colors.White);
            DefaultBrush = new SolidColorBrush(Colors.Transparent);
        }

        private string color;
        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                rectColor.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(value));
            }
        }

        private void RectColor_MouseEnter(object sender, MouseEventArgs e)
        {
            Background = HoverBrush;
        }

        private void RectColor_MouseLeave(object sender, MouseEventArgs e)
        {
            Background = DefaultBrush;
        }

        private void RectColor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataHolder.CurrentNote.Color = Color;
        }
    }
}
