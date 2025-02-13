using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinSimpleIDriver.Connector.SGT;

namespace WinSimpleIDriver
{
    
    public struct TableIdentity
    {
        public ushort Id;
        public string Title;
        public string Link; // Ссылка на уровень выше
    }

    static class DTLib
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

            #region DGV.Columns
            static public void CheckColumns()
            {
                bool checkE = cbEditor.Checked;
                dgv.Columns["sourceID"].Visible = checkE;
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
                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtSource.GetColumnIndexFilter(), DTLib.dtSource.GetPairFilter());
            }
            #endregion

            // Определение номеров колонок
            static public void LinkColumns(DataGridView sources)
            {
                dgv = sources;
                col = new DGVSourcesCol
                {
                    Calc = dgv.Columns["sourceCalc"].Index,
                    Title = dgv.Columns["sourceTitle"].Index,
                    Driver = dgv.Columns["sourceDriver"].Index,
                    Address = dgv.Columns["sourceAddress"].Index,
                    Desc = dgv.Columns["sourceDesc"].Index,
                    Status = dgv.Columns["sourceStatus"].Index,
                    Message = dgv.Columns["sourceMessage"].Index,
                    CountTags = dgv.Columns["sourceTags"].Index
                };
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
                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtGroup.GetColumnIndexFilter(), DTLib.dtGroup.GetPairFilter(text));
            }
            #endregion


            // Определение номеров колонок
            static public void LinkColumns(DataGridView groups)
            {
                dgv = groups;
                col = new DGVGroupsCol
                {
                    Calc = dgv.Columns["groupCalc"].Index,
                    Title = dgv.Columns["groupTitle"].Index,
                    Source = dgv.Columns["groupSource"].Index,
                    Desc = dgv.Columns["groupDesc"].Index,
                    Status = dgv.Columns["groupStatus"].Index
                };
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

            static public TextBox tbFilter;
            static public ComboBox coFilterSource;
            static public ComboBox coFilterGroup;
            static public ComboBox coFilterBlock;
            static public ComboBox coFilterPage;

            #region DGV.Columns
            static public void CheckColumns()
            {
                bool checkE = cbEditor.Checked;
                dgv.Columns["tagID"].Visible = checkE;
                dgv.Columns["tagAddress"].Visible = checkE;
                dgv.Columns["tagCommand"].Visible = checkE;
                dgv.Columns["tagWriteValue"].Visible = checkE;
                dgv.Columns["tagWriteTag"].Visible = checkE;
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
            }
            #endregion

            #region Filter
            static public void TextFilter()
            {
                string text1 = coFilterSource.Text;
                string text2 = coFilterGroup.Text;
                string text3 = coFilterBlock.Text;
                string text4 = coFilterPage.Text;

                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtTag.GetColumnIndexFilter(), DTLib.dtTag.GetPairFilter(text1, text2, text3, text4));
            }
            #endregion

            // Обновить связи тегов к источникам от групп
            static public void UpdateDGVTagSourceLink()
            {
                // Получить списки для...
                var collectionGroup = MyTree.SetTreeCollection(DTLib.dtGroup.dgv, DTLib.dtGroup.col.Title, DTLib.dtGroup.col.Source);

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string sourceTitle = "";

                    var group = row.Cells[DTLib.dtTag.col.Group].Value;
                    if (group != null)
                    {
                        string groupTitle = group.ToString();
                        if (String.IsNullOrWhiteSpace(groupTitle) == false)
                        {
                            var groupItem = collectionGroup.FirstOrDefault(x => x.Title == groupTitle);
                            if (groupItem.Id > 0)
                            {
                                sourceTitle = (String.IsNullOrWhiteSpace(groupItem.Link)) ? "" : groupItem.Link;
                            }
                        }
                    }

                    row.Cells[DTLib.dtTag.col.Source].Value = sourceTitle;
                }

            }

            // Определение номеров колонок
            static public void LinkColumns(DataGridView tags)
            {
                dgv = tags;
                col = new DGVTagsCol
                {
                    Calc = dgv.Columns["tagCalc"].Index,
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
            static public PairFilterCol[] GetPairFilter(string textSource, string textGroup, string textBlock, string textPage)
            {
                return new PairFilterCol[]
                {
                new PairFilterCol { col = col.Source, filter = textSource },
                new PairFilterCol { col = col.Group, filter = textGroup },
                new PairFilterCol { col = col.Block, filter = textBlock },
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
                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtStructure.GetColumnIndexFilter(), DTLib.dtStructure.GetPairFilter());
            }
            #endregion


            // Определение номеров колонок
            static public void LinkColumns(DataGridView structures)
            {
                dgv = structures;
                col = new DGVStructureCol
                {
                    Title = dgv.Columns["structureTitle"].Index,
                    Connector = dgv.Columns["structureConnector"].Index,
                    TagSource = dgv.Columns["structureTagSource"].Index,
                    Template = dgv.Columns["structureTemplate"].Index,
                    Group = dgv.Columns["structureGroup"].Index
                };

            }

            // Структуры. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.Title, col.Template, col.TagSource, col.Group, col.Connector
                };
            }
            static public PairFilterCol[] GetPairFilter()
            {
                return new PairFilterCol[] { };
            }

            
        }

        static public class dtTarget
        {
            static public DGVTargetCol col = new DGVTargetCol(); // Номера колонок в DGV

            static public DataGridView dgv;

            static public TextBox tbFilter;
            static public ComboBox coFilterParent;

            #region Filter
            static public void StructureTargetFilter()
            {
                string text = coFilterParent.Text;

                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtTarget.GetColumnIndexFilter(), DTLib.dtTarget.GetPairFilter(text));
            }
            #endregion


            // Определение номеров колонок
            static public void LinkColumns(DataGridView targets)
            {
                dgv = targets;
                col = new DGVTargetCol
                {
                    Structure = dgv.Columns["targetStructure"].Index,
                    Address = dgv.Columns["targetAddress"].Index,
                    Tag = dgv.Columns["targetTitle"].Index,
                    Desc = dgv.Columns["targetDesc"].Index
                };

            }

            // Цели. Номера колонок для фильтра в массив
            static public int[] GetColumnIndexFilter()
            {
                return new int[]
                {
                col.Tag, col.Desc, col.Address
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
                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtInclude.GetColumnIndexFilter(), DTLib.dtInclude.GetPairFilter());
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

                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtIncludeChild.GetColumnIndexFilter(), DTLib.dtIncludeChild.GetPairFilter(text));
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

        //
        static public DataRow GetDTRow(DataTable dt, uint Id)
        {
            DataRow rowTag = dt.Rows.Find(Id);
            return rowTag;
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
        static public ushort GetMaxID(DataGridView dgv)
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
        static public DataGridViewRow CloneRowWithValues(DataGridViewRow row)
        {
            DataGridViewRow clonedRow = (DataGridViewRow)row.Clone();
            for (Int32 index = 0; index < row.Cells.Count; index++)
            {
                clonedRow.Cells[index].Value = row.Cells[index].Value;
            }
            return clonedRow;
        }

        // При появлении новой строки таблицы
        static public void ForNewRow(DataGridView dgv)
        {
            var newID = DTLib.GetMaxID(dgv);
            var row = dgv.CurrentRow;
            if (row != null)
                row.Cells[0].Value = newID;
        }

        // Добавление ссылки на родительский элемент
        static public void SetParentInRow(DataGridView dgv, ComboBox cb, int colParentTitle)
        {
            string text = cb.Text;
            if (String.IsNullOrWhiteSpace(text) == false)
            {
                var row = dgv.CurrentRow;
                if (row != null)
                    row.Cells[colParentTitle].Value = text;
            }
        }

        // =============================================================================================

        // Расставить количества элементов
        static public void SetCountForUsed(DataGridView dgvSource, DataGridView dgvTag, int colTitle, int colCount, int colUsed)
        {
            Dictionary<string, int> dic = GetDicForUsed(dgvTag, colUsed);

            foreach (DataGridViewRow item in dgvSource.Rows)
            {
                var itemTitle = item.Cells[colTitle].Value;
                if (itemTitle == null)
                    continue;

                int count = 0;
                if (dic.ContainsKey(itemTitle.ToString()))
                    count = dic[itemTitle.ToString()];

                if (item.Cells[colCount].Value == null || count != (int)item.Cells[colCount].Value)
                    item.Cells[colCount].Value = count;

            }
        }

        // Получить словарь с количеством повторений
        static public Dictionary<string, int> GetDicForUsed(DataGridView dgv, int indexCol)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (DataGridViewRow item in dgv.Rows)
            {
                var objName = item.Cells[indexCol].Value;
                if (objName == null)
                    continue;
                string name = objName.ToString();
                if (dic.ContainsKey(name))
                {
                    dic[name]++;
                }
                else
                {
                    dic.Add(name, 1);
                }
            }
            return dic;
        }

        // Показать строку в таблице
        static public void ShowRow(DataGridView dgv, DataGridViewRow row)
        {
            row.Visible = true; // показываем столбец даже если он скрыт
            row.Selected = true;
            dgv.FirstDisplayedScrollingRowIndex = row.Index;
        }
        static public void ShowRow(DataGridView dgv, int Id = 0, string title = "")
        {
            if (Id > 0)
            {
                ShowRow(dgv, GetRowByID(dgv, Id));
            }
            else if (title != "")
            {
                ShowRow(dgv, GetRowByTitle(dgv, title));
            }
        }

        // Получить строку по ID
        static public DataGridViewRow GetRowByID(DataGridView dgv, int Id)
        {
            foreach (DataGridViewRow item in dgv.Rows)
            {
                if (item.IsNewRow)
                    continue;
                if (item.Cells[0].Value.ToString() == Id.ToString())
                    return item;
            }
            return dgv.Rows[dgv.Rows.Count - 1];
        }

        // Получить строку по Названию (колонка после ID)
        static public DataGridViewRow GetRowByTitle(DataGridView dgv, string title)
        {
            foreach (DataGridViewRow item in dgv.Rows)
            {
                if (item.IsNewRow)
                    continue;
                if (item.Cells[1].Value.ToString() == title)
                    return item;
            }
            return dgv.Rows[dgv.Rows.Count - 1];
        }

        // Добавление в текста в Combobox
        static public void SaveTextComboBox(ComboBox comboBox, string text = "")
        {
            if (text == "")
                text = comboBox.Text;

            if (String.IsNullOrWhiteSpace(text) == false)
            {
                if (comboBox.Items.Contains(text) == false)
                {
                    comboBox.Items.Add(text);
                }
            }
        }





    }
}
