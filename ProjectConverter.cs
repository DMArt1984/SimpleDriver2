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
        /// Обновляет Source.items на основе данных из EditorControl.sources.
        /// Если источник с данным Id уже существует, обновляются его изменяемые свойства.
        /// Если новый источник отсутствует – он добавляется,
        /// а если существующий источник отсутствует в новых данных – удаляется.
        /// </summary>
        public static void UpdateSources(IDeviceFactory deviceFactory)
        {
            // Получаем новый список источников
            var newSources = ConvertSources(deviceFactory);

            // Создаем словарь новых источников по Id
            var newDict = newSources.ToDictionary(s => s.Id);

            // Обновляем существующие источники
            foreach (var existing in Source.items.ToList())
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
                    Source.items.Remove(existing);
                }
            }

            // Добавляем новые источники, которых нет в Source.items.
            var currentIds = new HashSet<ushort>(Source.items.Select(s => s.Id));
            foreach (var newSource in newSources)
            {
                if (!currentIds.Contains(newSource.Id))
                {
                    Source.items.Add(newSource);
                }
            }
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
        public static void UpdateGroups(List<Source> updatedSources)
        {
            // Получаем новый список групп, используя обновленные источники
            var newGroups = ConvertGroups(updatedSources);

            // Формируем словарь для быстрого поиска по Id
            var newDict = newGroups.ToDictionary(g => g.Id);

            // Обновляем существующие группы
            // Создаем копию списка, чтобы безопасно перебирать при удалении
            foreach (var existing in Group.items.ToList())
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
                    Group.items.Remove(existing);
                }
            }

            // Добавляем новые группы, которых еще нет в Group.items
            var currentIds = new HashSet<ushort>(Group.items.Select(g => g.Id));
            foreach (var newGroup in newGroups)
            {
                if (!currentIds.Contains(newGroup.Id))
                {
                    Group.items.Add(newGroup);
                }
            }
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
        public static void UpdateTags(List<Group> updatedGroups)
        {
            // Получаем новый список тегов из обновлённых групп.
            var newTags = ConvertTags(updatedGroups);

            // Формируем словарь новых тегов по Id для быстрого поиска.
            var newDict = newTags.ToDictionary(t => t.Id);

            // Обновляем существующие теги.
            // Используем ToList(), чтобы избежать проблем при удалении из Tag.items во время перебора.
            foreach (var existing in Tag.items.ToList())
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
                    Tag.items.Remove(existing);
                }
            }

            // Добавляем новые теги, которых ещё нет в Tag.items.
            var currentIds = new HashSet<ushort>(Tag.items.Select(t => t.Id));
            foreach (var newTag in newTags)
            {
                if (!currentIds.Contains(newTag.Id))
                {
                    Tag.items.Add(newTag);
                }
            }
        }

    }
}

