/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Logic;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer StatusTimer { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            NotesList.ItemsSource = DataHolder.CurrentNotebook.Notes;            
            DataContext = DataHolder.CurrentNote;

            greenBut.BgColor = NoteColor.Green;
            lightBlueBut.BgColor = NoteColor.LightBlue;
            darkBlueBut.BgColor = NoteColor.DarkBlue;
            yellowBut.BgColor = NoteColor.Yellow;
            orangeBut.BgColor = NoteColor.Orange;
            redBut.BgColor = NoteColor.Red;

            NotesList.SelectedItem = DataHolder.CurrentNote;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(NotesList.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("DateModified", ListSortDirection.Ascending));
            SizeChanged += MainWindow_SizeChanged;
            LocationChanged += MainWindow_LocationChanged;
            statusPopup.VerticalOffset = Top + Height - 22;
            statusPopup.HorizontalOffset = Left + Width - 1;
            statusPopup.FlowDirection = FlowDirection.RightToLeft;


            StatusTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            StatusTimer.Tick += delegate (object sender, EventArgs e)
            {
                StatusTimer.Stop();
                if (statusPopup.IsOpen) statusPopup.IsOpen = false;
            };
        }       

        private void ShowStatus(string text)
        {
            statusLabel.Content = text;
            statusPopup.IsOpen = true;
            if (StatusTimer.IsEnabled)
                StatusTimer.Stop();
            StatusTimer.Start();
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            statusPopup.VerticalOffset = Top + Height - 22;
            statusPopup.HorizontalOffset = Left + Width - 1;
            PopupColors.HorizontalOffset++;
            PopupColors.HorizontalOffset--;
            PopupList.HorizontalOffset++;
            PopupList.HorizontalOffset--;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            statusPopup.VerticalOffset = Top + Height - 22;
            statusPopup.HorizontalOffset = Left + Width - 1;
        }                

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            textBox.SelectedText = string.Empty;
            if (e.Key == Key.Tab)
            {
                int pos = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(pos, "  ");
                textBox.CaretIndex = pos + 2;
                e.Handled = true;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {                
            DragMove();
        }
        private void CloseShortcut(Object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {

        }
        private void Window_Activated(object sender, EventArgs e)
        {
            
        }

        private void ButList_Click(object sender, RoutedEventArgs e)
        {
            PopupList.IsOpen = !PopupList.IsOpen;
        }

        private void ButClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButMax_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                return;
            }                
            WindowState = WindowState.Maximized;
        }

        private void ButMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButPin_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
            if (Topmost)
            {
                ButPin.SetIconRotation(0);
                ShowStatus("Always on top ON");
            }
                
            else
            {
                ButPin.SetIconRotation(-90);
                ShowStatus("Always on top OFF");
            }                    
        }

        private void ButNew_Click(object sender, RoutedEventArgs e)
        {
            if (!DataHolder.CurrentNote.IsEmpty())
            {
                DataHolder.CurrentNote = DataHolder.CurrentNotebook.CreateNote();
                DataContext = DataHolder.CurrentNote;
            }                
        }

        private void ButSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataHolder.CurrentNote.Serialize())
                ShowStatus("Note saved");
        }

        private void ButColor_Click(object sender, RoutedEventArgs e)
        {
            PopupColors.IsOpen = !PopupColors.IsOpen;
        }

        private void titleLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            titleLabel.Visibility = Visibility.Collapsed;
            titleTextBox.Visibility = Visibility.Visible;
            titleTextBox.Focus();
            titleTextBox.CaretIndex = titleTextBox.Text.Length;
        }

        private void titleTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)             
                textBox.Focus();           
        }

        private void titleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            titleLabel.Visibility = Visibility.Visible;
            titleTextBox.Visibility = Visibility.Collapsed;
        }

        private void NotesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotesList.SelectedIndex == -1)
                NotesList.SelectedItem = DataHolder.CurrentNote;
            DataHolder.CurrentNote = (Note)NotesList.SelectedItem;
            DataContext = DataHolder.CurrentNote;
        }

        private void OnNoteChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "HasChanged")
            {
                if (((Note)sender).HasChanged)
                    titleLabel.FontWeight = FontWeights.Bold;
                else
                    titleLabel.FontWeight = FontWeights.Normal;
            }            
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PopupColors.IsOpen = false;
            PopupList.IsOpen = false;
        }

        private void DeleteShortcut(object sender, ExecutedRoutedEventArgs e)
        {
            if (PopupList.IsOpen)
            {
                if (NotesList.SelectedIndex != -1)
                {
                    Note tempNote = (Note)NotesList.SelectedItem;
                    int temp = NotesList.SelectedIndex;
                    DataHolder.CurrentNotebook.RemoveNote((Note)NotesList.SelectedItem);
                    if (temp > NotesList.Items.Count - 1)
                        temp -= 1;
                    if (NotesList.Items.Count > 0)
                        NotesList.SelectedIndex = temp;
                    else
                    {
                        DataHolder.CurrentNote = DataHolder.CurrentNotebook.CreateNote();
                        DataContext = DataHolder.CurrentNote;
                    }
                    ShowStatus($"Deleted {tempNote.Title}");
                    e.Handled = true;
                }
            }
        }
    }
}
