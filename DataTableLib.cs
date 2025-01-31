using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsFormsIDevice.Connector.SGT;

namespace WinSimpleIDriver
{
    #region DGV
    public struct DGVSourcesCol
    {
        public int Calc;
        public int Title;
        public int Driver;
        public int Address;
        public int Desc;
        public int Status;
        public int Message;
    }
    public struct DGVGroupsCol
    {
        public int Calc;
        public int Title;
        public int Source;
        public int Desc;
        public int Status;
    }
    public struct DGVTagsCol
    {
        public int Calc;
        public int Title;
        public int Value;
        public int DataType;
        public int Address;
        public int Desc;
        public int Status;
        public int Message;
        public int Source;
        public int Group;
        public int Block;
        public int Page;
    }


    public struct DGVStructureCol
    {
        public int Title;
        public int Connector;
        public int TagSource;
        public int Template;
        public int Group;
    }
    public struct DGVTargetCol
    {
        public int Structure;
        public int Address;
        public int Tag;
        public int Desc;
    }


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

    #region Class Tree
    public enum TreeProjCategory
    {
        sources, // Источники
        sourceItem, // Источник
        groupItem, // Группа
        tagItem, // Тег
        structures, // Структуры
        targetItem, // Элементы структуры
        includes, // Классы
        changeItem, // Элементы класса
        blocks // Блоки
    }
    public class TreeProjTag
    {
        public TreeProjCategory category;
        public ushort Id;
        public TreeProjTag(TreeProjCategory category, ushort Id)
        {
            this.category = category;
            this.Id = Id;
        }
    }
    #endregion

    public struct TableIdentity
    {
        public ushort Id;
        public string Title;
    }

    class DataTableLib
    {
        // Номера колонок в DGV
        static public DGVSourcesCol sourcesCol = new DGVSourcesCol();
        static public DGVGroupsCol groupsCol = new DGVGroupsCol();
        static public DGVTagsCol tagsCol = new DGVTagsCol();
        static public DGVStructureCol structureCol = new DGVStructureCol();
        static public DGVTargetCol targetCol = new DGVTargetCol();
        static public DGVIncludeCol includeCol = new DGVIncludeCol();
        static public DGVChangeCol changeCol = new DGVChangeCol();
        
        // Определение номеров колонок
        static public void SetDGVColumns(DataGridView sources, DataGridView groups, DataGridView tags, 
                                            DataGridView includes, DataGridView changes,
                                            DataGridView structures, DataGridView targets)
        {
            sourcesCol = new DGVSourcesCol 
            {
                Calc = sources.Columns["sourceCalc"].Index,
                Title = sources.Columns["sourceTitle"].Index,
                Driver = sources.Columns["sourceDriver"].Index,
                Address = sources.Columns["sourceAddress"].Index,
                Desc = sources.Columns["sourceDesc"].Index,
                Status = sources.Columns["sourceStatus"].Index,
                Message = sources.Columns["sourceMessage"].Index
            };

            groupsCol = new DGVGroupsCol
            {
                Calc = groups.Columns["groupCalc"].Index,
                Title = groups.Columns["groupTitle"].Index,
                Source = groups.Columns["groupSource"].Index,
                Desc = groups.Columns["groupDesc"].Index,
                Status = groups.Columns["groupStatus"].Index
            };

            tagsCol = new DGVTagsCol
            {
                Calc = tags.Columns["tagCalc"].Index,
                Title = tags.Columns["tagTitle"].Index,
                Value = tags.Columns["tagValue"].Index,
                DataType = tags.Columns["tagDataType"].Index,
                Address = tags.Columns["tagAddress"].Index,
                Desc = tags.Columns["tagDesc"].Index,
                Status = tags.Columns["tagStatus"].Index,
                Message = tags.Columns["tagMessage"].Index,
                Source = tags.Columns["tagSource"].Index,
                Group = tags.Columns["tagGroup"].Index,
                Block = tags.Columns["tagBlock"].Index,
                Page = tags.Columns["tagPage"].Index
            };

            // ---

            structureCol = new DGVStructureCol
            {
                 Title = structures.Columns["structureTitle"].Index,
                 Connector = structures.Columns["structureConnector"].Index,
                 TagSource = structures.Columns["structureTagSource"].Index,
                 Template = structures.Columns["structureTemplate"].Index,
                 Group = structures.Columns["structureGroup"].Index
            };

            targetCol = new DGVTargetCol
            {
                 Structure = targets.Columns["targetStructure"].Index,
                 Address = targets.Columns["targetAddress"].Index,
                 Tag = targets.Columns["targetTitle"].Index,
                 Desc = targets.Columns["targetDesc"].Index
            };

            // ---

            includeCol = new DGVIncludeCol
            {
                Prefix = includes.Columns["includePrefix"].Index,
                FileName = includes.Columns["includeFileName"].Index
            };

            changeCol = new DGVChangeCol
            {
                Prefix = changes.Columns["changePrefix"].Index,
                ChangeFrom = changes.Columns["changeFrom"].Index,
                ChangeTo = changes.Columns["changeTo"].Index
            };

        }

