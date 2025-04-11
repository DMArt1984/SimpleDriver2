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


    }
}
