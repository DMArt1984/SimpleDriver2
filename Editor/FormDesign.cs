using System;
using System.Collections.Generic;
using System.Drawing;
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
        private List<ElementDataApp> elements = new List<ElementDataApp>(); // Список элементов приложения
        private ElementDataApp selectedElement;
        private Control clipboardControl; // Для копирования элементов
        private Point offset;
        private bool isResizing = false; // Флаг изменения размера
        private Point lastMousePosition;
        private ResizeDirection resizeDirection = ResizeDirection.None;

        private FormDesignProp openedPropertyForm;

        private uint controlID = 0;

        public FormDesign()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }

        private void FormDesign_Load(object sender, EventArgs e)
        {
            SetStatus();
            this.MouseMove += Form_MouseMove;
            this.MouseDown += Form_MouseDown;
            this.MouseUp += Form_MouseUp;
        }

        // ===================================================================================
        #region Работа с элементами

        private void SelectElement(ElementDataApp element)
        {
            selectedElement = element;
        }

        private void DeselectElement()
        {
            selectedElement = null;
        }

        public void AddElement(eElementType type)
        {
            ElementDataApp element = new ElementDataApp($"Element{++controlID}", type, CreateControlFromElement(type));
            elements.Add(element);
            this.Controls.Add(element.Control);
            AttachControlEvents(element.Control);
            element.Control.BringToFront();
        }

        private Control CreateControlFromElement(eElementType type)
        {
            Control control = null;

            switch (type)
            {
                case eElementType.Label:
                    control = new Label { Text = $"Label{controlID}" };
                    break;
                case eElementType.OutputBox:
                case eElementType.InputBox:
                    control = new TextBox { Text = $"TextBox{controlID}" };
                    if (type == eElementType.OutputBox)
                        ((TextBox)control).ReadOnly = true;
                    break;
                case eElementType.Button:
                    control = new Button { Text = $"Button{controlID}" };
                    break;
                case eElementType.PictureBox:
                case eElementType.Rectangle:
                    control = new PictureBox
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    break;
                case eElementType.Progress:
                    control = new ProgressBar();
                    break;
            }

            if (control != null)
            {
                control.Name = $"Element{controlID}";
                control.Left = 100;
                control.Top = 100;
                control.Width = 100;
                control.Height = 30;
                AttachControlEvents(control);
            }

            return control;
        }

        public void DeleteElement()
        {
            if (selectedElement != null)
            {
                this.Controls.Remove(selectedElement.Control);
                elements.Remove(selectedElement);
                selectedElement = null;
            }
        }

        #endregion

        // ===================================================================================
        #region Работа с PropertyGrid

        private void Element_DoubleClick(object sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                ElementDataApp element = elements.FirstOrDefault(x => x.Control == ctrl);
                if (element != null)
                {
                    if (openedPropertyForm == null || openedPropertyForm.IsDisposed)
                    {
                        openedPropertyForm = new FormDesignProp(element);
                        openedPropertyForm.Show();
                    }
                    else
                    {
                        openedPropertyForm.UpdateProperties(element);
                        openedPropertyForm.BringToFront();
                    }
                }
            }
        }

        #endregion

        // ===================================================================================
        #region Сохранение и загрузка

        private void SaveJson()
        {
            // 🔹 Обновляем `ElementDataApp` перед сохранением
            foreach (var element in elements)
            {
                element.X = element.Control.Left;
                element.Y = element.Control.Top;
                element.Width = element.Control.Width;
                element.Height = element.Control.Height;
                element.Text = element.Control.Text;
            }

            var jsonData = new
            {
                Main = elements.Select(e => e.ToJsonData()).ToList()
            };

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            File.WriteAllText("config.json", JsonConvert.SerializeObject(jsonData, settings));
        }


        private void LoadJson()
        {
            if (!File.Exists("config.json")) return;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<ElementDataJson>>>(File.ReadAllText("config.json"), settings);

            if (jsonData.ContainsKey("Main"))
            {
                foreach (var element in elements.Where(el => el.Control != null))
                {
                    this.Controls.Remove(element.Control);
                }
                elements.Clear();

                foreach (var el in jsonData["Main"])
                {
                    Control ctrl = CreateControlFromElement(el.ElementType);
                    if (ctrl != null)
                    {
                        this.Controls.Add(ctrl);
                        AttachControlEvents(ctrl);
                        ctrl.BringToFront();

                        var newElement = new ElementDataApp(el.Name, el.ElementType, ctrl)
                        {
                            X = el.X,
                            Y = el.Y,
                            Width = el.Width,
                            Height = el.Height,
                            Text = el.Text,
                            Size = el.Size,
                            ImagePath = el.ImagePath
                        };

                        elements.Add(newElement);
                    }
                }
            }
        }

        #endregion

        // ===================================================================================
        #region Обработка событий мыши

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl && ctrl != this)
            {
                ElementDataApp element = elements.FirstOrDefault(x => x.Control == ctrl);
                if (element != null)
                {
                    SelectElement(element);
                    resizeDirection = GetResizeDirection(ctrl, e.Location);

                    if (resizeDirection != ResizeDirection.None)
                    {
                        isResizing = true;
                        lastMousePosition = e.Location;
                    }
                    else
                    {
                        isResizing = false;
                        offset = new Point(e.X, e.Y);
                    }
                }
            }
            else
            {
                DeselectElement();
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedElement != null)
            {
                if (isResizing)
                {
                    int dx = e.X - lastMousePosition.X;
                    int dy = e.Y - lastMousePosition.Y;

                    switch (resizeDirection)
                    {
                        case ResizeDirection.Right:
                            selectedElement.Width = Math.Max(20, selectedElement.Width + dx);
                            break;
                        case ResizeDirection.Bottom:
                            selectedElement.Height = Math.Max(20, selectedElement.Height + dy);
                            break;
                    }

                    lastMousePosition = e.Location;
                }
                else if (resizeDirection == ResizeDirection.None && e.Button == MouseButtons.Left)
                {
                    selectedElement.X = e.X + selectedElement.X - offset.X;
                    selectedElement.Y = e.Y + selectedElement.Y - offset.Y;
                }

                // Теперь обновляем Control
                UpdateElementControl(selectedElement);
            }
        }


        private void UpdateElementControl(ElementDataApp element)
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
                    if (File.Exists(element.ImagePath))
                    {
                        pic.Image = Image.FromFile(element.ImagePath);
                        pic.Tag = element.ImagePath;
                    }
                }
            }
        }

        #endregion

        #region FormStatus

        /// <summary>
        /// Обновляет статусную строку с информацией о выбранном элементе.
        /// </summary>
        private void SetStatus()
        {
            if (selectedElement != null)
            {
                toolStripStatusLabelType.Text = selectedElement.ElementType.ToString();
                toolStripStatusLabelTitle.Text = selectedElement.Name;
                //toolStripStatusLabelPosition.Text = $"X: {selectedElement.X}, Y: {selectedElement.Y}";
                //toolStripStatusLabelSize.Text = $"W: {selectedElement.Width}, H: {selectedElement.Height}";
            }
            else
            {
                toolStripStatusLabelType.Text = "Нет элемента";
                toolStripStatusLabelTitle.Text = "";
                //toolStripStatusLabelPosition.Text = "";
                //toolStripStatusLabelSize.Text = "";
            }
        }

        /// <summary>
        /// Обновляет статусную строку с информацией о переданном элементе.
        /// </summary>
        /// <param name="element">Элемент, информацию о котором нужно отобразить.</param>
        private void SetStatus(ElementDataApp element)
        {
            if (element != null)
            {
                toolStripStatusLabelType.Text = element.ElementType.ToString();
                toolStripStatusLabelTitle.Text = element.Name;
                //toolStripStatusLabelPosition.Text = $"X: {element.X}, Y: {element.Y}";
                //toolStripStatusLabelSize.Text = $"W: {element.Width}, H: {element.Height}";
            }
            else
            {
                SetStatus();
            }
        }

        #endregion

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
            resizeDirection = ResizeDirection.None;

            if (selectedElement != null)
            {
                // 🔹 Обновляем данные в ElementDataApp после перемещения или изменения размера
                selectedElement.X = selectedElement.Control.Left;
                selectedElement.Y = selectedElement.Control.Top;
                selectedElement.Width = selectedElement.Control.Width;
                selectedElement.Height = selectedElement.Control.Height;

                // 🔹 Обновляем статусную строку
                SetStatus(selectedElement);

                // 🔹 Обновляем PropertyGrid, если он открыт
                if (openedPropertyForm != null && !openedPropertyForm.IsDisposed)
                {
                    openedPropertyForm.UpdateProperties(selectedElement);
                }
            }
        }

        /// <summary>
        /// Привязывает события к элементу управления.
        /// </summary>
        /// <param name="control">Элемент управления, к которому нужно привязать события.</param>
        private void AttachControlEvents(Control control)
        {
            control.MouseDown -= Form_MouseDown;
            control.MouseMove -= Form_MouseMove;
            control.MouseUp -= Form_MouseUp;
            control.MouseDoubleClick -= Element_DoubleClick;
            control.LocationChanged -= Element_LocationChanged;

            control.MouseDown += Form_MouseDown;
            control.MouseMove += Form_MouseMove;
            control.MouseUp += Form_MouseUp;
            control.MouseDoubleClick += Element_DoubleClick;
            control.LocationChanged += Element_LocationChanged;
        }

        /// <summary>
        /// Обработчик изменения позиции элемента управления.
        /// Обновляет данные в ElementDataApp при изменении местоположения элемента.
        /// </summary>
        /// <param name="sender">Элемент управления, изменивший местоположение.</param>
        /// <param name="e">Аргументы события.</param>
        private void Element_LocationChanged(object sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                // Находим соответствующий элемент
                var element = elements.FirstOrDefault(el => el.Control == ctrl);
                if (element != null)
                {
                    element.X = ctrl.Left;
                    element.Y = ctrl.Top;

                    // Обновляем PropertyGrid, если он открыт
                    if (openedPropertyForm != null && !openedPropertyForm.IsDisposed)
                    {
                        openedPropertyForm.UpdateProperties(element);
                    }
                }
            }
        }


        /// <summary>
        /// Определяет направление изменения размера элемента на основе положения курсора мыши.
        /// </summary>
        /// <param name="ctrl">Элемент управления</param>
        /// <param name="mousePosition">Текущая позиция курсора</param>
        /// <returns>Возвращает направление изменения размера</returns>
        private ResizeDirection GetResizeDirection(Control ctrl, Point mousePosition)
        {
            const int resizeMargin = 6;

            bool left = mousePosition.X < resizeMargin;
            bool right = mousePosition.X > ctrl.Width - resizeMargin;
            bool top = mousePosition.Y < resizeMargin;
            bool bottom = mousePosition.Y > ctrl.Height - resizeMargin;

            if (left && top) return ResizeDirection.TopLeft;
            if (right && top) return ResizeDirection.TopRight;
            if (left && bottom) return ResizeDirection.BottomLeft;
            if (right && bottom) return ResizeDirection.BottomRight;

            if (left) return ResizeDirection.Left;
            if (right) return ResizeDirection.Right;
            if (top) return ResizeDirection.Top;
            if (bottom) return ResizeDirection.Bottom;

            return ResizeDirection.None;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadJson();
        }

        private void saveJsonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveJson();
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

        // Копировать текущий элемент
        private void ToolStripMenuItemCommandCopy_Click(object sender, EventArgs e)
        {
            
        }

        // Удалить текущий элемент
        private void ToolStripMenuItemCommandDelete_Click(object sender, EventArgs e)
        {
           
        }

    }
}
