using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsFormsIDevice.Connector;
using WindowsFormsIDevice.Connector.SGT;

namespace WinSimpleIDriver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Table Enum
            // Устройства
            ComboBox cbDriver = new ComboBox();
            foreach (string title in Enum.GetNames(typeof(eDriverType)))
                cbDriver.Items.Add(title);
            ((DataGridViewComboBoxColumn)dataGridViewSource.Columns["sourceDriver"]).DataSource = cbDriver.Items;

            // Теги и структуры
            ComboBox cbTypeData = new ComboBox();
            foreach (string title in Enum.GetNames(typeof(eDataType)))
                cbTypeData.Items.Add(title);
            ((DataGridViewComboBoxColumn)dataGridViewTag.Columns["tagDataType"]).DataSource = cbTypeData.Items;
            #endregion

            #region View
            // Вид - Дерево
            bool check = ToolStripMenuItemViewTree.Checked;
            splitContainerForm.Panel1Collapsed = !check;

            // Источники
            CheckSourceColumns();

            // Группы
            CheckGroupColumns();

            #endregion


        }

        private void ToolStripMenuItemViewTree_Click(object sender, EventArgs e)
        {
            bool check = !ToolStripMenuItemViewTree.Checked;
            splitContainerForm.Panel1Collapsed = !check;
            ToolStripMenuItemViewTree.Checked = check;
        }

        // ===============================================================

        #region Source

        #region Sourse.Columns
        private void CheckSourceColumns()
        {
            bool checkE = checkBoxSourceEditor.Checked;
            dataGridViewSource.Columns["sourceID"].Visible = checkE;
            dataGridViewSource.Columns["sourceAutoRestart"].Visible = checkE;
            dataGridViewSource.Columns["sourceDriver"].Visible = checkE;
            dataGridViewSource.Columns["sourceAddress"].Visible = checkE;

            bool checkD = checkBoxSourceDesc.Checked;
            dataGridViewSource.Columns["sourceDesc"].Visible = checkD;

            bool checkR = checkBoxSourceRuntime.Checked;
            dataGridViewSource.Columns["sourceStatus"].Visible = checkR;
            dataGridViewSource.Columns["sourceMessage"].Visible = checkR;

            bool checkS = checkBoxSourceStatistic.Checked;
            dataGridViewSource.Columns["sourceTags"].Visible = checkS;
            dataGridViewSource.Columns["sourceStatistic"].Visible = checkS;

        }

        private void checkBoxSourceEditor_CheckedChanged(object sender, EventArgs e)
        {
            CheckSourceColumns();
        }

        private void checkBoxSourceRuntime_CheckedChanged(object sender, EventArgs e)
        {
            CheckSourceColumns();
        }

        private void checkBoxSourceDesc_CheckedChanged(object sender, EventArgs e)
        {
            CheckSourceColumns();
        }

        private void checkBoxSourceStatistic_CheckedChanged(object sender, EventArgs e)
        {
            CheckSourceColumns();
        }

        #endregion



        #endregion

        #region Group

        #region Group.Columns

        private void CheckGroupColumns()
        {
            bool checkE = checkBoxGroupEditor.Checked;
            dataGridViewGroup.Columns["groupID"].Visible = checkE;
            dataGridViewGroup.Columns["groupPeriod"].Visible = checkE;

            bool checkD = checkBoxGroupDesc.Checked;
            dataGridViewGroup.Columns["groupDesc"].Visible = checkD;

            bool checkR = checkBoxGroupRuntime.Checked;
            dataGridViewGroup.Columns["groupStatus"].Visible = checkR;

            bool checkS = checkBoxGroupStatistic.Checked;
            dataGridViewGroup.Columns["groupTags"].Visible = checkS;
            dataGridViewGroup.Columns["groupStatistic"].Visible = checkS;
        }

        private void checkBoxGroupEditor_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }

        private void checkBoxGroupDesc_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }

        private void checkBoxGroupStatistic_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }

        private void checkBoxGroupRuntime_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }



        #endregion

        #endregion


    }
}