        // Источники. Номера колонок для фильтра в массив
        static public int[] GetColumnIndexFilterSource()
        {
            return new int[]
            {
                sourcesCol.Title, sourcesCol.Driver, sourcesCol.Address, sourcesCol.Desc, sourcesCol.Status, sourcesCol.Message
            };
        }
        static public PairFilterCol[] GetPairFilterSource()
        {
            return new PairFilterCol[] { };
        }

        // Группы. Номера колонок для фильтра в массив
        static public int[] GetColumnIndexFilterGroup()
        {
            return new int[]
            {
                groupsCol.Title, groupsCol.Desc, groupsCol.Status
            };
        }
        static public PairFilterCol[] GetPairFilterGroup(string text)
        {
            return new PairFilterCol[] 
            { 
                new PairFilterCol { col = groupsCol.Source, filter = text }
            };
        }

        // Теги. Номера колонок для фильтра в массив
        static public int[] GetColumnIndexFilterTag()
        {
            return new int[]
            {
                tagsCol.Title, tagsCol.Value, tagsCol.DataType, tagsCol.Address, tagsCol.Desc, tagsCol.Status, tagsCol.Message
            };
        }
        static public PairFilterCol[] GetPairFilterTag(string textSource, string textGroup, string textBlock, string textPage)
        {
            return new PairFilterCol[]
            {
                new PairFilterCol { col = tagsCol.Source, filter = textSource },
                new PairFilterCol { col = tagsCol.Group, filter = textGroup },
                new PairFilterCol { col = tagsCol.Block, filter = textBlock },
                new PairFilterCol { col = tagsCol.Page, filter = textPage }
            };
        }

        // Структуры. Номера колонок для фильтра в массив
        static public int[] GetColumnIndexFilterStructure()
        {
            return new int[]
            {
                structureCol.Title, structureCol.Template, structureCol.TagSource, structureCol.Group, structureCol.Connector
            };
        }
        static public PairFilterCol[] GetPairFilterStructure()
        {
            return new PairFilterCol[] { };
        }

        // Цели. Номера колонок для фильтра в массив
        static public int[] GetColumnIndexFilterTarget()
        {
            return new int[]
            {
                targetCol.Tag, targetCol.Desc, targetCol.Address
            };
        }
        static public PairFilterCol[] GetPairFilterTarget(string textStructure)
        {
            return new PairFilterCol[]
            {
                new PairFilterCol { col = targetCol.Structure, filter = textStructure }
            };
        }


        // Классы. Номера колонок для фильтра в массив
        static public int[] GetColumnIndexFilterInclude()
        {
            return new int[]
            {
                includeCol.Prefix, includeCol.FileName
            };
        }
        static public PairFilterCol[] GetPairFilterInclude()
        {
            return new PairFilterCol[] { };
        }

        // Замены. Номера колонок для фильтра в массив
        static public int[] GetColumnIndexFilterChange()
        {
            return new int[]
            {
                changeCol.Prefix, changeCol.ChangeFrom, changeCol.ChangeTo
            };
        }
        static public PairFilterCol[] GetPairFilterChange(string text)
        {
            return new PairFilterCol[]
            {
                new PairFilterCol { col = changeCol.Prefix, filter = text }
            };
        }


        //
        static public DataGridViewRow GetRowDGV(DataGridView dgv, DataRow row)
        {
            var Id = Convert.ToString(row[0]);
            return dgv.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => r.Cells[0].Value.ToString() == Id)
                        .First();
        }

