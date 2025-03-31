using DML;
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

    static public class TagLib
    {
        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Tags");
        static public bool IsStructures(dynamic output) => JsonControl.IsProp(output, "Structures");
        static public bool IsTargetTags(dynamic output) => JsonControl.IsProp(output, "TargetTags");
        static public bool IsListBlocks(dynamic output) => JsonControl.IsProp(output, "Blocks");

        // Получение параметров тега
        static public void ParseItemTag(dynamic item, uint forId, out string title, out string source, out eDataType dataType, out bool off, out string address, out string description, out string writeTitle, out string groupTitle, out string constValue, out bool isCommand)
        {
            title = JsonControl.GetString(item, "Title", $"Tag #{forId}");
            source = JsonControl.GetString(item, "Source");
            groupTitle = JsonControl.GetString(item, "Group");
            dataType = JsonControl.GetTypeEnum<eDataType>(item, "DataType", eDataType.Binary);
            address = JsonControl.GetString(item, "Addr");
            off = JsonControl.GetBool(item, "Off");
            description = JsonControl.GetString(item, "Desc");
            writeTitle = JsonControl.GetString(item, "Write");
            constValue = JsonControl.GetString(item, "Value", null);
            isCommand = JsonControl.GetBool(item, "Command");
        }

        // Получение параметров структуры
        static public void ParseItemStructure(dynamic item, out string title, out string join, out string templateAddress, out eDataType dataType, out string tagSource, out string group, out string[] sourceTags, out List<TargetTag> targetTags)
        {
            title = JsonControl.GetString(item, "Title", $"noname #{DateTime.Now.Millisecond}");
            join = JsonControl.GetString(item, "Join", ".");
            templateAddress = JsonControl.GetString(item, "Address", "{#Source.[#Target]}");
            dataType = JsonControl.GetTypeEnum<eDataType>(item, "DataType", eDataType.Binary);
            tagSource = JsonControl.GetString(item, "Source", "");
            group = JsonControl.GetString(item, "Group", "");
            sourceTags = JsonControl.GetArrayString(item, "SourceTags");
            targetTags = new List<TargetTag>();
            if (IsTargetTags(item))
            {
                foreach (var target in item.TargetTags)
                {
                    ParseTargetTag(target, out string ttitle, out string taddress, out string tdesc);
                    if (String.IsNullOrWhiteSpace(taddress) == false)
                    {
                        targetTags.Add(new TargetTag { title = ttitle, address = taddress, desc = tdesc });
                    }
                }
            }
        }
        static public void ParseTargetTag(dynamic item, out string title, out string address, out string desc)
        {
            address = JsonControl.GetString(item, "Address", "");
            title = JsonControl.GetString(item, "Title", $"index{address}");
            desc = JsonControl.GetString(item, "Desc", title);
        }

    }

    #endregion
}
