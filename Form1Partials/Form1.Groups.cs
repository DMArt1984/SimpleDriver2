using DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver
{
    public partial class Form1
    {

        #region Group

        #region Group.Event
        private void buttonGroupView_Click(object sender, EventArgs e)
        {

        }
        private void buttonGroupDel_Click(object sender, EventArgs e)
        {

        }
        private void buttonGroupCopy_Click(object sender, EventArgs e)
        {

        }
        private void checkBoxGroupEditor_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupDesc_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupSource_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }
        #endregion

        #region Group.Filter

        #region Group.ComboFilter.Event

        private void comboBoxGroupFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.TextFilter();
        }

        private void comboBoxGroupFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxGroupFilterSource.Text))
                DataTableLib.dtGroup.TextFilter();
        }

        #endregion

        #region Group.TextFilter.Event
        private void textBoxGroupFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxGroupFilter.Text))
                DataTableLib.dtGroup.TextFilter();
        }
        #endregion

        private void buttonGroupFilter_Click(object sender, EventArgs e)
        {
            GroupFilter();
        }

        private void GroupFilter()
        {
            FormLib.SaveTextComboBox(comboBoxGroupFilterSource);
            DataTableLib.dtGroup.TextFilter();
        }

        #endregion

        #region Group.DGV.Event

        private void dataGridViewGroup_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeSGT();
        }

        private void dataGridViewGroup_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewGroup_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewGroup_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewGroup);
        }

        private void dataGridViewGroup_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }
        #endregion


        #endregion

    }
}
