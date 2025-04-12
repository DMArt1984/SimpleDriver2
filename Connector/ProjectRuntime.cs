using DML.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using WinSimpleIDriver;

namespace Connector
{
    /// <summary>
    /// Класс для преобразования данных из редактора проекта (EditorControl)
    /// в доменные объекты Source, Group и Tag.
    /// </summary>
    public static class ProjectRuntime
    {
        static private bool _online = false; // true - в режиме реального времени, false - в режиме редактирования
        static private bool _transition = false; // true - переход в режим реального времени, false - переход в режим редактирования
        static public bool online => _online; // свойство для доступа к состоянию
        static public bool transition => _transition; // свойство для доступа к состоянию перехода

        // ------------------------------------------------------------------------------------------------------------------

        static public List<Source> sources = new List<Source>();
        static public List<Group> groups = new List<Group>();
        static public List<Tag> tags = new List<Tag>();

        // ==================================================================================================================

        #region Source

        public static List<Source> ConvertEditorToControlSources(
                                        List<SourceEditor> eSources,
                                        IDeviceFactory deviceFactory,
                                        Action<Source> onCreate = null)
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
                        se.disableOnStart,            // параметр disable
                        se.auto,           // авто-опрос после открытия
                        se.reconnect,      // авто-переподключение
                        se.address,
                        se.description);

                    onCreate?.Invoke(source); // <<< подключение обработчиков

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
            // Используем ToList(), чтобы безопасно удалять элементы из cSources
            foreach (var existing in cSources.ToList())
            {
                if (newDict.TryGetValue(existing.Id, out var updated))
                {
                    // Если тип драйвера не совпадает, удаляем старый источник
                    if (existing.driverType != updated.driverType)
                    {
                        cSources.Remove(existing);
                    }
                    else
                    {
                        // Если драйвер тот же, обновляем все остальные свойства
                        if (existing.title!= updated.title)
                        {
                            existing.title = updated.title;
                        }
                        if (existing.Address != updated.Address)
                        {
                            existing.Address = updated.Address;
                        }
                        if (existing.AutoRequestAftereOpen != updated.AutoRequestAftereOpen)
                        {
                            existing.AutoRequestAftereOpen = updated.AutoRequestAftereOpen;
                        }
                        if (existing.AutoOpenAfterFail != updated.AutoOpenAfterFail)
                        {
                            existing.AutoOpenAfterFail = updated.AutoOpenAfterFail;
                        }
                        if (existing.description != updated.description)
                        {
                            existing.description = updated.description;
                        }
                    }
                }
                else
                {
                    // Если источник отсутствует в новом списке, удаляем его
                    cSources.Remove(existing);
                }
            }

            // Добавляем новые источники, которых нет в cSources
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

        public static List<Group> ConvertEditorToControlGroups(
                                        List<Source> cSources,
                                        List<GroupEditor> eGroups,
                                        Action<Group> onCreate = null)
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
                            ge.disableOnStart,          // здесь можем интерпретировать off как waitOff (при необходимости можно добавить отдельное свойство)
                            ge.description);

                        onCreate?.Invoke(group); // 👈 колбэк для подписки

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

        public static List<Tag> ConvertEditorToControlTags(
                                        List<Group> groups,
                                        List<TagEditor> eTags,
                                        Action<Tag> onCreate = null)
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

                        onCreate?.Invoke(tag); // 👈 подключение обработчиков событий

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
                    // Обновляем изменяемые свойства, если они отличаются
                    if (existing.Address != updated.Address)
                    {
                        existing.Address = updated.Address;
                    }
                    if (existing.DataType != updated.DataType)
                    {
                        existing.DataType = updated.DataType;
                    }
                    // Обновляем значение для записи
                    if (existing.WriteConstValue != updated.WriteConstValue)
                    {
                        existing.WriteConstValue = updated.WriteConstValue;
                    }
                    // Обновляем идентификатор тега-источника для записи
                    if (existing.WriteTagId != updated.WriteTagId)
                    {
                        existing.WriteTagId = updated.WriteTagId;
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


        // Запуск Runtime
        public static void StartRuntime(Form1 frm, ILogger logger)
        {
            _transition = true;

            // Фабрика устройств (предположим, уже создана в форме)
            IDeviceFactory deviceFactory = new DeviceFactory();

            ProjectRuntime.sources = ProjectRuntime.ConvertEditorToControlSources(
                    EditorControl.sources,
                    deviceFactory,
                    frm.SubscribeToSource);
            logger.OK("Источники (подключения) загружены", eMessageCategory.App); // Лог и статус

            ProjectRuntime.groups = ProjectRuntime.ConvertEditorToControlGroups(
                    ProjectRuntime.sources,
                    EditorControl.groups,
                    frm.SubscribeToGroup);
            logger.OK("Группа (опроса) загружены", eMessageCategory.App); // Лог и статус

            ProjectRuntime.tags = ProjectRuntime.ConvertEditorToControlTags(
                    ProjectRuntime.groups,
                    EditorControl.tags,
                    frm.SubscribeToTag);
            logger.OK("Теги загружены", eMessageCategory.App); // Лог и статус

            _online = true;
        }

        // Остановка Runtime
        public static void StopRuntime(Form1 frm, ILogger logger)
        {
            _transition = false;

            foreach (var source in sources)
            {
                frm.UnsubscribeFromSource(source);

                source.Off = true;
            }
            logger.OK("Источники (подключения) остановлены", eMessageCategory.App); // Лог и статус

            foreach (var group in groups)
            {
                frm.UnsubscribeFromGroup(group);

                group.Stop();
                group.Dispose();
                group.UnbindSource();
            }
            logger.OK("Группа (опроса) остановлены", eMessageCategory.App); // Лог и статус

            foreach (var tag in tags)
            {
                frm.UnsubscribeFromTag(tag);
            }
            logger.OK("Теги остановлены", eMessageCategory.App); // Лог и статус

            sources.Clear();
            groups.Clear();
            tags.Clear();

            Source.Clear();
            Group.Clear();
            Tag.Clear();

            GC.Collect();
            logger.OK("Память очищена", eMessageCategory.App); // Лог и статус

            _online = false;
        }

    }
}

