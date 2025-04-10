using Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSimpleIDriver;
using WinSimpleIDriver.Connector;

namespace DML
{
    #region DGV
    public struct DGVIncludeCol
    {
        public int Prefix;
        public int FileName;
    }
    public struct DGVChangeCol
    {
        public int Prefix;
        public int ChangeFrom;
        public int ChangeTo;
    }
    #endregion

    public struct TableIdentity
    {
        public ushort Id;
        public string Title;
        public string Link; // Ссылка на уровень выше
    }

    static class DataTableLib
    {
        public struct ColumnValue
        {
            public int column;
            public dynamic value;
            public ColumnValue(int column, dynamic value)
            {
                this.column = column;
                this.value = value;
            }
        }

        public struct PairFilterCol
        {
            public int col;
            public string filter;
        }

        #region Table Enum
        static public void SetTableEnum()
        {
            // Устройства
            ComboBox cbDriver = new ComboBox();
            foreach (string title in Enum.GetNames(typeof(eDriverType)))
                cbDriver.Items.Add(title);
            ((DataGridViewComboBoxColumn)dtSource.dgv.Columns[dtSource.col.Driver]).DataSource = cbDriver.Items;

            // Теги и структуры
            ComboBox cbTypeData = new ComboBox();
            foreach (string title in Enum.GetNames(typeof(eDataType)))
                cbTypeData.Items.Add(title);
            ((DataGridViewComboBoxColumn)dtTag.dgv.Columns[dtTag.col.DataType]).DataSource = cbTypeData.Items;
            ((DataGridViewComboBoxColumn)dtStructure.dgv.Columns[dtStructure.col.DataType]).DataSource = cbTypeData.Items;
            
        }
        #endregion

        static public void Clear()
        {
            dtTag.Clear();
            dtGroup.dgv.Rows.Clear();
            dtSource.dgv.Rows.Clear();

            dtStructTag.dgv.Rows.Clear();
            dtStructTarget.dgv.Rows.Clear();
            dtStructure.dgv.Rows.Clear();

            dtIncludeChild.dgv.Rows.Clear();
            dtInclude.dgv.Rows.Clear();
        }

        #region Source

        static public class dtSource
        {
            static public DGVSourcesCol col = new DGVSourcesCol(); // Номера колонок в DGV

            static public DataGridView dgv;

            static public CheckBox cbEditor;
            static public CheckBox cbDesc;
            static public CheckBox cbRuntime;
            static public CheckBox cbStatistic;

            static public TextBox tbFilter;

            #region DGV
            static public void DataToTable(List<SourceEditor> sources)
            {
                SourceLib.DataToTable(dgv, col, sources);
            }
            #endregion
            static public List<SourceEditor> TableToData()
            {
                return SourceLib.TableToData(dgv, col);
            }


            #region DGV.Columns
            static public void CheckColumns()
            {
                bool checkE = cbEditor.Checked;
                dgv.Columns["sourceID"].Visible = checkE;
                dgv.Columns["sourceAutomation"].Visible = checkE;
                dgv.Columns["sourceAutoRestart"].Visible = checkE;
                dgv.Columns["sourceDriver"].Visible = checkE;
                dgv.Columns["sourceAddress"].Visible = checkE;
                dgv.RowHeadersVisible = checkE;
                dgv.ReadOnly = !checkE;

                bool checkD = cbDesc.Checked;
                dgv.Columns["sourceDesc"].Visible = checkD;

                bool checkR = cbRuntime.Checked;
                dgv.Columns["sourceCalc"].Visible = checkR;
                dgv.Columns["sourceStatus"].Visible = checkR;
                dgv.Columns["sourceMessage"].Visible = checkR;

                bool checkS = cbStatistic.Checked;
                dgv.Columns["sourceTags"].Visible = checkS;
                dgv.Columns["sourceStatistic"].Visible = checkS;
            }

            #endregion

            #region Filter
            static public void TextFilter()
            {
                TableFilter(tbFilter.Text, dgv,
                    GetColumnIndexFilter(), GetPairFilter());
            }
            #endregion

            // Определение номеров колонок
            static public void LinkColumns(DataGridView sources)
            {
                dgv = sources;
                col = SourceLib.GetCols(dgv);
            }

            // Источники. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.Title, col.Driver, col.Address, col.Desc, col.Status, col.Message
                };
            }
            static public PairFilterCol[] GetPairFilter()
            {
                return new PairFilterCol[] { };
            }

            // Копировать стороку
            static public void CopyDGVRow()
            {
                DataTableLib.CopyDGVRow(dgv, col.Title);
            }

            // Удалить строку
            static public void DelDGVRow()
            {
                DataTableLib.DelDGVRow(dgv);
            }

