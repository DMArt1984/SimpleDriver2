using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System;

namespace Connector
{
    static class SourceGroupsHelper
    {
        // Добавление группы и привязка события тикания группы к обработчику запроса источника
        public static void AddGroup(Source source, Group group, ConcurrentDictionary<ushort, bool> roll)
        {
            // Подписываем обработчик события тикания группы
            group.tikTakReq += source.EventRequest;
            roll.TryAdd(group.Id, false);
        }

        // Удаление группы и отвязка события
        public static void RemoveGroup(Source source, Group group, ConcurrentDictionary<ushort, bool> roll)
        {
            group.tikTakReq -= source.EventRequest;
            roll.TryRemove(group.Id, out _);
        }

        // Привязка всех групп из списка
        public static void UseGroups(Source source, List<Group> groups, ConcurrentDictionary<ushort, bool> roll)
        {
            if (groups == null)
                return;

            foreach (var group in groups)
            {
                AddGroup(source, group, roll);
            }
        }
    }
}

