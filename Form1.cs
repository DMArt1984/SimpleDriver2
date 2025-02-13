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

using WinSimpleIDriver.Connector;
using WinSimpleIDriver.Connector.SGT;
using DML.Log;
using DML;

namespace WinSimpleIDriver
{
    
    public partial class Form1 : Form
    {
        // Дерево проекта
        TreeNode treeSGT; // Источники/Группы/Теги
        TreeNode treeBlock; // Блоки
        TreeNode treeStructure; // Структуры
        TreeNode treeInclude; // Классы

        // Теги
        DataTable dtTags;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Заголовок
            this.Text += $" {Settings.settingsFileName}";
            notifyIcon1.Text = this.Text;

            // Версия
            ToolStripMenuItemVer.Text += " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // свернуть окно
            if (Settings.tray)
                WindowState = FormWindowState.Minimized;

            // трей
            notifyIcon1.Visible = Settings.tray;

            // Log DGV
            LogForm.rowLong = AddRowLongLogDGV;
            LogForm.rowShort = AddRowShortLogDGV;


            // Form: Source
            DTLib.dtSource.LinkColumns(dataGridViewSource);
            DTLib.dtSource.cbEditor = checkBoxSourceEditor;
            DTLib.dtSource.cbDesc = checkBoxSourceDesc;
            DTLib.dtSource.cbRuntime = checkBoxSourceRuntime;
            DTLib.dtSource.cbStatistic = checkBoxSourceStatistic;
            DTLib.dtSource.tbFilter = textBoxSourceFilter;
            DTLib.dtSource.CheckColumns();

            // Form: Group
            DTLib.dtGroup.LinkColumns(dataGridViewGroup);
            DTLib.dtGroup.cbEditor = checkBoxGroupEditor;
            DTLib.dtGroup.cbDesc = checkBoxGroupDesc;
            DTLib.dtGroup.cbRuntime = checkBoxGroupRuntime;
            DTLib.dtGroup.cbStatistic = checkBoxGroupStatistic;
            DTLib.dtGroup.cbGroupSource = checkBoxGroupSource;
            DTLib.dtGroup.tbFilter = textBoxGroupFilter;
            DTLib.dtGroup.coFilterSource = comboBoxGroupFilterSource;
            DTLib.dtGroup.CheckColumns();

            // Form: Tag
            DTLib.dtTag.LinkColumns(dataGridViewTag);
            DTLib.dtTag.cbEditor = checkBoxTagEditor;
            DTLib.dtTag.cbDesc = checkBoxTagDesc;
            DTLib.dtTag.cbRuntime = checkBoxTagRuntime;
            DTLib.dtTag.cbStatistic = checkBoxTagStatistic;
            DTLib.dtTag.cbBP = checkBoxTagBP;
            DTLib.dtTag.cbSG = checkBoxTagSG;
            DTLib.dtTag.tbFilter = textBoxTagFilter;
            DTLib.dtTag.coFilterSource = comboBoxTagFilterSource;
            DTLib.dtTag.coFilterGroup = comboBoxTagFilterGroup;
            DTLib.dtTag.coFilterBlock = comboBoxTagFilterBlock;
            DTLib.dtTag.coFilterPage = comboBoxTagFilterPage;
            DTLib.dtTag.CheckColumns();

            // Form: Structure
            DTLib.dtStructure.LinkColumns(dataGridViewStructure);
            DTLib.dtStructure.tbFilter = textBoxStructureFilter;

            // Form: StructureTargetForm
            DTLib.dtTarget.LinkColumns(dataGridViewStructureTarget);
            DTLib.dtTarget.tbFilter = textBoxStructureTargetFilter;
            DTLib.dtTarget.coFilterParent = comboBoxStructureTargetFilterParent;

            // Form: Include
            DTLib.dtInclude.LinkColumns(dataGridViewInclude);
            DTLib.dtInclude.tbFilter = textBoxIncludeFilter;

            // Form: IncludeChild
            DTLib.dtIncludeChild.LinkColumns(dataGridViewIncludeChild);
            DTLib.dtIncludeChild.tbFilter = textBoxIncludeChildFilter;
            DTLib.dtIncludeChild.coFilterParent = comboBoxIncludeChildFilterParent;



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
            splitContainerTreeMain.Panel1Collapsed = !check;

