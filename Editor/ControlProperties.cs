using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinSimpleIDriver.Editor
{
    class ControlProperties
    {
        private Control control;

        public ControlProperties(Control ctrl)
        {
            this.control = ctrl;
            this.X = ctrl.Left;
            this.Y = ctrl.Top;
            this.Width = ctrl.Width;
            this.Height = ctrl.Height;
            this.Text = ctrl.Text;
            this.FontSize = ctrl.Font.Size;
        }

        [Category("Позиция"), DisplayName("X (по горизонтали)")]
        public int X { get; set; }

        [Category("Позиция"), DisplayName("Y (по вертикали)")]
        public int Y { get; set; }

        [Category("Размер"), DisplayName("Ширина")]
        public int Width { get; set; }

        [Category("Размер"), DisplayName("Высота")]
        public int Height { get; set; }

        [Category("Текст"), DisplayName("Текст элемента")]
        public string Text { get; set; }

        [Category("Шрифт"), DisplayName("Размер шрифта")]
        public float FontSize { get; set; }

        // Скрываем ненужные свойства
        [Browsable(false)]
        public Color BackColor { get; set; }

        [Browsable(false)]
        public bool Enabled { get; set; }

        // Применить изменения
        public void ApplyChanges()
        {
            control.Left = this.X;
            control.Top = this.Y;
            control.Width = this.Width;
            control.Height = this.Height;
            control.Text = this.Text;
            control.Font = new Font(control.Font.FontFamily, this.FontSize);
        }
    }
}
