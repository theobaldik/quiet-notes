/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using System;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace QuietNotes
{
    /// <summary>
    /// Interaction logic for TempPopup.xaml
    /// </summary>
    public partial class TempPopup : Popup
    {
        private DispatcherTimer Timer { get; set; }

        public TempPopup()
        {
            InitializeComponent();

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            Timer.Tick += delegate (object Sender, EventArgs e)
            {
                Timer.Stop();
                if (IsOpen) IsOpen = false;
            };
        }

        public void ShowText(string text)
        {
            label.Content = text;
            IsOpen = true;
            if (Timer.IsEnabled)
                Timer.Stop();
            Timer.Start();
        }
    }
}
