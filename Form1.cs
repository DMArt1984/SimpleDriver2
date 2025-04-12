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
using DML.Log;
using DML;
using System.IO;
using WinSimpleIDriver.Editor;
using Connector;

namespace WinSimpleIDriver
{
    
    public partial class Form1 : Form
    {
        // Теги
        DataTable dtTags;

        // logger 
        //ProcessMaster _master;
        public readonly ILogger loggerA = BaseLogger.GetLogger(LogTarget.FileConsoleForm);
        public readonly ILogger loggerB = BaseLogger.GetLogger(LogTarget.FileOnly);
        LabelLogger lLeft;
        LabelLogger lRight;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Заголовок
            //this.Text += $" {Settings.settingsFileName}";
            AppTitle(Settings.settingsFileName, "");
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

            #region Log DGV
            // Для вывода в toolStripStatusLabel
            lLeft = new LabelLogger(DrawLabelLeft);
            lRight = new LabelLogger(DrawLabelRight);

            // Привязываем делегат для логирования:
            // При поступлении лог-сообщения делегат добавляет новую строку в dataGridViewLog.
            FormLogger.Instance.FormLogDelegate = (eMessageType mt, eMessageCategory category, int code, string message) =>
            {
                //if (!true)
                //    return; // Логирование в таблицу отключено

                // Так как обновление UI должно происходить в главном потоке, используем Invoke.
                if (dataGridViewLog.InvokeRequired)
                {
                    dataGridViewLog.Invoke(new Action(() => AddLogRow(mt, category, code, message)));
                }
                else
                {
                    AddLogRow(mt, category, code, message);
                }
            };

            #endregion

            toolStripComboBoxEncoding.SelectedIndex = 0; // UTF-8

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

            // Form: StructureTarget
            DataTableLib.dtStructTarget.LinkColumns(dataGridViewStructureTarget);
            DataTableLib.dtStructTarget.tbFilter = null; // textBoxStructureFilter; // textBoxStructureTargetFilter;
            DataTableLib.dtStructTarget.coFilterParent = comboBoxStructureTargetFilterParent;

            // Form: StructureTag
            DataTableLib.dtStructTag.LinkColumns(dataGridViewStructureTag);
            DataTableLib.dtStructTag.tbFilter = null; // textBoxStructureFilter; // textBoxStructureTargetFilter;
            DataTableLib.dtStructTag.coFilterParent = comboBoxStructureTargetFilterParent;


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
            ToolStripMenuItemViewTree.Checked = false;
            bool check = ToolStripMenuItemViewTree.Checked;
            splitContainerTreeMain.Panel1Collapsed = !check;
            // Вид - Лог
            splitContainerLogMain.Panel2Collapsed = true;
            #endregion

            // Combo
            SetComboPlaceholder();

            // Очистить проект
            FormClear();

            // DataTables
            dtTags = DataTableLib.GetEmptyDataTableForTags(dataGridViewTag, "Tags");

            // Лог и статус
            loggerA.OK(SetLeftLabelMessage1("Приложение WinSimpleDriver запущено"), eMessageCategory.App);

        }

        // ================================================================================================================

        private void AppTitle(string settings, string fileName)
        {
            this.Text = ($"WinSimpleDriver {fileName} {settings}").Trim();
            notifyIcon1.Text = this.Text;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

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
        private async void ToolStripMenuItemNew_Click(object sender, EventArgs e)
        {
            await NewProject();
        }

        private async void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            await OpenProjectAsync(true);
        }

