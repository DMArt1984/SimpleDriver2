using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ProjectSettingsConverter
{
    /// <summary>
    /// Преобразует входной JSON в длинный вид, где массивы Sources, Groups и Tags находятся на корневом уровне.
    /// Если группы вложены в Source, у каждой группы добавляется параметр "Source" (на основании Title родительского источника).
    /// Если теги вложены в Group, у тега добавляется параметр "Group" (на основании Title группы).
    /// </summary>
    public static string ConvertToLongForm(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);

        // Массивы для итоговых источников, групп и тегов
        JArray longSources = new JArray();
        JArray longGroups = new JArray();
        JArray longTags = new JArray();

        // Обрабатываем источники.
        if (root["Sources"] != null)
        {
            foreach (JObject source in root["Sources"].OfType<JObject>())
            {
                // Клонируем источник, чтобы не изменять исходный объект.
                JObject sourceClone = (JObject)source.DeepClone();
                longSources.Add(sourceClone);

                // Если в источнике вложены группы, перемещаем их в корневой массив групп.
                if (sourceClone["Groups"] != null)
                {
                    foreach (JObject group in sourceClone["Groups"].OfType<JObject>())
                    {
                        // Если у группы не указан параметр "Source", добавляем его, используя Title источника.
                        if (group["Source"] == null && sourceClone["Title"] != null)
                        {
                            group["Source"] = sourceClone["Title"];
                        }
                        longGroups.Add(group);
                    }
                    // Удаляем вложенные группы из источника.
                    sourceClone.Remove("Groups");
                }
            }
        }

        // Если на корневом уровне есть группы, добавляем их в массив групп.
        if (root["Groups"] != null)
        {
            foreach (JObject group in root["Groups"].OfType<JObject>())
            {
                longGroups.Add(group);
            }
        }

        // Обрабатываем теги.
        // Сначала проходим по группам и если у группы вложены теги, переносим их в корневой массив тегов.
        foreach (JObject group in longGroups.OfType<JObject>())
        {
            if (group["Tags"] != null)
            {
                foreach (JObject tag in group["Tags"].OfType<JObject>())
                {
                    // Если у тега нет параметра "Group", устанавливаем его по Title родительской группы.
                    if (tag["Group"] == null && group["Title"] != null)
                    {
                        tag["Group"] = group["Title"];
                    }
                    longTags.Add(tag);
                }
                group.Remove("Tags");
            }
        }
        // Если на корневом уровне есть теги, добавляем их.
        if (root["Tags"] != null)
        {
            foreach (JObject tag in root["Tags"].OfType<JObject>())
            {
                longTags.Add(tag);
            }
        }

        // Формируем итоговый объект с массивами на корневом уровне.
        JObject longForm = new JObject
        {
            ["Sources"] = longSources,
            ["Groups"] = longGroups,
            ["Tags"] = longTags
        };

        return longForm.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Преобразует входной JSON в короткий вид с максимальной вложенностью:
    /// для каждого источника группы вкладываются в него, а теги — в группы.
    /// </summary>
    public static string ConvertToShortForm(string inputJson)
    {
        // Сначала получаем длинный вид.
        string longFormJson = ConvertToLongForm(inputJson);
        JObject longForm = JObject.Parse(longFormJson);

        // Извлекаем списки источников, групп и тегов.
        var sources = longForm["Sources"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var groups = longForm["Groups"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var tags = longForm["Tags"]?.OfType<JObject>().ToList() ?? new List<JObject>();

        // Для каждого источника находим группы, принадлежащие ему (по параметру "Source").
        foreach (var source in sources)
        {
            string sourceTitle = source["Title"]?.ToString();
            var sourceGroups = groups.Where(g => g["Source"] != null && g["Source"].ToString() == sourceTitle).ToList();
            if (sourceGroups.Any())
            {
                JArray nestedGroups = new JArray();
                foreach (var group in sourceGroups)
                {
                    // Удаляем параметр "Source" у группы, так как теперь она вложена.
                    group.Remove("Source");

                    // Для каждой группы находим теги, принадлежащие ей (по полю "Group").
                    string groupTitle = group["Title"]?.ToString();
                    var groupTags = tags.Where(t => t["Group"] != null && t["Group"].ToString() == groupTitle).ToList();
                    if (groupTags.Any())
                    {
                        JArray nestedTags = new JArray();
                        foreach (var tag in groupTags)
                        {
                            // Удаляем параметр "Group" у тега, так как он теперь вложен в группу.
                            tag.Remove("Group");
                            nestedTags.Add(tag);
                        }
                        group["Tags"] = nestedTags;
                    }
                    nestedGroups.Add(group);
                }
                source["Groups"] = nestedGroups;
            }
        }

        // Итоговый объект имеет только массив "Sources" на корневом уровне.
        JObject shortForm = new JObject
        {
            ["Sources"] = new JArray(sources)
        };

        return shortForm.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Преобразует входной JSON таким образом, что теги вкладываются в группы, а группы остаются на корневом уровне.
    /// Результирующая структура содержит массив "Sources" и массив "Groups",
    /// где у каждой группы, если имеются связанные теги, создаётся свойство "Tags".
    /// </summary>
    public static string ConvertToGroupNestedForm(string inputJson)
    {
        // Получаем длинный вид для гарантии наличия всех массивов на корневом уровне.
        string longFormJson = ConvertToLongForm(inputJson);
        JObject longForm = JObject.Parse(longFormJson);

        // Извлекаем группы и теги.
        var groups = longForm["Groups"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var tags = longForm["Tags"]?.OfType<JObject>().ToList() ?? new List<JObject>();

        // Для каждой группы находим теги, принадлежащие ей (по полю "Group").
        foreach (var group in groups)
        {
            string groupTitle = group["Title"]?.ToString();
            var groupTags = tags.Where(t => t["Group"] != null && t["Group"].ToString() == groupTitle).ToList();
            if (groupTags.Any())
            {
                JArray nestedTags = new JArray();
                foreach (var tag in groupTags)
                {
                    // Удаляем параметр "Group" из тега, так как он теперь вложен.
                    tag.Remove("Group");
                    nestedTags.Add(tag);
                }
                group["Tags"] = nestedTags;
            }
        }

        // Формируем итоговый объект, который содержит массивы "Sources" и "Groups" на корневом уровне.
        JObject groupNestedForm = new JObject
        {
            ["Sources"] = longForm["Sources"],
            ["Groups"] = new JArray(groups)
        };

        return groupNestedForm.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Преобразует входной JSON в форму, где все группы вложены в соответствующие источники, 
    /// а теги остаются на корневом уровне.
    /// </summary>
    public static string ConvertToSourceNestedForm(string inputJson)
    {
        // Преобразуем входной JSON в длинный вид, где все массивы находятся на корневом уровне.
        string longFormJson = ConvertToLongForm(inputJson);
        JObject longForm = JObject.Parse(longFormJson);

        // Извлекаем списки источников и групп.
        var sources = longForm["Sources"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var groups = longForm["Groups"]?.OfType<JObject>().ToList() ?? new List<JObject>();

        // Для каждого источника находим группы, принадлежащие ему (по параметру "Source")
        // и вкладываем их в свойство "Groups" источника.
        foreach (var source in sources)
        {
            string sourceTitle = source["Title"]?.ToString();
            var sourceGroups = groups.Where(g => g["Source"] != null && g["Source"].ToString() == sourceTitle).ToList();
            if (sourceGroups.Any())
            {
                JArray nestedGroups = new JArray();
                foreach (var group in sourceGroups)
                {
                    // Удаляем параметр "Source" из группы, так как теперь она вложена.
                    group.Remove("Source");
                    nestedGroups.Add(group);
                }
                source["Groups"] = nestedGroups;
            }
        }

        // Формируем итоговый объект, который содержит два корневых массива:
        // "Sources" с вложенными группами и "Tags" без изменений.
        JObject result = new JObject
        {
            ["Sources"] = new JArray(sources),
            ["Tags"] = longForm["Tags"] != null ? longForm["Tags"] : new JArray()
        };

        return result.ToString(Formatting.Indented);
    }

    // ======================================================================================================================

    /// <summary>
    /// Нормализует настройки, устанавливая параметр "Source" для группы,
    /// если она не имеет его, а один из источников содержит параметр "Group" (единичный) с именем этой группы.
    /// После обработки параметра "Group" у источника он удаляется.
    /// </summary>
    public static string NormalizeSourceGroup(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);

        // Получаем списки источников и групп из корневых массивов.
        var sources = root["Sources"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var groups = root["Groups"]?.OfType<JObject>().ToList() ?? new List<JObject>();

        // Проходим по каждому источнику.
        foreach (var source in sources)
        {
            // Если источник содержит параметр "Group" (единичный, а не массив "Groups")
            if (source["Group"] != null)
            {
                string groupTitle = source["Group"].ToString();
                // Ищем группу с совпадающим названием в корневом массиве "Groups"
                var matchingGroup = groups.FirstOrDefault(g => g["Title"] != null && g["Title"].ToString() == groupTitle);
                if (matchingGroup != null)
                {
                    // Если у найденной группы отсутствует параметр "Source" и источник имеет "Title"
                    if (matchingGroup["Source"] == null && source["Title"] != null)
                    {
                        matchingGroup["Source"] = source["Title"];
                    }
                }
                // После обработки удаляем параметр "Group" из источника, так как он больше не нужен.
                source.Remove("Group");
            }
        }

        return root.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Нормализует проект следующим образом:
    /// - Если в корневом массиве "Groups" содержится только один элемент, то для каждого тега из массива "Tags",
    ///   который не имеет параметра "Group" (или его значение пусто), устанавливается параметр "Group"
    ///   равный значению свойства "Title" единственной группы.
    /// - Если в корневом массиве "Sources" содержится только один элемент, то для каждого объекта из массива "Groups",
    ///   который не имеет параметра "Source" (или его значение пусто), устанавливается параметр "Source"
    ///   равный значению свойства "Title" единственного источника.
    /// </summary>
    public static string NormalizeSingleGroupAndSource(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);

        // Получаем списки источников, групп и тегов из корневых массивов.
        var sources = root["Sources"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var groups = root["Groups"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var tags = root["Tags"]?.OfType<JObject>().ToList() ?? new List<JObject>();

        // Если в проекте только один Group, устанавливаем его Title во все теги, у которых не задан параметр "Group".
        if (groups.Count == 1)
        {
            string groupTitle = groups[0]["Title"]?.ToString();
            if (!string.IsNullOrWhiteSpace(groupTitle))
            {
                foreach (var tag in tags)
                {
                    if (tag["Group"] == null || string.IsNullOrWhiteSpace(tag["Group"].ToString()))
                    {
                        tag["Group"] = groupTitle;
                    }
                }
            }
        }

        // Если в проекте только один Source, устанавливаем его Title во все группы, у которых не задан параметр "Source".
        if (sources.Count == 1)
        {
            string sourceTitle = sources[0]["Title"]?.ToString();
            if (!string.IsNullOrWhiteSpace(sourceTitle))
            {
                foreach (var group in groups)
                {
                    if (group["Source"] == null || string.IsNullOrWhiteSpace(group["Source"].ToString()))
                    {
                        group["Source"] = sourceTitle;
                    }
                }
            }
        }

        return root.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Нормализует настройки, перемещая массив "Tags" из объекта Source в соответствующую группу,
    /// если у этого источника задан единственный Group (свойство "Group").
    /// Если перенос происходит, то у каждого тега из этого массива удаляются параметры "Group" и "Source",
    /// так как теперь они вложены в нужное место.
    /// </summary>
    public static string NormalizeSourceTagsToGroup(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);

        // Получаем массивы Sources и Groups из корневого уровня.
        var sources = root["Sources"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var groups = root["Groups"]?.OfType<JObject>().ToList() ?? new List<JObject>();

        foreach (var source in sources)
        {
            // Проверяем, содержит ли Source массив "Tags" и свойство "Group" (единичное, а не массив "Groups")
            if (source["Tags"] != null && source["Tags"].Type == JTokenType.Array &&
                source["Group"] != null && source["Group"].Type == JTokenType.String)
            {
                string groupTitle = source["Group"].ToString();

                // Ищем группу в корневом массиве Groups по совпадению Title с groupTitle.
                var matchingGroup = groups.FirstOrDefault(g => g["Title"] != null && g["Title"].ToString() == groupTitle);
                if (matchingGroup != null)
                {
                    // Получаем массив тегов из Source.
                    JArray sourceTags = (JArray)source["Tags"];

                    // Если в группе уже есть свойство "Tags", то объединяем массивы,
                    // иначе создаем новое свойство "Tags" с данным массивом.
                    if (matchingGroup["Tags"] != null && matchingGroup["Tags"].Type == JTokenType.Array)
                    {
                        JArray groupTags = (JArray)matchingGroup["Tags"];
                        // Добавляем каждый тег из sourceTags в groupTags.
                        foreach (JObject tag in sourceTags.OfType<JObject>())
                        {
                            // Удаляем из тега параметры "Group" и "Source"
                            tag.Remove("Group");
                            tag.Remove("Source");
                            groupTags.Add(tag);
                        }
                    }
                    else
                    {
                        JArray newGroupTags = new JArray();
                        foreach (JObject tag in sourceTags.OfType<JObject>())
                        {
                            // Удаляем из тега параметры "Group" и "Source"
                            tag.Remove("Group");
                            tag.Remove("Source");
                            newGroupTags.Add(tag);
                        }
                        matchingGroup["Tags"] = newGroupTags;
                    }
                }
                // После переноса, удаляем массив "Tags" из источника,
                // а также свойство "Group", так как оно больше не нужно в Source.
                source.Remove("Tags");
                source.Remove("Group");
            }
        }

        return root.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Нормализует настройки, перемещая массив "Tags" из объекта Source в единственную группу, связанную с этим Source.
    /// Для каждого объекта Source ищется в корневом массиве "Groups" все группы, у которых параметр "Source" равен значению свойства "Title" этого Source.
    /// Если таких групп ровно одна, то переносится массив "Tags" из Source в эту группу. При этом у каждого перенесённого тега удаляются параметры "Group" и "Source",
    /// так как теперь они вложены в нужную группу.
    /// </summary>
    public static string NormalizeSourceTagsToUniqueGroup(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);

        // Получаем массивы Sources и Groups из корневого уровня.
        var sources = root["Sources"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var groups = root["Groups"]?.OfType<JObject>().ToList() ?? new List<JObject>();

        foreach (var source in sources)
        {
            // Если Source содержит массив "Tags"
            if (source["Tags"] != null && source["Tags"].Type == JTokenType.Array)
            {
                string sourceTitle = source["Title"]?.ToString();
                if (string.IsNullOrEmpty(sourceTitle))
                    continue;

                // Находим все группы, у которых параметр "Source" равен sourceTitle.
                var relatedGroups = groups.Where(g => g["Source"] != null && g["Source"].ToString() == sourceTitle).ToList();

                // Если таких групп ровно одна, переносим массив "Tags" из Source в эту группу.
                if (relatedGroups.Count == 1)
                {
                    JObject targetGroup = relatedGroups.First();
                    // Если у группы уже есть массив "Tags", используем его, иначе создаем новый.
                    JArray groupTags;
                    if (targetGroup["Tags"] != null && targetGroup["Tags"].Type == JTokenType.Array)
                    {
                        groupTags = (JArray)targetGroup["Tags"];
                    }
                    else
                    {
                        groupTags = new JArray();
                        targetGroup["Tags"] = groupTags;
                    }

                    // Переносим каждый тег из массива Source["Tags"].
                    JArray sourceTags = (JArray)source["Tags"];
                    foreach (JObject tag in sourceTags.OfType<JObject>())
                    {
                        // Удаляем из тега параметры "Group" и "Source", так как теперь он вложен.
                        tag.Remove("Group");
                        tag.Remove("Source");
                        groupTags.Add(tag);
                    }

                    // После переноса удаляем массив "Tags" из Source.
                    source.Remove("Tags");
                }
            }
        }

        return root.ToString(Formatting.Indented);
    }


}


