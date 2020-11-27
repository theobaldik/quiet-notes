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
using Logic;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ColorPickBut.xaml
    /// </summary>
    public partial class ColorPickBut : Button
    {
        public ColorPickBut()
        {
            InitializeComponent();
        }

        private string bgColor;
        public string BgColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(value));
            }
        }
        
        private void colorPickBut_Click(object sender, RoutedEventArgs e)
        {
            DataHolder.CurrentNote.Color = BgColor;
        }
    }
}
