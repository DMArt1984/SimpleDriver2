using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
            // Версия
            ToolStripMenuItemVer.Text += " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

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

            // Теги
            CheckTagColumns();

            #endregion


        }

        private void ToolStripMenuItemViewTree_Click(object sender, EventArgs e)
        {
            bool check = !ToolStripMenuItemViewTree.Checked;
            splitContainerForm.Panel1Collapsed = !check;
            ToolStripMenuItemViewTree.Checked = check;
        }

        // ===============================================================

        #region Source.DGV

        #region Sourse.DGV.Columns
        private void CheckSourceColumns()
        {
            bool checkE = checkBoxSourceEditor.Checked;
            dataGridViewSource.Columns["sourceID"].Visible = checkE;
            dataGridViewSource.Columns["sourceAutoRestart"].Visible = checkE;
            dataGridViewSource.Columns["sourceDriver"].Visible = checkE;
            dataGridViewSource.Columns["sourceAddress"].Visible = checkE;
            dataGridViewSource.RowHeadersVisible = checkE;
            dataGridViewSource.ReadOnly = !checkE;
            if (checkE)
            {
                dataGridViewSource.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            }
            else
            {
                dataGridViewSource.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            bool checkD = checkBoxSourceDesc.Checked;
            dataGridViewSource.Columns["sourceDesc"].Visible = checkD;

            bool checkR = checkBoxSourceRuntime.Checked;
            dataGridViewSource.Columns["sourceStatus"].Visible = checkR;
            dataGridViewSource.Columns["sourceMessage"].Visible = checkR;

            bool checkS = checkBoxSourceStatistic.Checked;
            dataGridViewSource.Columns["sourceTags"].Visible = checkS;
            dataGridViewSource.Columns["sourceStatistic"].Visible = checkS;

        }

        #region Source.DGV.Event
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


        #endregion

        #region Group.DGV

        #region Group.DGV.Columns

        private void CheckGroupColumns()
        {
            bool checkE = checkBoxGroupEditor.Checked;
            dataGridViewGroup.Columns["groupID"].Visible = checkE;
            dataGridViewGroup.Columns["groupPeriod"].Visible = checkE;
            dataGridViewGroup.RowHeadersVisible = checkE;
            dataGridViewGroup.ReadOnly = !checkE;
            if (checkE)
            {
                dataGridViewGroup.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            } else
            {
                dataGridViewGroup.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            bool checkD = checkBoxGroupDesc.Checked;
            dataGridViewGroup.Columns["groupDesc"].Visible = checkD;

            bool checkR = checkBoxGroupRuntime.Checked;
            dataGridViewGroup.Columns["groupStatus"].Visible = checkR;

            bool checkS = checkBoxGroupStatistic.Checked;
            dataGridViewGroup.Columns["groupTags"].Visible = checkS;
            dataGridViewGroup.Columns["groupStatistic"].Visible = checkS;
        }

        #region Group.DGV.Event

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

        #endregion

        #region Tag.DGV

        #region Tag.DGV.Columns

        private void CheckTagColumns()
        {
            bool checkE = checkBoxTagEditor.Checked;
            dataGridViewTag.Columns["tagID"].Visible = checkE;
            dataGridViewTag.Columns["tagAddress"].Visible = checkE;
            dataGridViewTag.Columns["tagCommand"].Visible = checkE;
            dataGridViewTag.Columns["tagWriteValue"].Visible = checkE;
            dataGridViewTag.Columns["tagWriteTag"].Visible = checkE;
            dataGridViewTag.RowHeadersVisible = checkE;
            dataGridViewTag.ReadOnly = !checkE;
            if (checkE)
            {
                dataGridViewTag.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            }
            else
            {
                dataGridViewTag.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            bool checkD = checkBoxTagDesc.Checked;
            dataGridViewTag.Columns["tagDesc"].Visible = checkD;
            

            bool checkR = checkBoxTagRuntime.Checked;
            dataGridViewTag.Columns["tagValue"].Visible = checkR;
            dataGridViewTag.Columns["tagStatus"].Visible = checkR;
            dataGridViewTag.Columns["tagMessage"].Visible = checkR;

            bool checkS = checkBoxTagStatistic.Checked;
            dataGridViewTag.Columns["tagStatistic"].Visible = checkS;

            bool checkBP = checkBoxTagBP.Checked;
            dataGridViewTag.Columns["tagBlock"].Visible = checkBP;
            dataGridViewTag.Columns["tagPage"].Visible = checkBP;
        }

        #region Tag.DGV.Event
        private void checkBoxTagEditor_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagRuntime_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagDesc_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagStatistic_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagBP_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }
        #endregion


        #endregion

        #endregion

        #region Menu.File.Event

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
