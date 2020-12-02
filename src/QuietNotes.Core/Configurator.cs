/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using System;
using System.IO;

namespace QuietNotes.Core
{
    public static class Configurator
    {
        public static Config Config { get; set; }

        private static string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string homePath = Path.Join(rootPath, @"QuietNotes");
        private static string configPath = Path.Join(homePath, @"config.json");
        private static string notesPath = Path.Join(homePath, @"Notes");

        public static string DocumentsPath { get => rootPath; }
        public static string HomePath { get => homePath; }
        public static string ConfigPath { get => configPath; }
        public static string NotesPath { get => notesPath; }

        public static void Init()
        {
            if (!Directory.Exists(homePath))
                Directory.CreateDirectory(homePath);

            if (!Directory.Exists(notesPath))
                Directory.CreateDirectory(notesPath);

            if (!File.Exists(configPath))
            {
                Config = new Config();
                Config.Serialize();
            }
            else
            {
                Config = Config.Deserialize();
            }
        }
    }
}
