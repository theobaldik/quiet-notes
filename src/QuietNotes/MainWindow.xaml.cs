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
using QuietNotes.Core;

namespace QuietNotes
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
            listNotes.ItemsSource = DataHolder.CurrentNotebook.Notes;            
            DataContext = DataHolder.CurrentNote;

            colorPickRectGreen.Color = NoteColor.Green;
            colorPickRectLightBlue.Color = NoteColor.LightBlue;
            colorPickRectDarkBlue.Color = NoteColor.DarkBlue;
            colorPickRectYellow.Color = NoteColor.Yellow;
            colorPickRectOrange.Color = NoteColor.Orange;
            colorPickRectRed.Color = NoteColor.Red;

            listNotes.SelectedItem = DataHolder.CurrentNote;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listNotes.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("DateModified", ListSortDirection.Ascending));

            SizeChanged += WinMain_SizeChanged;
            LocationChanged += WinMain_LocationChanged;

            popupStatus.VerticalOffset = Top + Height - 22;
            popupStatus.HorizontalOffset = Left + Width - 1;
            popupStatus.FlowDirection = FlowDirection.RightToLeft;


            StatusTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            StatusTimer.Tick += delegate (object sender, EventArgs e)
            {
                StatusTimer.Stop();
                if (popupStatus.IsOpen) popupStatus.IsOpen = false;
            };
        }       

        private void ShowStatus(string text)
        {
            labelStatus.Content = text;
            popupStatus.IsOpen = true;
            if (StatusTimer.IsEnabled)
                StatusTimer.Stop();
            StatusTimer.Start();
        }

        private void WinMain_LocationChanged(object sender, EventArgs e)
        {
            popupStatus.VerticalOffset = Top + Height - 22;
            popupStatus.HorizontalOffset = Left + Width - 1;
            popupColors.HorizontalOffset++;
            popupColors.HorizontalOffset--;
        }

        private void WinMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            popupStatus.VerticalOffset = Top + Height - 22;
            popupStatus.HorizontalOffset = Left + Width - 1;
        }                

        private void TextContent_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            textContent.SelectedText = string.Empty;
            if (e.Key == Key.Tab)
            {
                int pos = textContent.CaretIndex;
                textContent.Text = textContent.Text.Insert(pos, "  ");
                textContent.CaretIndex = pos + 2;
                e.Handled = true;
            }
        }

        private void WinMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {                
            DragMove();
        }
        private void ComClose_Executed(Object sender, ExecutedRoutedEventArgs e)
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
            if (listNotes.Visibility == Visibility.Collapsed)
                listNotes.Visibility = Visibility.Visible;
            else
                listNotes.Visibility = Visibility.Collapsed;
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
                butPin.SetIconRotation(0);
                ShowStatus("Always on top ON");
            }
                
            else
            {
                butPin.SetIconRotation(-90);
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
            popupColors.IsOpen = !popupColors.IsOpen;
        }

        private void LabelTitle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            labelTitle.Visibility = Visibility.Collapsed;
            textTitle.Visibility = Visibility.Visible;
            textTitle.Focus();
            textTitle.CaretIndex = textTitle.Text.Length;
        }

        private void TextTitle_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)             
                textContent.Focus();           
        }

        private void TextTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            labelTitle.Visibility = Visibility.Visible;
            textTitle.Visibility = Visibility.Collapsed;
        }

        private void ListNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listNotes.SelectedIndex == -1)
                listNotes.SelectedItem = DataHolder.CurrentNote;
            DataHolder.CurrentNote = (Note)listNotes.SelectedItem;
            DataContext = DataHolder.CurrentNote;
        }

        private void OnNoteChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "HasChanged")
            {
                if (((Note)sender).HasChanged)
                    labelTitle.FontWeight = FontWeights.Bold;
                else
                    labelTitle.FontWeight = FontWeights.Normal;
            }            
        }

        private void TextContent_GotFocus(object sender, RoutedEventArgs e)
        {
            popupColors.IsOpen = false;
        }

        private void ComDeleteNote_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listNotes.Visibility == Visibility.Visible)
            {
                if (listNotes.SelectedIndex != -1)
                {
                    Note tempNote = (Note)listNotes.SelectedItem;
                    int temp = listNotes.SelectedIndex;
                    DataHolder.CurrentNotebook.RemoveNote((Note)listNotes.SelectedItem);
                    if (temp > listNotes.Items.Count - 1)
                        temp -= 1;
                    if (listNotes.Items.Count > 0)
                        listNotes.SelectedIndex = temp;
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
