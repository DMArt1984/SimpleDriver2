using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver.Editor
{
    public enum eElementType
    {
        None = 0,
        Label = 1, // Label
        OutputBox = 2, // TextBox
        InputBox = 3, // TextBox
        IOBox = 4, // TextBox
        IOPop = 5, // TextBox
        Button = 6, // Button
        PictureBox = 10, // PictureBox
        ImageList = 11, // ImageList
        Rectangle = 20, // PictureBox без image
        Progress = 30, // ProgressBar
    }

    public class ElementData
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] // Преобразует eElementType в строку при сохранении
        public eElementType ElementType { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Text { get; set; }
        public float FontSize { get; set; }
        public int ZIndex { get; set; }
        public string ImagePath { get; set; }
    }
}
