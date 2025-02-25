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

                // Передаём объект для редактирования в PropertyGrid
                propertyGrid1.SelectedObject = new ElementProperties(element);
            }

            // Подписываемся на событие удаления
            this.element.OnElementDeleted += ElementDeletedHandler;

        }

        private void FormDesignProp_Load(object sender, EventArgs e)
        {

        }

        // Метод, вызываемый при удалении элемента
        private void ElementDeletedHandler(ElementDataApp el)
        {
            if (this.element == el)
            {
                this.Close(); // Закрываем окно свойств
            }
        }

        // Отписываемся от события при закрытии окна
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (element != null)
            {
                element.OnElementDeleted -= ElementDeletedHandler;
            }
            base.OnFormClosed(e);
        }
    }

    
}
