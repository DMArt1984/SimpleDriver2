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
using System.IO;
using WinSimpleIDriver.Editor;

namespace WinSimpleIDriver
{
    
    public partial class Form1 : Form
    {
        

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

            // Последние файлы
            UpdateRecentFilesMenu();

            // Log DGV
            LogForm.rowLong = AddRowLongLogDGV;
            LogForm.rowShort = AddRowShortLogDGV;


            // Form: Source
            DataTableLib.dtSource.LinkColumns(dataGridViewSource);
            DataTableLib.dtSource.cbEditor = checkBoxSourceEditor;
            DataTableLib.dtSource.cbDesc = checkBoxSourceDesc;
            DataTableLib.dtSource.cbRuntime = checkBoxSourceRuntime;
            DataTableLib.dtSource.cbStatistic = checkBoxSourceStatistic;
            DataTableLib.dtSource.tbFilter = textBoxSourceFilter;
            DataTableLib.dtSource.CheckColumns();

            // Form: Group
            DataTableLib.dtGroup.LinkColumns(dataGridViewGroup);
            DataTableLib.dtGroup.cbEditor = checkBoxGroupEditor;
            DataTableLib.dtGroup.cbDesc = checkBoxGroupDesc;
            DataTableLib.dtGroup.cbRuntime = checkBoxGroupRuntime;
            DataTableLib.dtGroup.cbStatistic = checkBoxGroupStatistic;
            DataTableLib.dtGroup.cbGroupSource = checkBoxGroupSource;
            DataTableLib.dtGroup.tbFilter = textBoxGroupFilter;
            DataTableLib.dtGroup.coFilterSource = comboBoxGroupFilterSource;
            DataTableLib.dtGroup.CheckColumns();

            // Form: Tag
            DataTableLib.dtTag.LinkColumns(dataGridViewTag);
            DataTableLib.dtTag.cbEditor = checkBoxTagEditor;
            DataTableLib.dtTag.cbDesc = checkBoxTagDesc;
            DataTableLib.dtTag.cbRuntime = checkBoxTagRuntime;
            DataTableLib.dtTag.cbStatistic = checkBoxTagStatistic;
            DataTableLib.dtTag.cbBP = checkBoxTagBP;
            DataTableLib.dtTag.cbSG = checkBoxTagSG;
            DataTableLib.dtTag.cbSave = checkBoxTagSave;
            DataTableLib.dtTag.cbAddress = checkBoxTagAddress;
            DataTableLib.dtTag.tbFilter = textBoxTagFilter;
            DataTableLib.dtTag.coFilterSource = comboBoxTagFilterSource;
            DataTableLib.dtTag.coFilterGroup = comboBoxTagFilterGroup;
            DataTableLib.dtTag.coFilterBlock = comboBoxTagFilterBlock;
            DataTableLib.dtTag.coFilterPage = comboBoxTagFilterPage;
            DataTableLib.dtTag.CheckColumns();

            // Form: Structure
            DataTableLib.dtStructure.LinkColumns(dataGridViewStructure);
            DataTableLib.dtStructure.tbFilter = textBoxStructureFilter;

            // Form: StructureTargetForm
            DataTableLib.dtTarget.LinkColumns(dataGridViewStructureTarget);
            DataTableLib.dtTarget.tbFilter = textBoxStructureTargetFilter;
            DataTableLib.dtTarget.coFilterParent = comboBoxStructureTargetFilterParent;

            // Form: Include
            DataTableLib.dtInclude.LinkColumns(dataGridViewInclude);
            DataTableLib.dtInclude.tbFilter = textBoxIncludeFilter;

            // Form: IncludeChild
            DataTableLib.dtIncludeChild.LinkColumns(dataGridViewIncludeChild);
            DataTableLib.dtIncludeChild.tbFilter = textBoxIncludeChildFilter;
            DataTableLib.dtIncludeChild.coFilterParent = comboBoxIncludeChildFilterParent;

