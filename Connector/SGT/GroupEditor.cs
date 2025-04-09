using DML;
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
        public ushort Id; // Уникальный идентификатор (0 - нет Id)
        public string title; // Название
        public bool disableOnStart; // Отключен при старте
        public uint updateRate; // Период опроса (мсек)
        public string description; // Описание
        public string sourceTitle; // Название источника
    }

    static public class GroupLib {
        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Groups");
        // Получение параметров группы
        static public void ParseItemGroup(dynamic item, uint forindex, out string title, out uint updateRate, out bool disableOnStart, out string description, out string sourceTitle)
        {
            title = JsonControl.GetString(item, "Title", $"Group #{forindex}");
            updateRate = (uint)JsonControl.GetInt(item, "UpdateRate");
            disableOnStart = JsonControl.GetBool(item, "Off");
            description = JsonControl.GetString(item, "Desc");
            sourceTitle = JsonControl.GetString(item, "Source");
            //tags = JsonControl.IsProp(item, "Tags") ? item.Tags : null;
        }

        // Распаковка групп
        static public List<GroupEditor> ParseGroups(dynamic section)
        {
            List<GroupEditor> items = new List<GroupEditor>();
            ushort groupId = 0; // ID 
            if (section != null)
            {
                foreach (dynamic item in section)
                {
                    GroupLib.ParseItemGroup(item, ++groupId, out string title, out uint updateRate, out bool disableOnStart, out string description, out string sourceTitle);
                    GroupEditor rowGroup = new GroupEditor
                    {
                        Id = groupId,
                        title = title,
                        disableOnStart = disableOnStart,
                        updateRate = updateRate,
                        description = description,
                        sourceTitle = sourceTitle
                    };
                    items.Add(rowGroup);
                }
            }
            return items;
        }
    }

}
