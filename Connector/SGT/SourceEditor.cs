using Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connector.SGT
{
    public struct cellSource
    {
        //ED public ushort Id;
        public DataGridViewRow row;
        public DataGridViewCell connection;
        public DataGridViewCell opened;
        public DataGridViewCell code;
        public DataGridViewCell message;
        public DataGridViewCell comment;
        public DataGridViewCell step;
        public DataGridViewCell statistic;
    }

    #region Editor

    public class SourceEditor // Редактирование
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public eDriverType driver; // Тип драйвера
        public string title; // Название драйвера
        public string address; // Строка подключения
        public bool off; // Отключение
        public string description; // Описание
        public bool auto; // Запуск опроса после открытия файла
        public bool reconnect; // Автоматическое переподключение
    }
    #endregion

}
