/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using QuietNotes.Core;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuietNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            view.SortDescriptions.Add(new SortDescription("DateModified", ListSortDirection.Descending));

            SizeChanged += WinMain_SizeChanged;
            LocationChanged += WinMain_LocationChanged;
            
            popupStatus.VerticalOffset = Top + Height - 20;
            popupStatus.HorizontalOffset = Left + Width - 1;
        }

        private void WinMain_LocationChanged(object sender, EventArgs e)
        {
            popupStatus.VerticalOffset = Top + Height - 20;
            popupStatus.HorizontalOffset = Left + Width - 1;
            popupColors.HorizontalOffset++;
            popupColors.HorizontalOffset--;
        }

        private void WinMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            popupStatus.VerticalOffset = Top + Height - 20;
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
                butPin.SetRotation(0);
                popupStatus.ShowText("Always on top ON");
            }

            else
            {
                butPin.SetRotation(-90);
                popupStatus.ShowText("Always on top OFF");
            }
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
                    popupStatus.ShowText($"Deleted {tempNote.Title}");
                    e.Handled = true;
                }
            }
        }

        private void ComNewNote_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!DataHolder.CurrentNote.IsEmpty())
            {
                DataHolder.CurrentNote = DataHolder.CurrentNotebook.CreateNote();
                DataContext = DataHolder.CurrentNote;
            }
        }

        private void ComSaveNote_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataHolder.CurrentNote.Serialize())
                popupStatus.ShowText("Note saved");
        }

        private void ComNextNote_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listNotes.SelectedIndex < listNotes.Items.Count - 1)
                listNotes.SelectedIndex++;
            else
                listNotes.SelectedIndex = 0;
        }

        private void ComPreviousNote_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listNotes.SelectedIndex > 0)
                listNotes.SelectedIndex--;
            else
                listNotes.SelectedIndex = listNotes.Items.Count - 1;
        }

        private void ComToggleListNotes(object sender, ExecutedRoutedEventArgs e)
        {
            if (listNotes.Visibility == Visibility.Collapsed)
                listNotes.Visibility = Visibility.Visible;
            else
                listNotes.Visibility = Visibility.Collapsed;
        }

        private void ComTogglePopupColors(object sender, ExecutedRoutedEventArgs e)
        {
            popupColors.IsOpen = !popupColors.IsOpen;
        }

    }
}
