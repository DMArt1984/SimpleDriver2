using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver.Editor
{
    public partial class FormDesignProp : Form
    {
        private Control targetControl;

        public FormDesignProp(Control control)
        {
            InitializeComponent();

            this.targetControl = control;

            this.Text = "Редактирование свойств";
            this.Size = new System.Drawing.Size(300, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            propertyGrid1.Dock = DockStyle.Fill;
            propertyGrid1.SelectedObject = targetControl;
            this.Controls.Add(propertyGrid1);
        }

        private void FormDesignProp_Load(object sender, EventArgs e)
        {

        }
    }
}