        static public DataGridViewRow GetRowDGV(DataGridView dgv, ushort Id)
        {
            var _Id = Convert.ToString(Id);
            return dgv.Rows
                       .Cast<DataGridViewRow>()
                       .Where(r => r.Cells[0].Value.ToString() == _Id)
                       .First();
        }

        static public DataRow GetDTRow(DataTable dt, uint Id)
        {
            DataRow rowTag = dt.Rows.Find(Id);
            return rowTag;
        }

        static public void SetValue(DataTable dt, uint Id, dynamic value)
        {
            DataRow rowTag = dt.Rows.Find(Id);
            if (rowTag != null)
                rowTag[tagsCol.Value] = value;
        }

        static public void SetValue(DataRow rowTag, dynamic value)
        {
            if (rowTag != null)
                rowTag[tagsCol.Value] = value;
        }

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

        static public void SetValue(DataTable dt, uint Id, ColumnValue[] cv)
        {
            DataRow row = dt.Rows.Find(Id);
            if (row != null)
            {
                foreach (var item in cv)
                {
                    row[item.column] = item.value;
                }
            }
        }

        static public void SetValue(DataTable dt, uint Id, int column, dynamic value)
        {
            DataRow row = dt.Rows.Find(Id);
            if (row != null)
                row[column] = value;
        }

        static public void SetValue(DataRow row, int column, dynamic value)
        {
            if (row != null)
                row[column] = value;
        }



        static public string GetTitleFromName(DataGridView dgv, string name)
        {
            return dgv.Columns[name].HeaderText;
        }

        static public string GetNameFromTitle(DataGridView dgv, string title)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.HeaderText == title)
                    return col.Name;
            }
            return "";
        }

        static public DataTable GetEmptyDataTableForTags(DataGridView dgv, string name)
        {
            DataTable table = new DataTable(name);

            for (int i = 0; i < dgv.ColumnCount; ++i)
            {
                table.Columns.Add(new DataColumn(dgv.Columns[i].Name));
                dgv.Columns[i].DataPropertyName = dgv.Columns[i].Name;
            }
            table.Columns[0].DataType = typeof(int);

            return table;
        }


        static public void LinkDatatTable(DataTable dt, BindingSource bind, DataGridView dgv)
        {

            // v2
            bind.DataSource = dt;
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = bind;
            dgv.Refresh();

        }

        #region Tags

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

        // ================================================================================

        static public void TableFilter(string FilterText, DataGridView dgv, int[] cells, PairFilterCol[] pairs)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;
                row.Visible = ( String.IsNullOrWhiteSpace(FilterText) || CellsContainsFilterA(row, cells, FilterText)) && CellsContainsFilterB(row, pairs);
            }
        }

        public struct PairFilterCol
        {
            public int col;
            public string filter;
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

        static public ushort GetSelIdFromTable(object senderDGV)
        {
            ushort Id = 0;
            DataGridView dgv = senderDGV as DataGridView;
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgv.SelectedRows[0];
                if (row != null)
                {
                    Id = ushort.Parse(row.Cells[0].Value.ToString());
                }
            }
            return Id;
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

        // Получить значение из выбранной строки
        static public string GetValueFromCurrentRow(DataGridView dgv, int col)
        {
            var row = dgv.CurrentRow;
            if (row == null)
                return "";

            if (row.IsNewRow)
                return "";

            var value = row.Cells[col].Value;
            if (value == null)
                return "";

            return value.ToString();
        }

        // ==============================================================================

        // Получить максимальный ID из таблицы
        static public ushort GetNewID(DataGridView dgv)
        {
            ushort newID = 0;
            foreach (DataGridViewRow item in dgv.Rows)
            {
                if (item.IsNewRow)
                    continue;

                if (item.Cells[0].Value == DBNull.Value)
                    continue;

                ushort id = Convert.ToUInt16(item.Cells[0].Value);
                if (id > newID)
                    newID = id;
            }
            return ++newID;
        }

        // Копировать строку таблицы
        static public DataGridViewRow CloneWithValues(DataGridViewRow row)
        {
            DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
            for (Int32 index = 0; index < row.Cells.Count; index++)
            {
                clonedRow.Cells[index].Value = row.Cells[index].Value;
            }
            return clonedRow;
        }

    }
}
