using Connector;
using DML;
using DML.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver
{
    public partial class Form1
    {
        private async Task NewProject()
        {
            loggerA.OK(SetLeftLabelMessage1("Создание нового проекта..."), eMessageCategory.App); // Лог и статус

            try
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

                loggerA.OK(SetLeftLabelMessage1("Новый проект создан"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1($"Ошибка создания нового проекта: {ex.Message}"), eMessageCategory.App);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        // Открыть проект (распаковка настроек из файла)
        private async Task OpenProjectAsync(bool select = true, string fileName = "")
        {
            loggerA.OK(SetLeftLabelMessage1("Открытие проекта..."), eMessageCategory.App); // Лог и статус

            try
            {
                // Получаем кодировку
                string encoding = toolStripComboBoxEncoding.Text;
                var enc = DecodeEncode.GetEncodingFromString(encoding);

                // Загрузка проекта JSON
                string input = FileControl.LoadFromFile(ref fileName, out string path, enc, select); // чтение из файла...

                // Далее?
                if (String.IsNullOrWhiteSpace(input))
                {
                    loggerA.Info(SetLeftLabelMessage1("Отмена открытия проекта"), eMessageCategory.App); // Лог и статус
                    return;
                }

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

                loggerA.OK(SetLeftLabelMessage1("Проект открыт"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1($"Ошибка открытия проекта: {ex.Message}"), eMessageCategory.App);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Положить на форму
        private void ProjectToForm()
        {
            loggerA.OK(SetLeftLabelMessage1("Передача модели в редактирование..."), eMessageCategory.App); // Лог и статус

            try
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

                loggerA.OK(SetLeftLabelMessage1("Передача модели в редактирование выполнено"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1($"Ошибка передачи модели в редактирование: {ex.Message}"), eMessageCategory.App);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Забрать из формы
        private void FormToProject()
        {
            EditorControl.sources = DataTableLib.dtSource.TableToData();
            EditorControl.groups = DataTableLib.dtGroup.TableToData();
            EditorControl.tags = DataTableLib.dtTag.TableToData();
        }

        // ---

        private async Task InportProjectFromExcel()
        {
            loggerA.OK(SetLeftLabelMessage1("Импорт проекта из Excel..."), eMessageCategory.App); // Лог и статус

            try
            {
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

                loggerA.OK(SetLeftLabelMessage1("Импорт проекта из Excel выполнен"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1($"Ошибка импорта проекта из Excel: {ex.Message}"), eMessageCategory.App);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ExportProjectToExcel()
        {
            loggerA.OK(SetLeftLabelMessage1("Экспорт проекта в Excel..."), eMessageCategory.App); // Лог и статус

            try
            {
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

                data = ProjectSettingsConverter.NormalizeAll(data);

                // Экспорт в Excel секции Data
                await ExcelJsonConverter.JsonToExcelAsync(data, file, progress);

                loggerA.OK(SetLeftLabelMessage1("Экспорт проекта в Excel выполнен"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1($"Ошибка экспорта проекта в Excel: {ex.Message}"), eMessageCategory.App);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ---

        // Открыть проект (распаковка настроек из файла)
        private async Task SaveProjectAsync(bool select = true, string fileName = "")
        {
            loggerA.OK(SetLeftLabelMessage1("Сохранение проекта..."), eMessageCategory.App); // Лог и статус

            try
            {
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

                loggerA.OK(SetLeftLabelMessage1("Проект сохранен"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1($"Ошибка сохранения проекта: {ex.Message}"), eMessageCategory.App);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private async Task JsonToModels()
        {
            loggerA.OK(SetLeftLabelMessage1("Передача Json в модели..."), eMessageCategory.App); // Лог и статус

            try
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
                loggerA.OK(SetLeftLabelMessage1("Передача Json в модели завершена"), eMessageCategory.App); // Лог и статус
            }
            catch (Exception ex)
            {
                loggerA.Error(ex.HResult, SetLeftLabelMessage1($"Ошибка передачи Json в модели: {ex.Message}"), eMessageCategory.App);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
