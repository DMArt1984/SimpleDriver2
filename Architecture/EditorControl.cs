using Connector;
using DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        static public void UnpackProject(dynamic data)
        {
            Clear();

            if (data == null)
                return;

            // Распаковка источников
            if (SourceLib.InProject(data))
                ParseSources(data.Sources);

            // Распаковка групп
            if (GroupLib.InProject(data))
                ParseGroups(data.Groups);

            // Распаковка тегов
            if (TagLib.InProject(data))
                ParseTags(data.Tags);

            // Блоки с тегами
            if (TagLib.IsListBlocks(data))
            {
                if (data.Blocks != null)
                {
                    foreach (var elItem in data.Blocks)
                    {
                        string nm = GetBlockName(elItem);
                        ParseTags(elItem.Tags, 0, 0, nm);
                    }
                }
            }

            // Установить ID и Title для объектов
            //CalcIdAndTitle();

            // Распаковка структур
            if (TagLib.IsStructures(data))
                ParseStructures(data.Structures);

            // Внешние проекты
            if (Include.InProject(data))
                ParseIncludes(data.Includes); // Распаковка настроек внешних проектов


        }

        // Распаковка источников
        static void ParseSources(dynamic data)
        {
            if (data != null)
            {
                
                foreach (dynamic item in data)
                {
                    SourceLib.ParseItemSource(item, ++sourceId, out string title, out eDriverType driver, out string address, out bool off, out string description, out dynamic tagsInSource, out bool auto, out bool reconnect);
                    SourceEditor rowSource = new SourceEditor
                    {
                        Id = sourceId,
                        driver = driver,
                        title = title,
                        address = address,
                        disableOnStart = off,
                        description = description,
                        auto = auto,
                        reconnect = reconnect
                    };
                    sources.Add(rowSource);

                    // теги
                    if (tagsInSource != null)
                        ParseTags(tagsInSource, sourceId, 0);

                    // Блоки с тегами
                    if (TagLib.IsListBlocks(item))
                    {
                        if (item.Blocks != null)
                        {
                            foreach (var elItem in item.Blocks)
                            {
                                string nm = GetBlockName(elItem);
                                ParseTags(elItem.Tags, sourceId, 0, nm);
                            }
                        }
                    }



                }


            }


        }

        // Распаковка групп
        static void ParseGroups(dynamic data)
        {
            if (data != null)
            {
                
                foreach (dynamic item in data)
                {
                    GroupLib.ParseItemGroup(item, ++groupId, out string title, out uint updateRate, out bool off, out string description, out string sourceTitle, out dynamic tagsInSource);
                    GroupEditor rowGroup = new GroupEditor
                    {
                        Id = groupId,
                        title = title,
                        disableOnStart = off,
                        updateRate = updateRate,
                        description = description,
                        sourceTitle = sourceTitle
                    };
                    groups.Add(rowGroup);

                    // теги
                    if (tagsInSource != null)
                        ParseTags(tagsInSource, 0, groupId);

                    // Блоки с тегами
                    if (TagLib.IsListBlocks(item))
                    {
                        if (item.Blocks != null)
                        {
                            foreach (var elItem in item.Blocks)
                            {
                                if (JsonControl.IsProp(elItem, "Tags") && elItem.Tags != null)
                                {
                                    string nm = GetBlockName(elItem);
                                    ParseTags(elItem.Tags, 0, groupId, nm);
                                }
                            }
                        }
                    }

                }
            }
        }

        // Распаковка тегов
        static void ParseTags(dynamic data, ushort sourceId = 0, ushort groupId = 0, string block = null)
        {
            if (data != null)
            {
                
                foreach (dynamic item in data)
                {
                    TagLib.ParseItemTag(item, ++tagId, out string title, out eDataType dataType, out bool off, out string address, out string description, out string writeTitle, out string groupTitle, out string constValue, out bool isCommand);
                    TagEditor oneTag = new TagEditor
                    {
                        Id = tagId,
                        title = title,
                        dataType = dataType,
                        groupTitle = groupTitle,
                        address = address,
                        disableOnStart = off,
                        isCommand = isCommand,
                        writeTitle = writeTitle,
                        constValue = constValue,
                        description = description,
                        block = block
                    };
                    tags.Add(oneTag);
                }
            }
        }

        // Установить ID и Title для объектов
        //static void CalcIdAndTitle()
        //{
        //    var sourceDict = sources.ToDictionary(x => x.Id);
        //    var groupDict = groups.ToDictionary(x => x.Id);

        //    foreach (var tag in tags)
        //    {
        //        if (sourceDict.TryGetValue(tag.sourceId, out var source))
        //        {
        //            tag.sourceTitle = source.title;
        //            tag.sourceId = source.Id;
        //        }
        //        if (groupDict.TryGetValue(tag.groupId, out var group))
        //        {
        //            tag.groupTitle = group.title;
        //            tag.groupId = group.Id;
        //        }
        //    }
        //}

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
            // здесь нужен код...
            return null; // нужно вернуть dynamic data
        }

        #endregion

        // ===========================================================

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
