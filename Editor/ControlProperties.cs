using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace WinSimpleIDriver.Editor
{
    class ControlProperties
    {
        private Control control;
        private Form mainForm;

        public ControlProperties(Control ctrl)
        {
            this.control = ctrl;
            this.mainForm = ctrl.FindForm();

            this.Title = ctrl.Name;
            this.X = ctrl.Left;
            this.Y = ctrl.Top;
            this.Width = ctrl.Width;
            this.Height = ctrl.Height;
            this.Text = ctrl.Text;
            this.Size = ctrl.Font.Size;

            if (ctrl is PictureBox pictureBox)
            {
                this.ImagePath = pictureBox.Tag as string;
            }
        }

        [Category("Общие"), DisplayName("Имя")]
        public string Title { get; set; }

        [Category("Позиция"), DisplayName("X (по горизонтали)")]
        public int X { get; set; }

        [Category("Позиция"), DisplayName("Y (по вертикали)")]
        public int Y { get; set; }

        [Category("Позиция"), DisplayName("Относительная позиция")]
        public bool Relative { get; set; }

        [Category("Размер"), DisplayName("Ширина")]
        public int Width { get; set; }

        [Category("Размер"), DisplayName("Высота")]
        public int Height { get; set; }

        [Category("Текст"), DisplayName("Текст элемента")]
        public string Text { get; set; }

        [Category("Формат"), DisplayName("Формат отображения")]
        public string Format { get; set; }

        [Category("Шрифт"), DisplayName("Размер шрифта")]
        public float Size { get; set; } // Обновлено с FontSize на Size

        [Category("Цвет"), DisplayName("Цвет элемента")]
        public string Color { get; set; }

        [Category("Изображение"), DisplayName("Файл изображения"), Browsable(true)]
        [Editor(typeof(ImagePathEditor), typeof(UITypeEditor))]
        public string ImagePath
        {
            get => control is PictureBox pictureBox ? pictureBox.Tag as string : null;
            set
            {
                if (control is PictureBox pictureBox)
                {
                    if (!string.IsNullOrEmpty(value) && System.IO.File.Exists(value))
                    {
                        pictureBox.Image = Image.FromFile(value);
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

        [Category("Изображение"), DisplayName("Список изображений")]
        public string[] ImagesName { get; set; }

        [Category("Слой"), DisplayName("Z-Индекс (порядок наложения)")]
        public int ZIndex
        {
            get => mainForm.Controls.GetChildIndex(control);
            set
            {
                if (value >= 0 && value < mainForm.Controls.Count)
                {
                    mainForm.Controls.SetChildIndex(control, value);
                }
            }
        }

        [Category("Команды"), DisplayName("Команда")]
        public string Command { get; set; }

        [Category("Отображение"), DisplayName("Видимость")]
        public string Visible { get; set; }

        [Category("Диапазон"), DisplayName("Минимальное значение")]
        public int Min { get; set; }

        [Category("Диапазон"), DisplayName("Максимальное значение")]
        public int Max { get; set; }

        [Category("Значение"), DisplayName("Текущее значение")]
        public string Value { get; set; }

        [Category("Список"), DisplayName("Имя списка")]
        public string ListName { get; set; }

        [Category("Описание"), DisplayName("Подсказка")]
        public string ToolTip { get; set; }

        [Category("Тег"), DisplayName("Название тега")]
        public string TagTitle { get; set; }

        // Скрываем ненужные свойства
        [Browsable(false)]
        public Color BackColor { get; set; }

        [Browsable(false)]
        public bool Enabled { get; set; }

        // Применить изменения
        public void ApplyChanges()
        {
            control.Name = this.Title;
            control.Left = this.X;
            control.Top = this.Y;
            control.Width = this.Width;
            control.Height = this.Height;
            control.Text = this.Text;
            control.Font = new Font(control.Font.FontFamily, this.Size);

            if (control is PictureBox pictureBox && !string.IsNullOrEmpty(ImagePath))
            {
                pictureBox.Image = Image.FromFile(ImagePath);
                pictureBox.Tag = ImagePath;
            }
        }
    }
}
