using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class NoteColor
    {
        public static string LightBlue { get => "#1395BA"; }
        public static string DarkBlue { get => "#0D3C55"; }
        public static string Green { get => "#A2B86C"; }
        public static string Yellow { get => "#EBC844"; }
        public static string Orange { get => "#F16C20"; }
        public static string Red { get => "#C02E1D"; }
    }

    public class Config
    {      
        public static Config Deserialize()
        {
            return Serializer.Deserialize<Config>(Configurator.ConfigPath);
        }
        public void Serialize()
        {
            Serializer.Serialize(Configurator.ConfigPath, this);
        }
        public string DefaultColor { get; set; } = NoteColor.LightBlue;
        public string DefaultTitle { get; set; } = "Untitled Note";
    }
}
