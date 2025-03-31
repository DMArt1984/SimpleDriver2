using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connector
{
    public struct cellGroup
    {
        public ushort Id;
        public DataGridViewRow row;
        public DataGridViewCell updateRate;
        public DataGridViewCell on;
        public DataGridViewCell description;
        public DataGridViewCell step;
        public DataGridViewCell statistic;
    }

    public class GroupEditor // Редактирование
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string title; // Название
        public bool off; // Отключение
        public uint updateRate; // Период опроса (мсек)
        public string description; // Описание
        public string sourceTitle; // Название источника
    }
}
