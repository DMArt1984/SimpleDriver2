using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinSimpleIDriver.Editor
{
    public partial class FormDesignProp : Form
    {
        private ElementDataApp selectedElement; // Теперь работаем с ElementDataApp

        public FormDesignProp(ElementDataApp element)
        {
            InitializeComponent();
            this.selectedElement = element;

            // Инициализация свойств
            if (selectedElement != null)
            {
                propertyGrid1.SelectedObject = new ElementProperties(selectedElement);
            }
        }

        /// <summary>
        /// Обновляет свойства элемента при изменениях в PropertyGrid.
        /// </summary>
        public void UpdateProperties(ElementDataApp element)
        {
            this.selectedElement = element;
            propertyGrid1.SelectedObject = new ElementProperties(selectedElement);
        }

        /// <summary>
        /// Применяет изменения к элементу управления на форме.
        /// </summary>
        private void ApplyChangesToControl()
        {
            if (selectedElement.Control != null)
            {
                selectedElement.Control.Left = selectedElement.X;
                selectedElement.Control.Top = selectedElement.Y;
                selectedElement.Control.Width = selectedElement.Width;
                selectedElement.Control.Height = selectedElement.Height;
                selectedElement.Control.Text = selectedElement.Text;

                if (selectedElement.Control is Label || selectedElement.Control is TextBox)
                {
                    selectedElement.Control.Font = new Font("Arial", selectedElement.Size);
                }

                if (selectedElement.Control is PictureBox pic && !string.IsNullOrEmpty(selectedElement.ImagePath))
                {
                    if (System.IO.File.Exists(selectedElement.ImagePath))
                    {
                        pic.Image = Image.FromFile(selectedElement.ImagePath);
                        pic.Tag = selectedElement.ImagePath;
                    }
                }

                selectedElement.Control.Invalidate(); // Перерисовываем элемент
            }
        }

        /// <summary>
        /// Обработчик события изменения свойства в PropertyGrid.
        /// </summary>
        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (selectedElement != null)
            {
                ApplyChangesToControl();
            }
        }

        private void FormDesignProp_Load(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Класс для привязки данных к PropertyGrid.
    /// </summary>
    public class ElementProperties
    {
        private ElementDataApp element;

        public ElementProperties(ElementDataApp element)
        {
            this.element = element;
        }

        [Category("Общие"), DisplayName("Название")]
        public string Name
        {
            get => element.Name;
            set { element.Name = value; ApplyChanges(); }
        }

        [Category("Размер и положение"), DisplayName("X")]
        public int X
        {
            get => element.X;
            set { element.X = value; ApplyChanges(); }
        }

        [Category("Размер и положение"), DisplayName("Y")]
        public int Y
        {
            get => element.Y;
            set { element.Y = value; ApplyChanges(); }
        }

        [Category("Размер и положение"), DisplayName("Ширина")]
        public int Width
        {
            get => element.Width;
            set { element.Width = value; ApplyChanges(); }
        }

        [Category("Размер и положение"), DisplayName("Высота")]
        public int Height
        {
            get => element.Height;
            set { element.Height = value; ApplyChanges(); }
        }

        [Category("Текст"), DisplayName("Текст")]
        public string Text
        {
            get => element.Text;
            set { element.Text = value; ApplyChanges(); }
        }

        [Category("Шрифт"), DisplayName("Размер шрифта")]
        public float Size
        {
            get => element.Size;
            set { element.Size = value; ApplyChanges(); }
        }

        [Category("Изображение"), DisplayName("Путь к изображению")]
        public string ImagePath
        {
            get => element.ImagePath;
            set { element.ImagePath = value; ApplyChanges(); }
        }

        private void ApplyChanges()
        {
            if (element.Control != null)
            {
                element.Control.Left = element.X;
                element.Control.Top = element.Y;
                element.Control.Width = element.Width;
                element.Control.Height = element.Height;
                element.Control.Text = element.Text;

                if (element.Control is Label || element.Control is TextBox)
                {
                    element.Control.Font = new Font("Arial", element.Size);
                }

                if (element.Control is PictureBox pic && !string.IsNullOrEmpty(element.ImagePath))
                {
                    if (System.IO.File.Exists(element.ImagePath))
                    {
                        pic.Image = Image.FromFile(element.ImagePath);
                        pic.Tag = element.ImagePath;
                    }
                }

                element.Control.Invalidate(); // Перерисовываем элемент
            }
        }
    }
}
