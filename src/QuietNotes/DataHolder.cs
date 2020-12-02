/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using QuietNotes.Core;

namespace QuietNotes
{
    public static class DataHolder
    {        
        public static Notebook CurrentNotebook { get; set; }        
        public static Note CurrentNote { get; set; }        
        public static NoteColor NoteColor { get; set; }        
    }
}