            // Form: Tree
            TreeLib.tree = treeViewProject;
            TreeLib.contextMenuStripTreeProj = contextMenuStripTreeProj;
            // Дерево
            TreeLib.treeSGT = treeViewProject.Nodes["Sources"]; // Источники/Группы/Теги
            TreeLib.treeBlock = treeViewProject.Nodes["Blocks"]; // Блоки
            TreeLib.treeStructure = treeViewProject.Nodes["Structures"]; // Структуры
            TreeLib.treeInclude = treeViewProject.Nodes["Includes"]; // Классы

            //
            DataTableLib.SetTableEnum();

            #region View
            // Вид - Дерево
            bool check = ToolStripMenuItemViewTree.Checked;
            splitContainerTreeMain.Panel1Collapsed = !check;

            #endregion

            // Combo
            SetComboPlaceholder();
            

            // Новый проект
            FormClear();

            // DataTables
            dtTags = DataTableLib.GetEmptyDataTableForTags(dataGridViewTag, "Tags");

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

        private async void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Открыть проект");
            await OpenProjectAsync(true);
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
            // DGV
            DataTableLib.Clear();
            //dataGridViewSource.Rows.Clear();
            //dataGridViewGroup.Rows.Clear();
            //dataGridViewTag.Rows.Clear();

            // treeView
            TreeLib.DrawTreeSGT();
            TreeLib.DrawTreeBlock();
            TreeLib.DrawTreeStructure();
            TreeLib.DrawTreeInclude();

            //DataTableLib.dtTag.DrawTable(EditorControl.tags);

        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Выход из приложения");
            this.Close();
        }

        // Открыть проект (распаковка настроек из файла)
        private async Task OpenProjectAsync(bool select = true, string fileName = "")
        {
            SetLeftLabelMessage1("Открытие проекта...");

            // Загрузка проекта JSON
            string input = FileControl.LoadFromFile(ref fileName, out string path, select); // чтение из файла...
            if (String.IsNullOrWhiteSpace(input))
                return;

            // Последние файлы
            string fullFileName = Path.Combine(path, fileName);
            FileControl.AddToRecentFiles(fullFileName); // Сохранение файла в истории

            await Task.Run(() =>
            {
                // получение JSON данных
                dynamic output = JsonControl.Deserialize_Json_Data(input);

                // распаковка проекта
                EditorControl.ParseData(output);
            });

            // Обновление UI (обновление меню и формы)
            UpdateRecentFilesMenu();
            BuildForForm();

            SetLeftLabelMessage1("Проект открыт!");
        }

        // Рисование на форме
        private void BuildForForm()
        {
            DataTableLib.dtSource.DrawTable(EditorControl.sources);
            DataTableLib.dtGroup.DrawTable(EditorControl.groups);
            DataTableLib.dtTag.DrawTable(EditorControl.tags);
            //...

            // link group -> source
            DataTableLib.dtTag.UpdateDGVTagSourceLink();
            // count
            DataTableLib.SetCountTagForUsed(dataGridViewSource, dataGridViewTag, DataTableLib.dtSource.col.Title, DataTableLib.dtSource.col.CountTags, DataTableLib.dtTag.col.Source);
            DataTableLib.SetCountTagForUsed(dataGridViewGroup, dataGridViewTag, DataTableLib.dtGroup.col.Title, DataTableLib.dtGroup.col.CountTags, DataTableLib.dtTag.col.Group);

            //
            TreeLib.DrawTreeSGT();
            TreeLib.DrawTreeBlock();
            TreeLib.DrawTreeStructure();
            TreeLib.DrawTreeStructure();

            //
            SetComboPlaceholder();

        }

        // ---

        #region Last open files

