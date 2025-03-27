using DML.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public static class FileControl
{
    const int version = 1000; // версия

    public static readonly ILogger logger;

    private static readonly string RecentFilesPath = Path.Combine(Application.StartupPath, "files.txt");
    private static readonly int MaxRecentFiles = 10; // Храним до 10 последних файлов

    static FileControl()
    {
        // Получаем логгер на основе нужного лог-таргета
        logger = BaseLogger.GetLogger(LogTarget.FileConsoleForm);
    }

    // Прочитать JSON-файл
    public static string LoadFromFile(ref string fileName, out string path, bool select = false, bool showNotFound = true, string filter = @"JSON-файл (*.json)|*.json")
    {
        path = string.Empty;

        string fullFileName = (select || string.IsNullOrEmpty(fileName)) ? SelectFileDialog(filter) : GetFullFilePath(fileName);
        if (string.IsNullOrEmpty(fullFileName))
            return null; // Пользователь нажал "Отмена" или путь некорректный

        if (!File.Exists(fullFileName))
        {
            if (showNotFound)
                ShowError($"Файл {fullFileName} не найден");
            return null;
        }

        fileName = Path.GetFileName(fullFileName);
        path = Path.GetDirectoryName(fullFileName) ?? string.Empty;

        return ReadFileContent(fullFileName, Encoding.Default);
    }

    // Записать JSON-файл
    public static void SaveToFile(ref string fileName, out string path, string json, string filter = @"JSON-файл (*.json)|*.json")
    {
        path = string.Empty;
        if (string.IsNullOrWhiteSpace(json))
            return;

        string fullFileName = string.IsNullOrEmpty(fileName) ? SaveFileDialog(filter) : GetFullFilePath(fileName);
        if (string.IsNullOrEmpty(fullFileName))
            return; // Пользователь нажал "Отмена" или путь некорректный

        fileName = Path.GetFileName(fullFileName);
        path = Path.GetDirectoryName(fullFileName) ?? string.Empty;

        WriteFileContent(fullFileName, json, Encoding.Default);
    }

    // Прочитать XML-файл
    public static string LoadSubstitutions(string fileName)
    {
        string fullFileName = GetFullFilePath(fileName);
        if (string.IsNullOrEmpty(fullFileName) || !File.Exists(fullFileName))
            return null;

        return ReadFileContent(fullFileName, Encoding.UTF8);
    }

    // Image из файла в String (Base64)
    public static string ImageFileToByteString(string fileName)
    {
        string fullFileName = GetFullFilePath(fileName);
        if (string.IsNullOrEmpty(fullFileName) || !File.Exists(fullFileName))
            return null;

        try
        {
            byte[] imageArray = File.ReadAllBytes(fullFileName);
            return Convert.ToBase64String(imageArray);
        }
        catch (Exception ex)
        {
            logger.Error(ex.HResult, $"Ошибка чтения файла {fullFileName}: {ex.Message}", eMessageCategory.FILE);
            return null;
        }
    }

    // ========================================================================

    // Добавить файл в список последних открытых
    public static void AddToRecentFiles(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;

        List<string> files = LoadRecentFiles();

        // Удаляем старую запись, если файл уже был в списке
        files.Remove(filePath);
        files.Insert(0, filePath); // Добавляем в начало списка

        // Ограничиваем количество записей
        if (files.Count > MaxRecentFiles)
            files = files.Take(MaxRecentFiles).ToList();

        // Сохраняем обновленный список
        try
        {
            File.WriteAllLines(RecentFilesPath, files);
        }
        catch (Exception ex)
        {
            logger.Error(ex.HResult, $"Ошибка сохранения списка последних файлов: {ex.Message}", eMessageCategory.FILE);
        }
    }

    // Загрузить список последних файлов
    public static List<string> LoadRecentFiles()
    {
        if (!File.Exists(RecentFilesPath)) return new List<string>();

        try
        {
            return File.ReadAllLines(RecentFilesPath).Where(File.Exists).ToList();
        }
        catch (Exception ex)
        {
            logger.Error(ex.HResult, $"Ошибка загрузки списка последних файлов: {ex.Message}", eMessageCategory.FILE);
            return new List<string>();
        }
    }

    // ======================== Вспомогательные методы ========================

    private static string GetFullFilePath(string fileName)
    {
        return string.IsNullOrWhiteSpace(fileName) ? null :
            Path.IsPathRooted(fileName) ? fileName : Path.Combine(Application.StartupPath, fileName);
    }

    private static string ReadFileContent(string filePath, Encoding enc)
    {
        try
        {
            return File.ReadAllText(filePath, enc);
        }
        catch (Exception ex)
        {
            logger.Error(ex.HResult, $"Ошибка открытия файла {filePath}: {ex.Message}", eMessageCategory.FILE);
            ShowError($"Ошибка открытия файла {Path.GetFileName(filePath)}: {ex.Message}");
            return null;
        }
    }

    private static void WriteFileContent(string filePath, string content, Encoding enc)
    {
        try
        {
            File.WriteAllText(filePath, content, enc);
        }
        catch (Exception ex)
        {
            logger.Error(ex.HResult, $"Ошибка сохранения файла {filePath}: {ex.Message}", eMessageCategory.FILE);
            ShowError($"Ошибка сохранения файла {Path.GetFileName(filePath)}: {ex.Message}");
        }
    }

    private static string SelectFileDialog(string filter)
    {
        using (var openFileDialog = new OpenFileDialog
        {
            AddExtension = true,
            DefaultExt = "json",
            Filter = filter,
            FilterIndex = 1,
            RestoreDirectory = true
        })
        {
            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
        }
    }

    private static string SaveFileDialog(string filter)
    {
        using (var saveFileDialog = new SaveFileDialog
        {
            AddExtension = true,
            DefaultExt = "json",
            Filter = filter,
            FilterIndex = 1,
            RestoreDirectory = true
        })
        {
            return saveFileDialog.ShowDialog() == DialogResult.OK ? saveFileDialog.FileName : null;
        }
    }

    private static void ShowError(string message)
    {
        MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
