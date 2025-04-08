using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinSimpleIDriver
{
    static class JsonFormViewer
    {
        /// <summary>
        /// Выводит содержимое JSON (в виде строки) в элемент RichTextBox с цветным форматированием синтаксиса.
        /// </summary>
        /// <param name="richTextBox">
        /// Элемент RichTextBox, в который будет выведен JSON.
        /// </param>
        /// <param name="jsonContent">
        /// Строка, содержащая JSON (например, считанный из файла).
        /// </param>
        public static void DisplayColoredJson(RichTextBox richTextBox, string jsonContent)
        {
            // Используем полученную строку, форматируем (prettify) JSON с отступами, если это возможно.
            string json = jsonContent;

            try
            {
                JToken parsedJson = JToken.Parse(json);
                json = parsedJson.ToString(Formatting.Indented);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка форматирования JSON: " + ex.Message);
            }

            // Очищаем и настраиваем RichTextBox.
            richTextBox.Clear();
            richTextBox.Text = json;
            richTextBox.BackColor = Color.White;
            richTextBox.Font = new Font("Consolas", 10);

            // Вызываем метод синтаксической подсветки.
            HighlightJson(richTextBox);
        }

        /// <summary>
        /// Метод, который осуществляет подсветку синтаксиса JSON в RichTextBox.
        /// Использует регулярные выражения для выделения ключей, строковых значений, чисел, логических значений и null.
        /// </summary>
        /// <param name="richTextBox">Элемент RichTextBox с текстом JSON.</param>
        private static void HighlightJson(RichTextBox richTextBox)
        {
            // Сохраняем текущую позицию курсора, чтобы в конце восстановить.
            int originalSelectionStart = richTextBox.SelectionStart;
            int originalSelectionLength = richTextBox.SelectionLength;

            // Определяем регулярные выражения для подсветки.
            // Ключи в JSON (строки перед двоеточием)
            string keyPattern = @"(""(\\[uU][0-9a-fA-F]{4}|\\[^u]|[^\\\""])*""(?=\s*:))";
            // Строковые значения
            string stringPattern = @"(?<=:\s*)(""(\\[uU][0-9a-fA-F]{4}|\\[^u]|[^\\\""])*"")";
            // Числа (целые и дробные, включая экспоненту)
            string numberPattern = @"(?<=:\s*)(-?\d+(\.\d+)?([eE][+\-]?\d+)?)";
            // Логические значения (true/false)
            string boolPattern = @"(?<=:\s*)(true|false)";
            // null
            string nullPattern = @"(?<=:\s*)(null)";

            // Подсвечиваем ключи (синим)
            ApplyRegexHighlighting(richTextBox, keyPattern, Color.Blue);
            // Подсвечиваем строковые значения (коричневым)
            ApplyRegexHighlighting(richTextBox, stringPattern, Color.Brown);
            // Подсвечиваем числа (пурпурным)
            ApplyRegexHighlighting(richTextBox, numberPattern, Color.Magenta);
            // Подсвечиваем логические значения (темно-голубым)
            ApplyRegexHighlighting(richTextBox, boolPattern, Color.DarkCyan);
            // Подсвечиваем null (серым)
            ApplyRegexHighlighting(richTextBox, nullPattern, Color.Gray);

            // Восстанавливаем оригинальное выделение.
            richTextBox.SelectionStart = originalSelectionStart;
            richTextBox.SelectionLength = originalSelectionLength;
            richTextBox.SelectionColor = richTextBox.ForeColor;
        }

        /// <summary>
        /// Применяет заданное регулярное выражение для подсветки найденных участков текста в RichTextBox заданным цветом.
        /// </summary>
        /// <param name="richTextBox">Элемент RichTextBox с текстом.</param>
        /// <param name="pattern">Регулярное выражение для поиска.</param>
        /// <param name="color">Цвет для подсветки найденных совпадений.</param>
        private static void ApplyRegexHighlighting(RichTextBox richTextBox, string pattern, Color color)
        {
            Regex regex = new Regex(pattern);
            foreach (Match match in regex.Matches(richTextBox.Text))
            {
                richTextBox.SelectionStart = match.Index;
                richTextBox.SelectionLength = match.Length;
                richTextBox.SelectionColor = color;
            }
        }
    }
}
