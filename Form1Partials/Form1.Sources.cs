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

        

        #region Runtime

        // Подписка на события источника
        public void SubscribeToSource(Source src)
        {
            src.eventStatus += SourceOnStatusChanged;
            src.eventError += SourceOnError;
            src.eventParams += SourceOnParamsChanged;
            src.eventReq += SourceOnDataReceived;
        }

        public void UnsubscribeFromSource(Source src)
        {
            src.eventStatus -= SourceOnStatusChanged;
            src.eventError -= SourceOnError;
            src.eventParams -= SourceOnParamsChanged;
            src.eventReq -= SourceOnDataReceived;
        }

        private void SourceOnStatusChanged(ushort Id, eSourceStatus status)
        {
            var row = DataTableLib.dtSource.FindRowByID(Id);
            if (row != null)
            {
                row.Cells[DataTableLib.dtSource.col.Status].Value = status.GetText();
            }
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