            // Открыть справку по драйверу
            public static void Help()
            {
                if (dgv == null || dgv.CurrentRow == null || col.Driver < 0 || col.Driver >= dgv.ColumnCount)
                    return;

                string typeName = dgv.CurrentRow.Cells[col.Driver].Value?.ToString();
                if (string.IsNullOrWhiteSpace(typeName))
                    return;

                if (!Enum.TryParse(typeName, out eDriverType est))
                    return; // Если строка не соответствует enum, просто выходим

                var dic = SourceHelp.HelpDicSource(est);
                if (dic == null || dic.Count == 0)
                    return; // Не открываем форму, если нет данных

                var help = new FormHelp
                {
                    Text = $"Справка по драйверу {typeName}",
                    dic = dic
                };

                help.Show();
            }

        }

        #endregion

        #region Group

        static public class dtGroup
        {
            static public DGVGroupsCol col = new DGVGroupsCol(); // Номера колонок в DGV

            static public DataGridView dgv;

            static public CheckBox cbEditor;
            static public CheckBox cbDesc;
            static public CheckBox cbRuntime;
            static public CheckBox cbStatistic;
            static public CheckBox cbGroupSource;

            static public TextBox tbFilter;
            static public ComboBox coFilterSource;

            #region DGV
            static public void DataToTable(List<GroupEditor> groups)
            {
                GroupLib.DataToTable(dgv, col, groups);
            }
            static public List<GroupEditor> TableToData()
            {
                return GroupLib.TableToData(dgv, col);
            }
            #endregion

            #region DGV.Columns
            static public void CheckColumns()
            {
                bool checkE = cbEditor.Checked;
                dgv.Columns["groupID"].Visible = checkE;
                dgv.Columns["groupPeriod"].Visible = checkE;
                dgv.RowHeadersVisible = checkE;
                dgv.ReadOnly = !checkE;

                bool checkD = cbDesc.Checked;
                dgv.Columns["groupDesc"].Visible = checkD;

                bool checkR = cbRuntime.Checked;
                dgv.Columns["groupCalc"].Visible = checkR;
                dgv.Columns["groupStatus"].Visible = checkR;

                bool checkST = cbStatistic.Checked;
                dgv.Columns["groupTags"].Visible = checkST;
                dgv.Columns["groupStatistic"].Visible = checkST;

                bool checkSource = cbGroupSource.Checked;
                dgv.Columns["groupSource"].Visible = checkSource;
            }
            #endregion

            #region Filter
            static public void TextFilter()
            {
                string text = coFilterSource.Text;
                TableFilter(tbFilter.Text, dgv,
                    GetColumnIndexFilter(), GetPairFilter(text));
            }
            #endregion


            // Определение номеров колонок
            static public void LinkColumns(DataGridView groups)
            {
                dgv = groups;
                col = GroupLib.GetCols(dgv);
            }

            // Группы. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.Title, col.Desc, col.Status
                };
            }
            static public PairFilterCol[] GetPairFilter(string text)
            {
                return new PairFilterCol[]
                {
                new PairFilterCol { col = col.Source, filter = text }
                };
            }

        }

        #endregion

        #region Tag

        static public class dtTag
        {
            static public DGVTagsCol col = new DGVTagsCol(); // Номера колонок в DGV

            static public DataGridView dgv;

            static public CheckBox cbEditor;
            static public CheckBox cbDesc;
            static public CheckBox cbRuntime;
            static public CheckBox cbStatistic;
            static public CheckBox cbBP;
            static public CheckBox cbSG;
            static public CheckBox cbSave;
            static public CheckBox cbAddress;

            static public TextBox tbFilter;
            static public ComboBox coFilterSource;
            static public ComboBox coFilterGroup;
            static public ComboBox coFilterBlock;
            static public ComboBox coFilterPage;

            static private DataTable tagTable = new DataTable(); // DataTable для хранения данных тегов
            static private BindingSource bindingSource = new BindingSource(); // BindingSource для привязки данных к DataGridView

            static public void Clear()
            {
                tagTable.Rows.Clear();
            }

            #region DGV
            static public void DataToTable(List<TagEditor> tags)
            {
                TagLib.DataToTable(dgv, col, tagTable, bindingSource, tags);
            }
            static public List<TagEditor> TableToData()
            {
                return TagLib.TableToData(dgv, col);
            }
            #endregion


            #region DGV.Columns
            static public void CheckColumns()
            {
                bool checkE = cbEditor.Checked;
                dgv.Columns["tagID"].Visible = checkE;
                dgv.Columns["tagCommand"].Visible = checkE;
                dgv.RowHeadersVisible = checkE;
                dgv.ReadOnly = !checkE;

                bool checkD = cbDesc.Checked;
                dgv.Columns["tagDesc"].Visible = checkD;

                bool checkR = cbRuntime.Checked;
                dgv.Columns["tagCalc"].Visible = checkR;
                dgv.Columns["tagValue"].Visible = checkR;
                dgv.Columns["tagStatus"].Visible = checkR;
                dgv.Columns["tagMessage"].Visible = checkR;

                bool checkS = cbStatistic.Checked;
                dgv.Columns["tagStatistic"].Visible = checkS;

                bool checkBP = cbBP.Checked;
                dgv.Columns["tagBlock"].Visible = checkBP;
                dgv.Columns["tagPage"].Visible = checkBP;

                bool checkSG = cbSG.Checked;
                dgv.Columns["tagSource"].Visible = checkSG;
                dgv.Columns["tagGroup"].Visible = checkSG;

                bool checkSave = cbSave.Checked;
                dgv.Columns["tagWriteValue"].Visible = checkSave;
                dgv.Columns["tagWriteTag"].Visible = checkSave;

                bool checkAddress = cbAddress.Checked;
                dgv.Columns["tagAddress"].Visible = checkAddress;
            }
            #endregion

            #region Filter
            static public void TextFilter()
            {
                string text1 = coFilterSource.Text;
                string text2 = coFilterGroup.Text;
                string text3 = coFilterBlock.Text;
                string text4 = coFilterPage.Text;

                //TableFilter(tbFilter.Text, dgv, GetColumnIndexFilter(), GetPairFilter(text1, text2, text3, text4));
                DataTableFilter(tbFilter.Text, tagTable, dtTag.bindingSource, GetColumnIndexFilter(), GetPairFilter(text1, text2, text4), new PairFilterCol { col = col.Block, filter = text3 });

            }
            #endregion

            // Обновить связи тегов к источникам от групп
            public static void UpdateDGVTagSourceLink()
            {
                if (dgv == null || dtGroup.dgv == null)
                    return;

                if (col.Group < 0 || col.Source < 0 || dtGroup.col.Title < 0 || dtGroup.col.Source < 0)
                    return;

                // Создаем безопасный словарь с проверкой null
                var groupCollection = MyTree.SetTreeCollection(dtGroup.dgv, dtGroup.col.Title, dtGroup.col.Source);
                if (groupCollection == null || !groupCollection.Any())
                    return;

                var groupDict = groupCollection
                    .Where(g => !string.IsNullOrWhiteSpace(g.Title))
                    .ToDictionary(g => g.Title, g => g.Link);

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    var groupTitle = row.Cells[col.Group].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(groupTitle)) // Проверяем на null и пустоту
                    {
                        row.Cells[col.Source].Value = "";
                        continue;
                    }

                    row.Cells[col.Source].Value = groupDict.TryGetValue(groupTitle, out string sourceTitle) ? sourceTitle : "";
                }
            }



            // Определение номеров колонок
            static public void LinkColumns(DataGridView tags)
            {
                dgv = tags;
                col = new DGVTagsCol
                {
                    Runtime = dgv.Columns["tagCalc"].Index,
                    Title = dgv.Columns["tagTitle"].Index,
                    Value = dgv.Columns["tagValue"].Index,
                    DataType = dgv.Columns["tagDataType"].Index,
                    Address = dgv.Columns["tagAddress"].Index,
                    Desc = dgv.Columns["tagDesc"].Index,
                    Status = dgv.Columns["tagStatus"].Index,
                    Message = dgv.Columns["tagMessage"].Index,
                    Source = dgv.Columns["tagSource"].Index,
                    Group = dgv.Columns["tagGroup"].Index,
                    Block = dgv.Columns["tagBlock"].Index,
                    Page = dgv.Columns["tagPage"].Index
                };

            }

            // Теги. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.Title, col.Value, col.DataType, col.Address, col.Desc, col.Status, col.Message
                };
            }
            static public PairFilterCol[] GetPairFilter(string textSource, string textGroup, string textPage)
            {
                return new PairFilterCol[]
                {
                new PairFilterCol { col = col.Source, filter = textSource },
                new PairFilterCol { col = col.Group, filter = textGroup },
                //new PairFilterCol { col = col.Block, filter = textBlock },
                new PairFilterCol { col = col.Page, filter = textPage }
                };
            }

            // ---------
            static public void SetTagValue(DataTable dt, uint Id, dynamic value)
            {
                DataRow rowTag = dt.Rows.Find(Id);
                if (rowTag != null)
                    rowTag[dtTag.col.Value] = value;
            }

            static public void SetTagValue(DataRow rowTag, dynamic value)
            {
                if (rowTag != null)
                    rowTag[dtTag.col.Value] = value;
            }

            // Открыть справку по тегам
            public static void Help()
            {
                if (dgv == null || dgv.CurrentRow == null || dtSource.dgv == null)
                    return;

                if (col.Source < 0 || col.Source >= dgv.ColumnCount ||
                    dtSource.col.Title < 0 || dtSource.col.Driver < 0)
                    return;

                string sourceTitle = dgv.CurrentRow.Cells[col.Source].Value?.ToString();
                if (string.IsNullOrWhiteSpace(sourceTitle))
                    return;

                var sourceRow = dtSource.dgv.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(item => item.Cells[dtSource.col.Title].Value?.ToString() == sourceTitle);

                string driverTitle = sourceRow?.Cells[dtSource.col.Driver].Value?.ToString();
                if (string.IsNullOrWhiteSpace(driverTitle))
                    return;

                if (!Enum.TryParse(driverTitle, out eDriverType est))
                    return; // Если строка не соответствует enum, просто выходим

                var dic = SourceHelp.HelpDicTag(est);
                if (dic == null || dic.Count == 0)
                    return; // Не создаем окно, если данных нет

                var help = new FormHelp
                {
                    Text = $"Справка по тегам {driverTitle}",
                    dic = dic
                };

                help.Show();
            }

            #region Tags.AddRow

            static public DataRow AddRowForTags(DataTable dt, DataGridView dgv,
                uint Id, string title,
                string value,
                eDataType dataType, string address,
                string sourceTitle, string groupTitle,
                string getWriteCell,
                bool off,
                long code,
                string status,
                string comment,
                string description,
                string block)
            {
                DataRow row = dt.NewRow();

                //row[GetTitleFromName(dgv, "tagID")] = Id; // 
                //row[GetTitleFromName(dgv, "tagName")] = title; // 
                //row[GetTitleFromName(dgv, "tagValue")] = value; //
                //row[GetTitleFromName(dgv, "tagDataType")] = dataType; // 
                //row[GetTitleFromName(dgv, "tagAddress")] = address; // 
                //row[GetTitleFromName(dgv, "tagSource")] = sourceTitle; // 
                //row[GetTitleFromName(dgv, "tagGroup")] = groupTitle; // 
                //row[GetTitleFromName(dgv, "tagWrite")] = getWriteCell; // 
                //row[GetTitleFromName(dgv, "tagOff")] = off; // 
                //row[GetTitleFromName(dgv, "tagCode")] = code; // 
                //row[GetTitleFromName(dgv, "tagStatus")] = status; // 
                //row[GetTitleFromName(dgv, "tagComment")] = comment; // 
                //row[GetTitleFromName(dgv, "tagDescription")] = description; // 
                //row[GetTitleFromName(dgv, "tagBlock")] = block; // 

                row["tagID"] = Id; // 
                row["tagName"] = title; // 
                row["tagValue"] = value; //
                row["tagDataType"] = dataType; // 
                row["tagAddress"] = address; // 
                row["tagSource"] = sourceTitle; // 
                row["tagGroup"] = groupTitle; // 
                row["tagWrite"] = getWriteCell; // 
                row["tagOff"] = off; // 
                row["tagCode"] = code; // 
                row["tagStatus"] = status; // 
                row["tagComment"] = comment; // 
                row["tagDescription"] = description; // 
                row["tagBlock"] = block; // 

                dt.Rows.Add(row);
                return row;
            }

            static public DataRow AddRowForEditTags(DataTable dt, DataGridView dgv,
                uint Id, string title,
                string sourceTitle, string groupTitle,
                string dataType, string address,
                string description,
                bool off,
                bool isCommand,
                string writeTitle, string constValue,
                string block)
            {
                DataRow row = dt.NewRow();

                row["tagId"] = Id; // 
                row["tagTitle"] = title; // 
                row["tagSource"] = sourceTitle; // 
                row["tagGroup"] = groupTitle; // 
                row["tagTypeData"] = dataType; //
                row["tagAddress"] = address; //
                row["tagDescription"] = description; // 
                row["tagOff"] = off; // 
                row["tagCommand"] = isCommand; //
                row["tagWrite"] = writeTitle; // 
                row["tagConst"] = constValue; // 
                row["tagBlock"] = block; // 

                dt.Rows.Add(row);
                return row;
            }

            #endregion

        }

        #endregion

        #region Structure and Target

        static public class dtStructure
        {
            static public DGVStructureCol col = new DGVStructureCol(); // Номера колонок в DGV

            static public DataGridView dgv;

            static public TextBox tbFilter;

            #region Filter
            static public void StructureFilter()
            {
                TableFilter(tbFilter.Text, dgv,
                    GetColumnIndexFilter(), GetPairFilter());
            }
            #endregion

            #region DGV.Add
            static public void DataToTable(List<StructureEditor> structures)
            {
                // Таблица источников
                dgv.Rows.Clear();
                foreach (var item in structures)
                {
                    DataGridViewRow row = (DataGridViewRow)dgv.Rows[0].Clone();
                    row.Cells[0].Value = item.Id;

                    row.Cells[col.Title].Value = item.title;
                    row.Cells[col.Join].Value = item.join;
                    row.Cells[col.TemplateAddress].Value = item.templateAddress;
                    row.Cells[col.Group].Value = item.group;
                    row.Cells[col.DataType].Value = item.dataType.ToString();

                    // -
                    dgv.Rows.Add(row);
                }
            }
            #endregion

            // Определение номеров колонок
            static public void LinkColumns(DataGridView structures)
            {
                dgv = structures;
                col = new DGVStructureCol
                {
                    Title = dgv.Columns["structureTitle"].Index,
                    Join = dgv.Columns["structureConnector"].Index,
                    //TagSource = dgv.Columns["structureTagSource"].Index,
                    TemplateAddress = dgv.Columns["structureTemplate"].Index,
                    Group = dgv.Columns["structureGroup"].Index,
                    DataType = dgv.Columns["structureDataType"].Index
                };

            }

            // Структуры. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.Title, col.TemplateAddress, col.Group, col.Join
                };
            }
            static public PairFilterCol[] GetPairFilter()
            {
                return new PairFilterCol[] { };
            }

            
        }

        static public class dtStructTarget
        {
            static public DGVStructTargetCol col = new DGVStructTargetCol(); // Номера колонок в DGV

            static public DataGridView dgv;

            static public TextBox tbFilter;
            static public ComboBox coFilterParent;

            #region Filter
            static public void StructureTargetFilter()
            {
                string text = coFilterParent?.Text ?? "";

                TableFilter(tbFilter?.Text ?? "", dgv,
                    GetColumnIndexFilter(), GetPairFilter(text));
            }
            #endregion

            #region DGV.Add
            static public void DataToTable(List<StructTargetEditor> targets)
            {
                // Таблица источников
                dgv.Rows.Clear();
                foreach (var item in targets)
                {
                    DataGridViewRow row = (DataGridViewRow)dgv.Rows[0].Clone();
                    row.Cells[0].Value = item.Id;

                    row.Cells[col.Structure].Value = item.structureTitle;
                    row.Cells[col.InnerTitle].Value = item.title;
                    row.Cells[col.InnerAddress].Value = item.innerAddress;
                    row.Cells[col.Desc].Value = item.desc;

                    // -
                    dgv.Rows.Add(row);
                }
            }
            #endregion


            // Определение номеров колонок
            static public void LinkColumns(DataGridView targets)
            {
                dgv = targets;
                col = new DGVStructTargetCol
                {
                    Structure = dgv.Columns["targetStructure"].Index,
                    InnerAddress = dgv.Columns["targetAddress"].Index,
                    InnerTitle = dgv.Columns["targetTitle"].Index,
                    Desc = dgv.Columns["targetDesc"].Index
                };

            }

            // Цели. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.InnerTitle, col.Desc, col.InnerAddress
                };
            }
            static public PairFilterCol[] GetPairFilter(string textStructure)
            {
                return new PairFilterCol[]
                {
                new PairFilterCol { col = col.Structure, filter = textStructure }
                };
            }

        }

        static public class dtStructTag
        {
            static public DGVStructTagCol col = new DGVStructTagCol(); // Номера колонок в DGV
            static public DataGridView dgv;

            static public TextBox tbFilter;
            static public ComboBox coFilterParent;

            #region Filter
            static public void StructureTagFilter()
            {
                string text = coFilterParent?.Text ?? "";

                TableFilter(tbFilter?.Text ?? "", dgv,
                    GetColumnIndexFilter(), GetPairFilter(text));
            }
            #endregion

            #region DGV.Add
            static public void DataToTable(List<StructTagEditor> tags)
            {
                // Таблица источников
                dgv.Rows.Clear();
                foreach (var item in tags)
                {
                    DataGridViewRow row = (DataGridViewRow)dgv.Rows[0].Clone();
                    row.Cells[0].Value = item.Id;

                    row.Cells[col.Structure].Value = item.structureTitle;
                    row.Cells[col.TagTitle].Value = item.title;

                    // -
                    dgv.Rows.Add(row);
                }
            }
            #endregion

            // Определение номеров колонок
            static public void LinkColumns(DataGridView targets)
            {
                dgv = targets;
                col = new DGVStructTagCol
                {
                    Structure = dgv.Columns["targetTagStruct"].Index,
                    TagTitle = dgv.Columns["targetTagTitle"].Index,
                };

            }

            // Теги структур. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.TagTitle
                };
            }
            static public PairFilterCol[] GetPairFilter(string textStructure)
            {
                return new PairFilterCol[]
                {
                new PairFilterCol { col = col.Structure, filter = textStructure }
                };
            }

        }

        #endregion

        #region Include and Change

        static public class dtInclude
        {
            static public DGVIncludeCol col = new DGVIncludeCol(); // Номера колонок в DGV

            static public DataGridView dgv;

            static public TextBox tbFilter;

            #region Filter
            static public void IncludeFilter()
            {
                TableFilter(tbFilter.Text, dgv,
                    GetColumnIndexFilter(), GetPairFilter());
            }
            #endregion

            #region DGV.Add
            static public void DataToTable(List<IncludeEditor> includes)
            {
                // Таблица источников
                dgv.Rows.Clear();
                foreach (var item in includes)
                {
                    DataGridViewRow row = (DataGridViewRow)dgv.Rows[0].Clone();
                    row.Cells[0].Value = item.Id;

                    row.Cells[col.Prefix].Value = item.prefix;
                    row.Cells[col.FileName].Value = item.fileName;

                    // -
                    dgv.Rows.Add(row);
                }
            }
            #endregion

            // Определение номеров колонок
            static public void LinkColumns(DataGridView includes)
            {
                dgv = includes;
                col = new DGVIncludeCol
                {
                    Prefix = dgv.Columns["includePrefix"].Index,
                    FileName = dgv.Columns["includeFileName"].Index
                };
            }

            // Классы. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.Prefix, col.FileName
                };
            }
            static public PairFilterCol[] GetPairFilter()
            {
                return new PairFilterCol[] { };
            }

            
        }

        static public class dtIncludeChild
        {
            static public DGVChangeCol col = new DGVChangeCol(); // Номера колонок в DGV

            static public DataGridView dgv;

            static public TextBox tbFilter;
            static public ComboBox coFilterParent;

            #region Filter
            static public void IncludeChildFilter()
            {
                string text = coFilterParent.Text;

                TableFilter(tbFilter.Text, dgv,
                    GetColumnIndexFilter(), GetPairFilter(text));
            }
            #endregion

            #region DGV.Add
            static public void DataToTable(List<IncludeChildEditor> includeChilds)
            {
                // Таблица источников
                dgv.Rows.Clear();
                foreach (var item in includeChilds)
                {
                    DataGridViewRow row = (DataGridViewRow)dgv.Rows[0].Clone();
                    row.Cells[0].Value = item.Id;

                    row.Cells[col.Prefix].Value = item.prefix;
                    row.Cells[col.ChangeFrom].Value = item.changeFrom;
                    row.Cells[col.ChangeTo].Value = item.changeTo;

                    // -
                    dgv.Rows.Add(row);
                }
            }
            #endregion

            // Определение номеров колонок
            static public void LinkColumns(DataGridView changes)
            {
                dgv = changes;
                col = new DGVChangeCol
                {
                    Prefix = dgv.Columns["changePrefix"].Index,
                    ChangeFrom = dgv.Columns["changeFrom"].Index,
                    ChangeTo = dgv.Columns["changeTo"].Index
                };
            }

            // Замены. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.Prefix, col.ChangeFrom, col.ChangeTo
                };
            }
            static public PairFilterCol[] GetPairFilter(string text)
            {
                return new PairFilterCol[]
                {
                new PairFilterCol { col = col.Prefix, filter = text }
                };
            }
        }

        #endregion

        #region LIB

        #region GET

        #region Header
        // Получить заголовок столбца по имени
        static public string GetTitleFromName(DataGridView dgv, string name)
        {
            return dgv.Columns[name].HeaderText;
        }
        // Получить тия столбца по заголовку
        static public string GetNameFromTitle(DataGridView dgv, string title)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.HeaderText == title)
                    return col.Name;
            }
            return "";
        }
        #endregion

        // Получить строку DataGridView по DataRow
        public static DataGridViewRow GetRowDGV(DataGridView dgv, DataRow row)
        {
            if (row == null || row.ItemArray.Length == 0) return null;
            return GetRowDGV(dgv, row[0]);
        }

        // Получить строку DataGridView по ID (объединение двух методов)
        public static DataGridViewRow GetRowDGV(DataGridView dgv, object id)
        {
            if (dgv == null || id == null) return null;
            return dgv.Rows
                      .Cast<DataGridViewRow>()
                      .FirstOrDefault(r => r.Cells[0].Value != null && r.Cells[0].Value.Equals(id));
        }

        // Получить строку DataTable по ID
        public static DataRow GetDTRow(DataTable dt, uint id)
        {
            return dt?.Rows.Find(id);
        }

        // Получить значение из текущей строки DataGridView
        public static string GetValueFromCurrentRow(DataGridView dgv, int col)
        {
            if (dgv?.CurrentRow?.Cells[col]?.Value == null || dgv.CurrentRow.IsNewRow)
                return string.Empty;

            return dgv.CurrentRow.Cells[col].Value.ToString();
        }

        // Получить пустую DataTable с колонками из DataGridView
        public static DataTable GetEmptyDataTableForTags(DataGridView dgv, string name)
        {
            if (dgv == null || dgv.ColumnCount == 0)
                throw new ArgumentException("DataGridView пуст или не инициализирован.", nameof(dgv));

            DataTable table = new DataTable(name);

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                var dataColumn = new DataColumn(column.Name)
                {
                    DataType = column.ValueType ?? typeof(string) // Используем тип столбца из DGV или `string`
                };
                table.Columns.Add(dataColumn);

                // Устанавливаем `DataPropertyName` только если он отличается
                if (column.DataPropertyName != column.Name)
                    column.DataPropertyName = column.Name;
            }

            return table;
        }


        // Получить строку по ID
        public static DataGridViewRow GetRowByID(DataGridView dgv, int id)
        {
            if (dgv == null || dgv.RowCount == 0) return null;

            var result = dgv.Rows
                      .Cast<DataGridViewRow>()
                      .FirstOrDefault(row => !row.IsNewRow &&
                       int.TryParse(row.Cells[0].Value?.ToString(), out int cellValue) &&
                       cellValue == id);

            return result;
        }

        // Получить строку DataGridView по названию (колонка после ID)
        public static DataGridViewRow GetRowByTitle(DataGridView dgv, string title, int indexTitle)
        {
            if (dgv == null || dgv.RowCount == 0 || string.IsNullOrWhiteSpace(title) || indexTitle < 0 || indexTitle >= dgv.ColumnCount)
                return null;

            return dgv.Rows
                      .Cast<DataGridViewRow>()
                      .FirstOrDefault(row => !row.IsNewRow &&
                                             row.Cells[indexTitle].Value != null &&
                                             !(row.Cells[indexTitle].Value is DBNull) &&
                                             row.Cells[indexTitle].Value.ToString().Trim().Equals(title.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // Получить ID из выделенной строки DataGridView
        public static uint GetSelIdFromTable(object senderDGV)
        {
            if (senderDGV is DataGridView dgv && dgv.SelectedRows.Count > 0)
            {
                var value = dgv.SelectedRows[0].Cells[0].Value;
                return value is uint id ? id : Convert.ToUInt32(value ?? 0);
            }
            return 0;
        }

        // Получить выбранную строку DataGridView
        public static DataGridViewRow GetSelRow(DataGridView dgv)
        {
            return dgv?.SelectedRows.Count > 0 ? dgv.SelectedRows[0] : null;
        }

        // Получить следующий доступный ID из DataGridView
        public static uint GetNextID(DataGridView dgv)
        {
            if (dgv == null || dgv.RowCount == 0) return 1; // Если таблица пуста, начинаем с 1

            var maxId = dgv.Rows
                           .Cast<DataGridViewRow>()
                           .Where(row => !row.IsNewRow && row.Cells[0].Value is uint)
                           .Select(row => (uint)row.Cells[0].Value)
                           .DefaultIfEmpty((uint)0) // Указываем явный тип `uint`
                           .Max();

            return (uint)(maxId + 1);
        }
        #endregion


        #region SET

        // Устанавливает значения в строку DataTable по ID
        public static void SetValues(DataTable dt, uint Id, ColumnValue[] cv)
        {
            if (dt?.Rows == null || cv == null || cv.Length == 0) return;

            DataRow row = dt.Rows.Find(Id);
            row?.BeginEdit();

            foreach (var item in cv)
            {
                if (row?.Table.Columns.Count > item.column) // Проверка, чтобы избежать выхода за границы массива
                    row[item.column] = item.value;
            }

            row?.EndEdit();
        }

        // Устанавливает значение в строку DataTable по ID
        public static void SetValue(DataTable dt, uint Id, int column, dynamic value)
        {
            if (dt?.Rows == null || column < 0 || column >= dt.Columns.Count) return;

            DataRow row = dt.Rows.Find(Id);
            if (row == null) return;

            row.BeginEdit();
            row[column] = value;
            row.EndEdit();
        }

        // Устанавливает значение в строку DataRow по индексу столбца
        public static void SetValue(DataRow row, int column, dynamic value)
        {
            if (row == null || column < 0 || column >= row.Table.Columns.Count) return;

            row.BeginEdit();
            row[column] = value;
            row.EndEdit();
        }

        // Добавление ссылки на родительский элемент в DataGridView
        public static void SetParentInRow(DataGridView dgv, ComboBox cb, int colParentTitle)
        {
            if (dgv == null || cb == null || dgv.RowCount == 0 || string.IsNullOrWhiteSpace(cb.Text))
                return;

            var row = dgv.CurrentRow;
            if (row == null || colParentTitle < 0 || colParentTitle >= dgv.ColumnCount)
                return;

            row.Cells[colParentTitle].Value = cb.Text;
        }

        // Расставить количества элементов
        public static void SetCountTagForUsed(DataGridView dgvTarget, DataGridView dgvList,
            int colTargetTitle, int colTargetCount, 
            int colUsed)
        {
            if (dgvTarget == null || dgvList == null || dgvTarget.RowCount == 0 || dgvList.RowCount == 0)
                return;

            if (colTargetTitle < 0 || colTargetTitle >= dgvTarget.ColumnCount || colTargetCount < 0 || colTargetCount >= dgvTarget.ColumnCount)
                return;

            var dic = GetDicForUsed(dgvList, colUsed);

            foreach (DataGridViewRow item in dgvTarget.Rows)
            {
                var itemTitle = item.Cells[colTargetTitle].Value?.ToString();
                if (string.IsNullOrWhiteSpace(itemTitle))
                    continue;

                dic.TryGetValue(itemTitle, out int count); // Оптимальный способ извлечения из `Dictionary`

                var currentValue = item.Cells[colTargetCount].Value as int?;
                if (currentValue == null || currentValue != count)
                {
                    item.Cells[colTargetCount].Value = count;
                }
            }
        }

        #endregion


        #region Show

        // Показать строку в таблице
        static public void ShowRow(DataGridView dgv, DataGridViewRow row)
        {
            row.Visible = true; // показываем столбец даже если он скрыт
            row.Selected = true;
            dgv.FirstDisplayedScrollingRowIndex = row.Index;
        }
        static public void ShowRow(DataGridView dgv, int Id, string title, int indexTitle)
        {
            if (Id > 0)
            {
                var row = GetRowByID(dgv, Id);
                if (row == null)
                    return;
                ShowRow(dgv, row);
            }
            else if (title != "" && indexTitle > 0)
            {
                var row = GetRowByTitle(dgv, title, indexTitle);
                if (row == null)
                    return;
                ShowRow(dgv, row);
            }
        }
        #endregion


        #region Table Filter [1 DataGridView]
        static public void TableFilter(string FilterText, DataGridView dgv, int[] cells, PairFilterCol[] pairs)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;
                row.Visible = (String.IsNullOrWhiteSpace(FilterText) || CellsContainsFilterA(row, cells, FilterText)) && CellsContainsFilterB(row, pairs);
            }
        }
        static public bool CellsContainsFilterB(DataGridViewRow row, PairFilterCol[] pairs)
        {
            bool result = true;
            foreach (var item in pairs)
            {
                result = result && (String.IsNullOrWhiteSpace(item.filter) || (row.Cells[item.col].Value != null && row.Cells[item.col].Value.ToString() == (item.filter)));
            }
            return result;
        }
        // Несколько фильтров для одной ячейки
        static public bool CellContainsFilters(DataGridViewRow row, int indexCell, string[] filter)
        {
            foreach (var item in filter)
            {
                if (CellContainsFilter(row, indexCell, item))
                    return true;
            }
            return false;
        }

        // Один фильтр для нескольких ячеек
        static public bool CellsContainsFilterA(DataGridViewRow row, int[] indexCells, string filter)
        {
            foreach (var index in indexCells)
            {
                if (CellContainsFilter(row, index, filter))
                    return true;
            }
            return false;
        }

        // Один фильтр для одной ячейки
        static public bool CellContainsFilter(DataGridViewRow row, int indexCell, string filter)
        {
            if (row == null || row.IsNewRow || indexCell < 0 || String.IsNullOrWhiteSpace(filter))
                return false;
            return (row.Cells[indexCell].Value == null) ? false : row.Cells[indexCell].Value.ToString().Contains(filter);
        }

        // Замена для нескольких ячеек
        static public int ReplaceFilter(DataGridViewRow row, int[] indexCells, string ValueFrom, string ValueTo)
        {
            int i = 0;
            foreach (var index in indexCells)
            {
                i += ReplaceFilter(row, index, ValueFrom, ValueTo);
            }
            return i;
        }

        // Замена для одной ячейки
        static public int ReplaceFilter(DataGridViewRow row, int indexCell, string ValueFrom, string ValueTo)
        {
            if (row == null || indexCell < 0 || String.IsNullOrWhiteSpace(ValueFrom))
                return 0;

            if (row.Cells[indexCell].Value == null)
                return 0;

            if (row.Cells[indexCell].Value.ToString().Contains(ValueFrom) == false)
                return 0;

            row.Cells[indexCell].Value = row.Cells[indexCell].Value.ToString().Replace(ValueFrom, ValueTo);
            return 1;
        }



        #endregion

        #region Table Filter [2 DataTable]

        /// <summary>
        /// Фильтрует данные в DataTable через BindingSource, используя текстовый фильтр и пары "колонка-значение".
        /// </summary>
        /// <param name="midText">Текстовый фильтр для поиска в нескольких колонках.</param>
        /// <param name="bindingSource">BindingSource, привязанный к DataTable.</param>
        /// <param name="columnIndices">Индексы колонок, в которых выполняется поиск по текстовому фильтру.</param>
        /// <param name="equalPairs">Пары "индекс колонки - фильтр", которые должны точно соответствовать значениям в таблице.</param>
        public static void DataTableFilter(string midText, DataTable dt, BindingSource bindingSource, int[] columnIndices, PairFilterCol[] equalPairs, PairFilterCol blockPair)
        {
            if (bindingSource == null)
                return;

            var filters = new List<string>();

            // 1. Добавляем текстовый фильтр для указанных колонок по индексам
            if (!string.IsNullOrWhiteSpace(midText) && columnIndices?.Length > 0)
            {
                var textFilters = columnIndices
                    .Where(index => index >= 0 && index < dt.Columns.Count)
                    .Select(index => $"CONVERT([{dt.Columns[index].ColumnName}], System.String) LIKE '%{midText.Replace("'", "''")}%'")
                    .ToArray();

                if (textFilters.Length > 0)
                {
                    filters.Add($"({string.Join(" OR ", textFilters)})");
                }
            }

            // 2. Добавляем точные фильтры по парам "индекс колонки - значение"
            if (equalPairs?.Length > 0)
            {
                foreach (var pair in equalPairs)
                {
                    if (!string.IsNullOrWhiteSpace(pair.filter) &&
                        pair.col >= 0 && pair.col < dt.Columns.Count)
                    {
                        string columnName = dt.Columns[pair.col].ColumnName;
                        filters.Add($"[{columnName}] = '{pair.filter.Replace("'", "''")}'");
                    }
                }
            }

            // 4.1. Добавляем фильтры блоков (title.???) по парам "индекс колонки - значение"
            {

                if (!string.IsNullOrWhiteSpace(blockPair.filter) &&
                    blockPair.col >= 0 && blockPair.col < dt.Columns.Count)
                {
                    string columnName = dt.Columns[blockPair.col].ColumnName;
                    filters.Add($"( [{columnName}] = '{blockPair.filter.Replace("'", "''")}' OR CONVERT([{columnName}], System.String) LIKE '{blockPair.filter.Replace("'", "''")}.%' )");
                }

            }

            // 4. Объединяем все фильтры в одно выражение
            string filterExpression = string.Join(" AND ", filters);

            // 5. Применяем фильтрацию к BindingSource
            bindingSource.Filter = filterExpression;
        }

        #endregion


        // Привязка DataTable к DataGridView через BindingSource
        public static void LinkDatatTable(DataTable dt, BindingSource bind, DataGridView dgv)
        {
            if (dt == null || bind == null || dgv == null)
                return;

            bind.DataSource = dt;
            dgv.DataSource = bind;
            dgv.AutoGenerateColumns = false;

            if (dgv.InvokeRequired)
                dgv.Invoke(new MethodInvoker(dgv.Refresh));
            else
                dgv.Refresh();
        }

        // Клонировать строку DataGridView с копированием значений
        public static DataGridViewRow CloneRowWithValues(DataGridViewRow row)
        {
            if (row == null)
                return null;

            var clonedRow = (DataGridViewRow)row.Clone();

            for (int i = 0; i < Math.Min(row.Cells.Count, clonedRow.Cells.Count); i++)
            {
                clonedRow.Cells[i].Value = row.Cells[i].Value;
            }

            return clonedRow;
        }

        // Копирует выбранную строку в DataGridView с новым ID
        public static void CopyDGVRow(DataGridView dgv, int colTitle)
        {
            if (dgv == null || dgv.RowCount == 0 || colTitle < 0 || colTitle >= dgv.ColumnCount)
                return;

            var row = GetSelRow(dgv);
            if (row == null)
                return;

            var newRow = CloneRowWithValues(row);
            if (newRow == null)
                return;

            var nextID = GetNextID(dgv);
            newRow.Cells[0].Value = nextID;

            var titleValue = row.Cells[colTitle].Value?.ToString();
            newRow.Cells[colTitle].Value = string.IsNullOrWhiteSpace(titleValue) ? $"ID{nextID}" : $"{titleValue}_ID{nextID}";

            dgv.Rows.Add(newRow);
        }

        // Удаление текущей строки
        static public void DelDGVRow(DataGridView dgv)
        {
            var row = GetSelRow(dgv);
            if (row != null && row.IsNewRow == false)
            {
                dgv.Rows.Remove(row);
            }
        }

        // При появлении новой строки таблицы
        static public void ForNewRow(DataGridView dgv)
        {
            var newID = DataTableLib.GetNextID(dgv);
            var row = dgv.CurrentRow;
            if (row != null)
                row.Cells[0].Value = newID;
        }


        // Получить словарь с количеством повторений значений в указанной колонке DataGridView
        public static Dictionary<string, int> GetDicForUsed(DataGridView dgv, int indexCol)
        {
            if (dgv == null || dgv.RowCount == 0 || indexCol < 0 || indexCol >= dgv.ColumnCount)
                return new Dictionary<string, int>();

            var dic = new Dictionary<string, int>();

            foreach (DataGridViewRow item in dgv.Rows)
            {
                if (item.IsNewRow) continue; // Пропускаем новую строку

                var cellValue = item.Cells[indexCol].Value;
                if (cellValue == null || cellValue == DBNull.Value) continue;

                string name = cellValue.ToString();
                if (dic.TryGetValue(name, out int count))
                {
                    dic[name] = count + 1;
                }
                else
                {
                    dic[name] = 1;
                }
            }

            return dic;
        }




        #endregion

        #region GPT

        // 🔹 Проверяет, выполняется ли код в UI-потоке
        private static void EnsureUIThread(Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }

        #endregion

    }

}
