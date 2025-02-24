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
        private List<ElementData> elements = new List<ElementData>();
        private Control selectedControl;
        private Control clipboardControl; // Для копирования элементов
        private Point offset;
        private bool isResizing = false; // Флаг изменения размера
        private Point lastMousePosition; // Последняя позиция мыши
        private ResizeDirection resizeDirection = ResizeDirection.None; // Направление изменения
        private PictureBox backgroundPictureBox = new PictureBox();

        private FormDesignProp openedPropertyForm; // Открываем PropertyForm

        //private HashSet<string> allowedTypes = new HashSet<string> { "Label", "TextBox", "PictureBox" };
        //private HashSet<Type> allowedTypes = new HashSet<Type> { typeof(Label), typeof(TextBox), typeof(PictureBox) };

        //private bool mouseMove = false;

        private uint controlID = 0; // Идентификатор элемента
        public FormDesign()
        {
            InitializeComponent();
            // Включаем двойную буферизацию для уменьшения мерцания
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.UpdateStyles();

            InitializeBackgroundImage();
        }

        private void InitializeBackgroundImage()
        {
            // v1
            //backgroundPictureBox.Dock = DockStyle.Fill;
            //backgroundPictureBox.SizeMode = PictureBoxSizeMode.Normal; // PictureBoxSizeMode.StretchImage;

            // v2
            backgroundPictureBox.Location = new Point(0, menuStrip1.Height);
            backgroundPictureBox.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - menuStrip1.Height);
            backgroundPictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            this.Controls.Add(backgroundPictureBox);
            backgroundPictureBox.SendToBack();
        }

        private void FormDesign_Load(object sender, EventArgs e)
        {
            //LoadJson(); // Автоматическая загрузка
            SetStatus();

            this.MouseMove += Form_MouseMove;
            this.MouseDown += Form_MouseDown;
            this.MouseUp += Form_MouseUp;
        }

        #region Events

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (selectedControl != null)
            {
                using (Pen pen = new Pen(Color.Blue, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    Rectangle rect = new Rectangle(
                        selectedControl.Left - 2, selectedControl.Top - 2,
                        selectedControl.Width + 4, selectedControl.Height + 4
                    );
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }



        #region Events.Mouse

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl && ctrl != this && ctrl != backgroundPictureBox)
            {
                SelectElement(ctrl); // Выбираем элемент и рисуем рамку
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

                SetStatus(ctrl);
            }
            else
            {
                DeselectElement(); // Убираем рамку
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedControl != null)
            {
                // Сохраняем старую область для обновления
                Rectangle oldBounds = new Rectangle(
                    selectedControl.Left - 2, selectedControl.Top - 2,
                    selectedControl.Width + 4, selectedControl.Height + 4
                );

                if (isResizing)
                {
                    int dx = e.X - lastMousePosition.X;
                    int dy = e.Y - lastMousePosition.Y;

                    switch (resizeDirection)
                    {
                        case ResizeDirection.Right:
                            selectedControl.Width = Math.Max(20, selectedControl.Width + dx);
                            break;
                        case ResizeDirection.Bottom:
                            selectedControl.Height = Math.Max(20, selectedControl.Height + dy);
                            break;
                    }

                    lastMousePosition = e.Location;
                }
                else if (resizeDirection == ResizeDirection.None && e.Button == MouseButtons.Left)
                {
                    selectedControl.Left = e.X + selectedControl.Left - offset.X;
                    selectedControl.Top = e.Y + selectedControl.Top - offset.Y;
                }

                // Создаем новую область для обновления
                Rectangle newBounds = new Rectangle(
                    selectedControl.Left - 2, selectedControl.Top - 2,
                    selectedControl.Width + 4, selectedControl.Height + 4
                );

                // Перерисовываем только измененные области
                Invalidate(oldBounds);
                Invalidate(newBounds);

                // Устанавливаем корректный курсор
                selectedControl.Cursor = GetResizeCursor(GetResizeDirection(selectedControl, e.Location));
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
            resizeDirection = ResizeDirection.None;

            if (selectedControl != null)
            {
                Invalidate();
                Update(); // Принудительное обновление формы

                // Обновляем PropertyGrid
                if (openedPropertyForm != null && !openedPropertyForm.IsDisposed)
                {
                    openedPropertyForm.UpdateProperties(selectedControl);
                }
            }
        }



        #endregion

        #endregion

        // =====================================================================================

        #region Copy and Delete

        #region Events
        private void ToolStripMenuItemCommandCopy_Click(object sender, EventArgs e)
        {
            CopyElement();
            PasteElement();
        }

        private void ToolStripMenuItemCommandDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        #endregion

        // Копировать элемент в буфер
        public void CopyElement()
        {
            if (selectedControl != null)
            {
                clipboardControl = selectedControl; // Сохраняем копируемый элемент
                SetStatus(selectedControl);
            }
        }
        // Вставить элемент из буфера
        public void PasteElement()
        {
            if (clipboardControl != null)
            {
                Control newControl = CloneControl(clipboardControl);
                this.Controls.Add(newControl);
                newControl.BringToFront();
                selectedControl = newControl;
                SetStatus(selectedControl);
            }
        }
        // Клонирование элемента
        private Control CloneControl(Control original)
        {
            Control clone = null;

            if (original is Label lbl)
            {
                clone = new Label
                {
                    Text = lbl.Text,
                    Size = lbl.Size,
                    Location = new Point(lbl.Left + 10, lbl.Top + 10),
                    Font = lbl.Font,
                    BackColor = lbl.BackColor
                };
            }
            else if (original is TextBox txt)
            {
                clone = new TextBox
                {
                    Text = txt.Text,
                    Size = txt.Size,
                    Location = new Point(txt.Left + 10, txt.Top + 10),
                    Font = txt.Font
                };
            }
            else if (original is PictureBox pic)
            {
                clone = new PictureBox
                {
                    Size = pic.Size,
                    Location = new Point(pic.Left + 10, pic.Top + 10),
                    Image = pic.Image,
                    SizeMode = pic.SizeMode,
                    BorderStyle = pic.BorderStyle
                };
            }

            clone.Name = $"{original.Name}_copy";

            if (clone != null)
            {
                clone.MouseDown += Form_MouseDown;
                clone.MouseMove += Form_MouseMove;
                clone.MouseUp += Form_MouseUp;
                clone.MouseDoubleClick += Element_DoubleClick;
            }

            return clone;
        }

        // Удалить текущий элемент
        public void Delete()
        {
            if (selectedControl != null)
            {
                this.Controls.Remove(selectedControl);
                selectedControl.Dispose();
                selectedControl = null;
                SetStatus();
            }
        }

        #endregion


        private void AttachControlEvents(Control control)
        {
            control.MouseDown += Form_MouseDown;
            control.MouseMove += Form_MouseMove;
            control.MouseUp += Form_MouseUp;
            control.MouseDoubleClick += Element_DoubleClick;
            control.LocationChanged += Element_LocationChanged;
        }

        // ===================================================================================

        #region File

        #region Events

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadJson();
        }
        private void saveJsonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveJson();
        }

        #endregion

        private void LoadJson()
        {
            if (!File.Exists("config.json")) return;

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore // Игнорируем отсутствующие поля
            };

            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<ElementData>>>(File.ReadAllText("config.json"), settings);

            if (jsonData.ContainsKey("Main"))
            {
                this.Controls.OfType<Control>()
                    .Where(c => GetElementType(c) != eElementType.None)
                    .ToList().ForEach(c => { this.Controls.Remove(c); c.Dispose(); });

                foreach (var el in jsonData["Main"])
                {
                    // Проверка на null перед загрузкой
                    el.Text = el.Text ?? "";
                    el.Size = el.Size > 0 ? el.Size : 12.0f;
                    el.Min = el.Min > 0 ? el.Min : 0;
                    el.Max = el.Max > 0 ? el.Max : 100;
                    el.Value = el.Value ?? "";
                    el.ListName = el.ListName ?? "";
                    el.ToolTip = el.ToolTip ?? "";
                    el.TagTitle = el.TagTitle ?? "";

                    Control ctrl = CreateControlFromElement(el);
                    if (ctrl != null)
                    {
                        this.Controls.Add(ctrl);
                        AttachControlEvents(ctrl);
                        this.Controls.SetChildIndex(ctrl, el.ZIndex);
                    }
                }
            }
        }

        private void SaveJson()
        {
            var jsonData = new
            {
                Main = this.Controls.OfType<Control>()
                    .Where(c => c != backgroundPictureBox && !(c is MenuStrip))
                    .Select(c => new ElementData
                    {
                        Name = c.Name,
                        ElementType = GetElementType(c),
                        X = c.Left,
                        Y = c.Top,
                        Relative = false,
                        Width = c.Width,
                        Height = c.Height,
                        Text = c.Text,
                        Format = null,
                        Size = c.Font?.Size ?? 12.0f,
                        Color = null,
                        ZIndex = this.Controls.GetChildIndex(c),
                        ImagePath = c is PictureBox pic ? pic.Tag as string : null,
                        ImagesName = null,
                        Command = null,
                        Visible = null,
                        Min = 0,
                        Max = 0,
                        Value = null,
                        ListName = null,
                        ToolTip = null,
                        TagTitle = null
                    }).ToList()
            };

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore // Убираем null-поля
            };

            File.WriteAllText("config.json", JsonConvert.SerializeObject(jsonData, settings));
        }

        private eElementType GetElementType(Control c)
        {
            if (c is Label) return eElementType.Label;
            if (c is TextBox txt)
            {
                if (txt.ReadOnly) return eElementType.OutputBox;
                return eElementType.InputBox;
            }
            if (c is Button) return eElementType.Button;
            if (c is PictureBox pic)
            {
                return pic.Image != null ? eElementType.PictureBox : eElementType.Rectangle;
            }
            if (c is ProgressBar) return eElementType.Progress;

            return eElementType.None; // Если элемент не распознан, он теперь не попадет в JSON
        }





        private Control CreateControlFromElement(ElementData el)
        {
            Control control = null;

            switch (el.ElementType)
            {
                case eElementType.Label:
                    control = new Label { Text = el.Text };
                    break;

                case eElementType.OutputBox:
                case eElementType.InputBox:
                case eElementType.IOBox:
                case eElementType.IOPop:
                    control = new TextBox { Text = el.Text };
                    if (el.ElementType == eElementType.OutputBox)
                        ((TextBox)control).ReadOnly = true;
                    break;

                case eElementType.Button:
                    control = new Button { Text = el.Text };
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
                control.Name = el.Name;
                control.Left = el.X;
                control.Top = el.Y;
                control.Width = el.Width;
                control.Height = el.Height;

                // Применяем шрифт только для Label и TextBox
                if (control is Label || control is TextBox)
                {
                    if (el.Size <= 0)
                    {
                        el.Size = 12.0f; // Гарантия, что размер шрифта всегда больше 0
                    }
                    control.Font = new Font("Arial", el.Size);
                }

                if (control is PictureBox pic && !string.IsNullOrEmpty(el.ImagePath) && File.Exists(el.ImagePath))
                {
                    pic.Image = Image.FromFile(el.ImagePath);
                    pic.Tag = el.ImagePath;
                }
            }

            return control;
        }


        #endregion

        // ===================================================================================

        #region FormStatus

        // Текущий элемент
        private void SetStatus(Control ctrl)
        {
            string controlTypeName = ctrl.GetType().Name;
            SetStatus(controlTypeName, ctrl.Name);
        }
        private void SetStatus(string valueType = "", string valueTitle = "")
        {
            toolStripStatusLabelType.Text = valueType;
            toolStripStatusLabelTitle.Text = valueTitle;
        }


        #endregion

        // ===================================================================================

        #region Form background image

        #region Events
        private void ToolStripMenuItemBackgroundImage_Click(object sender, EventArgs e)
        {
            LoadBackgroundImage();
        }

        private void ToolStripMenuItemRemoveBackImage_Click(object sender, EventArgs e)
        {
            RemoveBackgroundImage();
        }
        #endregion

        /// <summary>
        /// Загружает фоновое изображение напрямую в `BackgroundImage`.
        /// </summary>
        private void LoadBackgroundImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.png;*.bmp",
                Title = "Выберите фоновое изображение"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Удаляем предыдущее изображение, если оно было
                    if (this.BackgroundImage != null)
                    {
                        this.BackgroundImage.Dispose();
                        this.BackgroundImage = null;
                    }

                    // Загружаем новое изображение как фон формы
                    this.BackgroundImage = Image.FromFile(openFileDialog.FileName);
                    this.BackgroundImageLayout = ImageLayout.None; // Растягиваем на всю форму

                    // 🔹 Принудительное обновление формы и рамки
                    Invalidate();
                    Update();
                    UpdateSelectionFrame();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Удаляет фоновое изображение.
        /// </summary>
        private void RemoveBackgroundImage()
        {
            if (this.BackgroundImage != null)
            {
                this.BackgroundImage.Dispose(); // Освобождаем память
                this.BackgroundImage = null; // Убираем изображение

                // 🔹 Обновляем отображение, чтобы вернуть прозрачный фон
                Invalidate();
                UpdateSelectionFrame();
            }
        }

        #endregion

        // ===================================================================================

        #region Elements

        #region Select/Deselect
        /// <summary>
        /// Обновляет рамку при выборе элемента.
        /// </summary>
        private void SelectElement(Control ctrl)
        {
            selectedControl = ctrl;
            Invalidate(); // Перерисовываем рамку
        }

        /// <summary>
        /// Убирает рамку при клике на форму.
        /// </summary>
        private void DeselectElement()
        {
            selectedControl = null;
            Invalidate(); // Убираем рамку
        }

        /// <summary>
        /// Обновляет рамку при изменении размера или перемещении элемента.
        /// </summary>
        private void UpdateSelectionFrame()
        {
            if (selectedControl != null)
            {
                Invalidate();
            }
        }
        #endregion

        #region Resize and Move
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

        private ResizeDirection GetResizeDirection(Control ctrl, Point mousePosition)
        {
            const int resizeMargin = 6; // Отступ для изменения размера

            bool right = mousePosition.X > ctrl.Width - resizeMargin;
            bool bottom = mousePosition.Y > ctrl.Height - resizeMargin;

            if (right) return ResizeDirection.Right;
            if (bottom) return ResizeDirection.Bottom;

            return ResizeDirection.None;
        }


        #endregion

        #endregion

        // =============================================================================

        #region ADD NEW Element

        #region Events
        private void ToolStripMenuItemAddControlLabel_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.Label);
        }

        private void ToolStripMenuItemAddControlOutput_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.InputBox);
        }

        private void ToolStripMenuItemAddControlPicture_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.PictureBox);

        }

        private void ToolStripMenuItemAddControlOutput_Click_1(object sender, EventArgs e)
        {
            AddElement(eElementType.OutputBox);
        }

        private void ToolStripMenuItemAddControlRectangle_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.Rectangle);
        }
        #endregion

        #region My Elements

        // Добавление Label
        public void AddLabel()
        {
            string title = $"Label{++controlID}";
            Label lbl = new Label
            {
                Name = title,
                Text = title,
                Width = 200,
                Height = 25,
                Left = 100,
                Top = 100,
                Font = new Font("Arial", 12),
                BackColor = Color.Transparent
            };

            AttachControlEvents(lbl);
            this.Controls.Add(lbl);
            lbl.BringToFront();
        }

        // Добавление TextBox
        public void AddTextBox(eElementType type)
        {
            string title = $"{type}{++controlID}";
            TextBox txt = new TextBox
            {
                Name = title,
                Text = title,
                Width = 200,
                Height = 25,
                Left = 100,
                Top = 100,
                ReadOnly = (type == eElementType.OutputBox) // OutputBox только для чтения
            };

            AttachControlEvents(txt);
            this.Controls.Add(txt);
            txt.BringToFront();
        }

        // Добавление Button
        public void AddButton()
        {
            string title = $"Button{++controlID}";
            Button btn = new Button
            {
                Name = title,
                Text = "Кнопка",
                Width = 100,
                Height = 30,
                Left = 100,
                Top = 100
            };

            AttachControlEvents(btn);
            this.Controls.Add(btn);
            btn.BringToFront();
        }

        // Добавление PictureBox
        public void AddPictureBox()
        {
            string title = $"PictureBox{++controlID}";
            PictureBox pic = new PictureBox
            {
                Name = title,
                Width = 100,
                Height = 100,
                Left = 100,
                Top = 100,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.LightGray
            };

            AttachControlEvents(pic);
            this.Controls.Add(pic);
            pic.BringToFront();
        }

        // Добавление ProgressBar
        public void AddProgressBar()
        {
            string title = $"Progress{++controlID}";
            ProgressBar progress = new ProgressBar
            {
                Name = title,
                Width = 200,
                Height = 25,
                Left = 100,
                Top = 100
            };

            AttachControlEvents(progress);
            this.Controls.Add(progress);
            progress.BringToFront();
        }

        #endregion

        public void AddElement(eElementType type)
        {
            switch (type)
            {
                case eElementType.Label:
                    AddLabel();
                    break;
                case eElementType.OutputBox:
                case eElementType.InputBox:
                case eElementType.IOBox:
                case eElementType.IOPop:
                    AddTextBox(type);
                    break;
                case eElementType.Button:
                    AddButton();
                    break;
                case eElementType.PictureBox:
                case eElementType.Rectangle:
                    AddPictureBox();
                    break;
                case eElementType.Progress:
                    AddProgressBar();
                    break;
            }
        }

        #endregion

        // =============================================================================

        // Открыть окно свойств
        private void Element_DoubleClick(object sender, EventArgs e)
        {
            // v1
            //if (sender is Control ctrl)
            //{
            //    FormDesignProp propForm = new FormDesignProp(ctrl);
            //    propForm.Show(this);
            //}

            // v2
            if (sender is Control ctrl)
            {
                if (openedPropertyForm == null || openedPropertyForm.IsDisposed)
                {
                    openedPropertyForm = new FormDesignProp(ctrl);
                    openedPropertyForm.Show();
                }
                else
                {
                    openedPropertyForm.UpdateProperties(ctrl);
                    openedPropertyForm.BringToFront();
                }
            }
        }

        // Обновляем свойства в окне PropertyGrid
        private void Element_LocationChanged(object sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                if (openedPropertyForm != null && !openedPropertyForm.IsDisposed)
                {
                    openedPropertyForm.UpdateProperties(ctrl);
                }
            }
        }


    }
}
