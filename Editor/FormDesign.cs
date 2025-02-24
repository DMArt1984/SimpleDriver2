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
        private PictureBox backgroundPictureBox = new PictureBox();

        private FormDesignProp openedPropertyForm; // Открываем PropertyForm

        //private HashSet<string> allowedTypes = new HashSet<string> { "Label", "TextBox", "PictureBox" };
        private HashSet<Type> allowedTypes = new HashSet<Type> { typeof(Label), typeof(TextBox), typeof(PictureBox) };

        private bool mouseMove = false;

        private uint controlID = 0; // Идентификатор элемента
        public FormDesign()
        {
            InitializeComponent();
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

        
        #region Events.Mouse

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl && ctrl != this && ctrl != backgroundPictureBox)
            {
                selectedControl = ctrl;
                mouseMove = true;
                offset = new Point(e.X, e.Y);
                SetStatus(ctrl);
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedControl != null && mouseMove)
            {
                selectedControl.Left = e.X + selectedControl.Left - offset.X;
                selectedControl.Top = e.Y + selectedControl.Top - offset.Y;
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            mouseMove = false;
            //selectedControl = null;
            //SetStatus();

            // ---
            //if (selectedControl != null)
            //{
            //    elements.Add(new ElementData
            //    {
            //        Name = selectedControl.Name,
            //        Type = selectedControl.GetType().Name,
            //        X = selectedControl.Left,
            //        Y = selectedControl.Top,
            //        Height = selectedControl.Height,
            //        Width = selectedControl.Width,
            //        Text = selectedControl.Text,
            //        FontSize = selectedControl.Font.Size
            //    });
            //    selectedControl = null;
            //}
        }
        #endregion

        #region Events.Menu
        private void loadBacgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.png;*.bmp",
                Title = "Выберите фоновое изображение"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                backgroundPictureBox.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        #region Events.Menu.Add
        private void ToolStripMenuItemAddControlLabel_Click(object sender, EventArgs e)
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
                BackColor = Color.Transparent // Прозрачный фон
            };

            AttachControlEvents(lbl);
            //lbl.MouseDown += Form_MouseDown;
            //lbl.MouseMove += Form_MouseMove;
            //lbl.MouseUp += Form_MouseUp;
            //lbl.MouseDoubleClick += Element_DoubleClick; // Открывает окно редактирования
            //lbl.LocationChanged += Element_LocationChanged; // Динамичское обновление PropertyGrid

            this.Controls.Add(lbl);
            lbl.BringToFront();
        }

        private void ToolStripMenuItemAddControlOutput_Click(object sender, EventArgs e)
        {
            string title = $"TextBox{++controlID}";
            TextBox txt = new TextBox
            {
                Name = title,
                Text = title,
                Width = 200,
                Height = 25,
                Left = 100,
                Top = 100
            };

            AttachControlEvents(txt);
            //txt.MouseDown += Form_MouseDown;
            //txt.MouseMove += Form_MouseMove;
            //txt.MouseUp += Form_MouseUp;
            //txt.MouseDoubleClick += Element_DoubleClick; // Открывает окно редактирования
            //txt.LocationChanged += Element_LocationChanged; // Динамичское обновление PropertyGrid

            this.Controls.Add(txt);
            txt.BringToFront();
        }

        private void ToolStripMenuItemAddControlPicture_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = new PictureBox
            {
                Name = "Picture" + (this.Controls.OfType<PictureBox>().Count() + 1),
                Width = 100,
                Height = 100,
                Left = 200,
                Top = menuStrip1.Height + 100,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.LightGray // Цвет фона для наглядности
            };

            AttachControlEvents(pictureBox);
            //pictureBox.MouseDown += Form_MouseDown;
            //pictureBox.MouseMove += Form_MouseMove;
            //pictureBox.MouseUp += Form_MouseUp;
            //pictureBox.MouseDoubleClick += Element_DoubleClick;
            //pictureBox.LocationChanged += Element_LocationChanged; // Обновление PropertyGrid

            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();

        }
        #endregion

        private void ToolStripMenuItemCommandCopy_Click(object sender, EventArgs e)
        {
            CopyElement();
            PasteElement();
        }

        private void ToolStripMenuItemCommandDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        #region File
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadJson();
        }
        private void saveJsonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveJson();
        }
        #endregion

        #endregion

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




        #endregion

        #region Copy and Delete

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

        private void AttachControlEvents(Control control)
        {
            control.MouseDown += Form_MouseDown;
            control.MouseMove += Form_MouseMove;
            control.MouseUp += Form_MouseUp;
            control.MouseDoubleClick += Element_DoubleClick;
            control.LocationChanged += Element_LocationChanged;
        }

        #region File

        private void LoadJson()
        {
            if (!File.Exists("config.json")) return;

            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<ElementData>>>(File.ReadAllText("config.json"));

            if (jsonData.ContainsKey("Main"))
            {
                // Удаляем только элементы, которые соответствуют eElementType (не удаляем системные элементы)
                this.Controls.OfType<Control>()
                    .Where(c => GetElementType(c) != eElementType.None) // Удаляем только распознанные элементы
                    .ToList()
                    .ForEach(c => { this.Controls.Remove(c); c.Dispose(); });

                // Загружаем новые элементы
                foreach (var el in jsonData["Main"])
                {
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
                    .Where(c => c != backgroundPictureBox && !(c is MenuStrip) && !(c is StatusStrip)) // Исключаем StatusStrip
                    .Select(c =>
                    {
                        eElementType type = GetElementType(c);
                        if (type == eElementType.None) return null; // Пропускаем элементы без типа

                return new ElementData
                        {
                            Name = c.Name,
                            ElementType = type,
                            X = c.Left,
                            Y = c.Top,
                            Width = c.Width,
                            Height = c.Height,
                            Text = c.Text,
                            FontSize = c.Font.Size,
                            ZIndex = this.Controls.GetChildIndex(c),
                            ImagePath = c is PictureBox pic ? pic.Tag as string : null
                        };
                    })
                    .Where(el => el != null) // Убираем null элементы
                    .ToList()
            };

            File.WriteAllText("config.json", JsonConvert.SerializeObject(jsonData, Formatting.Indented));
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
                control.Font = new Font("Arial", el.FontSize);

                if (control is PictureBox pic && !string.IsNullOrEmpty(el.ImagePath) && File.Exists(el.ImagePath))
                {
                    pic.Image = Image.FromFile(el.ImagePath);
                    pic.Tag = el.ImagePath;
                }
            }

            return control;
        }


        #endregion

    }
}
