using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver.DML
{
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public static class ColorConverterHelper
    {
        //Color color1 = Color.Red;
        //string colorString1 = ColorConverterHelper.ColorToString(color1);
        //Console.WriteLine(colorString1); // "Red"

        //Color color2 = Color.FromArgb(255, 128, 64, 32);
        //string colorString2 = ColorConverterHelper.ColorToString(color2);
        //Console.WriteLine(colorString2); // "#FF804020"

        //Color parsedColor1 = ColorConverterHelper.StringToColor("Blue");
        //Console.WriteLine(parsedColor1); // Color.Blue

        //Color parsedColor2 = ColorConverterHelper.StringToColor("#FF0000");
        //Console.WriteLine(parsedColor2); // Color.Red

        //Color parsedColor3 = ColorConverterHelper.StringToColor("255,128,64,32");
        //Console.WriteLine(parsedColor3); // Color.FromArgb(255, 128, 64, 32)

        /// <summary>
        /// Преобразует объект Color в строку.
        /// </summary>
        public static string ColorToString(Color color)
        {
            // Если это известный цвет, сохраняем имя
            if (color.IsKnownColor)
            {
                return color.Name; // Например, "Red"
            }

            // Если это кастомный цвет, сохраняем в формате ARGB
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"; // Например, "#FFFF8040"
        }

        /// <summary>
        /// Преобразует строку в объект Color, анализируя формат.
        /// </summary>
        public static Color StringToColor(string colorString)
        {
            if (string.IsNullOrWhiteSpace(colorString))
                throw new ArgumentException("Color string is empty or null");

            colorString = colorString.Trim();

            // 1. Попытка интерпретировать как имя цвета
            if (Enum.IsDefined(typeof(KnownColor), colorString))
            {
                return Color.FromName(colorString); // Например, "Red"
            }

            // 2. Попытка интерпретировать как HTML-код цвета (например, "#FF0000")
            if (Regex.IsMatch(colorString, @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$"))
            {
                return ColorTranslator.FromHtml(colorString);
            }

            // 3. Попытка интерпретировать как ARGB (например, "255,128,64,32")
            string[] rgbaParts = colorString.Split(',');
            if (rgbaParts.Length == 4 &&
                int.TryParse(rgbaParts[0], out int a) &&
                int.TryParse(rgbaParts[1], out int r) &&
                int.TryParse(rgbaParts[2], out int g) &&
                int.TryParse(rgbaParts[3], out int b))
            {
                return Color.FromArgb(a, r, g, b);
            }

            throw new FormatException("Invalid color format");
        }
    }



}
