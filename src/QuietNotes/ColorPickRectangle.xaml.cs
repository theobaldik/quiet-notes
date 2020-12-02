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
