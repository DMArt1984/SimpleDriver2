using Connector;
using DML;
using LogCodeMessage;
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

        #region Source

        #region Source.Event
        private void checkBoxSourceEditor_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceDesc_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CheckColumns();
        }
        private void buttonSourceCopy_Click(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CopyDGVRow();
        }
        private void buttonSourceDel_Click(object sender, EventArgs e)
        {
            DataTableLib.dtSource.DelDGVRow();
        }
        private void buttonSourceHelp_Click(object sender, EventArgs e)
        {
            DataTableLib.dtSource.Help();
        }
        private void buttonSourceView_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Source.Filter

        #region Source.TextFilter.Event
        private void textBoxSourceFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxSourceFilter.Text))
                DataTableLib.dtSource.TextFilter();
        }

        private void buttonSourceFilter_Click(object sender, EventArgs e)
        {
            DataTableLib.dtSource.TextFilter();
        }

        #endregion



        #endregion

        #region Source.DGV.Event

        private void dataGridViewSource_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewSource_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeSGT();
        }

        private void dataGridViewSource_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewSource_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewSource_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewSource);
        }

        private void dataGridViewSource_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        #endregion

        #endregion

        #region Runtime

        private void SubscribeToSource(Source src)
        {
            src.eventStatus += SourceOnStatusChanged;
            src.eventError += SourceOnError;
            src.eventParams += SourceOnParamsChanged;
            src.eventReq += SourceOnDataReceived;
        }

        private void SourceOnStatusChanged(ushort id, eSourceStatus status)
        {
            // например, обновление таблицы/лога
        }

        private void SourceOnError(ushort id, CodeMessage error)
        {
            // лог ошибки
        }

        private void SourceOnParamsChanged(SourceParam param)
        {
            // возможно, обновление UI
        }

        private void SourceOnDataReceived(ushort sourceId, ushort groupId, List<ITagResult> results, int counter, int fails, int all, int good)
        {
            // работа с результатами
        }
        #endregion

    }
}
