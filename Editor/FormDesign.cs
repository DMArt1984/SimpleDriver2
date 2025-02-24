using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WinSimpleIDriver.Editor
{
    public partial class FormDesign : Form
    {
        public enum ResizeDirection
        {
            None, Right, Bottom
        }

        private List<ElementDataApp> appElements = new List<ElementDataApp>();

        private bool isDragging = false;
        private bool isResizing = false;
        private ResizeDirection resizeDirection = ResizeDirection.None;
        private Point offset;
        private Control selectedControl = null;

        private uint elementID = 0; // абсолютный идентификатор

        private const int ResizeMargin = 6; // Отступ в пикселях для изменения размеров

        public FormDesign()
        {
            InitializeComponent();
        }

        private void FormDesign_Load(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemAddControlLabel_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.Label);
        }

        private void ToolStripMenuItemAddControlInput_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.InputBox);
        }

        private void ToolStripMenuItemAddControlOutput_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.OutputBox);
        }

        private void ToolStripMenuItemAddControlPicture_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.PictureBox);
        }

        private void ToolStripMenuItemAddControlRectangle_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.Rectangle);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveJsonToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemCommandCopy_Click(object sender, EventArgs e)
        {
            if (selectedControl == null) return; // Проверяем, есть ли выбранный элемент

            // Находим соответствующий элемент в списке
            var originalElement = appElements.FirstOrDefault(el => el.Control == selectedControl);
            if (originalElement == null) return;

            // Увеличиваем идентификатор
            elementID++;

            // Создаем копию элемента
            var newElement = new ElementDataApp
            {
                ElementType = originalElement.ElementType,
                Control = CreateControl(originalElement.ElementType)
            };

            // Проверяем, был ли создан Control
            if (newElement.Control == null) return;

            // Присваиваем уникальное имя элементу
            newElement.Control.Name = $"{originalElement.ElementType}{elementID}";

            // Копируем основные свойства
            newElement.Control.Left = originalElement.Control.Left + 10; // Смещаем копию
            newElement.Control.Top = originalElement.Control.Top + 10;
            newElement.Control.Width = originalElement.Control.Width;
            newElement.Control.Height = originalElement.Control.Height;
            newElement.Control.Text = originalElement.Control.Text;

            if (newElement.Control is PictureBox pic && originalElement.Control is PictureBox originalPic)
            {
                pic.Image = originalPic.Image; // Копируем изображение
            }

            // Привязываем события для перемещения и изменения размеров
            AttachControlEvents(newElement.Control);

            // Добавляем элемент в список
            appElements.Add(newElement);

            // Добавляем Control на форму
            this.Controls.Add(newElement.Control);

            // Устанавливаем новый элемент как выбранный
            selectedControl = newElement.Control;
        }


        private void ToolStripMenuItemCommandDelete_Click(object sender, EventArgs e)
        {
            if (selectedControl == null) return; // Проверяем, есть ли выбранный элемент

            // Находим соответствующий элемент в списке
            var elementToRemove = appElements.FirstOrDefault(el => el.Control == selectedControl);
            if (elementToRemove == null) return;

            // Удаляем элемент из списка и с формы
            appElements.Remove(elementToRemove);
            this.Controls.Remove(selectedControl);
            selectedControl.Dispose();

            // Сбрасываем выбранный элемент
            selectedControl = null;

            // Обновляем статусную строку
            UpdateStatus(null);
        }

        // ===================================================================================



        // Общая функция для добавления элементов
        private void AddElement(eElementType type)
        {
            // Увеличиваем идентификатор
            elementID++;

            // Создаем новый элемент данных
            var newElement = new ElementDataApp
            {
                ElementType = type,
                Control = CreateControl(type)
            };

            // Проверяем, был ли создан Control
            if (newElement.Control == null) return;

            // Присваиваем уникальное имя элементу
            newElement.Control.Name = $"{type}{elementID}";

            // Устанавливаем координаты элемента на форме
            newElement.Control.Left = 100;
            newElement.Control.Top = 100;

            // Привязываем события для перемещения и изменения размеров
            AttachControlEvents(newElement.Control);

            // Добавляем элемент в список
            appElements.Add(newElement);

            // Добавляем Control на форму
            this.Controls.Add(newElement.Control);
        }


        // Функция для создания Control на основе eElementType
        private Control CreateControl(eElementType type)
        {
            var controlMap = ElementDataApp.GetClassFromType();

            if (!controlMap.ContainsKey(type))
                return null;

            switch (controlMap[type])
            {
                case eUsedFormClass.Label:
                    return new Label { Text = "Label", AutoSize = true };

                case eUsedFormClass.TextBox:
                    return new TextBox();

                case eUsedFormClass.Button:
                    return new Button { Text = "Button" };

                case eUsedFormClass.PictureBox:
                    return new PictureBox { BorderStyle = BorderStyle.FixedSingle, Size = new Size(100, 100) };

                //case eUsedFormClass.ImageList:
                //    return new ImageList();

                case eUsedFormClass.ProgressBar:
                    return new ProgressBar();

                default:
                    return null;
            }
        }

        // Привязывает обработчики событий для перемещения элементов
        private void AttachControlEvents(Control control)
        {
            control.MouseDown += Control_MouseDown;
            control.MouseMove += Control_MouseMove;
            control.MouseUp += Control_MouseUp;
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl)
            {
                resizeDirection = GetResizeDirection(ctrl, e.Location);

                if (resizeDirection != ResizeDirection.None)
                {
                    isResizing = true;
                    selectedControl = ctrl;
                    offset = e.Location; // Устанавливаем начальную точку
                }
                else
                {
                    isDragging = true;
                    selectedControl = ctrl;
                    offset = e.Location;
                }

                // Обновляем статусную строку
                UpdateStatus(selectedControl);
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl)
            {
                ctrl.Cursor = GetResizeCursor(GetResizeDirection(ctrl, e.Location));
            }

            if (isDragging && selectedControl != null)
            {
                selectedControl.Left = e.X + selectedControl.Left - offset.X;
                selectedControl.Top = e.Y + selectedControl.Top - offset.Y;
            }
            else if (isResizing && selectedControl != null)
            {
                int dx = e.X - offset.X;
                int dy = e.Y - offset.Y;

                if (resizeDirection == ResizeDirection.Right)
                {
                    selectedControl.Width = Math.Max(20, selectedControl.Width + dx);
                }
                else if (resizeDirection == ResizeDirection.Bottom)
                {
                    selectedControl.Height = Math.Max(20, selectedControl.Height + dy);
                }

                offset = new Point(e.X, e.Y); // Обновляем offset для плавности
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            isResizing = false;

            // Если кнопка мыши все еще нажата – не сбрасываем selectedControl
            if (Control.MouseButtons != MouseButtons.None)
            {
                return;
            }

            if (sender is Control ctrl && ctrl == selectedControl)
            {
                selectedControl = ctrl;
            }
        }

        private ResizeDirection GetResizeDirection(Control ctrl, Point mousePosition)
        {
            bool right = mousePosition.X >= ctrl.Width - ResizeMargin;
            bool bottom = mousePosition.Y >= ctrl.Height - ResizeMargin;

            if (right && bottom) return ResizeDirection.None; // Не даем менять размер в обоих направлениях одновременно
            if (right) return ResizeDirection.Right;
            if (bottom) return ResizeDirection.Bottom;

            return ResizeDirection.None;
        }

        private Cursor GetResizeCursor(ResizeDirection direction)
        {
            switch (direction)
            {
                case ResizeDirection.Right:
                    return Cursors.SizeWE;
                case ResizeDirection.Bottom:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Default;
            }
        }

        /// <summary>
        /// Обновляет статусную строку с информацией о выбранном элементе.
        /// </summary>
        /// <param name="ctrl">Выбранный элемент управления.</param>
        private void UpdateStatus(Control ctrl)
        {
            if (ctrl == null)
            {
                toolStripStatusLabelType.Text = "Не выбрано";
                toolStripStatusLabelTitle.Text = "";
                return;
            }

            // Получаем соответствие eElementType -> eUsedFormClass
            var typeMap = ElementDataApp.GetClassFromType();
            var element = appElements.FirstOrDefault(el => el.Control == ctrl);

            if (element != null && typeMap.ContainsKey(element.ElementType))
            {
                toolStripStatusLabelType.Text = typeMap[element.ElementType].ToString(); // Отображаем eUsedFormClass
                toolStripStatusLabelTitle.Text = ctrl.Name; // Отображаем имя элемента
            }
            else
            {
                toolStripStatusLabelType.Text = "Неизвестный элемент";
                toolStripStatusLabelTitle.Text = ctrl.Name;
            }
        }

        private void ToolStripMenuItemZindexBack_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemZindexFront_Click(object sender, EventArgs e)
        {

        }
    }


}
