using Connector;
using DML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSimpleIDriver.Connector;

namespace WinSimpleIDriver
{
    static class EditorControl
    {
        static public string fullFileName = "";

        static public List<SourceEditor> sources;
        static public List<TagEditor> tags;
        static public List<GroupEditor> groups;

        static public List<StructureEditor> structures;
        static public List<StructTargetEditor> structTargets;
        static public List<StructTagEditor> structTags;

        static public List<IncludeEditor> includes;
        static public List<IncludeChildEditor> includeChilds;

        static ushort sourceId = 0;
        static ushort groupId = 0;
        static ushort tagId = 0;
        static uint blockUnnamedId = 0;

        static uint structureId = 0;
        static uint structTargetId = 0;
        static uint structTagId = 0;

        static uint includeId = 0;
        static uint includeChildId = 0;

        // Очистка данных
        static public void Clear()
        {
            sources = new List<SourceEditor>();
            tags = new List<TagEditor>();
            groups = new List<GroupEditor>();

            structures = new List<StructureEditor>();
            structTargets = new List<StructTargetEditor>();
            structTags = new List<StructTagEditor>();

            includes = new List<IncludeEditor>();
            includeChilds = new List<IncludeChildEditor>();

            sourceId = 0;
            groupId = 0;
            tagId = 0;
            blockUnnamedId = 0;

            structureId = 0;
            structTargetId = 0;
            structTagId = 0;

            includeId = 0;
            includeChildId = 0;
        }

        #region Unpack

        // Распаковка проекта
        static public void UnpackProject(string input)
        {
            // получение JSON данных
            dynamic settings = JsonControl.Deserialize_Json_Data(input);

            Clear();

            if (settings == null)
                return;

            if (JsonControl.IsProp(settings, "Data"))
            {
                var data = settings.Data; // Sources, Groups, Tags

                // Распаковка источников
                if (SourceLib.InProject(data))
                    sources = SourceLib.ParseSources(data.Sources);

                // Распаковка групп
                if (GroupLib.InProject(data))
                    groups = GroupLib.ParseGroups(data.Groups);

                // Распаковка тегов
                if (TagLib.InProject(data))
                    tags = TagLib.ParseTags(data.Tags);

            }

            // Распаковка структур
            if (TagLib.IsStructures(settings))
                ParseStructures(settings.Structures);

            // Внешние проекты
            if (Include.InProject(settings))
                ParseIncludes(settings.Includes); // Распаковка настроек внешних проектов

        }

        // Распаковка структур
        static void ParseStructures(dynamic data)
        {
            if (data != null)
            {
                foreach (dynamic item in data)
                {
                    if (TagLib.IsTargetTags(item))
                    {
                        TagLib.ParseItemStructure(item, out string title, out string join, out string address, out eDataType dataType, out string source, out string group, out string[] sourceTags, out List <TargetTag> targetTags);
                        //---
                        StructureEditor oneStructure = new StructureEditor
                        {
                            Id = ++structureId,
                            title = title,
                            join = join,
                            templateAddress = address,
                            dataType = dataType,
                            tagSource = source,
                            group = group,
                        };
                        structures.Add(oneStructure);
                        //---
                        foreach (var target in targetTags)
                        {
                            StructTargetEditor oneTarget = new StructTargetEditor
                            {
                                Id = ++structTargetId,
                                structureTitle = title,
                                innerAddress = target.address,
                                title = target.title,
                                desc = target.desc
                            };
                            structTargets.Add(oneTarget);
                        }
                        //
                        foreach (var tagTitle in sourceTags)
                        {
                            StructTagEditor oneTag = new StructTagEditor
                            {
                                Id = ++structTagId,
                                structureTitle = title,
                                title = tagTitle,
                            };
                            structTags.Add(oneTag);
                        }

                    }
                }
            }
        }

        // Распаковка внешних проектов
        static void ParseIncludes(dynamic data)
        {
            if (data != null)
            {
                foreach (dynamic item in data)
                {
                    Include.ParseItemInclude(item, out string fileName, out string prefix, out Dictionary<string, string> changes);

                    IncludeEditor oneInclude = new IncludeEditor
                    {
                        Id = ++includeId,
                        fileName = fileName,
                        prefix = prefix,
                        //changes = changes
                    };

                    foreach (var oneChange in changes)
                    {
                        IncludeChildEditor ice = new IncludeChildEditor
                        {
                            Id = ++includeChildId,
                            prefix = prefix,
                            changeFrom = oneChange.Key,
                            changeTo = oneChange.Value
                        };
                        includeChilds.Add(ice);
                    }

                    includes.Add(oneInclude);
                }
            }
        }

        #endregion

        #region Pack

