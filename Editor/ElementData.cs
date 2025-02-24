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

    public enum eElementEditor
    {
        Name,        // Имя элемента
        Text,        // Текст элемента
        X,    // Позиция X
        Y,    // Позиция Y
        Width,        // Ширина
        Height,         // высота
        FontSize,    // Размер шрифта (для Label, TextBox)
        ForeColor,   // Цвет текста
        BackColor,   // Цвет фона
        Visible,     // Видимость элемента
        ImagePath    // Путь к изображению (для PictureBox)
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
        // ✅ Объявляем событие
        public event Action<ElementDataApp> OnElementDeleted;

        // ✅ Метод, вызывающий событие
        public void Delete()
        {
            OnElementDeleted?.Invoke(this); // Запускаем событие
        }

        public eElementType ElementType { get; set; }
        public Control Control { get; set; }

        public bool Relative { get; set; }

        public int ZIndex { get; set; }
        public string ImagePath { get; set; }

        public static Dictionary<eElementType, eUsedFormClass> GetClassFromType()
        {
            return new Dictionary<eElementType, eUsedFormClass>
            {
                { eElementType.Label, eUsedFormClass.Label },
                { eElementType.OutputBox, eUsedFormClass.TextBox },
                { eElementType.InputBox, eUsedFormClass.TextBox },
                { eElementType.IOBox, eUsedFormClass.TextBox },
                { eElementType.IOPop, eUsedFormClass.TextBox },
                { eElementType.Button, eUsedFormClass.Button },
                { eElementType.PictureBox, eUsedFormClass.PictureBox },
                { eElementType.ImageList, eUsedFormClass.ImageList },
                { eElementType.Rectangle, eUsedFormClass.PictureBox },
                { eElementType.Progress, eUsedFormClass.ProgressBar }
            };
        }


    }


}
