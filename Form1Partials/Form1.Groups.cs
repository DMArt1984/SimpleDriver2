using Connector;
using DML;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinSimpleIDriver
{
    public partial class FormMain
    {

        #region Runtime

        // Подписка на события группы
        public void SubscribeToGroup(GROUP group)
        {
            group.eventStatus += GroupOnStatusChanged;
            group.eventParams += GroupOnParamsChanged;
            group.tikTakReq += GroupOnCycleRequest;
        }
        public void UnsubscribeFromGroup(GROUP group)
        {
            group.eventStatus -= GroupOnStatusChanged;
            group.eventParams -= GroupOnParamsChanged;
            group.tikTakReq -= GroupOnCycleRequest;
        }

        private void GroupOnStatusChanged(ushort Id, eGroupStatus status)
        {
            var row = DataTableLib.dtGroup.FindRowByID(Id);
            if (row != null)
            {
                row.Cells[DataTableLib.dtGroup.col.Status].Value = $"{status} {status.GetText()}";
            }
        }

        private void GroupOnParamsChanged(GroupParamStatus info)
        {
            
        }

        private void GroupOnCycleRequest(IGroupTickAndOff group)
        {
            var row = DataTableLib.dtGroup.FindRowByID(group.Id);
            if (row != null)
            {
                row.Cells[DataTableLib.dtGroup.col.Statistic].Value = group.TickCount;
            }
        }

        #endregion

    }
}
