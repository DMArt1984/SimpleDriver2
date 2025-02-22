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
        private ControlProperties controlProperties;

        public FormDesignProp(Control control)
        {
            InitializeComponent();

            this.targetControl = control;
            this.controlProperties = new ControlProperties(control);

            this.Text = "Редактир свойств";
            //this.Size = new System.Drawing.Size(300, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            propertyGrid1.Dock = DockStyle.Fill;
            propertyGrid1.SelectedObject = controlProperties; // targetControl;
            //this.Controls.Add(propertyGrid1);
        }

        private void FormDesignProp_Load(object sender, EventArgs e)
        {

        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            controlProperties.ApplyChanges();
        }

        public void UpdateProperties(Control control)
        {
            this.targetControl = control;
            this.controlProperties = new ControlProperties(control);
            propertyGrid1.SelectedObject = controlProperties;
            propertyGrid1.Refresh(); // Обновляем PropertyGrid
        }

    }
}
