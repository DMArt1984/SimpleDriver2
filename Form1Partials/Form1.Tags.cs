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


        // =================================================================================================================

        // Подписка на события тегов
        public void SubscribeToTag(Tag tag)
        {
            tag.eventRuntime += TagOnRuntimeChanged;
            tag.eventParam += TagOnParamChanged;
        }
        public void UnsubscribeFromTag(Tag tag)
        {
            tag.eventRuntime -= TagOnRuntimeChanged;
            tag.eventParam -= TagOnParamChanged;
        }

        private void TagOnRuntimeChanged(ushort id)
        {
            //Log($"Тег {id} обновил значение");
            // Можно обновить ячейку в DataGridView, статус и т.д.
        }

        private void TagOnParamChanged(TagParam param)
        {
            //Log($"Тег {param.Id}: обновлены параметры — Addr={param.address}, Тип={param.dataType}, Off={param.off}");
            // Можно отразить это в таблице или UI
        }


    }
}
