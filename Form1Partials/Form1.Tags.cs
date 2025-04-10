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

        #region Tag

        #region Tag.Event
        private void checkBoxTagEditor_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagDesc_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagBP_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagSG_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }
        private void checkBoxTagSave_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }
        private void checkBoxTagAddress_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }
        private void buttonTagHelp_Click(object sender, EventArgs e)
        {
            DataTableLib.dtTag.Help();
        }
        private void buttonTagView_Click(object sender, EventArgs e)
        {

        }
        private void buttonTagDel_Click(object sender, EventArgs e)
        {

        }
        private void buttonTagCopy_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Tag.Filter

        #region Tag.ComboFilter.Event

        private void comboBoxTagFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterSource.Text))
                DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterGroup_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterGroup.Text))
                DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterBlock_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterBlock.Text))
                DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterPage_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterPage.Text))
                DataTableLib.dtTag.TextFilter();
        }

        #endregion

        #region Tag.TextFilter.Event
        private void textBoxTagFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxTagFilter.Text))
                DataTableLib.dtTag.TextFilter();
        }
        #endregion

        private void buttonTagFilter_Click(object sender, EventArgs e)
        {
            TagFilter();
        }

        private void TagFilter()
        {
            FormLib.SaveTextComboBox(comboBoxTagFilterSource);
            FormLib.SaveTextComboBox(comboBoxTagFilterGroup);
            FormLib.SaveTextComboBox(comboBoxTagFilterBlock);
            FormLib.SaveTextComboBox(comboBoxTagFilterPage);
            DataTableLib.dtTag.TextFilter();
        }


        #endregion

        #region Tag.DGV

        #endregion

        #region Tag.DGV.Event

        private void dataGridViewTag_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewTag_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewTag.CurrentCell.ColumnIndex == DataTableLib.dtTag.col.Group)
            {
                DataTableLib.dtTag.UpdateDGVTagSourceLink();
            }
            TreeLib.DrawTreeSGT();
            TreeLib.DrawTreeBlock();
        }

        private void dataGridViewTag_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewTag_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewTag_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewTag);
        }

        private void dataGridViewTag_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }






        #endregion

        #endregion

        // ================================================================================================================

        #region Structure

        #region Structure.Event
        private void buttonStructureLeft_Click(object sender, EventArgs e)
        {
            splitContainerStructure.Panel2Collapsed = !splitContainerStructure.Panel2Collapsed;
            SetComboBoxTargetFilterStructure();
        }

        private void dataGridViewStructure_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewStructure);
        }
        #endregion

        #region Structure.Filter

        #region Structure.TextFilter.Event
        private void textBoxStructureFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxStructureFilter.Text))
            {
                DataTableLib.dtStructure.StructureFilter();
                //DataTableLib.dtStructTarget.StructureTargetFilter();
            }
        }
        private void buttonStructureFilter_Click(object sender, EventArgs e)
        {
            DataTableLib.dtStructure.StructureFilter();
            //DataTableLib.dtStructTarget.StructureTargetFilter();
        }


        #endregion
        private void dataGridViewStructure_SelectionChanged(object sender, EventArgs e)
        {
            FormLib.SaveTextComboBox(comboBoxStructureTargetFilterParent);
            SetComboBoxTargetFilterStructure();
        }

        private void SetComboBoxTargetFilterStructure()
        {
            string text = (splitContainerStructure.Panel1Collapsed) ? "" : (DataTableLib.GetValueFromCurrentRow(dataGridViewStructure, DataTableLib.dtStructure.col.Title));
            comboBoxStructureTargetFilterParent.Text = text;
            DataTableLib.dtStructTarget.StructureTargetFilter();
            DataTableLib.dtStructTag.StructureTagFilter();
        }



        #endregion

        #region Structure-Target

        private void buttonStructureRight_Click(object sender, EventArgs e)
        {
            splitContainerStructure.Panel1Collapsed = !splitContainerStructure.Panel1Collapsed;
            SetComboBoxTargetFilterStructure();
        }

        #region StructureTarget.Filter

        #region StructureTarget.ComboFilter.Event
        private void comboBoxTargetFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtStructTarget.StructureTargetFilter();
            DataTableLib.dtStructTag.StructureTagFilter();
        }
        private void comboBoxTargetFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxStructureTargetFilterParent.Text))
                DataTableLib.dtStructTarget.StructureTargetFilter();
        }

        private void buttonTargetFilter_Click(object sender, EventArgs e)
        {
            TargetAndTagFilter();
        }

        private void TargetAndTagFilter()
        {
            FormLib.SaveTextComboBox(comboBoxStructureTargetFilterParent);
            DataTableLib.dtStructTarget.StructureTargetFilter();
            DataTableLib.dtStructTag.StructureTagFilter();
        }

        private void dataGridViewTarget_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewStructureTarget); // new ID

            DataTableLib.SetParentInRow(dataGridViewStructureTarget, comboBoxStructureTargetFilterParent, DataTableLib.dtStructTarget.col.Structure); // filter

        }





        #endregion

        #endregion

        #endregion


        private void checkBoxStructCol_CheckedChanged(object sender, EventArgs e)
        {
            var check = checkBoxStructCol.Checked;
            dataGridViewStructureTag.Columns[DataTableLib.dtStructTag.col.Structure].Visible = check;
            dataGridViewStructureTarget.Columns[DataTableLib.dtStructTarget.col.Structure].Visible = check;

        }


        #endregion

        private void dataGridViewStructure_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeStructure();
        }

        private void dataGridViewStructureTag_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeStructure();
        }

        private void dataGridViewStructureTarget_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeStructure();
        }

    }
}
