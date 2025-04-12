using Connector;
using DML;
using DocumentFormat.OpenXml.Bibliography;
using LogCodeMessage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WinSimpleIDriver.UserControl
{
    public partial class SourceForm: Form
    {
        public ushort Id;
        public FormMain parent;

        private SOURCE item;
        private bool useEvents = false;

        public SourceForm()
        {
            InitializeComponent();
        }

        private void SourceForm_Load(object sender, EventArgs e)
        {
            // Подписка на закрытие родительской формы
            if (parent != null)
                parent.FormClosed += Parent_FormClosed;

            // Новый
            if (Id == 0)
            {
                this.Text = $"Новый источник данных";

                tabControl1.Visible = true;
                tabControl1.TabPages.Remove(tabControl1.TabPages["tabPageControl"]);
                tabControl1.TabPages.Remove(tabControl1.TabPages["tabPageSetting"]);
                tabControl1.TabPages.Remove(tabControl1.TabPages["tabPageError"]);
                tabControl1.TabPages.Remove(tabControl1.TabPages["tabPageLog"]);

                richTextBoxDesc.ReadOnly = false;
                richTextBoxDesc.BorderStyle = BorderStyle.FixedSingle;

                // Драйверы
                comboBoxNewDriver.Items.Clear();
                foreach (string title in Enum.GetNames(typeof(eDriverType)))
                    comboBoxNewDriver.Items.Add(title);
                if (comboBoxNewDriver.Items.Count > 0)
                    comboBoxNewDriver.SelectedIndex = 0;

                return;
            }

            // Существующий
            item = SOURCE.Item(Id);
            if (item == null)
            {
                this.Text = $"Источник данных ID={Id} не найден!";
                return;
            }

            this.Text = $"Источник данных ID={item.Id} {item.title}";
            textBoxTitle.Text = item.title;
            textBoxDriver.Text = item.driverType.ToString();
            textBoxConnection.Text = item.Address;
            richTextBoxDesc.Text = item.description;

            checkBoxEnableLog.Enabled = item.IsSupportLog();
            if (item.IsSupportLog() == false)
                richTextBoxLog.Text = "Не поддерживается";

            // ping and host buttons
            bool net = item.IsNet();
            buttonTestHost.Visible = net;
            buttonTestPing.Visible = net;

            //
            tabControl1.Visible = true;
            tabControl1.TabPages.Remove(tabControl1.TabPages["tabPageNew"]);

            // подписки
            SubscribeToSource(item);


        }

        #region Events
        private void Parent_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close(); // Закрыть текущую (дочернюю) форму
        }

        public void SubscribeToSource(SOURCE src)
        {
            useEvents = true;
            src.eventStatus += SourceOnStatusChanged;
            src.eventError += SourceOnError;
            src.eventParams += SourceOnParamsChanged;
            src.eventReq += SourceOnDataReceived;
        }

        public void UnsubscribeFromSource(SOURCE src)
        {
            if (useEvents == false)
                return;

            src.eventStatus -= SourceOnStatusChanged;
            src.eventError -= SourceOnError;
            src.eventParams -= SourceOnParamsChanged;
            src.eventReq -= SourceOnDataReceived;
            useEvents = false;
        }

        private void SourceOnStatusChanged(ushort Id, eSourceStatus status)
        {

        }

        private void SourceOnError(ushort Id, CodeMessage error)
        {

        }

        private void SourceOnParamsChanged(SourceParam param)
        {

        }

        private void SourceOnDataReceived(ushort sourceId, ushort groupId, List<ITagResult> results, int counter, int fails, int all, int good)
        {

        }
        #endregion

        private void SourceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (parent != null)
                parent.FormClosed -= Parent_FormClosed;

            if (item != null)
                UnsubscribeFromSource(item);
        }
    }
}
