/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using QuietNotes.Core;

namespace QuietNotes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);            
            Configurator.Init();
            DataHolder.CurrentNotebook = new Notebook();
            DataHolder.CurrentNotebook.DeserializeAll();
            DataHolder.CurrentNote = DataHolder.CurrentNotebook.CreateNote();
            StartupFile(e.Args);
            DataHolder.NoteColor = new NoteColor();
        }
        private void StartupFile(string[] args)
        {
            if (args.Length > 0)
            {
                string filePath = args.Last();
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length < 10000000)
                        DataHolder.CurrentNote.Title = Path.GetFileNameWithoutExtension(filePath);
                        DataHolder.CurrentNote.Content = File.ReadAllText(filePath);
                }                
            }
        }
    }
}