        // Заполняем меню "Последние файлы"
        private void UpdateRecentFilesMenu()
        {
            ToolStripMenuItemLastFiles.DropDownItems.Clear();

            List<string> recentFiles = FileControl.LoadRecentFiles();

            if (recentFiles.Count == 0)
            {
                ToolStripMenuItemLastFiles.DropDownItems.Add("Нет недавних файлов").Enabled = false;
                return;
            }

            foreach (string file in recentFiles)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(file);
                item.Click += (sender, e) => OpenProjectFromRecent(file);
                ToolStripMenuItemLastFiles.DropDownItems.Add(item);
            }
        }

        // Открыть проект из списка последних файлов
        private async void OpenProjectFromRecent(string filePath)
        {
            await OpenProjectAsync(false, filePath);
        }

        #endregion

        #endregion

        // ================================================================================================================

        #region Source

        #region Source.Event
        private void checkBoxSourceEditor_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceDesc_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CheckColumns();
        }

        private void checkBoxSourceStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CheckColumns();
        }
        private void buttonSourceCopy_Click(object sender, EventArgs e)
        {
            DataTableLib.dtSource.CopyDGVRow();
        }
        private void buttonSourceDel_Click(object sender, EventArgs e)
        {
            DataTableLib.dtSource.DelDGVRow();
        }
        private void buttonSourceHelp_Click(object sender, EventArgs e)
        {
            DataTableLib.dtSource.Help();
        }
        #endregion

        #region Source.Filter

        #region Source.TextFilter.Event
        private void textBoxSourceFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxSourceFilter.Text))
                DataTableLib.dtSource.TextFilter();
        }

        private void buttonSourceFilter_Click(object sender, EventArgs e)
        {
            DataTableLib.dtSource.TextFilter();
        }

        #endregion

        

        #endregion

        #region Source.DGV.Event

        private void dataGridViewSource_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewSource_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeSGT();
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

        #region Group.Event

        private void checkBoxGroupEditor_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupDesc_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }

        private void checkBoxGroupSource_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.CheckColumns();
        }
        #endregion

        #region Group.Filter

        #region Group.ComboFilter.Event

        private void comboBoxGroupFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtGroup.TextFilter();
        }

        private void comboBoxGroupFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxGroupFilterSource.Text))
                DataTableLib.dtGroup.TextFilter();
        }
        
        #endregion

        #region Group.TextFilter.Event
        private void textBoxGroupFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxGroupFilter.Text))
                DataTableLib.dtGroup.TextFilter();
        }
        #endregion

        private void buttonGroupFilter_Click(object sender, EventArgs e)
        {
            GroupFilter();
        }

        private void GroupFilter()
        {
            FormLib.SaveTextComboBox(comboBoxGroupFilterSource);
            DataTableLib.dtGroup.TextFilter();
        }

        #endregion

        #region Group.DGV.Event

        private void dataGridViewGroup_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridViewGroup_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeSGT();
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

        #region Tag.Event
        private void checkBoxTagEditor_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagRuntime_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagDesc_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagStatistic_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagBP_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }

        private void checkBoxTagSG_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }
        private void checkBoxTagSave_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }
        private void checkBoxTagAddress_CheckedChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.CheckColumns();
        }
        private void buttonTagHelp_Click(object sender, EventArgs e)
        {
            DataTableLib.dtTag.Help();
        }
        #endregion

        #region Tag.Filter

        #region Tag.ComboFilter.Event

        private void comboBoxTagFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterSource.Text))
                DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterGroup_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterGroup.Text))
                DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterBlock_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterBlock.Text))
                DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTableLib.dtTag.TextFilter();
        }

        private void comboBoxTagFilterPage_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxTagFilterPage.Text))
                DataTableLib.dtTag.TextFilter();
        }

        #endregion

        #region Tag.TextFilter.Event
        private void textBoxTagFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxTagFilter.Text))
                DataTableLib.dtTag.TextFilter();
        }
        #endregion

        private void buttonTagFilter_Click(object sender, EventArgs e)
        {
            TagFilter();
        }

        private void TagFilter()
        {
            FormLib.SaveTextComboBox(comboBoxTagFilterSource);
            FormLib.SaveTextComboBox(comboBoxTagFilterGroup);
            FormLib.SaveTextComboBox(comboBoxTagFilterBlock);
            FormLib.SaveTextComboBox(comboBoxTagFilterPage);
            DataTableLib.dtTag.TextFilter();
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
            if (dataGridViewTag.CurrentCell.ColumnIndex == DataTableLib.dtTag.col.Group)
            {
                DataTableLib.dtTag.UpdateDGVTagSourceLink();
            }
            TreeLib.DrawTreeSGT();
            TreeLib.DrawTreeBlock();
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

        #region Include.Event
        private void buttonIncludeLeft_Click(object sender, EventArgs e)
        {
            splitContainerInclude.Panel2Collapsed = !splitContainerInclude.Panel2Collapsed;
            SetComboBoxIncludeChildFilterInclude();
        }

        private void dataGridViewInclude_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewInclude); // new ID
        }
        #endregion

        #region Include.Filter

        #region Include.TextFilter.Event

        private void textBoxIncludeFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxIncludeFilter.Text))
                DataTableLib.dtInclude.IncludeFilter();
        }
        private void buttonIncludeFilter_Click(object sender, EventArgs e)
        {
            DataTableLib.dtInclude.IncludeFilter();
        }
        

        #endregion

        #endregion

        private void dataGridViewInclude_SelectionChanged(object sender, EventArgs e)
        {
            FormLib.SaveTextComboBox(comboBoxIncludeChildFilterParent);
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
            DataTableLib.dtIncludeChild.IncludeChildFilter();
        }
        private void comboBoxChangeFilterInclude_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxIncludeChildFilterParent.Text))
                DataTableLib.dtIncludeChild.IncludeChildFilter();
        }
        #endregion

        #region IncludeChild.TextFilter.Event

        private void textBoxChangeFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxIncludeChildFilter.Text))
                DataTableLib.dtIncludeChild.IncludeChildFilter();
        }

        private void buttonChangeFilter_Click(object sender, EventArgs e)
        {
            IncludeChildFilter();
        }

        private void IncludeChildFilter()
        {
            FormLib.SaveTextComboBox(comboBoxIncludeChildFilterParent);
            DataTableLib.dtIncludeChild.IncludeChildFilter();
        }

        private void SetComboBoxIncludeChildFilterInclude()
        {
            string text = (splitContainerInclude.Panel1Collapsed) ? "" : (DataTableLib.GetValueFromCurrentRow(dataGridViewInclude, DataTableLib.dtInclude.col.Prefix));
            comboBoxIncludeChildFilterParent.Text = text;
            DataTableLib.dtIncludeChild.IncludeChildFilter();
        }

        

        private void dataGridViewIncludeChild_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewIncludeChild); // new ID

            DataTableLib.SetParentInRow(dataGridViewIncludeChild, comboBoxIncludeChildFilterParent, DataTableLib.dtIncludeChild.col.Prefix); // filter

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
            DataTableLib.ForNewRow(dataGridViewStructure);
        }
        #endregion

        #region Structure.Filter

        #region Structure.TextFilter.Event
        private void textBoxStructureFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxStructureFilter.Text))
                DataTableLib.dtStructure.StructureFilter();
        }
        private void buttonStructureFilter_Click(object sender, EventArgs e)
        {
            DataTableLib.dtStructure.StructureFilter();
        }

        

        #endregion
        private void dataGridViewStructure_SelectionChanged(object sender, EventArgs e)
        {
            FormLib.SaveTextComboBox(comboBoxStructureTargetFilterParent);
            SetComboBoxTargetFilterStructure();
        }

        private void SetComboBoxTargetFilterStructure()
        {
            string text = (splitContainerStructure.Panel1Collapsed) ? "" : (DataTableLib.GetValueFromCurrentRow(dataGridViewStructure, DataTableLib.dtStructure.col.Title));
            comboBoxStructureTargetFilterParent.Text = text;
            DataTableLib.dtTarget.StructureTargetFilter();
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
            DataTableLib.dtTarget.StructureTargetFilter();
        }
        private void comboBoxTargetFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxStructureTargetFilterParent.Text))
                DataTableLib.dtTarget.StructureTargetFilter();
        }

        private void buttonTargetFilter_Click(object sender, EventArgs e)
        {
            TargetFilter();
        }

        private void TargetFilter()
        {
            FormLib.SaveTextComboBox(comboBoxStructureTargetFilterParent);
            DataTableLib.dtTarget.StructureTargetFilter();
        }

        private void textBoxTargetFilter_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxStructureTargetFilter.Text))
                DataTableLib.dtTarget.StructureTargetFilter();
        }

        private void dataGridViewTarget_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewStructureTarget); // new ID

            DataTableLib.SetParentInRow(dataGridViewStructureTarget, comboBoxStructureTargetFilterParent, DataTableLib.dtTarget.col.Structure); // filter

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
        private void tabFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteTabFilter();
        }
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
            TreeLib.TreeProjContexMenu(selNode);
        }
        #endregion

        


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

                case TreeProjCategory.blockItem:
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

        // Установить значения из дерева в соответствующие поля фильтра
        private void WriteTabFilter()
        {
            var selNode = treeViewProject.SelectedNode;

            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);
            string text = tpt.text; // selNode.Text;

            // Категория из дерева
            switch (tpt.category)
            {
                case TreeProjCategory.sources:
                    break;

                case TreeProjCategory.sourceItem:
                    comboBoxGroupFilterSource.Text = text;
                    comboBoxTagFilterSource.Text = text;
                    //DataTableLib.dtGroup.TextFilter();
                    //DataTableLib.dtTag.TextFilter();
                    GroupFilter();
                    TagFilter();
                    break;

                case TreeProjCategory.groupItem:
                    comboBoxTagFilterGroup.Text = text;
                    //DataTableLib.dtTag.TextFilter();
                    GroupFilter();
                    break;

                case TreeProjCategory.tagItem:
                    break;

                case TreeProjCategory.blockItem:
                    comboBoxTagFilterBlock.Text = text;
                    //DataTableLib.dtTag.TextFilter();
                    TagFilter();
                    break;

                case TreeProjCategory.structures:
                    comboBoxStructureTargetFilterParent.Text = text;
                    TargetFilter();
                    break;

                case TreeProjCategory.targetItem:
                    break;

                case TreeProjCategory.includes:
                    comboBoxIncludeChildFilterParent.Text = text;
                    IncludeChildFilter();
                    break;

                case TreeProjCategory.includeItem:
                    break;

            }

        }

        #endregion


        #endregion

        #region TAB

        #region TAB.Event
        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            var selNode = treeViewProject.SelectedNode;
            string text = selNode.Text;
            Clipboard.SetText(text);
        }
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            string tabName = tabControlProject.TabPages[tabControlProject.SelectedIndex].Name;
            switch (tabName)
            {
                case "tabPageSource":
                    DataTableLib.SetCountTagForUsed(dataGridViewSource, dataGridViewTag, DataTableLib.dtSource.col.Title, DataTableLib.dtSource.col.CountTags, DataTableLib.dtTag.col.Source);
                    break;

                case "tabPageGroup":
                    DataTableLib.SetCountTagForUsed(dataGridViewGroup, dataGridViewTag, DataTableLib.dtGroup.col.Title, DataTableLib.dtGroup.col.CountTags, DataTableLib.dtTag.col.Group);
                    break;

                case "tabPageTag":
                    DataTableLib.dtTag.UpdateDGVTagSourceLink();
                    break;

            }

            //DrawTreeSGT();
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
                    DataTableLib.ShowRow(dataGridViewSource, Id, title, DataTableLib.dtSource.col.Title);
                    break;

                case TreeProjCategory.groupItem:
                    tabControlProject.SelectTab(tabPageGroup);
                    DataTableLib.ShowRow(dataGridViewGroup, Id, title, DataTableLib.dtGroup.col.Title);
                    break;

                case TreeProjCategory.tagItem:
                    tabControlProject.SelectTab(tabPageTag);
                    DataTableLib.ShowRow(dataGridViewTag, Id, title, DataTableLib.dtTag.col.Title);
                    break;

                case TreeProjCategory.blockItem:
                    tabControlProject.SelectTab(tabPageTag);
                    break;


                case TreeProjCategory.structures:
                    tabControlProject.SelectTab(tabPageStructure);
                    break;

                case TreeProjCategory.structureItem:
                    tabControlProject.SelectTab(tabPageStructure);
                    DataTableLib.ShowRow(dataGridViewStructure, Id, title, DataTableLib.dtStructure.col.Title);
                    break;

                case TreeProjCategory.targetItem:
                    tabControlProject.SelectTab(tabPageStructure);
                    DataTableLib.ShowRow(dataGridViewStructureTarget, Id, title, DataTableLib.dtTarget.col.Tag); // ?
                    break;


                case TreeProjCategory.includes:
                    tabControlProject.SelectTab(tabPageInclude);
                    break;

                case TreeProjCategory.includeItem :
                    tabControlProject.SelectTab(tabPageInclude);
                    DataTableLib.ShowRow(dataGridViewInclude, Id, title, DataTableLib.dtInclude.col.Prefix); // ?
                    break;

                case TreeProjCategory.changeItem:
                    tabControlProject.SelectTab(tabPageInclude);
                    DataTableLib.ShowRow(dataGridViewIncludeChild, Id, title, DataTableLib.dtIncludeChild.col.ChangeFrom); // ?
                    break;
            }
        }
        #endregion

        // ======================================================================

        private void SetComboPlaceholder()
        {
            return;
            InitializeComboBoxWithDefaultItem(comboBoxGroupFilterSource, "Источник");
            InitializeComboBoxWithDefaultItem(comboBoxTagFilterSource, "Источник");
            InitializeComboBoxWithDefaultItem(comboBoxTagFilterGroup, "Группа");
            InitializeComboBoxWithDefaultItem(comboBoxTagFilterBlock, "Блок");
            InitializeComboBoxWithDefaultItem(comboBoxTagFilterPage, "Страница");
            InitializeComboBoxWithDefaultItem(comboBoxStructureTargetFilterParent, "Структура");
            InitializeComboBoxWithDefaultItem(comboBoxIncludeChildFilterParent, "Класс");
        }

        private void InitializeComboBoxWithDefaultItem(ComboBox comboBox, string placeholderText)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add(placeholderText);
            comboBox.SelectedIndex = 0;
            comboBox.ForeColor = Color.Gray;

            comboBox.SelectedIndexChanged += (s, e) =>
            {
                comboBox.ForeColor = (comboBox.SelectedIndex == 0) ? Color.Gray : Color.Black;
            };
        }

        // ================================================================================================================

        // TEST
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeLib.DrawTreeSGT(); // Построение дерева Источники/Группы/Теги
            TreeLib.DrawTreeBlock(); // Построить дерево для Блоков
            TreeLib.DrawTreeStructure(); // Построить дерево для Структур
            TreeLib.DrawTreeInclude(); // Построить дерево для Классов
        }

        private void testTagSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTableLib.dtTag.UpdateDGVTagSourceLink();
        }

        private void buttonTagCopy_Click(object sender, EventArgs e)
        {

        }

        private void buttonGroupCopy_Click(object sender, EventArgs e)
        {

        }


        // Открыть форму дизайна
        private void ToolStripMenuItemDesign_Click(object sender, EventArgs e)
        {
            var design = new FormDesign
            {
            };

            design.Show();
        }


    }
}