        // Упаковка проекта
        static public dynamic PackProject()
        {
            // Создаём корневой объект для настроек проекта
            JObject root = new JObject();

            // Формирование раздела Data с Sources, Groups и Tags
            JObject data = new JObject();

            // Источники
            JArray arrSources = new JArray();
            if (sources != null)
            {
                foreach (var src in sources)
                {
                    // Собираем свойства источника согласно схеме UnpackProject
                    JObject jSrc = new JObject();
                    //jSrc["Id"] = src.Id;
                    jSrc["Title"] = src.title;
                    jSrc["Driver"] = src.driver.ToString(); // можно изменить вывод драйвера при необходимости
                    jSrc["Address"] = src.address;

                    if (src.disableOnStart)
                        jSrc["Off"] = src.disableOnStart;

                    if (String.IsNullOrWhiteSpace(src.description) == false)
                        jSrc["Desc"] = src.description;

                    if (src.auto)
                        jSrc["Auto"] = src.auto;

                    if (src.reconnect)
                        jSrc["Reconnect"] = src.reconnect;
                    
                    arrSources.Add(jSrc);
                }
            }
            data["Sources"] = arrSources;

            // Группы
            JArray arrGroups = new JArray();
            if (groups != null)
            {
                foreach (var grp in groups)
                {
                    JObject jGrp = new JObject();
                    //jGrp["Id"] = grp.Id;
                    jGrp["Title"] = grp.title;
                    jGrp["UpdateRate"] = grp.updateRate;
                    jGrp["Source"] = grp.sourceTitle;

                    if (grp.disableOnStart)
                        jGrp["Off"] = grp.disableOnStart;

                    if (String.IsNullOrWhiteSpace(grp.description) == false)
                        jGrp["Desc"] = grp.description;

                    arrGroups.Add(jGrp);
                }
            }
            data["Groups"] = arrGroups;

            // Теги
            JArray arrTags = new JArray();
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    JObject jTag = new JObject();
                    //jTag["Id"] = tag.Id;
                    jTag["Title"] = tag.title;
                    jTag["Group"] = tag.groupTitle;
                    jTag["DataType"] = tag.dataType.ToString();
                    jTag["Addr"] = tag.address;
                    
                    if (tag.disableOnStart)
                        jTag["Off"] = tag.disableOnStart;

                    if (String.IsNullOrWhiteSpace(tag.description) == false)
                        jTag["Desc"] = tag.description;

                    if (String.IsNullOrWhiteSpace(tag.writeTitle) == false)
                        jTag["Write"] = tag.writeTitle;

                    if (String.IsNullOrWhiteSpace(tag.constValue) == false)
                        jTag["Value"] = tag.constValue;

                    if (tag.isCommand)
                        jTag["Command"] = tag.isCommand;

                    if (String.IsNullOrWhiteSpace(tag.block) == false)
                        jTag["Block"] = tag.block;

                    arrTags.Add(jTag);
                }
            }
            data["Tags"] = arrTags;

            root["Data"] = data;

            // Формирование раздела Structures если данные присутствуют
            if (structures != null && structures.Count > 0)
            {
                JArray arrStructures = new JArray();
                foreach (var structEditor in structures)
                {
                    JObject jStruct = new JObject();
                    jStruct["Id"] = structEditor.Id;
                    jStruct["Title"] = structEditor.title;
                    jStruct["Join"] = structEditor.join;
                    jStruct["Address"] = structEditor.templateAddress;
                    jStruct["DataType"] = structEditor.dataType.ToString();
                    jStruct["Source"] = structEditor.tagSource;
                    jStruct["Group"] = structEditor.group;
                    // Формирование массива SourceTags по данным из structTags
                    JArray arrSourceTags = new JArray();
                    if (structTags != null)
                    {
                        foreach (var st in structTags.Where(t => t.structureTitle == structEditor.title))
                        {
                            arrSourceTags.Add(st.title);
                        }
                    }
                    jStruct["SourceTags"] = arrSourceTags;
                    // Формирование массива TargetTags по данным из structTargets
                    JArray arrTargetTags = new JArray();
                    if (structTargets != null)
                    {
                        foreach (var tt in structTargets.Where(t => t.structureTitle == structEditor.title))
                        {
                            JObject jTarget = new JObject();
                            jTarget["Title"] = tt.title;
                            jTarget["Address"] = tt.innerAddress;
                            jTarget["Desc"] = tt.desc;
                            arrTargetTags.Add(jTarget);
                        }
                    }
                    jStruct["TargetTags"] = arrTargetTags;
                    arrStructures.Add(jStruct);
                }
                root["Structures"] = arrStructures;
            }

            // Формирование раздела Includes если данные присутствуют
            if (includes != null && includes.Count > 0)
            {
                JArray arrIncludes = new JArray();
                foreach (var incl in includes)
                {
                    JObject jIncl = new JObject();
                    jIncl["Id"] = incl.Id;
                    jIncl["fileName"] = incl.fileName;
                    jIncl["prefix"] = incl.prefix;
                    // Для изменений собираем словарь изменений из includeChilds по совпадению префикса
                    JObject jChanges = new JObject();
                    if (includeChilds != null)
                    {
                        foreach (var child in includeChilds.Where(c => c.prefix == incl.prefix))
                        {
                            jChanges[child.changeFrom] = child.changeTo;
                        }
                    }
                    jIncl["changes"] = jChanges;
                    arrIncludes.Add(jIncl);
                }
                root["Includes"] = arrIncludes;
            }

            // Дополнительно можно сохранить имя файла проекта
            //if (!string.IsNullOrWhiteSpace(fullFileName))
            //{
            //    root["fullFileName"] = fullFileName;
            //}

            // получение строки из JSON данных
            string settings = JsonControl.Serialize_Json_Data(root);
            return settings;
        }

        #endregion

        // =============================================================================================================

        // Получить название блока
        static string GetBlockName(dynamic item, string prefix = "")
        {
            string nm = (++blockUnnamedId).ToString();
            if (JsonControl.IsProp(item, "Desc"))
            {
                if (!String.IsNullOrWhiteSpace(item.Desc))
                    nm = item.Desc;
            }

            if (String.IsNullOrWhiteSpace(prefix) == false)
            {
                nm = prefix + "." + nm;
            }

            return nm;
        }


    }
}