            #endregion

            // Дерево
            treeSGT = treeViewProject.Nodes["Sources"]; // Источники/Группы/Теги
            treeBlock = treeViewProject.Nodes["Blocks"]; // Блоки
            treeStructure = treeViewProject.Nodes["Structures"]; // Структуры
            treeInclude = treeViewProject.Nodes["Includes"]; // Классы

            // Новый проект
            FormClear();

            // DataTables
            dtTags = DTLib.GetEmptyDataTableForTags(dataGridViewTag, "Tags");

            SetLeftLabelMessage1();

            

            //MessageBox.Show(Settings.x);
        }

        // ================================================================================================================

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        // ================================================================================================================

        #region Status
        // Установить сообщение 1
        private string SetLeftLabelMessage1(string message = "")
        {
            toolStripStatusLabelMessage1.Text = message;
            LogHelper.LogApp(message);
            return message;
        }
        // Установить сообщение 2
        private string SetMidLabelMessage2(string message = "")
        {
            toolStripStatusLabelMessage2.Text = message;
            LogHelper.LogApp(message);
            return message;
        }
        // Установить сообщение 3
        private string SetRightLabelMessage3(string message = "")
        {
            toolStripStatusLabelMessage3.Text = message;
            LogHelper.LogApp(message);
            return message;
        }

        #endregion


        #region Log

        // Дублировать лог в таблицу формы
        private void AddRowLongLogDGV(eMessageType mtype, string category, string message)
        {
            if (Settings.logTable == false || String.IsNullOrWhiteSpace(message))
                return;

            if (InvokeRequired)
            {
                Invoke((Action<eMessageType, string, string>)AddRowLongLogDGV, mtype, category, message);
            }
            else
            {
                dataGridViewLog.Rows.Add(dataGridViewLog.Rows.Count + 1, DateTime.Now, category, mtype.ToString().ToUpper(), message);
            }
        }
        private void AddRowShortLogDGV(CodeMessage cm, string category)
        {
            AddRowLongLogDGV(LogForm.GetMessageType(cm), category, cm.message);
        }

        #endregion

        // ================================================================================================================

