﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Logic
{
    public class Notebook
    {
        #region ID Handling
        private Stack<int> idStack;
        private void LoadIDs()
        {
            int range = Notes.Count - 1;
            for (int i = 0; i < range; i++)
            {
                int diff = Notes[i + 1].ID - Notes[i].ID;
                for (int j = 1; j < diff; j++)
                {
                    idStack.Push(i + j);
                }
            }
        }
        private int PopID()
        {
            if (idStack.Count == 0)
            {
                PushID(Notes.Count);
            }
            return idStack.Pop();
        }
        private void PushID(int id)
        {
            idStack.Push(id);
        }
        #endregion

        public ObservableCollection<Note> Notes { get; private set; }
               

        public Notebook()
        {
            idStack = new Stack<int>();
            Notes = new ObservableCollection<Note>();
            LoadIDs();            
        }    

        public void DeserializeAll()
        {
            string[] filePaths = Directory.GetFiles(Configurator.NotesPath);
            foreach (string filePath in filePaths)
            {
                Notes.Add(Note.Deserialize(filePath));
            }    
        }

        public void SerializeAll()
        {
            foreach (Note note in Notes)
            {
                note.Serialize();
            }
        }

        public Note CreateNote()
        {
            Note note = new Note()
            {
                ID = PopID(),
                Color = Configurator.Config.DefaultColor,
                Content = "",
                DateModified = DateTime.Now,
                HasChanged = true,
                Title = Configurator.Config.DefaultTitle
            };
            Notes.Add(note);
            return note;
        }

        public void RemoveNote(Note note)
        {
            Notes.Remove(note);
            note.Delete();
        }
    }
}