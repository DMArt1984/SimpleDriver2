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
            bool check = ToolStripMenuItemViewTree.Checked;
            splitContainerTreeMain.Panel1Collapsed = !check;

            #endregion

            // Combo
            SetComboPlaceholder();

            // Очистить проект
            FormClear();

            // DataTables
            dtTags = DataTableLib.GetEmptyDataTableForTags(dataGridViewTag, "Tags");

            SetLeftLabelMessage1();

            //var codeMessage = CodeMessageFactory.FromEnum(eSourceStatus.closed);
            //MessageBox.Show(Settings.x);
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

        #region Status
        // Установить сообщение 1
        private string SetLeftLabelMessage1(string message = "")
        {
            toolStripStatusLabelMessage1.Text = message;
            //ProcessMaster _tempLog = new ProcessMaster(LogTarget.FileOnly);
            //_tempLog.Info(message);
            loggerB.Info(message);
            return message;
        }
        // Установить сообщение 2
        private string SetMidLabelMessage2(string message = "")
        {
            toolStripStatusLabelMessage2.Text = message;
            //ProcessMaster _tempLog = new ProcessMaster(LogTarget.FileOnly);
            //_tempLog.Info(message);
            loggerB.Info(message);
            return message;
        }
        // Установить сообщение 3
        private string SetRightLabelMessage3(string message = "")
        {
            toolStripStatusLabelMessage3.Text = message;
            //ProcessMaster _tempLog = new ProcessMaster(LogTarget.FileOnly);
            //_tempLog.Info(message);
            loggerB.Info(message);
            return message;
        }

        #endregion


        #region LOG

        /// <summary>
        /// Добавляет строку в dataGridViewLog с информацией о логе и меняет цвет фона строки в зависимости от типа сообщения.
        /// После добавления строки применяется текущая сортировка таблицы.
        /// Если в таблице более 100 строк, то удаляются 10 строк с наименьшим значением logID.
        /// </summary>
        /// <param name="mt">Тип сообщения (например, OK, info, error).</param>
        /// <param name="category">Категория лога.</param>
        /// <param name="code">Код сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        private void AddLogRow(eMessageType mt, eMessageCategory category, int code, string message)
        {
            // Добавляем новую строку в dataGridViewLog
            int rowIndex = dataGridViewLog.Rows.Add();
            DataGridViewRow row = dataGridViewLog.Rows[rowIndex];

            // Заполняем ячейки данными
            row.Cells["logDT"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            row.Cells["logCategory"].Value = category.ToString();
            row.Cells["logType"].Value = mt.ToString();
            row.Cells["logCode"].Value = code.ToString();
            row.Cells["logText"].Value = message;

            // Меняем цвет фона строки в зависимости от типа сообщения
            switch (mt)
            {
                case eMessageType.OK:
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    break;
                case eMessageType.ERROR:
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                    break;
                default:
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    break;
            }

            // Если в таблице более 100 строк, удаляем 10 строк с наименьшим значением logID
            int maxRows = 100;
            int removeRows = 10;
            if (dataGridViewLog.Rows.Count > maxRows)
            {
                var rowsToRemove = dataGridViewLog.Rows
                    .Cast<DataGridViewRow>()
                    .OrderBy(r => (r.Cells["logDT"].Value))
                    .Take(removeRows)
                    .ToList();
                foreach (var r in rowsToRemove)
                {
                    dataGridViewLog.Rows.Remove(r);
                }
            }

            // Применяем текущую сортировку таблицы, если она задана
            if (dataGridViewLog.SortedColumn != null)
            {
                // Определяем направление сортировки
                ListSortDirection direction = dataGridViewLog.SortOrder == SortOrder.Ascending ?
                                                ListSortDirection.Ascending : ListSortDirection.Descending;
                // Сортируем по текущему отсортированному столбцу с указанным направлением
                dataGridViewLog.Sort(dataGridViewLog.SortedColumn, direction);
            }
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
        private async void ToolStripMenuItemNew_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Новый проект");
            //FormClear();

            await NewProject();

        }

        private async Task NewProject()
        {
            string input = ProjectSettingsConverter.CheckSectionData("");
            richTextBoxJsonProject.Text = input;
            // Нарисовать дерево
            JsonTreeViewHelper.PopulateTreeViewFromJson(input, treeViewJsonProject);

            // распаковка проекта
            await Task.Run(() =>
            {
                EditorControl.UnpackProject(input);
            });

            // Обновление UI (обновление меню и формы)
            ProjectToForm();

            //
            AppTitle(Settings.settingsFileName, "");
        }

        private async void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Открыть проект");
            await OpenProjectAsync(true);
        }

        private async void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Сохранить проект");
            await SaveProjectAsync(false, EditorControl.fullFileName);
        }

        private async void ToolStripMenuItemSaveAs_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Сохранить проект как...");
            await SaveProjectAsync(true);
        }

        private async void ToolStripMenuItemImport_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Импорт проекта");
            string file = FileControl.SelectExcelImportFile();
            if (string.IsNullOrEmpty(file))
                return;

            // Создаем объект IProgress<int>, который обновляет метку lblStatus
            IProgress<string> progress = new Progress<string>(processed =>
            {
                // Обновление UI происходит в UI-потоке автоматически
                jsonProjectStatistic.Text = $"{processed}";
            });

            // Импорт из Excel секции Data
            string data = await ExcelJsonConverter.ExcelToJsonAsync(file, progress);

            // Замена секции Data на нормализованную
            string input = richTextBoxJsonProject.Text;
            input = ProjectSettingsConverter.CheckSectionData(input);

            //
            string json = ProjectSettingsConverter.ReplaceSection(input, "Data", data);

            //
            JsonFormViewer.DisplayColoredJson(richTextBoxJsonProject, json);
            jsonProjStatustic();

            // Нарисовать дерево
            JsonTreeViewHelper.PopulateTreeViewFromJson(json, treeViewJsonProject);
        }

        private async void ToolStripMenuItemExport_Click(object sender, EventArgs e)
        {
            SetLeftLabelMessage1("Экспорт проекта");
            string file = FileControl.SelectExcelExportFile();
            if (string.IsNullOrEmpty(file))
                return;

            // Создаем объект IProgress<int>, который обновляет метку lblStatus
            IProgress<int> progress = new Progress<int>(processed =>
            {
                // Обновление UI происходит в UI-потоке автоматически
                jsonProjectStatistic.Text = $"Обработано тегов: {processed}";
            });

            string json = richTextBoxJsonProject.Text;
            json = ProjectSettingsConverter.CheckSectionData(json);

            // Извлечение секции Data
            string data = ProjectSettingsConverter.ExtractSection(json, "Data");

            data= ProjectSettingsConverter.NormalizeAll(data);

            // Экспорт в Excel секции Data
            await ExcelJsonConverter.JsonToExcelAsync(data, file, progress);

        }

        // Новый проект
        private void FormClear()
        {
            // окно файла проекта
            richTextBoxJsonProject.Text = "";

            // DGV
            DataTableLib.Clear();

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

            // Получаем кодировку
            string encoding = toolStripComboBoxEncoding.Text;
            var enc = DecodeEncode.GetEncodingFromString(encoding);

            // Загрузка проекта JSON
            string input = FileControl.LoadFromFile(ref fileName, out string path, enc, select); // чтение из файла...

            // Далее?
            if (String.IsNullOrWhiteSpace(input))
                return;

            // NormalizeAll
            ApplyNormalization(ProjectSettingsConverter.NormalizeAll, input);

            // Последние файлы
            string fullFileName = Path.Combine(path, fileName);
            FileControl.AddToRecentFiles(fullFileName); // Сохранение файла в истории

            //
            EditorControl.fullFileName = fullFileName;
            AppTitle(Settings.settingsFileName, fileName);

            // распаковка проекта
            await Task.Run(() =>
            {
                EditorControl.UnpackProject(input);
            });

            // Обновление UI (обновление меню и формы)
            UpdateRecentFilesMenu();
            ProjectToForm();

            SetLeftLabelMessage1("Проект открыт!");
        }

        // Положить на форму
        private void ProjectToForm()
        {
            DataTableLib.dtSource.DataToTable(EditorControl.sources);
            DataTableLib.dtGroup.DataToTable(EditorControl.groups);
            DataTableLib.dtTag.DataToTable(EditorControl.tags);

            DataTableLib.dtStructure.DataToTable(EditorControl.structures);
            DataTableLib.dtStructTarget.DataToTable(EditorControl.structTargets);
            DataTableLib.dtStructTag.DataToTable(EditorControl.structTags);
            DataTableLib.dtInclude.DataToTable(EditorControl.includes);
            DataTableLib.dtIncludeChild.DataToTable(EditorControl.includeChilds);
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
            TreeLib.DrawTreeInclude();

            //
            SetComboPlaceholder();

        }


        // Забрать из формы
        private void FormToProject()
        {
            EditorControl.sources = DataTableLib.dtSource.TableToData();
            EditorControl.groups = DataTableLib.dtGroup.TableToData();
            EditorControl.tags = DataTableLib.dtTag.TableToData();
        }

        // ---

        // Открыть проект (распаковка настроек из файла)
        private async Task SaveProjectAsync(bool select = true, string fileName = "")
        {
            SetLeftLabelMessage1("Сохранение проекта...");

            //
            FormToProject();

            // упаковка проекта
            string output = await Task.Run(EditorControl.PackProject);

            // Вернуть на экран
            JsonFormViewer.DisplayColoredJson(richTextBoxJsonProject, output);
            jsonProjStatustic();

            // Нарисовать дерево
            JsonTreeViewHelper.PopulateTreeViewFromJson(output, treeViewJsonProject);

            // Получаем кодировку
            string encoding = toolStripComboBoxEncoding.Text;
            var enc = DecodeEncode.GetEncodingFromString(encoding);

            //
            FileControl.SaveToFile(ref fileName, out string path, output, enc); // запись в файл...

            SetLeftLabelMessage1("Проект сохранен!");
        }

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

                case TreeProjCategory.structTagItem:
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
                    TargetAndTagFilter();
                    break;

                case TreeProjCategory.structTagItem:
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

        private void dataGridViewInclude_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeInclude();
        }

        private void dataGridViewIncludeChild_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            TreeLib.DrawTreeInclude();
        }

        // ====================================================================================================

        #region LabelAndText
        private void OnlyLabel(eMessageType mtype, string message, ToolStripStatusLabel labelType = null, ToolStripStatusLabel labelMessage = null)
        {
            if (labelType != null)
            {
                labelType.ForeColor = LogHelper.GetColorForMessage(mtype);
                labelType.Text = FormText(LogHelper.TypeMessage(mtype));
            }

            if (labelMessage != null)
            {
                if (labelType == null)
                    labelMessage.ForeColor = LogHelper.GetColorForMessage(mtype);
                labelMessage.Text = FormText(message);
            }
        }

        private string FormText(string value)
        {
            int w = this.Width / 8;
            if (value.Length > w)
                return value.Substring(0, w) + "...";

            return value;
        }
        #endregion
        #region Draw Label
        private void DrawLabelLeft(eMessageType messageType, string message)
        {
            OnlyLabel(messageType, message, toolStripStatusLabelMessage1, null);
        }
        private void DrawLabelRight(eMessageType messageType, string message)
        {
            OnlyLabel(messageType, message, toolStripStatusLabelMessage2, toolStripStatusLabelMessage3);
        }

        #endregion

        // ====================================================================================================

        #region Project file

        /// <summary>
        /// Применяет переданную функцию нормализации к входной строке и возвращает результат.
        /// </summary>
        /// <param name="inputJson">Входная строка JSON, которую необходимо нормализовать.</param>
        /// <param name="normalizeFunc">
        /// Функция нормализации, принимающая строку JSON и возвращающая нормализованную строку.
        /// Например, функция NormalizeAll.
        /// </param>
        /// <returns>Строка, полученная в результате применения normalizeFunc к inputJson.</returns>
        private void ApplyNormalization(Func<string, string> normalizeFunc, string fromExternal = null)
        {
            if (normalizeFunc == null)
                throw new ArgumentNullException(nameof(normalizeFunc));

            // Получить json
            string input = "";
            if (fromExternal == null)
            {
                input = richTextBoxJsonProject.Text;
            }
            else
            {
                input = fromExternal;
            }

            // Извлечение секции Data
            string data = ProjectSettingsConverter.ExtractSection(input, "Data");

            // Нормализация секции Data
            data = normalizeFunc(data);

            // Замена секции Data на нормализованную
            input = ProjectSettingsConverter.ReplaceSection(input, "Data", data);

            // Вернуть на экран
            JsonFormViewer.DisplayColoredJson(richTextBoxJsonProject, input);
            jsonProjStatustic();

            // Нарисовать дерево
            JsonTreeViewHelper.PopulateTreeViewFromJson(input, treeViewJsonProject);

        }

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
            // Получить с экрана
            string input = richTextBoxJsonProject.Text;

            // Извлечение секции Data из JSON
            string data = ProjectSettingsConverter.ExtractSection(input, "Data");

            // Нормализация секции Data
            data = ProjectSettingsConverter.NormalizeAll(data);

            // Замена секции Data на нормализованную
            input = ProjectSettingsConverter.ReplaceSection(input, "Data", data);

            // распаковка проекта
            await Task.Run(() =>
            {
                EditorControl.UnpackProject(input);
            });

        }

        private void buttonModelsToTables_Click(object sender, EventArgs e)
        {
            ProjectToForm();
        }

        private void buttonModelsToRuntime_Click(object sender, EventArgs e)
        {

        }

        private void buttonTablesToModels_Click(object sender, EventArgs e)
        {
            FormToProject();
        }

        private async void buttonModelsToJson_Click(object sender, EventArgs e)
        {
            // упаковка проекта
            string output = await Task.Run(EditorControl.PackProject);

            // Вернуть на экран
            JsonFormViewer.DisplayColoredJson(richTextBoxJsonProject, output);
            jsonProjStatustic();

            // Нарисовать дерево
            JsonTreeViewHelper.PopulateTreeViewFromJson(output, treeViewJsonProject);

        }

        // Новый проект после загрузки формы
        private async void Form1_Shown(object sender, EventArgs e)
        {
            await NewProject();
        }
    }
}