        #region Menu.File.Event
        private void ToolStripMenuItemViewTree_Click(object sender, EventArgs e)
        {
            bool check = !ToolStripMenuItemViewTree.Checked;
            splitContainerTreeMain.Panel1Collapsed = !check;
            ToolStripMenuItemViewTree.Checked = check;
        }
        private void Log2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (splitContainerLogMain.Panel1Collapsed == false && splitContainerLogMain.Panel2Collapsed == false)
            {
                splitContainerLogMain.Panel1Collapsed = true;
                splitContainerLogMain.Panel2Collapsed = false;
            }
            else if (splitContainerLogMain.Panel1Collapsed == true && splitContainerLogMain.Panel2Collapsed == false)
            {
                splitContainerLogMain.Panel1Collapsed = false;
                splitContainerLogMain.Panel2Collapsed = true;
            }
            else
            {
                splitContainerLogMain.Panel1Collapsed = false;
                splitContainerLogMain.Panel2Collapsed = false;
            }
        }
        private void ToolStripMenuItemNew_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Новый проект");
            FormClear();
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Открыть проект");

        }

        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Сохранить проект");

        }

        private void ToolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Сохранить проект как...");

        }

        private void ToolStripMenuItemImport_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Импорт проекта");

        }

        private void ToolStripMenuItemExport_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Экспорт проекта");
        }

        // Новый проект
        private void FormClear()
        {
            // treeView
            DrawTreeSGT();
            DrawTreeBlock();
            DrawTreeStructure();
            DrawTreeInclude();

            // DGV
            dataGridViewSource.Rows.Clear();
            dataGridViewGroup.Rows.Clear();
            dataGridViewTag.Rows.Clear();

        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Выход из приложения");
            this.Close();
        }

        #endregion

        // ================================================================================================================

        

        // ================================================================================================================

        #region Source

        #region Source.Event
        private void checkBoxSourceEditor_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceDesc_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtSource.CheckColumns();
        }
        #endregion

        #region Source.Filter

        #region Source.TextFilter.Event
        private void textBoxSourceFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxSourceFilter.Text))
                DTLib.dtSource.TextFilter();
        }

        private void buttonSourceFilter_Click(object sender, EventArgs e)
        {
            DTLib.dtSource.TextFilter();
        }

        #endregion

        

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
            DTLib.ForNewRow(dataGridViewSource);
        }

        private void dataGridViewSource_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        #endregion


        #endregion

        // ================================================================================================================

        #region Group

        #region Group.Event

        private void checkBoxGroupEditor_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupDesc_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupSource_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtGroup.CheckColumns();
        }
        #endregion

        #region Group.Filter

        #region Group.ComboFilter.Event

        private void comboBoxGroupFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DTLib.dtGroup.TextFilter();
        }

        private void comboBoxGroupFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxGroupFilterSource.Text))
                DTLib.dtGroup.TextFilter();
        }
        
        #endregion

        #region Group.TextFilter.Event
        private void textBoxGroupFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxGroupFilter.Text))
                DTLib.dtGroup.TextFilter();
        }
        #endregion

        private void buttonGroupFilter_Click(object sender, EventArgs e)
        {
            DTLib.SaveTextComboBox(comboBoxGroupFilterSource);
            DTLib.dtGroup.TextFilter();
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
            DTLib.ForNewRow(dataGridViewGroup);
        }

        private void dataGridViewGroup_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }
        #endregion


        #endregion

        // ================================================================================================================

        #region Tag

        #region Tag.Event
        private void checkBoxTagEditor_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.CheckColumns();
        }

        private void checkBoxTagRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.CheckColumns();
        }

        private void checkBoxTagDesc_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.CheckColumns();
        }

        private void checkBoxTagStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.CheckColumns();
        }

        private void checkBoxTagBP_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.CheckColumns();
        }

        private void checkBoxTagSG_CheckedChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.CheckColumns();
        }
        #endregion

        #region Tag.Filter

        #region Tag.ComboFilter.Event

        private void comboBoxTagFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterSource.Text))
                DTLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterGroup_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterGroup.Text))
                DTLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterBlock_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterBlock.Text))
                DTLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            DTLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterPage_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterPage.Text))
                DTLib.dtTag.TextFilter();
        }

        #endregion

        #region Tag.TextFilter.Event
        private void textBoxTagFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxTagFilter.Text))
                DTLib.dtTag.TextFilter();
        }
        #endregion

        private void buttonTagFilter_Click(object sender, EventArgs e)
        {
            DTLib.SaveTextComboBox(comboBoxTagFilterSource);
            DTLib.SaveTextComboBox(comboBoxTagFilterGroup);
            DTLib.SaveTextComboBox(comboBoxTagFilterBlock);
            DTLib.SaveTextComboBox(comboBoxTagFilterPage);
            DTLib.dtTag.TextFilter();
        }

        

        #endregion

        #region Tag.DGV
        
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
            DTLib.ForNewRow(dataGridViewTag);
        }

        private void dataGridViewTag_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }






        #endregion

        #endregion

        // ================================================================================================================

        #region Include

        #region Include.Event
        private void buttonIncludeLeft_Click(object sender, EventArgs e)
        {
            splitContainerInclude.Panel2Collapsed = !splitContainerInclude.Panel2Collapsed;
            SetComboBoxIncludeChildFilterInclude();
        }

        private void dataGridViewInclude_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DTLib.ForNewRow(dataGridViewInclude); // new ID
        }
        #endregion

        #region Include.Filter

        #region Include.TextFilter.Event

        private void textBoxIncludeFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxIncludeFilter.Text))
                DTLib.dtInclude.IncludeFilter();
        }
        private void buttonIncludeFilter_Click(object sender, EventArgs e)
        {
            DTLib.dtInclude.IncludeFilter();
        }
        

        #endregion

        #endregion

        private void dataGridViewInclude_SelectionChanged(object sender, EventArgs e)
        {
            DTLib.SaveTextComboBox(comboBoxIncludeChildFilterParent);
            SetComboBoxIncludeChildFilterInclude();
        }


        #region Include-Child

        private void buttonIncludeRight_Click(object sender, EventArgs e)
        {
            splitContainerInclude.Panel1Collapsed = !splitContainerInclude.Panel1Collapsed;
            SetComboBoxIncludeChildFilterInclude();
        }

        #region IncludeChild.Filter

        #region IncludeChild.ComboFilter.Event
        private void comboBoxChangeFilterInclude_SelectedIndexChanged(object sender, EventArgs e)
        {
            DTLib.dtIncludeChild.IncludeChildFilter();
        }
        private void comboBoxChangeFilterInclude_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxIncludeChildFilterParent.Text))
                DTLib.dtIncludeChild.IncludeChildFilter();
        }
        #endregion

        #region IncludeChild.TextFilter.Event

        private void textBoxChangeFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxIncludeChildFilter.Text))
                DTLib.dtIncludeChild.IncludeChildFilter();
        }

        private void buttonChangeFilter_Click(object sender, EventArgs e)
        {
            DTLib.SaveTextComboBox(comboBoxIncludeChildFilterParent);
            DTLib.dtIncludeChild.IncludeChildFilter();
        }

        private void SetComboBoxIncludeChildFilterInclude()
        {
            string text = (splitContainerInclude.Panel1Collapsed) ? "" : (DTLib.GetValueFromCurrentRow(dataGridViewInclude, DTLib.dtInclude.col.Prefix));
            comboBoxIncludeChildFilterParent.Text = text;
            DTLib.dtIncludeChild.IncludeChildFilter();
        }

        

        private void dataGridViewIncludeChild_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DTLib.ForNewRow(dataGridViewIncludeChild); // new ID

            DTLib.SetParentInRow(dataGridViewIncludeChild, comboBoxIncludeChildFilterParent, DTLib.dtIncludeChild.col.Prefix); // filter

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

        #region Structure.Event
        private void buttonStructureLeft_Click(object sender, EventArgs e)
        {
            splitContainerStructure.Panel2Collapsed = !splitContainerStructure.Panel2Collapsed;
            SetComboBoxTargetFilterStructure();
        }

        private void dataGridViewStructure_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DTLib.ForNewRow(dataGridViewStructure);
        }
        #endregion

        #region Structure.Filter

        #region Structure.TextFilter.Event
        private void textBoxStructureFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxStructureFilter.Text))
                DTLib.dtStructure.StructureFilter();
        }
        private void buttonStructureFilter_Click(object sender, EventArgs e)
        {
            DTLib.dtStructure.StructureFilter();
        }

        

        #endregion
        private void dataGridViewStructure_SelectionChanged(object sender, EventArgs e)
        {
            DTLib.SaveTextComboBox(comboBoxStructureTargetFilterParent);
            SetComboBoxTargetFilterStructure();
        }

        private void SetComboBoxTargetFilterStructure()
        {
            string text = (splitContainerStructure.Panel1Collapsed) ? "" : (DTLib.GetValueFromCurrentRow(dataGridViewStructure, DTLib.dtStructure.col.Title));
            comboBoxStructureTargetFilterParent.Text = text;
            DTLib.dtTarget.StructureTargetFilter();
        }

        

        #endregion

        #region Structure-Target

        private void buttonStructureRight_Click(object sender, EventArgs e)
        {
            splitContainerStructure.Panel1Collapsed = !splitContainerStructure.Panel1Collapsed;
            SetComboBoxTargetFilterStructure();
        }

        #region StructureTarget.Filter

        #region StructureTarget.ComboFilter.Event
        private void comboBoxTargetFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DTLib.dtTarget.StructureTargetFilter();
        }
        private void comboBoxTargetFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxStructureTargetFilterParent.Text))
                DTLib.dtTarget.StructureTargetFilter();
        }

        private void buttonTargetFilter_Click(object sender, EventArgs e)
        {
            DTLib.SaveTextComboBox(comboBoxStructureTargetFilterParent);
            DTLib.dtTarget.StructureTargetFilter();
        }

        private void textBoxTargetFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxStructureTargetFilter.Text))
                DTLib.dtTarget.StructureTargetFilter();
        }

        private void dataGridViewTarget_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DTLib.ForNewRow(dataGridViewStructureTarget); // new ID

            DTLib.SetParentInRow(dataGridViewStructureTarget, comboBoxStructureTargetFilterParent, DTLib.dtTarget.col.Structure); // filter

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

        #region Tree.Event
        // Двойное нажатие на ветку из дерева
        private void treeViewProject_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selNode = treeViewProject.SelectedNode;
            TreeProjectSelector(selNode);
            OpenTab(selNode);
            SetTreeFilter(selNode);
        }

        // Выбрана ветка из дерева
        private void treeViewProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selNode = treeViewProject.SelectedNode;
            TreeProjContexMenu(selNode);
        }
        #endregion

        // Пункты контекстного меню
        private void TreeProjContexMenu(TreeNode selNode)
        {
            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);

            // Контекстное меню в зависимости от категории
            contextMenuStripTreeProj.Items[0].Visible = false;
            switch (tpt.category)
            {
                case TreeProjCategory.sourceItem:
                case TreeProjCategory.groupItem:
                case TreeProjCategory.tagItem:
                case TreeProjCategory.structureItem:
                case TreeProjCategory.targetItem:
                case TreeProjCategory.includeItem:
                case TreeProjCategory.changeItem:
                    contextMenuStripTreeProj.Items[0].Visible = true;
                    break;

            }
        }


        // Добавить в фильт тегов источник, группу и т.п.
        public void SetTreeFilter(TreeNode selNode)
        {
            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);
            switch (tpt.category)
            {
                case TreeProjCategory.sourceItem:
                    //SetTreeSourceFilter(selNode);
                    break;

                case TreeProjCategory.groupItem:
                    //SetTreeGroupFilter(selNode);
                    break;

                case TreeProjCategory.blocks:
                    //SetTreeBlockFilter(selNode);
                    break;
            }
        }

        // Редактирование ветки из дерева
        private void TreeProjectSelector(TreeNode selNode)
        {
            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);

            // Категория из дерева
            switch (tpt.category)
            {
                case TreeProjCategory.sources:
                    break;

                case TreeProjCategory.sourceItem:
                    break;

                case TreeProjCategory.groupItem:
                    break;

                case TreeProjCategory.tagItem:
                    break;

                case TreeProjCategory.structures:
                    break;

                case TreeProjCategory.targetItem:
                    break;

                case TreeProjCategory.includes:
                    break;

                case TreeProjCategory.includeItem:
                    break;

            }

        }

        #region Tree.lib

        // Построить дерево для Источники/Группы/Теги
        private void DrawTreeSGT()
        {
            // Получить списки для дерева
            var collectionSource = MyTree.SetTreeCollection(dataGridViewSource, DTLib.dtSource.col.Title);
            var collectionGroup = MyTree.SetTreeCollection(dataGridViewGroup, DTLib.dtGroup.col.Title, DTLib.dtGroup.col.Source);
            var collectionTag = MyTree.SetTreeCollection(dataGridViewTag, DTLib.dtTag.col.Title, DTLib.dtTag.col.Group);

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
                var block = row.Cells[DTLib.dtTag.col.Block].Value;
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
                        next.Tag = new TreeProjTag(TreeProjCategory.blockItem, 0);
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
            var collectionStructure = MyTree.SetTreeCollection(dataGridViewStructure, DTLib.dtStructure.col.Title);
            var collectionTarget = MyTree.SetTreeCollection(dataGridViewStructureTarget, DTLib.dtTarget.col.Tag, DTLib.dtTarget.col.Structure);

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
                tnStructure.Tag = new TreeProjTag(TreeProjCategory.structureItem, itemStructure.Id);
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

        // Построить дерево для Классов
        private void DrawTreeInclude()
        {
            // Получить списки для дерева
            var collectionInclude = MyTree.SetTreeCollection(dataGridViewInclude, DTLib.dtInclude.col.Prefix);
            var collectionChange = MyTree.SetTreeCollection(dataGridViewIncludeChild, DTLib.dtIncludeChild.col.ChangeFrom, DTLib.dtIncludeChild.col.Prefix);

            // Классы
            treeInclude.Nodes.Clear();
            //treeInclude = new TreeNode("Классы");
            treeInclude.Tag = new TreeProjTag(TreeProjCategory.includes, 0);
            treeInclude.NodeFont = new Font(this.Font.FontFamily, 12, FontStyle.Regular);
            //treeInclude.ImageIndex = 2;
            //treeInclude.SelectedImageIndex = 1;

            // Список классов
            foreach (var itemInclude in collectionInclude)
            {
                TreeNode tnInclude = new TreeNode($"{itemInclude.Title}");
                tnInclude.Tag = new TreeProjTag(TreeProjCategory.includeItem, itemInclude.Id);
                tnInclude.NodeFont = new Font(this.Font.FontFamily, 12, FontStyle.Regular);
                tnInclude.ImageIndex = 0;

                // Список замен
                foreach (var itemChange in collectionChange)
                {
                    if (itemChange.Link != itemInclude.Title)
                        continue;

                    TreeNode tnChange = new TreeNode($"{itemChange.Title}");
                    tnChange.Tag = new TreeProjTag(TreeProjCategory.changeItem, itemChange.Id);
                    tnChange.NodeFont = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
                    tnChange.ImageIndex = 0;

                    tnInclude.Nodes.Add(tnChange); // замена
                }
                treeInclude.Nodes.Add(tnInclude); // классы
            }
        }


        #endregion


        #endregion

        #region TAB

        #region TAB.Event
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            string tabName = tabControlProject.TabPages[tabControlProject.SelectedIndex].Name;
            switch (tabName)
            {
                case "tabPageSource":
                    DTLib.SetCountForUsed(dataGridViewSource, dataGridViewTag, DTLib.dtSource.col.Title, DTLib.dtSource.col.CountTags, DTLib.dtTag.col.Source);
                    break;

            }
        }
        #endregion

        private void OpenTab(TreeNode selNode)
        {
            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);

            OpenTab(tpt.category, tpt.Id);
        }

        public void OpenTab(TreeProjCategory category, int Id = 0, string title = "")
        {
            // Открыть активную вкладку
            // Открыть активную вкладку
            switch (category)
            {
                case TreeProjCategory.sources:
                    tabControlProject.SelectTab(tabPageSource);
                    break;

                case TreeProjCategory.sourceItem:
                    tabControlProject.SelectTab(tabPageSource);
                    DTLib.ShowRow(dataGridViewSource, Id, title);
                    break;

                case TreeProjCategory.groupItem:
                    tabControlProject.SelectTab(tabPageGroup);
                    DTLib.ShowRow(dataGridViewGroup, Id, title);
                    break;

                case TreeProjCategory.tagItem:
                    tabControlProject.SelectTab(tabPageTag);
                    DTLib.ShowRow(dataGridViewTag, Id, title);
                    break;


                case TreeProjCategory.structures:
                    tabControlProject.SelectTab(tabPageStructure);
                    break;

                case TreeProjCategory.structureItem:
                    tabControlProject.SelectTab(tabPageStructure);
                    DTLib.ShowRow(dataGridViewStructure, Id, title);
                    break;

                case TreeProjCategory.targetItem:
                    tabControlProject.SelectTab(tabPageStructure);
                    DTLib.ShowRow(dataGridViewStructureTarget, Id, title);
                    break;


                case TreeProjCategory.includes:
                    tabControlProject.SelectTab(tabPageInclude);
                    break;

                case TreeProjCategory.includeItem :
                    tabControlProject.SelectTab(tabPageInclude);
                    DTLib.ShowRow(dataGridViewInclude, Id, title);
                    break;

                case TreeProjCategory.changeItem:
                    tabControlProject.SelectTab(tabPageInclude);
                    DTLib.ShowRow(dataGridViewIncludeChild, Id, title);
                    break;
            }
        }
        #endregion

        // ================================================================================================================



        // ================================================================================================================

        // TEST
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawTreeSGT(); // Построение дерева Источники/Группы/Теги
            DrawTreeBlock(); // Построить дерево для Блоков
            DrawTreeStructure(); // Построить дерево для Структур
            DrawTreeInclude(); // Построить дерево для Классов
        }

        private void testTagSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DTLib.dtTag.UpdateDGVTagSourceLink();
        }

        private void buttonSourceCopy_Click(object sender, EventArgs e)
        {

        }




        // ===============================================================================================================



    }
}
