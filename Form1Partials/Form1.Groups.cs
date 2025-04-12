using Connector;
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

        private void GroupOnCycleRequest(IGroupOff group)
        {
            
        }

        #endregion

    }
}
