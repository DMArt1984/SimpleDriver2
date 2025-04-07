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
        #region Source

        public static List<Source> ConvertEditorToControlSources(List<SourceEditor> eSources, IDeviceFactory deviceFactory)
        {
            var cSources = new List<Source>();
            if (eSources != null)
            {
                foreach (var se in eSources)
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
                    cSources.Add(source);
                }
            }
            return cSources;
        }

        public static void UpdateEditorToControlSources(List<SourceEditor> eSources, List<Source> cSources, IDeviceFactory deviceFactory)
        {
            // Получаем новый список источников
            var newSources = ConvertEditorToControlSources(eSources, deviceFactory);

            // Создаем словарь новых источников по Id
            var newDict = newSources.ToDictionary(s => s.Id);

            // Обновляем существующие источники
            foreach (var existing in cSources)
            {
                if (newDict.TryGetValue(existing.Id, out var updated))
                {
                    // Обновляем изменяемые свойства, например, Address.
                    // Если в вашем классе Source имеются другие изменяемые свойства,
                    // их можно добавить сюда.
                    if (existing.Address != updated.Address)
                    {
                        existing.Address = updated.Address;
                    }
                    // Здесь можно добавить обновление других параметров, если они реализованы через свойства.
                }
                else
                {
                    // Если источник отсутствует в новом списке, удаляем его.
                    cSources.Remove(existing);
                }
            }

            // Добавляем новые источники, которых нет в Source.items.
            var currentIds = new HashSet<ushort>(cSources.Select(s => s.Id));
            foreach (var newSource in newSources)
            {
                if (!currentIds.Contains(newSource.Id))
                {
                    cSources.Add(newSource);
                }
            }
        }

        #endregion

        #region Group

        public static List<Group> ConvertEditorToControlGroups(List<Source> cSources, List<GroupEditor> eGroups)
        {
            var groups = new List<Group>();
            if (eGroups != null)
            {
                foreach (var ge in eGroups)
                {
                    // Ищем родительский источник по совпадению названия
                    Source parentSource = cSources.FirstOrDefault(s => s.title.Equals(ge.sourceTitle, StringComparison.OrdinalIgnoreCase));
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
        public static void UpdateEditorToControlGroups(List<Source> updatedSources, List<GroupEditor> eGroups, List<Group> cGroups)
        {
            // Получаем новый список групп, используя обновленные источники
            var newGroups = ConvertEditorToControlGroups(updatedSources, eGroups);

            // Формируем словарь для быстрого поиска по Id
            var newDict = newGroups.ToDictionary(g => g.Id);

            // Обновляем существующие группы
            // Создаем копию списка, чтобы безопасно перебирать при удалении
            foreach (var existing in cGroups.ToList())
            {
                if (newDict.TryGetValue(existing.Id, out var updated))
                {
                    // Обновляем изменяемые свойства
                    if (existing.UpdateRate != updated.UpdateRate)
                    {
                        existing.UpdateRate = updated.UpdateRate;
                    }
                    // Обновляем состояние off, если необходимо (это может зависеть от логики приложения)
                    if (existing.Off != updated.Off)
                    {
                        existing.Off = updated.Off;
                    }
                    // Если в вашем классе Group есть метод для обновления описания или других параметров,
                    // вызовите его здесь. Если свойства заданы только для чтения, их можно обновлять через методы.
                    // Например, если бы существовал метод UpdateDetails(string description), то:
                    // existing.UpdateDetails(updated.description);
                }
                else
                {
                    // Если текущая группа отсутствует в новом наборе, удаляем её
                    cGroups.Remove(existing);
                }
            }

            // Добавляем новые группы, которых еще нет в Group.items
            var currentIds = new HashSet<ushort>(cGroups.Select(g => g.Id));
            foreach (var newGroup in newGroups)
            {
                if (!currentIds.Contains(newGroup.Id))
                {
                    cGroups.Add(newGroup);
                }
            }
        }

        #endregion

        #region Tag

        public static List<Tag> ConvertEditorToControlTags(List<Group> groups, List<TagEditor> eTags)
        {
            var tags = new List<Tag>();
            if (eTags != null)
            {
                foreach (var te in eTags)
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
        public static void UpdateEditorToControlTags(List<Group> updatedGroups, List<TagEditor> eTags, List<Tag> cTags)
        {
            // Получаем новый список тегов из обновлённых групп.
            var newTags = ConvertEditorToControlTags(updatedGroups, eTags);

            // Формируем словарь новых тегов по Id для быстрого поиска.
            var newDict = newTags.ToDictionary(t => t.Id);

            // Обновляем существующие теги.
            // Используем ToList(), чтобы избежать проблем при удалении из Tag.items во время перебора.
            foreach (var existing in cTags.ToList())
            {
                if (newDict.TryGetValue(existing.Id, out var updated))
                {
                    // Обновляем изменяемые свойства, если они отличаются.
                    if (existing.Address != updated.Address)
                    {
                        existing.Address = updated.Address;
                    }
                    if (existing.Off != updated.Off)
                    {
                        existing.Off = updated.Off;
                    }
                    if (existing.DataType != updated.DataType)
                    {
                        existing.DataType = updated.DataType;
                    }
                    // Если имеются и другие изменяемые свойства (например, description),
                    // их можно обновить аналогичным образом.
                }
                else
                {
                    // Если текущий тег отсутствует в новом наборе – удаляем его.
                    cTags.Remove(existing);
                }
            }

            // Добавляем новые теги, которых ещё нет в Tag.items.
            var currentIds = new HashSet<ushort>(cTags.Select(t => t.Id));
            foreach (var newTag in newTags)
            {
                if (!currentIds.Contains(newTag.Id))
                {
                    cTags.Add(newTag);
                }
            }
        }

        #endregion

    }
}

