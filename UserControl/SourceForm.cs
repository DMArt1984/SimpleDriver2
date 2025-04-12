using Connector;
using DocumentFormat.OpenXml.Bibliography;
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

        public SourceForm()
        {
            InitializeComponent();
        }

        private void SourceForm_Load(object sender, EventArgs e)
        {
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
                comboBoxNewDriver.SelectedIndex = 0;

                return;
            }

            item = SOURCE.Item(Id);
            if (item == null)
            {
                this.Text = $"Источник данных ID={Id} не найден!";

                return;
            }

            this.Text = $"Источник данных ID={item.Id} {item.title}";
            textBoxTitle.Text = item.title;
            textBoxDriver.Text = item.driverType.ToString();
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
            //useEvents = true;
            //parent.mainIncommingClose += MeClose;
            //item.eventError += EventSourceError;
            //item.eventStatus += EventSourceStatus;
            //item.eventParams += EventSourceParams;
            //item.eventReq += EventSourceReq;
            //item.eventTraffic += EventTraffic;

            //
            //item.Refresh();

        }
    }
}
