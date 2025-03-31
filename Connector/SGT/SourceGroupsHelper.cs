using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System;

namespace Connector
{
    static class SourceGroupsHelper
    {
        // Добавление группы и привязка событий
        public static void AddGroup(Source source, Group group, ConcurrentDictionary<ushort, bool> roll)
        {
            if (group.AddManager(source.Id, out GroupManager gm))
            {
                gm.tikTakReq += source.EventRequest;
                roll.TryAdd(group.Id, false);
            }
        }

        // Удаление группы и отвязка событий
        public static void RemoveGroup(Source source, Group group, ConcurrentDictionary<ushort, bool> roll)
        {
            group.RemoveManagers(source.EventRequest);
            roll.TryRemove(group.Id, out _);
        }

        // Использовать группы из списка
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

