using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSimpleIDriver.Connector.SGT;

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

    public enum eUsedFormClass
    {
        Label,
        TextBox,
        Button,
        PictureBox,
        ImageList,
        ProgressBar
    }

    /// <summary>
    /// Данные элемента, хранимые в JSON-файле.
    /// </summary>
    public class ElementDataJson
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] // Преобразует eElementType в строку при сохранении
        public eElementType ElementType { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public bool Relative { get; set; } // Относительная позиция X, Y
        public int Height { get; set; }
        public int Width { get; set; }

        public string Text { get; set; }
        public string Format { get; set; } // Формат текста
        public float Size { get; set; }
        public string Color { get; set; }

        public int ZIndex { get; set; }

        public string ImagePath { get; set; }
        public string[] ImagesName { get; set; }

        public string Command { get; set; }
        public string Visible { get; set; }

        public int Min { get; set; }
        public int Max { get; set; }
        public string Value { get; set; }
        public string ListName { get; set; }
        public string ToolTip { get; set; } // Описание при наведении мыши

        public string TagTitle { get; set; } // Тег для записи значения

        public ElementDataJson() { }
    }

    /// <summary>
    /// Данные элемента для работы в приложении.
    /// </summary>
    public class ElementDataApp
    {
        public string Name { get; set; }
        public eElementType ElementType { get; set; }
        public Control Control { get; set; } // Связанный элемент формы

        public ElementDataApp(string name, eElementType elementType, Control control)
        {
            Name = name;
            ElementType = elementType;
            Control = control;
        }

        /// <summary>
        /// Конвертирует данные приложения в JSON-совместимый объект.
        /// </summary>
        public ElementDataJson ToJsonData()
        {
            return new ElementDataJson
            {
                Name = this.Name,
                ElementType = this.ElementType,
                X = this.Control.Left,
                Y = this.Control.Top,
                Width = this.Control.Width,
                Height = this.Control.Height,
                Text = this.Control.Text,
                Size = this.Control.Font?.Size ?? 12.0f,
                Color = null,
                ZIndex = this.Control.Parent?.Controls.GetChildIndex(this.Control) ?? 0,
                ImagePath = this.Control is PictureBox pic ? pic.Tag as string : null
            };
        }

        /// <summary>
        /// Создает объект ElementDataApp из JSON-данных и привязывает к переданному элементу управления.
        /// </summary>
        public static ElementDataApp FromJsonData(ElementDataJson jsonData, Control control)
        {
            return new ElementDataApp(jsonData.Name, jsonData.ElementType, control);
        }
    }


}