        private async void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            await SaveProjectAsync(false, EditorControl.fullFileName);
        }

        private async void ToolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            await SaveProjectAsync(true);
        }

        private async void ToolStripMenuItemImport_Click(object sender, EventArgs e)
        {
            await InportProjectFromExcel();
        }

        private async void ToolStripMenuItemExport_Click(object sender, EventArgs e)
        {
            await ExportProjectToExcel();
        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            loggerA.OK(SetLeftLabelMessage1("Выход из приложения!"), eMessageCategory.App); // Лог и статус
            this.Close();
        }

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

        private void checkBoxIncludePrefix_CheckedChanged(object sender, EventArgs e)
        {
            var check = checkBoxIncludePrefix.Checked;
            dataGridViewIncludeChild.Columns[DataTableLib.dtIncludeChild.col.Prefix].Visible = check;
        }

        #endregion

        // ================================================================================================================

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

                case TreeProjCategory.structTagItem:
                    tabControlProject.SelectTab(tabPageStructure);
                    DataTableLib.ShowRow(dataGridViewStructureTag, Id, title, DataTableLib.dtStructTag.col.TagTitle); // ?
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

        // ================================================================================================================

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


        // Открыть форму дизайна
        private void ToolStripMenuItemDesign_Click(object sender, EventArgs e)
        {
            var design = new FormDesign
            {
            };

            design.Show();
        }

        private void dataGridViewStructureTag_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewStructureTag); // new ID
            DataTableLib.SetParentInRow(dataGridViewStructureTag, comboBoxStructureTargetFilterParent, DataTableLib.dtStructTag.col.Structure); // filter

        }


        private void dataGridViewInclude_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeInclude();
        }

        private void dataGridViewIncludeChild_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeInclude();
        }

        // ====================================================================================================

        #region Project file
        private void toolStripButtonNormalize_Click(object sender, EventArgs e)
        {
            // NormalizeAll
            ApplyNormalization(ProjectSettingsConverter.NormalizeAll);
        }

        private void toolStripButtonLong_Click(object sender, EventArgs e)
        {
            // ConvertToLongForm
            ApplyNormalization(ProjectSettingsConverter.ConvertToLongForm);
        }

        private void toolStripButtonShort_Click(object sender, EventArgs e)
        {
            // ConvertToShortForm
            ApplyNormalization(ProjectSettingsConverter.ConvertToShortForm);
        }

        private void toolStripButtonGroup_Click(object sender, EventArgs e)
        {
            // ConvertToGroupNestedForm
            ApplyNormalization(ProjectSettingsConverter.ConvertToGroupNestedForm);
        }

        private void toolStripButtonSource_Click(object sender, EventArgs e)
        {
            // ConvertToSourceNestedForm
            ApplyNormalization(ProjectSettingsConverter.ConvertToSourceNestedForm);
        }

        private void toolStripButtonInBlock_Click(object sender, EventArgs e)
        {
            // ReintegrateTagsToBlocks
            ApplyNormalization(ProjectSettingsConverter.ReintegrateTagsToBlocks);
        }

        private void toolStripButtonOutBlock_Click(object sender, EventArgs e)
        {
            // ExtractTagsFromBlocks
            ApplyNormalization(ProjectSettingsConverter.ExtractTagsFromBlocks);
        }

        // Статистика
        private void jsonProjStatustic()
        {
            ProjectSettingsConverter.GetJsonStatistics(richTextBoxJsonProject.Text, out int s, out int g, out int t);
            jsonProjectStatistic.Text = $"{s} - {g} - {t}";
        }

        // ------------------------------------------------------------------------------------------------------------------



        #endregion

        private async void buttonJsonToModels_Click(object sender, EventArgs e)
        {
            await JsonToModels();
        }

        private void buttonModelsToTables_Click(object sender, EventArgs e)
        {
            ProjectToForm();
        }

        

        private void buttonTablesToModels_Click(object sender, EventArgs e)
        {
            FormToProject();
        }

        private async void buttonModelsToJson_Click(object sender, EventArgs e)
        {
            await ModelsToJson();
        }

        // Новый проект после загрузки формы
        private async void Form1_Shown(object sender, EventArgs e)
        {
            await NewProject();
        }

        // Запуск Runtime
        private async void buttonModelsToRuntime_Click(object sender, EventArgs e)
        {
            loggerA.OK(SetLeftLabelMessage1("Запуск Runtime..."), eMessageCategory.App); // Лог и статус
            try
            {
                await Task.Run(() => ProjectRuntime.StartRuntime(this, loggerA));
                loggerA.OK(SetLeftLabelMessage1("Runtime запущен"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1("Ошибка запуска Runtime: " + ex.Message), eMessageCategory.App);
            }
        }

        // Стоп Runtime
        private async void buttonStopRuntime_Click(object sender, EventArgs e)
        {
            loggerA.OK(SetLeftLabelMessage1("Останов Runtime..."), eMessageCategory.App); // Лог и статус
            try
            {
                await Task.Run(() => ProjectRuntime.StopRuntime(this, loggerA));
                loggerA.OK(SetLeftLabelMessage1("Runtime остановлен"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1("Ошибка остановки Runtime: " + ex.Message), eMessageCategory.App);
            }
        }

        #region Group

        #region Group.Event
        private void buttonGroupView_Click(object sender, EventArgs e)
        {

        }
        private void buttonGroupDel_Click(object sender, EventArgs e)
        {

        }
        private void buttonGroupCopy_Click(object sender, EventArgs e)
        {

        }
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
        private void buttonSourceView_Click(object sender, EventArgs e)
        {

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
        private void buttonTagView_Click(object sender, EventArgs e)
        {

        }
        private void buttonTagDel_Click(object sender, EventArgs e)
        {

        }
        private void buttonTagCopy_Click(object sender, EventArgs e)
        {

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
            {
                DataTableLib.dtStructure.StructureFilter();
                //DataTableLib.dtStructTarget.StructureTargetFilter();
            }
        }
        private void buttonStructureFilter_Click(object sender, EventArgs e)
        {
            DataTableLib.dtStructure.StructureFilter();
            //DataTableLib.dtStructTarget.StructureTargetFilter();
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
            DataTableLib.dtStructTarget.StructureTargetFilter();
            DataTableLib.dtStructTag.StructureTagFilter();
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
            DataTableLib.dtStructTarget.StructureTargetFilter();
            DataTableLib.dtStructTag.StructureTagFilter();
        }
        private void comboBoxTargetFilterSource_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(comboBoxStructureTargetFilterParent.Text))
                DataTableLib.dtStructTarget.StructureTargetFilter();
        }

        private void buttonTargetFilter_Click(object sender, EventArgs e)
        {
            TargetAndTagFilter();
        }

        private void TargetAndTagFilter()
        {
            FormLib.SaveTextComboBox(comboBoxStructureTargetFilterParent);
            DataTableLib.dtStructTarget.StructureTargetFilter();
            DataTableLib.dtStructTag.StructureTagFilter();
        }

        private void dataGridViewTarget_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            DataTableLib.ForNewRow(dataGridViewStructureTarget); // new ID

            DataTableLib.SetParentInRow(dataGridViewStructureTarget, comboBoxStructureTargetFilterParent, DataTableLib.dtStructTarget.col.Structure); // filter

        }





        #endregion

        #endregion

        #endregion


        private void checkBoxStructCol_CheckedChanged(object sender, EventArgs e)
        {
            var check = checkBoxStructCol.Checked;
            dataGridViewStructureTag.Columns[DataTableLib.dtStructTag.col.Structure].Visible = check;
            dataGridViewStructureTarget.Columns[DataTableLib.dtStructTarget.col.Structure].Visible = check;

        }


        #endregion

        private void dataGridViewStructure_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeStructure();
        }

        private void dataGridViewStructureTag_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeStructure();
        }

        private void dataGridViewStructureTarget_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeStructure();
        }

    }
}
