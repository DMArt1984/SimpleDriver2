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
        public string ElementType { get; set; }

        public string Page { get; set; } // Название страницы
        public string Template { get; set; } // Используемый шаблон
        public int Group { get; set; } // Номер группы шаблона

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
        public eElementType ElementType { get; set; } // тип элемента
        public Control Control { get; set; } // control элемента

        public bool Relative { get; set; } // Относительная позиция
        public int ZIndex { get; set; } // Уровень расположения слоя
        public string ImagePath { get; set; } // Путь к файлу рисунка

        public string Page { get; set; } // Название страницы
        public string Template { get; set; } // Используемый шаблон
        public int Group { get; set; } // Номер группы шаблона

        // Объявляем событие
        public event Action<ElementDataApp> OnElementDeleted;

        // Метод, вызывающий событие
        public void Delete()
        {
            OnElementDeleted?.Invoke(this); // Запускаем событие
        }

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

    /// <summary>
    /// Преобразования между собой
    /// </summary>
    public class ElementConverter
    {
        // Пример вызова: ElementDataApp elementApp = ConvertToApp(jsonData, CreateControl);
        public static ElementDataApp ConvertToApp(ElementDataJson jsonData, Func<eElementType, Control> createControl)
        {
            if (!Enum.TryParse(jsonData.ElementType, out eElementType elementType))
            {
                throw new ArgumentException($"Неизвестный тип элемента: {jsonData.ElementType}");
            }

            Control control = createControl(elementType);

            if (control != null)
            {
                control.Name = jsonData.Name;
                control.Left = jsonData.X;
                control.Top = jsonData.Y;
                control.Width = jsonData.Width;
                control.Height = jsonData.Height;

                if (control is Label || control is TextBox)
                    control.Font = new Font(control.Font.FontFamily, jsonData.Size);

                if (control is PictureBox pic && !string.IsNullOrEmpty(jsonData.ImagePath) && File.Exists(jsonData.ImagePath))
                {
                    pic.Image = Image.FromFile(jsonData.ImagePath);
                    pic.Tag = jsonData.ImagePath;
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }

            return new ElementDataApp
            {
                ElementType = elementType,
                Control = control,
                Page = jsonData.Page,
                Template = jsonData.Template,
                Group = jsonData.Group,
                Relative = jsonData.Relative,
                ZIndex = jsonData.ZIndex,
                ImagePath = jsonData.ImagePath
            };
        }


        public static ElementDataJson ConvertToJson(ElementDataApp appData)
        {
            return new ElementDataJson
            {
                Name = appData.Control?.Name,
                ElementType = appData.ElementType.ToString(), // Преобразуем в строку
                Page = appData.Page,
                Template = appData.Template,
                Group = appData.Group,
                X = appData.Control?.Left ?? 0,
                Y = appData.Control?.Top ?? 0,
                Relative = appData.Relative,
                Width = appData.Control?.Width ?? 0,
                Height = appData.Control?.Height ?? 0,
                ImagePath = appData.ImagePath,
                ZIndex = appData.ZIndex,
                Size = appData.Control?.Font.Size ?? 12.0f
            };
        }



    }

}
