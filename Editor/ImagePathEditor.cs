using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WinSimpleIDriver.Editor
{
    public class ImagePathEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal; // Открываем модальное окно
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (editorService != null)
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.Filter = "Изображения|*.jpg;*.png;*.bmp";
                        openFileDialog.Title = "Выберите изображение";

                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            return openFileDialog.FileName;
                        }
                    }
                }
            }
            return value; // Если ничего не выбрано, возвращаем текущее значение
        }
    }
}
