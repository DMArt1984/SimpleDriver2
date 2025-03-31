using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connector
{
    public struct cellTag
    {
        public ushort Id;
        public DataGridViewRow row;
        public DataGridViewCell value;
        public DataGridViewCell address;
        public DataGridViewCell dataType;
        public DataGridViewCell writeValue;
        public DataGridViewCell on;
        public DataGridViewCell code;
        public DataGridViewCell message;
        public DataGridViewCell comment;

    }

    #region EDITOR

    public class TagEditor // Редактирование
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string title; // Название
        public eDataType dataType;
        public uint sourceId; // ID драйвера
        public uint groupId; // ID группы
        public string sourceTitle; // Название драйвера
        public string groupTitle; // Название группы
        public string address; // адрес
        public bool off; // отключение
        public bool isCommand; // запрос по команде
        public string writeTitle; // Источник новых значений (имя тега)
        public string constValue; // Записываемое значение
        public string description; // Описание
        public string block; // Блок
    }

    public struct TargetTag
    {
        public string title;
        public string desc;
        public string address;
    }

    public class StructureEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string title { get; set; } // Название
        public string join { get; set; } // Соединитель
        public eDataType dataType { get; set; } // Тип данных
        public string tagSource { get; set; } // тег-источник
        public string templateAddress { get; set; } // шаблон адреса
        public string group { get; set; } // группа

        //public string sourceTags { get; set; } // tag1;tag2;tag3
    }
    public class StructTargetEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string structureTitle { get; set; }  // Название структуры
        public string title { get; set; }  // Название тега
        public string innerAddress { get; set; }  // Адрес
        public string desc { get; set; }  // Описание тега
    }

    public class StructTagEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string structureTitle { get; set; }  // Название структуры
        public string title { get; set; }  // Название тега
    }

    #endregion
}
