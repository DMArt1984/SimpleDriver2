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
        public int Status;
        public int Message;
    }
    public struct DGVGroupsCol
    {
        public int Title;
        public int Status;
    }
    public struct DGVTagsCol
    {
        public int Title;
        public int Value;
        public int Status;
        public int Message;

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
                Status = sources.Columns["sourceStatus"].Index,
                Message = sources.Columns["sourceMessage"].Index
            };

            groupsCol = new DGVGroupsCol
            {
                Title = groups.Columns["groupTitle"].Index,
                Status = groups.Columns["groupStatus"].Index
            };

            tagsCol = new DGVTagsCol
            {
                 Title = tags.Columns["tagTitle"].Index,
                 Value = tags.Columns["tagValue"].Index,
                 Status = tags.Columns["tagStatus"].Index,
                 Message = tags.Columns["tagMessage"].Index
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
    }
}
