using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace WinSimpleIDriver.Editor
{
    public class ElementProperties
    {
        private ElementDataApp element;

        public ElementProperties(ElementDataApp element)
        {
            this.element = element;
        }

        [Category("Общие"), DisplayName("Имя")]
        public string Name
        {
            get => element.Control.Name;
            set => element.Control.Name = value;
        }

        [Category("Позиция"), DisplayName("X")]
        public int X
        {
            get => element.Control.Left;
            set => element.Control.Left = value;
        }

        [Category("Позиция"), DisplayName("Y")]
        public int Y
        {
            get => element.Control.Top;
            set => element.Control.Top = value;
        }

        [Category("Размер"), DisplayName("Ширина")]
        public int Width
        {
            get => element.Control.Width;
            set => element.Control.Width = value;
        }

        [Category("Размер"), DisplayName("Высота")]
        public int Height
        {
            get => element.Control.Height;
            set => element.Control.Height = value;
        }

        [Category("Шрифт"), DisplayName("Размер шрифта")]
        public float FontSize
        {
            get => element.Control.Font.Size;
            set => element.Control.Font = new Font(element.Control.Font.FontFamily, value);
        }

        [Category("Видимость"), DisplayName("Видимый")]
        public bool Visible
        {
            get => element.Control.Visible;
            set => element.Control.Visible = value;
        }

        [Category("Цвет"), DisplayName("Цвет текста")]
        public Color ForeColor
        {
            get => element.Control.ForeColor;
            set => element.Control.ForeColor = value;
        }

        [Category("Цвет"), DisplayName("Цвет фона")]
        public Color BackColor
        {
            get => element.Control.BackColor;
            set => element.Control.BackColor = value;
        }

        [Category("Свойства элемента"), DisplayName("Относительное позиционирование")]
        public bool Relative
        {
            get => element.Relative;
            set => element.Relative = value;
        }

        [Category("Слой"), DisplayName("Z-Индекс")]
        public int ZIndex
        {
            get => element.ZIndex;
            set
            {
                element.ZIndex = value;
                element.Control.Parent.Controls.SetChildIndex(element.Control, value);
            }
        }

        [Category("Изображение"), DisplayName("Путь к изображению"), Browsable(true)]
        [Editor(typeof(ImagePathEditor), typeof(UITypeEditor))]
        public string ImagePath
        {
            get => element.ImagePath;
            set
            {
                element.ImagePath = value;
                if (element.Control is PictureBox pictureBox)
                {
                    if (!string.IsNullOrEmpty(value) && System.IO.File.Exists(value))
                    {
                        pictureBox.Image = Image.FromFile(value);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox.Tag = value;
                    }
                    else
                    {
                        pictureBox.Image = null;
                        pictureBox.Tag = null;
                    }
                }
            }
        }

        [Category("Шаблоны"), DisplayName("Используемый шаблон")]
        public string Template
        {
            get => element.Template;
            set => element.Template = value;
        }

        [Category("Шаблоны"), DisplayName("Номер группы")]
        public int Group
        {
            get => element.Group;
            set => element.Group = value;
        }

        [Category("Страница"), DisplayName("Название страницы")]
        public string Page
        {
            get => element.Page;
            set => element.Page = value;
        }


    }
}

