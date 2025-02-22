using DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSimpleIDriver.Connector;
using WinSimpleIDriver.Connector.SGT;

namespace WinSimpleIDriver
{
    static class EditorControl
    {

        static public List<SourceEditor> sources;
        static public List<TagEditor> tags;
        static public List<GroupEditor> groups;

        static uint sourceId = 0;
        static uint groupId = 0;
        static uint tagId = 0;
        static uint blockUnnamedId = 0;

        // Очистка данных
        static public void Clear()
        {
            sources = new List<SourceEditor>();
            tags = new List<TagEditor>();
            groups = new List<GroupEditor>();

            sourceId = 0;
            groupId = 0;
            tagId = 0;
            blockUnnamedId = 0;
        }

        // Распаковка проекта
        static public void ParseData(dynamic output)
        {
            Clear();

            if (output == null)
                return;

            // Распаковка источников
            if (Source.InProject(output))
                ParseSources(output.Sources);

            // Распаковка групп
            if (Group.InProject(output))
                ParseGroups(output.Groups);

            // Распаковка тегов
            if (Tag.InProject(output))
                ParseTags(output.Tags);

            // Блоки с тегами
            if (Tag.IsListBlocks(output))
            {
                if (output.Blocks != null)
                {
                    foreach (var elItem in output.Blocks)
                    {
                        string nm = GetBlockName(elItem);
                        ParseTags(elItem.Tags, 0, 0, nm);
                    }
                }
            }

            // Установить ID и Title для объектов
            CalcIdAndTitle(); 

        }

        // Распаковка источников
        static void ParseSources(dynamic data)
        {
            if (data != null)
            {
                
                foreach (dynamic item in data)
                {
                    Source.ParseItemSource(item, ++sourceId, out string title, out eDriverType driver, out string address, out string groupTitle, out bool off, out string description, out dynamic tagsInSource, out bool auto, out bool reconnect);
                    SourceEditor rowSource = new SourceEditor
                    {
                        Id = sourceId,
                        groupId = 0,
                        groupTitle = groupTitle,
                        driver = driver,
                        title = title,
                        address = address,
                        off = off,
                        description = description,
                        auto = auto,
                        reconnect = reconnect
                    };
                    sources.Add(rowSource);

                    // теги
                    if (tagsInSource != null)
                        ParseTags(tagsInSource, sourceId, 0);

                    // Блоки с тегами
                    if (Tag.IsListBlocks(item))
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
                    Group.ParseItemGroup(item, ++groupId, out string title, out uint updateRate, out bool off, out string description, out string sourceTitle, out dynamic tagsInSource);
                    GroupEditor rowGroup = new GroupEditor
                    {
                        Id = groupId,
                        title = title,
                        off = off,
                        updateRate = updateRate,
                        description = description,
                        sourceTitle = sourceTitle
                    };
                    groups.Add(rowGroup);

                    // теги
                    if (tagsInSource != null)
                        ParseTags(tagsInSource, 0, groupId);

                    // Блоки с тегами
                    if (Tag.IsListBlocks(item))
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
        static void ParseTags(dynamic data, uint sourceId = 0, uint groupId = 0, string block = null)
        {
            if (data != null)
            {
                
                foreach (dynamic item in data)
                {
                    Tag.ParseItemTag(item, ++tagId, out string title, out string sourceTitle, out eDataType dataType, out bool off, out string address, out string description, out string writeTitle, out string groupTitle, out string constValue, out bool isCommand);
                    TagEditor oneTag = new TagEditor
                    {
                        Id = tagId,
                        title = title,
                        dataType = dataType,
                        sourceId = sourceId,
                        groupId = groupId,
                        sourceTitle = sourceTitle,
                        groupTitle = groupTitle,
                        address = address,
                        off = off,
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
        static void CalcIdAndTitle()
        {
            var sourceDict = sources.ToDictionary(x => x.Id);
            var groupDict = groups.ToDictionary(x => x.Id);

            foreach (var tag in tags)
            {
                if (sourceDict.TryGetValue(tag.sourceId, out var source))
                {
                    tag.sourceTitle = source.title;
                    tag.sourceId = source.Id;
                }
                if (groupDict.TryGetValue(tag.groupId, out var group))
                {
                    tag.groupTitle = group.title;
                    tag.groupId = group.Id;
                }
            }

            foreach (var source in sources)
            {
                if (source.groupId == 0 && !string.IsNullOrWhiteSpace(source.groupTitle) && groupDict.TryGetValue(source.groupId, out var group))
                {
                    source.groupId = group.Id;
                }
            }
        }



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
