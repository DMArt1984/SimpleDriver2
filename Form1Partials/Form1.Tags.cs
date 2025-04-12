using Connector;
using DML;
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

        private void TagOnRuntimeChanged(ushort Id)
        {
            
        }

        private void TagOnParamChanged(TagParam param)
        {
            //Log($"Тег {param.Id}: обновлены параметры — Addr={param.address}, Тип={param.dataType}, Off={param.off}");
            // Можно отразить это в таблице или UI
        }


    }
}
