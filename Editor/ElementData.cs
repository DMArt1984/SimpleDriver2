using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        public Control Control { get; set; }

        public int X
        {
            get => Control?.Left ?? 0;
            set { if (Control != null) Control.Left = value; }
        }

        public int Y
        {
            get => Control?.Top ?? 0;
            set { if (Control != null) Control.Top = value; }
        }

        public int Width
        {
            get => Control?.Width ?? 0;
            set { if (Control != null) Control.Width = value; }
        }

        public int Height
        {
            get => Control?.Height ?? 0;
            set { if (Control != null) Control.Height = value; }
        }


        public bool Relative { get; set; }
        public string Text { get; set; }
        public float Size { get; set; }
        public string Color { get; set; }
        public int ZIndex { get; set; }
        public string ImagePath { get; set; }

        // Конструктор, который принимает 3 аргумента
        public ElementDataApp(string name, eElementType elementType, Control control)
        {
            Name = name;
            ElementType = elementType;
            Control = control;
        }

        public ElementDataApp(ElementDataJson data)
        {
            this.Name = data.Name;
            this.ElementType = data.ElementType;
            this.X = data.X;
            this.Y = data.Y;
            this.Relative = data.Relative;
            this.Width = data.Width;
            this.Height = data.Height;
            this.Text = data.Text;
            this.Size = data.Size;
            this.Color = data.Color;
            this.ZIndex = data.ZIndex;
            this.ImagePath = data.ImagePath;

            this.Control = CreateControl();
            if (this.Control != null)
            {
                this.Control.Name = this.Name;
                this.Control.Left = this.X;
                this.Control.Top = this.Y;
                this.Control.Width = this.Width;
                this.Control.Height = this.Height;

                if (this.Control is Label || this.Control is TextBox)
                    this.Control.Font = new Font("Arial", this.Size);

                if (this.Control is PictureBox pic && !string.IsNullOrEmpty(this.ImagePath) && File.Exists(this.ImagePath))
                {
                    pic.Image = Image.FromFile(this.ImagePath);
                    pic.Tag = this.ImagePath;
                }
            }
        }

        // Метод для получения данных в формате JSON
        public ElementDataJson ToJsonData()
        {
            return new ElementDataJson
            {
                Name = this.Name,
                ElementType = this.ElementType,
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Text = Control?.Text,
                Size = Control?.Font.Size ?? 12.0f
            };
        }

        private Control CreateControl()
        {
            switch (this.ElementType)
            {
                case eElementType.Label:
                    return new Label { Text = this.Text };
                case eElementType.OutputBox:
                case eElementType.InputBox:
                case eElementType.IOBox:
                case eElementType.IOPop:
                    return new TextBox { Text = this.Text, ReadOnly = (this.ElementType == eElementType.OutputBox) };
                case eElementType.Button:
                    return new Button { Text = this.Text };
                case eElementType.PictureBox:
                case eElementType.Rectangle:
                    return new PictureBox
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                case eElementType.Progress:
                    return new ProgressBar();
                default:
                    return null;
            }
        }
    }


}
