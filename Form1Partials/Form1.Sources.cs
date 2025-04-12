using Connector;
using DML;
using DocumentFormat.OpenXml.Office2010.Excel;
using LogCodeMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinSimpleIDriver
{
    public partial class Form1
    {

        #region Runtime

        // Подписка на события источника
        public void SubscribeToSource(SOURCE src)
        {
            src.eventStatus += SourceOnStatusChanged;
            src.eventError += SourceOnError;
            src.eventParams += SourceOnParamsChanged;
            src.eventReq += SourceOnDataReceived;
        }

        public void UnsubscribeFromSource(SOURCE src)
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
                row.Cells[DataTableLib.dtSource.col.Status].Value = $"{status} {status.GetText()}";
            }
        }

        private void SourceOnError(ushort Id, CodeMessage error)
        {
            var row = DataTableLib.dtSource.FindRowByID(Id);
            if (row != null)
            {
                row.Cells[DataTableLib.dtSource.col.Status].Value = $"{error.code} {error.message}";
            }
        }

        private void SourceOnParamsChanged(SourceParam param)
        {
            
        }

        private void SourceOnDataReceived(ushort sourceId, ushort groupId, List<ITagResult> results, int counter, int fails, int all, int good)
        {
            var row = DataTableLib.dtSource.FindRowByID(sourceId);
            if (row != null)
            {
                row.Cells[DataTableLib.dtSource.col.Statistic].Value = $"{counter} [{fails}] {all}/{good}";
            }
        }
        #endregion

    }
}
