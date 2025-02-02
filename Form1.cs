using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsFormsIDevice.Connector;
using WindowsFormsIDevice.Connector.SGT;

namespace WinSimpleIDriver
{
    
    public partial class Form1 : Form
    {
        // Дерево проекта
        TreeNode treeSGT; // Источники/Группы/Теги
        TreeNode treeBlock; // Блоки
        TreeNode treeStructure; // Структуры
        TreeNode treeInclude; // Классы
        TreeNode treePage; // Страницы

        // Теги
        DataTable dtTags;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Версия
            ToolStripMenuItemVer.Text += " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // Установка номеров колонок
            DataTableLib.SetDGVColumns(dataGridViewSource, dataGridViewGroup, dataGridViewTag,
                                        dataGridViewInclude, dataGridViewChange,
                                        dataGridViewStructure, dataGridViewTarget);

            #region Table Enum
            // Устройства
            ComboBox cbDriver = new ComboBox();
            foreach (string title in Enum.GetNames(typeof(eDriverType)))
                cbDriver.Items.Add(title);
            ((DataGridViewComboBoxColumn)dataGridViewSource.Columns["sourceDriver"]).DataSource = cbDriver.Items;

            // Теги и структуры
            ComboBox cbTypeData = new ComboBox();
            foreach (string title in Enum.GetNames(typeof(eDataType)))
                cbTypeData.Items.Add(title);
            ((DataGridViewComboBoxColumn)dataGridViewTag.Columns["tagDataType"]).DataSource = cbTypeData.Items;
            ((DataGridViewComboBoxColumn)dataGridViewStructure.Columns["structureDataType"]).DataSource = cbTypeData.Items;
            #endregion

            #region View
            // Вид - Дерево
            bool check = ToolStripMenuItemViewTree.Checked;
            splitContainerForm.Panel1Collapsed = !check;

            // Источники
            CheckSourceColumns();

            // Группы
            CheckGroupColumns();

            // Теги
            CheckTagColumns();

            #endregion

            // Дерево
            treeSGT = treeView1.Nodes["Sources"]; // Источники/Группы/Теги
            treeBlock = treeView1.Nodes["Blocks"]; // Блоки
            treeStructure = treeView1.Nodes["Structures"]; // Структуры
            treeInclude = treeView1.Nodes["Includes"]; // Классы
            treePage = treeView1.Nodes["Pages"]; // Страницы

            // Новый проект
            FormClear();

            // DataTables
            dtTags = DataTableLib.GetEmptyDataTableForTags(dataGridViewTag, "Tags");
        }

        // ================================================================================================================

        #region Menu.File.Event

        private void ToolStripMenuItemNew_Click(object sender, EventArgs e)
        {
            FormClear();
        }

        // Новый проект
        private void FormClear()
        {
            // treeView
            //treeSGT.Nodes.Clear();
            //treeBlock.Nodes.Clear();
            //treeStructure.Nodes.Clear();
            treeInclude.Nodes.Clear();
            treePage.Nodes.Clear();
            DrawTreeSGT();
            DrawTreeBlock();
            DrawTreeStructure();

            // DGV
            dataGridViewSource.Rows.Clear();
            dataGridViewGroup.Rows.Clear();
            dataGridViewTag.Rows.Clear();

        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }





        #endregion


        private void ToolStripMenuItemViewTree_Click(object sender, EventArgs e)
        {
            bool check = !ToolStripMenuItemViewTree.Checked;
            splitContainerForm.Panel1Collapsed = !check;
            ToolStripMenuItemViewTree.Checked = check;
        }

        // ================================================================================================================

        private void SaveTextComboBox(ComboBox comboBox, string text = "")
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

        // ================================================================================================================

        #region Source

        #region Sourse.DGV.Columns
        private void CheckSourceColumns()
        {
            bool checkE = checkBoxSourceEditor.Checked;
            dataGridViewSource.Columns["sourceID"].Visible = checkE;
            dataGridViewSource.Columns["sourceAutoRestart"].Visible = checkE;
            dataGridViewSource.Columns["sourceDriver"].Visible = checkE;
            dataGridViewSource.Columns["sourceAddress"].Visible = checkE;
            dataGridViewSource.RowHeadersVisible = checkE;
            dataGridViewSource.ReadOnly = !checkE;

            bool checkD = checkBoxSourceDesc.Checked;
            dataGridViewSource.Columns["sourceDesc"].Visible = checkD;

            bool checkR = checkBoxSourceRuntime.Checked;
            dataGridViewSource.Columns["sourceCalc"].Visible = checkR;
            dataGridViewSource.Columns["sourceStatus"].Visible = checkR;
            dataGridViewSource.Columns["sourceMessage"].Visible = checkR;

            bool checkS = checkBoxSourceStatistic.Checked;
            dataGridViewSource.Columns["sourceTags"].Visible = checkS;
            dataGridViewSource.Columns["sourceStatistic"].Visible = checkS;

        }


