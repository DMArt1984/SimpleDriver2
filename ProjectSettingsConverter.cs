using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ProjectSettingsConverter
{
    /// <summary>
    /// Приводит исходный JSON в базовую длинную форму и последовательно выполняет нормализацию:
    /// 1. ConvertToLongForm – перевод всех объектов в корневые массивы (Sources, Groups, Tags).
    /// 2. NormalizeSourceGroup – если у Source задан единичный параметр Group, находит соответствующую группу и устанавливает ей параметр Source, затем удаляет этот параметр из Source.
    /// 3. NormalizeSingleGroupAndSource – если в проекте только один Source или только один Group, устанавливает во все группы и теги соответствующие параметры.
    /// 4. NormalizeSourceTagsToUniqueGroup – переносит массив Tags из Source в связанную группу (если для Source существует ровно одна группа).
    /// 5. NormalizeDuplicateEntriesMerge – объединяет дублирующиеся записи по свойству Title или, если объединение невозможно, переименовывает их.
    /// </summary>
    public static string NormalizeAll(string inputJson)
    {
        // 1. Распаковка из блоков (Blocks)
        string result = ExtractTagsFromBlocks(inputJson);

        // 2. Приводим JSON к длинной форме (все массивы на корневом уровне).
        result = ConvertToLongForm(result);

        // 3. Нормализуем параметр Group у источников.
        result = NormalizeSourceGroup(result);

        // 4. Если в проекте только один Source или только один Group – устанавливаем соответствующие значения во все элементы.
        result = NormalizeSingleGroupAndSource(result);

        // 5. Переносим массив Tags из Source в группу, если для Source существует единственная соответствующая группа.
        result = NormalizeSourceTagsToUniqueGroup(result);

        // 6. Объединяем дублирующиеся записи по свойству Title (либо объединяя, либо переименовывая их).
        result = NormalizeDuplicateEntriesMerge(result);

        return result;
    }


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
    /// Нормализует перенос массива "Tags" из объекта Source в соответствующую группу, 
    /// если данный Source использует ровно одну группу.
    /// Для определения целевой группы метод ищет в корневом массиве "Groups" группу, у которой
    /// свойство "Source" равно значению свойства "Title" данного Source, и при этом название группы
    /// совпадает со значением, сохранённым в свойстве "OriginalGroup" источника.
    /// Если таких групп ровно одна, то все теги из Source перемещаются в эту группу, а у каждого перенесённого тега удаляются свойства "Group" и "Source".
    /// После переноса массив "Tags" удаляется из Source.
    /// </summary>
    /// <param name="inputJson">
    /// Исходная строка JSON с настройками, где объекты Source могут содержать массив "Tags", а также может быть сохранено значение целевого Group в свойстве "OriginalGroup".
    /// </param>
    /// <returns>
    /// Нормализованная строка JSON, в которой для каждого Source, использующего ровно один Group, массив "Tags" перенесён в соответствующую группу.
    /// </returns>
    public static string NormalizeSourceTagsToUniqueGroup(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);

        // Получаем списки Sources и Groups из корневого уровня.
        var sources = root["Sources"]?.OfType<JObject>().ToList() ?? new List<JObject>();
        var groups = root["Groups"]?.OfType<JObject>().ToList() ?? new List<JObject>();

        foreach (var source in sources)
        {
            // Определяем целевой Group для источника.
            // Сначала пробуем получить свойство "Group". Если его нет, то пытаемся взять "OriginalGroup".
            string groupIndicator = null;
            if (source["Group"] != null && !string.IsNullOrWhiteSpace(source["Group"].ToString()))
            {
                groupIndicator = source["Group"].ToString();
            }
            else if (source["OriginalGroup"] != null && !string.IsNullOrWhiteSpace(source["OriginalGroup"].ToString()))
            {
                groupIndicator = source["OriginalGroup"].ToString();
            }

            // Если индикатор группы не найден или Source не содержит массив Tags – переходим к следующему Source.
            if (string.IsNullOrEmpty(groupIndicator) || source["Tags"] == null || source["Tags"].Type != JTokenType.Array)
                continue;

            // Получаем Title источника
            string sourceTitle = source["Title"]?.ToString();
            if (string.IsNullOrEmpty(sourceTitle))
                continue;

            // Находим все группы, у которых свойство "Source" равно sourceTitle и Title группы совпадает с groupIndicator.
            var relatedGroups = groups.Where(g =>
                                        g["Source"] != null && g["Source"].ToString() == sourceTitle &&
                                        g["Title"] != null && g["Title"].ToString() == groupIndicator)
                                      .ToList();
            // Если таких групп ровно одна, переносим массив "Tags" из Source в эту группу.
            if (relatedGroups.Count == 1)
            {
                JObject targetGroup = relatedGroups.First();
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

                JArray sourceTags = (JArray)source["Tags"];
                foreach (JObject tag in sourceTags.OfType<JObject>())
                {
                    tag.Remove("Group");
                    tag.Remove("Source");
                    groupTags.Add(tag);
                }
                // После успешного переноса удаляем массив "Tags" из Source.
                source.Remove("Tags");
            }
        }

        return root.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Нормализует настройки, обрабатывая дублирующиеся значения свойства "Title".
    /// Для каждого массива (Sources, Groups, Tags) если встречаются объекты с одинаковым Title,
    /// то для второго и последующих экземпляров к Title добавляется суффикс _copyN, где N – номер копии.
    /// Первый экземпляр с данным Title остаётся без изменений.
    /// </summary>
    public static string NormalizeDuplicateEntries(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);
        // Список имен массивов, которые нужно обработать
        string[] arrayNames = { "Sources", "Groups", "Tags" };

        foreach (var arrayName in arrayNames)
        {
            if (root[arrayName] != null && root[arrayName].Type == JTokenType.Array)
            {
                var array = (JArray)root[arrayName];
                // Словарь для хранения количества встреч Title
                Dictionary<string, int> titleCounts = new Dictionary<string, int>();
                foreach (JObject item in array.OfType<JObject>())
                {
                    JToken titleToken = item["Title"];
                    if (titleToken != null)
                    {
                        string title = titleToken.ToString();
                        if (!titleCounts.ContainsKey(title))
                        {
                            // Первый экземпляр – сохраняем как есть.
                            titleCounts[title] = 0;
                        }
                        else
                        {
                            // Повторный экземпляр – увеличиваем счётчик и обновляем Title
                            titleCounts[title]++;
                            string newTitle = $"{title}_copy{titleCounts[title]}";
                            item["Title"] = newTitle;
                        }
                    }
                }
            }
        }

        return root.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Нормализует дублирующиеся записи в корневых массивах (например, "Sources", "Groups" или "Tags") по свойству "Title".
    /// Если для объектов с одинаковым Title можно выполнить объединение (то есть все совпадающие свойства имеют идентичные значения),
    /// то все эти объекты объединяются в один (при этом объединяются все уникальные свойства);
    /// если объединить объекты невозможно (при обнаружении конфликта – например, один объект имеет свойство "Desc" со значением A, а другой – "Desc" со значением B),
    /// то объединение не выполняется, и для каждого объекта из этой группы применяется переименование, добавляя суффиксы _copy1, _copy2 и т.д. (первый объект остаётся без изменений).
    /// В итоге для каждого Title либо происходит объединение, либо остаются все оригинальные объекты с уникальными именами.
    /// </summary>
    /// <param name="inputJson">
    /// Исходная строка JSON, содержащая корневые массивы, например, "Sources", "Groups" и "Tags".
    /// </param>
    /// <returns>
    /// Нормализованная строка JSON, в которой для каждого массива дублирующиеся объекты обработаны согласно вышеописанному правилу.
    /// </returns>
    public static string NormalizeDuplicateEntriesMerge(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);
        // Массивы, для которых проводится нормализация
        string[] arrayNames = { "Sources", "Groups", "Tags" };

        foreach (var arrayName in arrayNames)
        {
            if (root[arrayName] != null && root[arrayName].Type == JTokenType.Array)
            {
                JArray array = (JArray)root[arrayName];
                // Группируем объекты по значению свойства "Title"
                var groupsByTitle = array.OfType<JObject>()
                                         .GroupBy(obj => obj["Title"]?.ToString() ?? string.Empty)
                                         .Where(g => !string.IsNullOrEmpty(g.Key) && g.Count() > 1);

                // Для хранения объектов, которые мы будем добавлять вместо объединённой группы
                List<JObject> mergedObjects = new List<JObject>();
                // Для хранения объектов, для которых объединение не удалось – будем применять переименование (fallback)
                List<JObject> nonMergedObjects = new List<JObject>();

                // Итоговый список объектов, который заменит исходный массив
                List<JObject> resultObjects = new List<JObject>();

                // Создадим список всех объектов, группируя по Title.
                // Для тех Title, которые встречаются один раз, оставляем объект без изменений.
                var singleObjects = array.OfType<JObject>()
                                           .Where(obj => string.IsNullOrEmpty(obj["Title"]?.ToString()) ||
                                                         array.Count(x => x["Title"]?.ToString() == obj["Title"]?.ToString()) == 1);
                resultObjects.AddRange(singleObjects);

                // Обрабатываем каждую группу с одинаковым Title
                foreach (var group in groupsByTitle)
                {
                    bool mergeable = true;
                    Dictionary<string, JToken> mergedProperties = new Dictionary<string, JToken>();
                    string commonTitle = group.Key;

                    // Перебираем все объекты в группе.
                    foreach (JObject obj in group)
                    {
                        foreach (var prop in obj.Properties())
                        {
                            // Пропускаем свойство Title
                            if (prop.Name == "Title")
                                continue;

                            // Если такое свойство уже встречалось в предыдущих объектах группы
                            if (mergedProperties.ContainsKey(prop.Name))
                            {
                                // Если значение отличается, устанавливаем флаг невозможности объединения.
                                if (!JToken.DeepEquals(mergedProperties[prop.Name], prop.Value))
                                {
                                    mergeable = false;
                                    break;
                                }
                            }
                            else
                            {
                                // Добавляем свойство, если ранее не встречалось.
                                mergedProperties[prop.Name] = prop.Value.DeepClone();
                            }
                        }
                        if (!mergeable)
                            break;
                    }

                    if (mergeable)
                    {
                        // Объединяем все объекты в один
                        JObject merged = new JObject();
                        merged["Title"] = commonTitle;
                        foreach (var kvp in mergedProperties)
                        {
                            merged[kvp.Key] = kvp.Value;
                        }
                        mergedObjects.Add(merged);
                        resultObjects.Add(merged);
                    }
                    else
                    {
                        // Если объединить объекты нельзя – применяем fallback:
                        // Оставляем все объекты этой группы, но переименовываем их, добавляя суффикс _copyN
                        int count = 0;
                        foreach (JObject obj in group)
                        {
                            // Клонируем объект, чтобы не изменять оригинал.
                            JObject clone = (JObject)obj.DeepClone();
                            if (count > 0)
                            {
                                clone["Title"] = $"{commonTitle}_copy{count}";
                            }
                            nonMergedObjects.Add(clone);
                            resultObjects.Add(clone);
                            count++;
                        }
                    }
                }

                // Заменяем исходный массив на результат
                root[arrayName] = new JArray(resultObjects);
            }
        }

        return root.ToString(Formatting.Indented);
    }

    // ======================================================================================================

    /// <summary>
    /// Преобразует вложенную структуру Blocks в плоскую, минимально вложенную.
    /// Для каждого листового массива тегов в исходном Blocks создаётся составной ключ,
    /// который формируется объединением имен всех уровней (разделённых точкой).
    /// Результирующий объект Blocks содержит только пары "составной ключ" – "массив тегов".
    /// </summary>
    public static string FlattenBlocksToMinimalNesting(string inputJson)
    {
        // Парсинг исходного JSON
        JObject root = JObject.Parse(inputJson);
        // Новый объект, в который будем складывать плоскую структуру Blocks
        JObject flatBlocks = new JObject();

        // Рекурсивная функция для обхода вложенной структуры Blocks.
        // currentPath накапливает составное имя ключа по мере обхода.
        void TraverseBlocks(JToken token, string currentPath)
        {
            if (token is JObject obj)
            {
                foreach (var prop in obj.Properties())
                {
                    // Формируем новый составной ключ: если currentPath пустой, то просто имя свойства,
                    // иначе объединяем через точку.
                    string newPath = string.IsNullOrEmpty(currentPath) ? prop.Name : currentPath + "." + prop.Name;
                    TraverseBlocks(prop.Value, newPath);
                }
            }
            else if (token is JArray arr)
            {
                // Достигли листового узла (массив тегов).
                // Сохраняем массив в flatBlocks с составным ключом.
                flatBlocks[currentPath] = arr;
            }
            // Если токен имеет иной тип, его можно игнорировать.
        }

        // Если в корневом объекте есть Blocks, обходим его и формируем плоскую структуру
        if (root["Blocks"] != null)
        {
            TraverseBlocks(root["Blocks"], "");
            // Заменяем исходное свойство Blocks на полученную плоскую структуру.
            root["Blocks"] = flatBlocks;
        }

        return root.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Extracts tags from the "Blocks" section of the JSON, adds a "Block" property to each extracted tag
    /// with the value corresponding to its nesting path, appends these tags to the root-level "Tags" array,
    /// and then removes the "Blocks" section from the JSON.
    /// </summary>
    public static string ExtractTagsFromBlocks(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);

        // List to accumulate extracted tags from the Blocks section.
        List<JObject> extractedTags = new List<JObject>();

        // Recursive function to traverse the Blocks structure.
        void TraverseBlocks(JToken token, string currentPath)
        {
            if (token is JObject obj)
            {
                // For each property update the path and recursively traverse its value.
                foreach (var prop in obj.Properties())
                {
                    string newPath = string.IsNullOrEmpty(currentPath) ? prop.Name : currentPath + "." + prop.Name;
                    TraverseBlocks(prop.Value, newPath);
                }
            }
            else if (token is JArray array)
            {
                // Assume array elements are tags.
                foreach (JToken item in array)
                {
                    if (item is JObject tagObj)
                    {
                        // Set the "Block" property to the current path.
                        tagObj["Block"] = currentPath;
                        extractedTags.Add(tagObj);
                    }
                }
            }
            // Other token types are ignored.
        }

        // If the JSON contains a "Blocks" section, traverse it.
        if (root["Blocks"] != null)
        {
            TraverseBlocks(root["Blocks"], "");
            // After extraction, remove the "Blocks" section.
            root.Remove("Blocks");
        }

        // Obtain or create the root-level "Tags" array.
        JArray rootTags;
        if (root["Tags"] != null && root["Tags"].Type == JTokenType.Array)
        {
            rootTags = (JArray)root["Tags"];
        }
        else
        {
            rootTags = new JArray();
            root["Tags"] = rootTags;
        }

        // Add all extracted tags to the root-level "Tags" array.
        foreach (var tag in extractedTags)
        {
            rootTags.Add(tag);
        }

        return root.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Reinserts tags into a nested "Blocks" structure based on their "Block" property.
    /// For each tag in the root-level "Tags" array that contains a "Block" property,
    /// the method removes that property from the tag and uses its value (which is expected 
    /// to be a dot-separated string, e.g. "Name.A.4") to create a nested structure in a new 
    /// "Blocks" section. The tag is then appended to the array at the leaf node of that structure.
    /// Tags that do not have a "Block" property remain in the root-level "Tags" array.
    /// After processing, the "Blocks" section is added to the root if not empty.
    /// </summary>
    public static string ReintegrateTagsToBlocks(string inputJson)
    {
        JObject root = JObject.Parse(inputJson);

        // Create a new JObject to build the Blocks structure.
        JObject blocks = new JObject();

        // Get the root-level Tags array. If absent, создаём пустой.
        JArray tags = root["Tags"] as JArray ?? new JArray();

        // List для хранения тегов, у которых не задан параметр Block.
        List<JObject> remainingTags = new List<JObject>();

        // Обходим все теги в корневом массиве.
        foreach (JObject tag in tags.OfType<JObject>().ToList())
        {
            JToken blockToken = tag["Block"];
            if (blockToken != null)
            {
                // Извлекаем значение свойства Block, например "Name.A.4"
                string blockPath = blockToken.ToString();
                // Удаляем свойство "Block" из тега, поскольку оно больше не потребуется.
                tag.Remove("Block");

                // Разбиваем путь по точке.
                string[] pathParts = blockPath.Split('.');
                JObject current = blocks;
                // Проходим по всем частям пути, кроме последней, создавая вложенные объекты, если их нет.
                for (int i = 0; i < pathParts.Length - 1; i++)
                {
                    string key = pathParts[i];
                    if (current[key] == null || current[key].Type != JTokenType.Object)
                    {
                        current[key] = new JObject();
                    }
                    current = (JObject)current[key];
                }
                // Последняя часть пути используется как ключ для массива тегов.
                string leafKey = pathParts.Last();
                JArray tagArray;
                if (current[leafKey] == null || current[leafKey].Type != JTokenType.Array)
                {
                    tagArray = new JArray();
                    current[leafKey] = tagArray;
                }
                else
                {
                    tagArray = (JArray)current[leafKey];
                }
                // Добавляем тег в найденный (или созданный) массив.
                tagArray.Add(tag);
            }
            else
            {
                // Если у тега нет свойства Block, оставляем его в корневом массиве.
                remainingTags.Add(tag);
            }
        }

        // Обновляем корневой массив "Tags" оставшимися тегами.
        root["Tags"] = new JArray(remainingTags);

        // Если полученная структура Blocks содержит данные, добавляем её в корневой объект.
        if (blocks.HasValues)
        {
            root["Blocks"] = blocks;
        }
        else
        {
            // Если Blocks пуст, убираем его (если оно было).
            root.Remove("Blocks");
        }

        return root.ToString(Formatting.Indented);
    }


}


