using DML.Log;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

public static class FileControl
{
    static int ver = 100; // версия

    // Прочитать JSON-файл
    public static string LoadFromFile(ref string fileName, out string path, bool select = false, bool showNotFound = true, string filter = @"JSON-файл (*.json)|*.json")
    {
        path = string.Empty;

        string fullFileName = select ? SelectFileDialog(filter) : GetFullFilePath(fileName);
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
            LogHelper.Log($"Ошибка чтения файла {fullFileName}", ex);
            return null;
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
            LogHelper.Log($"Ошибка открытия файла {filePath}", ex);
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
            LogHelper.Log($"Ошибка сохранения файла {filePath}", ex);
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
