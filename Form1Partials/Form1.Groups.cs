using Connector;
using DML;
using DocumentFormat.OpenXml.Office2010.Excel;
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
                row.Cells[DataTableLib.dtGroup.col.Status].Value = status.GetText();
            }
        }

        private void GroupOnParamsChanged(GroupParamStatus info)
        {
            // Пример: обновить параметры в таблице, логах или метках
            //Log($"Группа {info.Id}: обновлены параметры — частота {info.UpdateRate} мс, " +
            //    $"Off={info.Off}, IsStopped={info.IsStopped}");

            // Визуальное обновление, если реализовано
            // DataTableLib.dtGroup?.UpdateParams(info);
        }

        private void GroupOnCycleRequest(IGroupOff group)
        {
            // Пример: лог или реакция на событие таймера группы
            //Log($"Циклический опрос группы {group.Id}");

            // Можно передать в Source, обновить UI или статистику
            // или собрать метрики по нагрузке
        }

        #endregion

    }
}