        #endregion

        #region Source.Event
        private void checkBoxSourceEditor_CheckedChanged(object sender, EventArgs e)
        {
            CheckSourceColumns();
        }

        private void checkBoxSourceRuntime_CheckedChanged(object sender, EventArgs e)
        {
            CheckSourceColumns();
        }

        private void checkBoxSourceDesc_CheckedChanged(object sender, EventArgs e)
        {
            CheckSourceColumns();
        }

        private void checkBoxSourceStatistic_CheckedChanged(object sender, EventArgs e)
        {
            CheckSourceColumns();
        }
        #endregion

        #region Source.Filter

        #region Source.TextFilter.Event
        private void textBoxSourceFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxSourceFilter.Text))
                SourceFilter();
        }

        private void buttonSourceFilter_Click(object sender, EventArgs e)
        {
            SourceFilter();
        }

        #endregion

        private void SourceFilter()
        {
            DataTableLib.TableFilter(textBoxSourceFilter.Text, dataGridViewSource, 
                DataTableLib.GetColumnIndexFilterSource(), DataTableLib.GetPairFilterSource());
        }

        #endregion

        #region Source.DGV.Event

        private void dataGridViewSource_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewSource_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewSource_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewSource_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewSource_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewSource);
        }

        private void dataGridViewSource_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        #endregion


        #endregion

        // ================================================================================================================

        #region Group

        #region Group.DGV.Columns

        private void CheckGroupColumns()
        {
            bool checkE = checkBoxGroupEditor.Checked;
            dataGridViewGroup.Columns["groupID"].Visible = checkE;
            dataGridViewGroup.Columns["groupPeriod"].Visible = checkE;
            dataGridViewGroup.RowHeadersVisible = checkE;
            dataGridViewGroup.ReadOnly = !checkE;

            bool checkD = checkBoxGroupDesc.Checked;
            dataGridViewGroup.Columns["groupDesc"].Visible = checkD;

            bool checkR = checkBoxGroupRuntime.Checked;
            dataGridViewGroup.Columns["groupCalc"].Visible = checkR;
            dataGridViewGroup.Columns["groupStatus"].Visible = checkR;

            bool checkST = checkBoxGroupStatistic.Checked;
            dataGridViewGroup.Columns["groupTags"].Visible = checkST;
            dataGridViewGroup.Columns["groupStatistic"].Visible = checkST;

            bool checkSource = checkBoxGroupSource.Checked;
            dataGridViewGroup.Columns["groupSource"].Visible = checkSource;
        }


        #endregion

        #region Group.Event

        private void checkBoxGroupEditor_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }

        private void checkBoxGroupDesc_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }

        private void checkBoxGroupStatistic_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }

        private void checkBoxGroupRuntime_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }

        private void checkBoxGroupSource_CheckedChanged(object sender, EventArgs e)
        {
            CheckGroupColumns();
        }
        #endregion

        #region Group.Filter

        #region Group.ComboFilter.Event

        private void comboBoxGroupFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SaveTextComboBox(comboBoxGroupFilterSource);
            GroupFilter();
        }

        private void comboBoxGroupFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxGroupFilterSource.Text))
                GroupFilter();
        }
        
        #endregion

        #region Group.TextFilter.Event
        private void textBoxGroupFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxGroupFilter.Text))
                GroupFilter();
        }
        #endregion

        private void buttonGroupFilter_Click(object sender, EventArgs e)
        {
            SaveTextComboBox(comboBoxGroupFilterSource);
            GroupFilter();
        }

        private void GroupFilter()
        {
            string text = comboBoxGroupFilterSource.Text;
            DataTableLib.TableFilter(textBoxGroupFilter.Text, dataGridViewGroup, 
                DataTableLib.GetColumnIndexFilterGroup(), DataTableLib.GetPairFilterGroup(text));
        }

        #endregion

        #region Group.DGV.Event

        private void dataGridViewGroup_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewGroup_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewGroup_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewGroup_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewGroup);
        }

        private void dataGridViewGroup_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }
        #endregion


        #endregion

        // ================================================================================================================

        #region Tag

        #region Tag.DGV.Columns

        private void CheckTagColumns()
        {
            bool checkE = checkBoxTagEditor.Checked;
            dataGridViewTag.Columns["tagID"].Visible = checkE;
            dataGridViewTag.Columns["tagAddress"].Visible = checkE;
            dataGridViewTag.Columns["tagCommand"].Visible = checkE;
            dataGridViewTag.Columns["tagWriteValue"].Visible = checkE;
            dataGridViewTag.Columns["tagWriteTag"].Visible = checkE;
            dataGridViewTag.RowHeadersVisible = checkE;
            dataGridViewTag.ReadOnly = !checkE;

            bool checkD = checkBoxTagDesc.Checked;
            dataGridViewTag.Columns["tagDesc"].Visible = checkD;
            

            bool checkR = checkBoxTagRuntime.Checked;
            dataGridViewTag.Columns["tagCalc"].Visible = checkR;
            dataGridViewTag.Columns["tagValue"].Visible = checkR;
            dataGridViewTag.Columns["tagStatus"].Visible = checkR;
            dataGridViewTag.Columns["tagMessage"].Visible = checkR;

            bool checkS = checkBoxTagStatistic.Checked;
            dataGridViewTag.Columns["tagStatistic"].Visible = checkS;

            bool checkBP = checkBoxTagBP.Checked;
            dataGridViewTag.Columns["tagBlock"].Visible = checkBP;
            dataGridViewTag.Columns["tagPage"].Visible = checkBP;

            bool checkSG = checkBoxTagSG.Checked;
            dataGridViewTag.Columns["tagSource"].Visible = checkSG;
            dataGridViewTag.Columns["tagGroup"].Visible = checkSG;
        }


        #endregion

        #region Tag.Event
        private void checkBoxTagEditor_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagRuntime_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagDesc_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagStatistic_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagBP_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }

        private void checkBoxTagSG_CheckedChanged(object sender, EventArgs e)
        {
            CheckTagColumns();
        }
        #endregion

        #region Tag.Filter

        #region Tag.ComboFilter.Event

        private void comboBoxTagFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            TagFilter();
        }

        private void comboBoxTagFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterSource.Text))
                TagFilter();
        }

        private void comboBoxTagFilterGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            TagFilter();
        }

        private void comboBoxTagFilterGroup_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterGroup.Text))
                TagFilter();
        }

        private void comboBoxTagFilterBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            TagFilter();
        }

        private void comboBoxTagFilterBlock_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterBlock.Text))
                TagFilter();
        }

        private void comboBoxTagFilterPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            TagFilter();
        }

        private void comboBoxTagFilterPage_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterPage.Text))
                TagFilter();
        }

        #endregion

        #region Tag.TextFilter.Event
        private void textBoxTagFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxTagFilter.Text))
                TagFilter();
        }
        #endregion

        private void buttonTagFilter_Click(object sender, EventArgs e)
        {
            SaveTextComboBox(comboBoxTagFilterSource);
            SaveTextComboBox(comboBoxTagFilterGroup);
            SaveTextComboBox(comboBoxTagFilterBlock);
            SaveTextComboBox(comboBoxTagFilterPage);
            TagFilter();
        }

        private void TagFilter()
        {
            string text1 = comboBoxTagFilterSource.Text;
            string text2 = comboBoxTagFilterGroup.Text;
            string text3 = comboBoxTagFilterBlock.Text;
            string text4 = comboBoxTagFilterPage.Text;

            DataTableLib.TableFilter(textBoxTagFilter.Text, dataGridViewTag,
                DataTableLib.GetColumnIndexFilterTag(), DataTableLib.GetPairFilterTag(text1, text2, text3, text4));
        }

        #endregion

        #region Tag.DGV
        // Обновить связи тегов к источникам от групп
        private void UpdateDGVTagSourceLink()
        {
            // Получить списки для...
            var collectionGroup = MyTree.SetTreeCollection(dataGridViewGroup, DataTableLib.groupsCol.Title, DataTableLib.groupsCol.Source);

            foreach (DataGridViewRow row in dataGridViewTag.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string sourceTitle = "";

                var group = row.Cells[DataTableLib.tagsCol.Group].Value;
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

                row.Cells[DataTableLib.tagsCol.Source].Value = sourceTitle;
            }

        }
        #endregion


        #region Tag.DGV.Event

        private void dataGridViewTag_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewTag_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewTag_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewTag_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewTag_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewTag);
        }

        private void dataGridViewTag_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }






        #endregion

        #endregion

        // ================================================================================================================

        #region Include

        private void buttonIncludeLeft_Click(object sender, EventArgs e)
        {
            splitContainerInclude.Panel2Collapsed = !splitContainerInclude.Panel2Collapsed;
            SetComboBoxChangeFilterInclude();
        }

        private void dataGridViewInclude_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewInclude); // new ID
        }

        #region Include.Filter

        #region Include.TextFilter.Event

        private void textBoxIncludeFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxIncludeFilter.Text))
                IncludeFilter();
        }
        private void buttonIncludeFilter_Click(object sender, EventArgs e)
        {
            IncludeFilter();
        }
        private void IncludeFilter()
        {
            DataTableLib.TableFilter(textBoxIncludeFilter.Text, dataGridViewInclude,
                DataTableLib.GetColumnIndexFilterInclude(), DataTableLib.GetPairFilterInclude());
        }

        #endregion

        #endregion

        private void dataGridViewInclude_SelectionChanged(object sender, EventArgs e)
        {
            SaveTextComboBox(comboBoxChangeFilterInclude);
            SetComboBoxChangeFilterInclude();
        }


        #region Include-Change

        private void buttonIncludeRight_Click(object sender, EventArgs e)
        {
            splitContainerInclude.Panel1Collapsed = !splitContainerInclude.Panel1Collapsed;
            SetComboBoxChangeFilterInclude();
        }

        #region Change.Filter

        #region Change.ComboFilter.Event
        private void comboBoxChangeFilterInclude_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeFilter();
        }
        private void comboBoxChangeFilterInclude_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxChangeFilterInclude.Text))
                ChangeFilter();
        }
        #endregion

        #region Change.TextFilter.Event

        private void textBoxChangeFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxChangeFilter.Text))
                ChangeFilter();
        }

        private void buttonChangeFilter_Click(object sender, EventArgs e)
        {
            SaveTextComboBox(comboBoxChangeFilterInclude);
            ChangeFilter();
        }

        private void SetComboBoxChangeFilterInclude()
        {
            string text = (splitContainerInclude.Panel1Collapsed) ? "" : (DataTableLib.GetValueFromCurrentRow(dataGridViewInclude, DataTableLib.includeCol.Prefix));
            comboBoxChangeFilterInclude.Text = text;
            ChangeFilter();
        }

        private void ChangeFilter()
        {
            string text = comboBoxChangeFilterInclude.Text;
            
            DataTableLib.TableFilter(textBoxChangeFilter.Text, dataGridViewChange,
                DataTableLib.GetColumnIndexFilterChange(), DataTableLib.GetPairFilterChange(text));
        }

        private void dataGridViewChange_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewChange); // new ID

            DataTableLib.SetParentInRow(dataGridViewChange, comboBoxChangeFilterInclude, DataTableLib.changeCol.Prefix); // filter

            //string text = comboBoxChangeFilterInclude.Text;
            //if (String.IsNullOrWhiteSpace(text) == false)
            //{
            //    var row = dataGridViewChange.CurrentRow;
            //    if (row != null)
            //        row.Cells[DataTableLib.changeCol.Prefix].Value = text;
            //}
        }

        #endregion

        #endregion

        #endregion

        #endregion

        // ================================================================================================================

        #region Structure

        private void buttonStructureLeft_Click(object sender, EventArgs e)
        {
            splitContainerStructure.Panel2Collapsed = !splitContainerStructure.Panel2Collapsed;
            SetComboBoxTargetFilterStructure();
        }

        private void dataGridViewStructure_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewStructure);
        }

        #region Structure.Filter

        #region Structure.TextFilter.Event
        private void textBoxStructureFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxStructureFilter.Text))
                StructureFilter();
        }
        private void buttonStructureFilter_Click(object sender, EventArgs e)
        {
            StructureFilter();
        }

        private void StructureFilter()
        {
            DataTableLib.TableFilter(textBoxStructureFilter.Text, dataGridViewStructure,
                DataTableLib.GetColumnIndexFilterStructure(), DataTableLib.GetPairFilterStructure());
        }

        #endregion
        private void dataGridViewStructure_SelectionChanged(object sender, EventArgs e)
        {
            SaveTextComboBox(comboBoxTargetFilterSource);
            SetComboBoxTargetFilterStructure();
        }

        private void SetComboBoxTargetFilterStructure()
        {
            string text = (splitContainerStructure.Panel1Collapsed) ? "" : (DataTableLib.GetValueFromCurrentRow(dataGridViewStructure, DataTableLib.structureCol.Title));
            comboBoxTargetFilterSource.Text = text;
            TargetFilter();
        }

        private void TargetFilter()
        {
            string text = comboBoxTargetFilterSource.Text;

            DataTableLib.TableFilter(textBoxTargetFilter.Text, dataGridViewTarget,
                DataTableLib.GetColumnIndexFilterTarget(), DataTableLib.GetPairFilterTarget(text));
        }

        #endregion


        #region Structure-Target

        private void buttonStructureRight_Click(object sender, EventArgs e)
        {
            splitContainerStructure.Panel1Collapsed = !splitContainerStructure.Panel1Collapsed;
            SetComboBoxTargetFilterStructure();
        }

        #region Target.Filter

        #region Target.ComboFilter.Event
        private void comboBoxTargetFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            TargetFilter();
        }
        private void comboBoxTargetFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTargetFilterSource.Text))
                TargetFilter();
        }

        private void buttonTargetFilter_Click(object sender, EventArgs e)
        {
            SaveTextComboBox(comboBoxTargetFilterSource);
            TargetFilter();
        }

        private void textBoxTargetFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxTargetFilter.Text))
                TargetFilter();
        }

        private void dataGridViewTarget_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewTarget); // new ID

            DataTableLib.SetParentInRow(dataGridViewTarget, comboBoxTargetFilterSource, DataTableLib.targetCol.Structure); // filter

            // filter
            //string text = comboBoxTargetFilterSource.Text;
            //if (String.IsNullOrWhiteSpace(text) == false)
            //{
            //    var row = dataGridViewTarget.CurrentRow;
            //    if (row != null)
            //        row.Cells[DataTableLib.targetCol.Structure].Value = text;
            //}
        }





        #endregion

        #endregion

        #endregion

        #endregion

        // ================================================================================================================

        #region Tree

        

        #region Tree.lib
        
        // Построить дерево для Источники/Группы/Теги
        private void DrawTreeSGT()
        {
            // Получить списки для дерева
            var collectionSource = MyTree.SetTreeCollection(dataGridViewSource, DataTableLib.sourcesCol.Title);
            var collectionGroup = MyTree.SetTreeCollection(dataGridViewGroup, DataTableLib.groupsCol.Title, DataTableLib.groupsCol.Source);
            var collectionTag = MyTree.SetTreeCollection(dataGridViewTag, DataTableLib.tagsCol.Title, DataTableLib.tagsCol.Group);

            // Источники
            treeSGT.Nodes.Clear();
            //treeSGT = new TreeNode("Источники данных");
            treeSGT.Tag = new TreeProjTag(TreeProjCategory.sources, 0);
            treeSGT.NodeFont = new Font(this.Font.FontFamily, 12, FontStyle.Regular);
            //treeSGT.ImageIndex = 2;
            //treeSGT.SelectedImageIndex = 1;

            // Список источников
            foreach (var itemSource in collectionSource)
            {
                TreeNode tnSource = new TreeNode($"{itemSource.Title}");
                tnSource.Tag = new TreeProjTag(TreeProjCategory.sourceItem, itemSource.Id);
                tnSource.NodeFont = new Font(this.Font.FontFamily, 12, FontStyle.Regular);
                tnSource.ImageIndex = 0;

                // Список групп
                foreach (var itemGroup in collectionGroup)
                {
                    if (itemGroup.Link != itemSource.Title)
                        continue;

                    TreeNode tnGroup = new TreeNode($"{itemGroup.Title}");
                    tnGroup.Tag = new TreeProjTag(TreeProjCategory.groupItem, itemGroup.Id);
                    tnGroup.NodeFont = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
                    tnGroup.ImageIndex = 0;

                    // Список тегов
                    foreach (var itemTag in collectionTag)
                    {
                        if (itemTag.Link != itemGroup.Title)
                            continue;

                        TreeNode tnTag = new TreeNode($"{itemTag.Title}");
                        tnTag.Tag = new TreeProjTag(TreeProjCategory.tagItem, itemTag.Id);
                        tnTag.NodeFont = new Font(this.Font.FontFamily, 8, FontStyle.Regular);
                        tnTag.ImageIndex = 0;

                        tnGroup.Nodes.Add(tnTag); // тег
                    }

                    tnSource.Nodes.Add(tnGroup); // группа
                }

                treeSGT.Nodes.Add(tnSource); // источник
            }
            //treeSGT.Text += (EditorControl.sources.Count > 0) ? $" [ {EditorControl.sources.Count} ]" : "";
            //treeViewProject.Nodes.Add(treeSources);
        }

        // Построить дерево для Блоков
        private void DrawTreeBlock()
        {
            // Получение списка путей групп из таблицы тегов
            List<string> blocks = new List<string>();
            foreach (DataGridViewRow row in dataGridViewTag.Rows)
            {
                var block = row.Cells[DataTableLib.tagsCol.Block].Value;
                if (block == null)
                    continue;

                if (String.IsNullOrWhiteSpace(block.ToString()))
                    continue;

                blocks.Add(block.ToString());
            }
            // Убираем повторения
            blocks = blocks.Distinct().OrderBy(x => x).ToList();

            // Блоки
            treeBlock.Nodes.Clear();
            //treeBlock = new TreeNode("Блоки");
            treeBlock.Tag = new TreeProjTag(TreeProjCategory.blocks, 0);
            treeBlock.NodeFont = new Font(this.Font.FontFamily, 12, FontStyle.Regular);
            //treeBlock.ImageIndex = 2;
            //treeBlock.SelectedImageIndex = 1;

            // Перебор всех путей
            foreach (string pathBlock in blocks)
            {
                if (String.IsNullOrWhiteSpace(pathBlock))
                    continue;

                TreeNode tn = treeBlock;
                string[] parts = pathBlock.Split('.'); // A.B.C.D
                foreach (string item in parts)
                {
                    if (String.IsNullOrWhiteSpace(item))
                        continue;

                    var exist = tn.Nodes.Find(item, false);
                    if (exist == null || exist.Length == 0)
                    {
                        TreeNode next = new TreeNode(item);
                        next.Name = item;
                        next.Tag = new TreeProjTag(TreeProjCategory.blocks, 0);
                        next.NodeFont = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
                        tn.Nodes.Add(next);
                        tn = next;
                    }
                    else
                    {
                        tn = exist[0];
                    }

                }

            }

        }

        // Построить дерево для Структур
        private void DrawTreeStructure()
        {
            // Получить списки для дерева
            var collectionStructure = MyTree.SetTreeCollection(dataGridViewStructure, DataTableLib.structureCol.Title);
            var collectionTarget = MyTree.SetTreeCollection(dataGridViewTarget, DataTableLib.targetCol.Tag, DataTableLib.targetCol.Structure);

            // Структуры
            treeStructure.Nodes.Clear();
            //treeStructure = new TreeNode("Структуры");
            treeStructure.Tag = new TreeProjTag(TreeProjCategory.structures, 0);
            treeStructure.NodeFont = new Font(this.Font.FontFamily, 12, FontStyle.Regular);
            //treeStructure.ImageIndex = 2;
            //treeStructure.SelectedImageIndex = 1;

            // Список структур
            foreach (var itemStructure in collectionStructure)
            {
                TreeNode tnStructure = new TreeNode($"{itemStructure.Title}");
                tnStructure.Tag = new TreeProjTag(TreeProjCategory.structures, itemStructure.Id);
                tnStructure.NodeFont = new Font(this.Font.FontFamily, 12, FontStyle.Regular);
                tnStructure.ImageIndex = 0;

                // Список тегов структур
                foreach (var itemTarget in collectionTarget)
                {
                    if (itemTarget.Link != itemStructure.Title)
                        continue;

                    TreeNode tnTarget = new TreeNode($"{itemTarget.Title}");
                    tnTarget.Tag = new TreeProjTag(TreeProjCategory.targetItem, itemTarget.Id);
                    tnTarget.NodeFont = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
                    tnTarget.ImageIndex = 0;

                    tnStructure.Nodes.Add(tnTarget); // тег структуры

                }

                treeStructure.Nodes.Add(tnStructure); // структура
            }

        }

        #endregion


        #endregion


        // TEST
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawTreeSGT(); // Построение дерева Источники/Группы/Теги
            DrawTreeBlock(); // Построить дерево для Блоков
            DrawTreeStructure(); // Построить дерево для Структур
        }

        private void testTagSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateDGVTagSourceLink();
        }



    }
}
