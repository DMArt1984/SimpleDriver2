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
    public struct DGVSourcesCol
    {
        public int Title;
        public int Driver;
        public int Address;
        public int Desc;
        public int Status;
        public int Message;
    }
    public struct DGVGroupsCol
    {
        public int Title;
        public int Source;
        public int Desc;
        public int Status;
    }
    public struct DGVTagsCol
    {
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

    class DataTableLib
    {
        // Номера колонок в DGV
        static public DGVSourcesCol sourcesCol = new DGVSourcesCol();
        static public DGVGroupsCol groupsCol = new DGVGroupsCol();
        static public DGVTagsCol tagsCol = new DGVTagsCol();

        // Определение номеров колонок
        static public void SetDGVColumns(DataGridView sources, DataGridView groups, DataGridView tags)
        {
            sourcesCol = new DGVSourcesCol 
            { 
                Title = sources.Columns["sourceTitle"].Index,
                Driver = sources.Columns["sourceDriver"].Index,
                Address = sources.Columns["sourceAddress"].Index,
                Desc = sources.Columns["sourceDesc"].Index,
                Status = sources.Columns["sourceStatus"].Index,
                Message = sources.Columns["sourceMessage"].Index
            };

            groupsCol = new DGVGroupsCol
            {
                Title = groups.Columns["groupTitle"].Index,
                Source = groups.Columns["groupSource"].Index,
                Desc = groups.Columns["groupDesc"].Index,
                Status = groups.Columns["groupStatus"].Index
            };

            tagsCol = new DGVTagsCol
            {
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
        }

        // Номера колонок для фильтра в массив
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

        // Номера колонок для фильтра в массив
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

        // Номера колонок для фильтра в массив
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
                row.Visible = ( String.IsNullOrWhiteSpace(FilterText) || CellsContainsFilterA(row, cells, FilterText) || CellsContainsFilterB(row, pairs));
            }
        }

        public struct PairFilterCol
        {
            public int col;
            public string filter;
        }

        static public bool CellsContainsFilterB(DataGridViewRow row, PairFilterCol[] pairs)
        {
            foreach (var item in pairs)
            {
                if (row.Cells[item.col].Value.ToString().Contains(item.filter))
                    return true;
            }
            return false;
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

    }
}
