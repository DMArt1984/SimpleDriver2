using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class ExcelJsonConverter
{
    /// <summary>
    /// Конвертирует нормализованный JSON в книгу Excel.
    /// В книге создаются три листа: "Sources", "Groups" и "Tags". На каждом листе строки – это объекты массива,
    /// а столбцы – свойства объектов.
    /// </summary>
    /// <param name="json">Строка JSON с нормализованными данными (объект с массивами Sources, Groups, Tags).</param>
    /// <param name="filePath">Путь для сохранения Excel-файла.</param>
    public static void JsonToExcel(string json, string filePath)
    {
        // Парсим JSON-строку в JObject
        JObject jObject = JObject.Parse(json);

        // Преобразуем каждый массив в DataTable (ключи – столбцы, объекты – строки)
        DataTable dtSources = ConvertJArrayToDataTable(jObject["Sources"] as JArray, "Sources");
        DataTable dtGroups = ConvertJArrayToDataTable(jObject["Groups"] as JArray, "Groups");
        DataTable dtTags = ConvertJArrayToDataTable(jObject["Tags"] as JArray, "Tags");

        // Создаем новую книгу Excel
        using (XLWorkbook workbook = new XLWorkbook())
        {
            workbook.Worksheets.Add(dtSources, "Sources");
            workbook.Worksheets.Add(dtGroups, "Groups");
            workbook.Worksheets.Add(dtTags, "Tags");

            workbook.SaveAs(filePath);
        }
    }

    /// <summary>
    /// Конвертирует книгу Excel в JSON-строку.
    /// Ожидается, что книга содержит листы с названиями "Sources", "Groups" и "Tags".
    /// На каждом листе первая строка определяет имена столбцов (ключи), а последующие строки – объекты.
    /// Полученный JSON имеет структуру: { "Sources": [...], "Groups": [...], "Tags": [...] }.
    /// </summary>
    /// <param name="filePath">Путь к Excel-файлу.</param>
    /// <returns>Строка JSON с данными.</returns>
    public static string ExcelToJson(string filePath)
    {
        JObject result = new JObject();

        using (XLWorkbook workbook = new XLWorkbook(filePath))
        {
            JArray arrSources = ConvertWorksheetToJArray(workbook.Worksheet("Sources"));
            JArray arrGroups = ConvertWorksheetToJArray(workbook.Worksheet("Groups"));
            JArray arrTags = ConvertWorksheetToJArray(workbook.Worksheet("Tags"));

            result["Sources"] = arrSources;
            result["Groups"] = arrGroups;
            result["Tags"] = arrTags;
        }

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

        // Определяем набор столбцов как объединение всех ключей объектов массива
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

        // Заполняем строки
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
    /// Преобразует рабочий лист (worksheet) Excel в JArray.
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

        // Первая строка для заголовков
        var headerRow = range.FirstRowUsed().Cells().Select(c => c.GetString()).ToList();

        // Обрабатываем каждую последующую строку
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
