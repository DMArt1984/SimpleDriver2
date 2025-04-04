using System;
using System.Collections.Generic;
using System.Linq;
using Connector;
using WinSimpleIDriver;
using WinSimpleIDriver.Connector; // для доступа к EditorControl

namespace Connector
{
    /// <summary>
    /// Класс для преобразования данных из редактора проекта (EditorControl)
    /// в доменные объекты Source, Group и Tag.
    /// </summary>
    public static class ProjectConverter
    {
        /// <summary>
        /// Преобразует список SourceEditor из EditorControl в список объектов Source.
        /// </summary>
        /// <param name="deviceFactory">Инстанс фабрики устройств для создания источников.</param>
        /// <returns>Список объектов Source.</returns>
        public static List<Source> ConvertSources(IDeviceFactory deviceFactory)
        {
            var sources = new List<Source>();
            if (EditorControl.sources != null)
            {
                foreach (var se in EditorControl.sources)
                {
                    // Преобразуем идентификатор (uint) в ushort (при условии, что он входит в диапазон)
                    var source = new Source(
                        (ushort)se.Id,
                        se.title,
                        se.driver,         // eDriverType
                        deviceFactory,
                        se.off,            // параметр disable
                        se.auto,           // авто-опрос после открытия
                        se.reconnect,      // авто-переподключение
                        se.address,
                        se.description);
                    sources.Add(source);
                }
            }
            return sources;
        }

        /// <summary>
        /// Преобразует список GroupEditor из EditorControl в список объектов Group.
        /// Для определения родительского источника используется поле sourceTitle.
        /// </summary>
        /// <param name="sources">Список источников, полученных ранее.</param>
        /// <returns>Список объектов Group.</returns>
        public static List<Group> ConvertGroups(List<Source> sources)
        {
            var groups = new List<Group>();
            if (EditorControl.groups != null)
            {
                foreach (var ge in EditorControl.groups)
                {
                    // Ищем родительский источник по совпадению названия
                    Source parentSource = sources.FirstOrDefault(s => s.title.Equals(ge.sourceTitle, StringComparison.OrdinalIgnoreCase));
                    if (parentSource != null)
                    {
                        var group = new Group(
                            (ushort)ge.Id,
                            ge.title,
                            parentSource,
                            ge.updateRate,
                            ge.off,          // здесь можем интерпретировать off как waitOff (при необходимости можно добавить отдельное свойство)
                            ge.description);
                        groups.Add(group);
                    }
                }
            }
            return groups;
        }

        /// <summary>
        /// Преобразует список TagEditor из EditorControl в список объектов Tag.
        /// Для определения родительской группы используется поле groupTitle.
        /// </summary>
        /// <param name="groups">Список групп, полученных ранее.</param>
        /// <returns>Список объектов Tag.</returns>
        public static List<Tag> ConvertTags(List<Group> groups)
        {
            var tags = new List<Tag>();
            if (EditorControl.tags != null)
            {
                foreach (var te in EditorControl.tags)
                {
                    // Ищем родительскую группу по совпадению названия
                    Group parentGroup = groups.FirstOrDefault(g => g.title.Equals(te.groupTitle, StringComparison.OrdinalIgnoreCase));
                    if (parentGroup != null)
                    {
                        var tag = new Tag(
                            (ushort)te.Id,
                            te.title,
                            parentGroup,
                            te.dataType,
                            te.address,
                            te.description);
                        tags.Add(tag);
                    }
                }
            }
            return tags;
        }
    }
}

