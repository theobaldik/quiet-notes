/* Copyright (C) 2020 Filip Klopec
 * Released under the GNU GPLv3, read the file 'LICENSE' for more information.
 */

using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace QuietNotes.Core
{
    public static class Serializer
    {
        public static string LoadJson(string filePath)
        {
            if (File.Exists(filePath))
                return File.ReadAllText(filePath, Encoding.UTF8);
            else return null;
        }

        private static void SaveJson(string filePath, string content)
        {
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }

        public static void Serialize(string filePath, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            SaveJson(filePath, json);
        }

        public static T Deserialize<T>(string filePath)
        {
            string json = LoadJson(filePath);
            if (json != null)
                return JsonConvert.DeserializeObject<T>(json);
            return default(T);
        }
    }
}
