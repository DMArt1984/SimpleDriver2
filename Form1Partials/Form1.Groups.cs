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
        public void SubscribeToGroup(Group group)
        {
            group.eventStatus += GroupOnStatusChanged;
            group.eventParams += GroupOnParamsChanged;
            group.tikTakReq += GroupOnCycleRequest;
        }
        public void UnsubscribeFromGroup(Group group)
        {
            group.eventStatus -= GroupOnStatusChanged;
            group.eventParams -= GroupOnParamsChanged;
            group.tikTakReq -= GroupOnCycleRequest;
        }

        private void GroupOnStatusChanged(ushort groupId, eGroupStatus status)
        {
            // Пример: обновить статус в таблице групп (если есть DataTableLib.dtGroup)
            //Log($"Группа {groupId} изменила статус на {status.GetText()}");

            // Обновить визуально, если нужно
            // DataTableLib.dtGroup?.UpdateStatus(groupId, status); // если есть метод
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
