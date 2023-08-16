using System.Collections.Generic;
using UnityEngine;

namespace AlexDev.SpaceTanks
{
    public static class PlayerColors
    {
        public static Dictionary<string, string> _leaderBoardColors = new Dictionary<string, string>()
        {
            {"Black",       "#000000" },
            {"Silver",      "#C0C0C0" },
            {"White",       "#FFFFFF" },
            {"Fuchsia",     "#FF00FF" },
            {"Purple",      "#800080" },
            {"Red",         "#FF0000" },
            {"Maroon",      "#800000" },
            {"Yellow",      "#FFFF00" },
            {"Olive",       "#808000" },
            {"Lime",        "#00FF00" },
            {"Green",       "#008000" },
            {"Aqua",        "#00FFFF" },
            {"Teal",        "#008080" },
            {"Blue",        "#0000FF" },
            {"Navy",        "#000080" },
            {"DeepPink",    "#FF1493" },
            {"Orange",      "#FFA500" },
            {"Indigo",      "#4B0082" },
            {"StateBlue",   "#6A5ACD" },
            {"Brown",       "#A52A2A" }
        };
        public static string[] _colorKeys = new string[]
            {
                //"Black",
                "Yellow",
                "Fuchsia",
                "Purple",
                "Red",
                "Maroon",
                "Yellow",
                "Olive",
                "Lime",
                "Silver",
                "White",
                "Green",
                "Aqua",
                "Teal",
                "Blue",
                "Navy",
                "DeepPink",
                "Orange",
                "Indigo",
                "StateBlue",
                "Brown"
            };

        private static int _currentColor = 0;

        public static string GetNewColor
        {
            get
            {
                if (_currentColor >= _colorKeys.Length)
                    _currentColor = 0;
                return _colorKeys[_currentColor++];
            }
        }
        
        public static Color ConvertHexToRgb(string hexColor)
        {
            hexColor = hexColor.Replace("0x", "");
            hexColor = hexColor.Replace("#", "");
            byte r = byte.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = 255;
            if (hexColor.Length == 8)
            {
                a = byte.Parse(hexColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color(r, g, b, a);
        }

        public static string GetHexColor(string colorName)
        {
            return _leaderBoardColors[colorName];
        }

        public static Color GetRgbColor(string colorName)
        {
            return ConvertHexToRgb(GetHexColor(colorName));
        }
    }
}
