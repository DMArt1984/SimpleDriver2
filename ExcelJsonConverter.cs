using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class ExcelJsonConverter
{
    /// <summary>
    /// Асинхронно конвертирует нормализованный JSON в книгу Excel.
    /// В книге создаются три листа: "Sources", "Groups" и "Tags". На каждом листе строки – это объекты массива,
    /// а столбцы – свойства объектов.
    /// Параметр tagProgress позволяет оповещать о прогрессе обработки тегов (например, сколько строк обработано).
    /// </summary>
    /// <param name="json">Строка JSON с нормализованными данными (объект с массивами Sources, Groups, Tags).</param>
    /// <param name="filePath">Путь для сохранения Excel-файла.</param>
    /// <param name="tagProgress">Опциональный параметр для отчета о количестве обработанных тегов.</param>
    /// <returns>Task, завершающую операцию экспорта.</returns>
    public static async Task JsonToExcelAsync(string json, string filePath, IProgress<int> tagProgress = null)
    {
        // Парсим JSON-строку в JObject
        JObject jObject = JObject.Parse(json);

        // Преобразуем каждый массив в DataTable.
        DataTable dtSources = ConvertJArrayToDataTable(jObject["Sources"] as JArray, "Sources");
        DataTable dtGroups = ConvertJArrayToDataTable(jObject["Groups"] as JArray, "Groups");
        // Для таблицы Tags мы будем отправлять прогресс
        DataTable dtTags = await Task.Run(() => ConvertJArrayToDataTableWithProgress(jObject["Tags"] as JArray, "Tags", tagProgress));

        // Создаем книгу Excel асинхронно через Task.Run
        await Task.Run(() =>
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(dtSources, "Sources");
                workbook.Worksheets.Add(dtGroups, "Groups");
                workbook.Worksheets.Add(dtTags, "Tags");

                workbook.SaveAs(filePath);
            }
        });
    }

    /// <summary>
    /// Асинхронно конвертирует книгу Excel в JSON-строку.
    /// Ожидается, что книга содержит листы с названиями "Sources", "Groups" и "Tags".
    /// Первая строка каждого листа – заголовки, остальные строки – объекты.
    /// </summary>
    /// <param name="filePath">Путь к Excel-файлу.</param>
    /// <param name="progress">Опциональный параметр для отчета (например, окончание обработки каждого листа).</param>
    /// <returns>Task со строкой JSON.</returns>
    public static async Task<string> ExcelToJsonAsync(string filePath, IProgress<string> progress = null)
    {
        JObject result = new JObject();

        await Task.Run(() =>
        {
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                JArray arrSources = ConvertWorksheetToJArray(workbook.Worksheet("Sources"));
                progress?.Report("Лист Sources обработан");
                JArray arrGroups = ConvertWorksheetToJArray(workbook.Worksheet("Groups"));
                progress?.Report("Лист Groups обработан");
                JArray arrTags = ConvertWorksheetToJArray(workbook.Worksheet("Tags"));
                progress?.Report("Лист Tags обработан");

                result["Sources"] = arrSources;
                result["Groups"] = arrGroups;
                result["Tags"] = arrTags;
            }
        });

        return result.ToString(Formatting.Indented);
    }

    /// <summary>
    /// Преобразует JArray, содержащий объекты, в DataTable.
    /// Определяются все уникальные ключи объектов, которые становятся столбцами, а каждый объект – строкой.
    /// </summary>
    /// <param name="array">JArray с объектами.</param>
    /// <param name="tableName">Имя DataTable.</param>
    /// <returns>DataTable с данными из JArray.</returns>
    private static DataTable ConvertJArrayToDataTable(JArray array, string tableName)
    {
        DataTable dt = new DataTable(tableName);
        if (array == null)
            return dt;

        // Определяем набор столбцов как объединение всех ключей объектов массива.
        HashSet<string> columns = new HashSet<string>();
        foreach (JObject obj in array.OfType<JObject>())
        {
            foreach (var prop in obj.Properties())
            {
                columns.Add(prop.Name);
            }
        }
        foreach (var colName in columns)
        {
            dt.Columns.Add(colName);
        }

        // Заполняем строки.
        foreach (JObject obj in array.OfType<JObject>())
        {
            DataRow row = dt.NewRow();
            foreach (DataColumn col in dt.Columns)
            {
                JToken value;
                if (obj.TryGetValue(col.ColumnName, out value))
                {
                    row[col] = value.Type == JTokenType.Null ? (object)DBNull.Value : (object)value.ToString();
                }
                else
                {
                    row[col] = DBNull.Value;
                }
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    /// <summary>
    /// Преобразует JArray, содержащий объекты, в DataTable с возможностью передачи прогресса при заполнении строк для таблицы "Tags".
    /// Если tableName равен "Tags", для каждой обработанной строки вызывается tagProgress.Report, передавая номер строки.
    /// </summary>
    /// <param name="array">JArray с объектами.</param>
    /// <param name="tableName">Имя DataTable.</param>
    /// <param name="tagProgress">Объект IProgress для отчёта прогресса.</param>
    /// <returns>DataTable с данными из JArray.</returns>
    private static DataTable ConvertJArrayToDataTableWithProgress(JArray array, string tableName, IProgress<int> tagProgress)
    {
        DataTable dt = new DataTable(tableName);
        if (array == null)
            return dt;

        HashSet<string> columns = new HashSet<string>();
        foreach (JObject obj in array.OfType<JObject>())
        {
            foreach (var prop in obj.Properties())
            {
                columns.Add(prop.Name);
            }
        }
        foreach (var colName in columns)
        {
            dt.Columns.Add(colName);
        }

        int processedCount = 0;
        foreach (JObject obj in array.OfType<JObject>())
        {
            DataRow row = dt.NewRow();
            foreach (DataColumn col in dt.Columns)
            {
                JToken value;
                if (obj.TryGetValue(col.ColumnName, out value))
                {
                    row[col] = value.Type == JTokenType.Null ? (object)DBNull.Value : (object)value.ToString();
                }
                else
                {
                    row[col] = DBNull.Value;
                }
            }
            dt.Rows.Add(row);
            processedCount++;
            tagProgress?.Report(processedCount);
        }

        return dt;
    }

    /// <summary>
    /// Преобразует рабочий лист Excel в JArray.
    /// Первая строка листа считается заголовками (имена столбцов), остальные строки – объектами.
    /// </summary>
    /// <param name="ws">Рабочий лист Excel.</param>
    /// <returns>JArray, содержащий объекты, полученные из строк листа.</returns>
    private static JArray ConvertWorksheetToJArray(IXLWorksheet ws)
    {
        JArray arr = new JArray();
        var range = ws.RangeUsed();
        if (range == null)
            return arr;

        // Первая строка для заголовков.
        var headerRow = range.FirstRowUsed().Cells().Select(c => c.GetString()).ToList();

        // Обрабатываем каждую последующую строку.
        foreach (var row in range.RowsUsed().Skip(1))
        {
            JObject obj = new JObject();
            var cells = row.Cells().ToList();
            for (int i = 0; i < headerRow.Count; i++)
            {
                string header = headerRow[i];
                string cellValue = (i < cells.Count) ? cells[i].GetString() : "";
                obj[header] = cellValue;
            }
            arr.Add(obj);
        }
        return arr;
    }
}
