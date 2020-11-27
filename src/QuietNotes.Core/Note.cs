/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Logic
{
    public class Note : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static Note Deserialize(string filePath)
        {            
            return Serializer.Deserialize<Note>(filePath);
        }

        public bool Serialize()
        {
            if (!IsEmpty())
            {
                string filePath = Path.Combine(Configurator.NotesPath, FileName);
                Serializer.Serialize(filePath, this);
                HasChanged = false;
                return true;
            }
            return false;
        }

        public void Delete()
        {
            string filePath = Path.Combine(Configurator.NotesPath, FileName);
            File.Delete(filePath);
        }

        public bool IsEmpty()
        {
            return Title == Configurator.Config.DefaultTitle && String.IsNullOrWhiteSpace(Content);
        }

        private string FileName
        {
            get => $"{ID.ToString().PadLeft(6, '0')}";    
        }

        public int ID { get; set; }
        private string color;
        public string Color
        {
            get { return color; }
            set { DateModified = DateTime.Now; color = value; OnPropertyChanged("Color"); } 
        }
        private string title;
        public string Title
        {
            get { if (String.IsNullOrWhiteSpace(title)) return Configurator.Config.DefaultTitle; return title; }
            set { DateModified = DateTime.Now; title = value; OnPropertyChanged("Title"); }
        }
        private string content;
        public string Content
        {
            get { return content; }
            set { DateModified = DateTime.Now; content = value; OnPropertyChanged("Content"); }
        }
        private DateTime dateModified;
        public DateTime DateModified
        {
            get { return dateModified; }
            set { dateModified = value; OnPropertyChanged("DateModified"); }
        }

        private bool hasChanged;
        public bool HasChanged
        { 
            get { return hasChanged; }
            set { hasChanged = value; OnPropertyChanged("HasChanged"); }
        }
    }
}
