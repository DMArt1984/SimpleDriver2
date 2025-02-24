using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinSimpleIDriver.Editor
{
    public partial class FormDesignProp : Form
    {
        public ElementDataApp element;

        public FormDesignProp(ElementDataApp element)
        {
            InitializeComponent();

            this.element = element;

            if (element != null && element.Control != null)
            {
                this.Text = element.Control.Name; // Устанавливаем заголовок формы в имя элемента
            }

        }

        private void FormDesignProp_Load(object sender, EventArgs e)
        {

        }
    }

    
}
