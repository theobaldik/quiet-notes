using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Logic;

namespace GUI
{
    public static class DataHolder
    {        
        public static Notebook CurrentNotebook { get; set; }        
        public static Note CurrentNote { get; set; }        
        public static NoteColor NoteColor { get; set; }        
    }
}
