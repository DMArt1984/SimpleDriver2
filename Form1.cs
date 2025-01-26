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

        // ================================================================================================================

        #region Menu.File.Event

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }





        #endregion


        private void ToolStripMenuItemViewTree_Click(object sender, EventArgs e)
        {
            bool check = !ToolStripMenuItemViewTree.Checked;
            splitContainerForm.Panel1Collapsed = !check;
            ToolStripMenuItemViewTree.Checked = check;
        }

        // ================================================================================================================

        #region Source

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


        #endregion

        #region Source.Event
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

        #region Source.TextFilter.Event
        private void textBoxSourceFilter_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Source.DGV.Event

        private void dataGridViewSource_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewSource_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewSource_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewSource_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewSource_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridViewSource_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        #endregion


        #endregion

        // ================================================================================================================

        #region Group

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

            bool checkST = checkBoxGroupStatistic.Checked;
            dataGridViewGroup.Columns["groupTags"].Visible = checkST;
            dataGridViewGroup.Columns["groupStatistic"].Visible = checkST;

            bool checkSource = checkBoxGroupSource.Checked;
            dataGridViewGroup.Columns["groupSource"].Visible = checkSource;
        }


        #endregion

        #region Group.Event

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

        private void checkBoxGroupSource_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }
        #endregion

        #region Group.ComboFilter.Event

        private void comboBoxGroupFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxGroupFilterSource_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Group.TextFilter.Event
        private void textBoxGroupFilter_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Group.DGV.Event

        private void dataGridViewGroup_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewGroup_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewGroup_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewGroup_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridViewGroup_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }
        #endregion


        #endregion

        // ================================================================================================================

        #region Tag

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

            bool checkSG = checkBoxTagSG.Checked;
            dataGridViewTag.Columns["tagSource"].Visible = checkSG;
            dataGridViewTag.Columns["tagGroup"].Visible = checkSG;
        }


        #endregion

        #region Tag.Event
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

        private void checkBoxTagSG_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }
        #endregion

        #region Tag.ComboFilter.Event

        private void comboBoxTagFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTagFilterSource_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTagFilterGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTagFilterGroup_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTagFilterBlock_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTagFilterBlock_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTagFilterPage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTagFilterPage_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion

        #region Tag.TextFilter.Event
        private void textBoxTagFilter_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Tag.DGV.Event

        private void dataGridViewTag_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewTag_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewTag_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewTag_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewTag_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridViewTag_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        


        #endregion

        #endregion

        // ================================================================================================================



    }
}
