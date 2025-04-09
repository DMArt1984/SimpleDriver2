using Connector;
using DML;
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
            // здесь нужен код...
            return null; // нужно вернуть dynamic data
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
