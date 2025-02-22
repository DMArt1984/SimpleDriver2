using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WinSimpleIDriver.Editor
{
    public partial class FormDesign : Form
    {
        private List<ElementData> elements = new List<ElementData>();
        private Control selectedControl;
        private Point offset;
        private PictureBox backgroundPictureBox = new PictureBox();

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
            this.MouseMove += Form_MouseMove;
            this.MouseDown += Form_MouseDown;
            this.MouseUp += Form_MouseUp;
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl && ctrl != this && ctrl != backgroundPictureBox)
            {
                selectedControl = ctrl;
                offset = new Point(e.X, e.Y);
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedControl != null)
            {
                selectedControl.Left = e.X + selectedControl.Left - offset.X;
                selectedControl.Top = e.Y + selectedControl.Top - offset.Y;
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedControl != null)
            {
                elements.Add(new ElementData
                {
                    Name = selectedControl.Name,
                    Type = selectedControl.GetType().Name,
                    X = selectedControl.Left,
                    Y = selectedControl.Top,
                    Height = selectedControl.Height,
                    Width = selectedControl.Width,
                    Text = selectedControl.Text,
                    FontSize = selectedControl.Font.Size
                });
                selectedControl = null;
            }
        }

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

        private void addLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Label lbl = new Label
            {
                Name = "Press1A",
                Text = "P",
                Width = 20,
                Height = 25,
                Left = 100,
                Top = 100,
                Font = new Font("Arial", 12),
                BackColor = Color.Transparent // Прозрачный фон
            };

            lbl.MouseDown += Form_MouseDown;
            lbl.MouseMove += Form_MouseMove;
            lbl.MouseUp += Form_MouseUp;
            lbl.MouseDoubleClick += Element_DoubleClick; // Открывает окно редактирования

            this.Controls.Add(lbl);
            lbl.BringToFront();
        }

        private void addOutputboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextBox txt = new TextBox
            {
                Name = "Press1B",
                Width = 60,
                Height = 25,
                Left = 150,
                Top = 100,
                Text = "{SupplyLinePressure}"
            };

            txt.MouseDown += Form_MouseDown;
            txt.MouseMove += Form_MouseMove;
            txt.MouseUp += Form_MouseUp;
            txt.MouseDoubleClick += Element_DoubleClick; // Открывает окно редактирования

            this.Controls.Add(txt);
            txt.BringToFront();
        }

        private void Element_DoubleClick(object sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                FormDesignProp propForm = new FormDesignProp(ctrl);
                propForm.Show(this);
            }
        }

        private void saveJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jsonData = new
            {
                Desc = "SupplyLinePressure",
                Elements = elements
            };

            string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            File.WriteAllText("config.json", json);
            MessageBox.Show("JSON сохранен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
