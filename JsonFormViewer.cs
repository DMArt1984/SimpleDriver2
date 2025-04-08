using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinSimpleIDriver
{
    static class JsonFormViewer
    {
        /// <summary>
        /// Выводит содержимое JSON (в виде строки) в элемент RichTextBox с цветной подсветкой синтаксиса
        /// и с измененным размером шрифта в зависимости от уровня вложенности:
        /// - уровень 0 (корневой уровень) – шрифт в 2 раза больше базового,
        /// - уровень 1 – шрифт в 1.5 раза больше базового,
        /// - для остальных уровней базовый размер шрифта.
        /// </summary>
        /// <param name="richTextBox">Элемент RichTextBox, в который будет выведен JSON.</param>
        /// <param name="jsonContent">Строка, содержащая JSON.</param>
        public static void DisplayColoredJson(RichTextBox richTextBox, string jsonContent)
        {
            // Парсим и форматируем JSON с отступами
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

            // Очищаем RichTextBox и устанавливаем базовые параметры
            richTextBox.Clear();
            richTextBox.BackColor = Color.White;
            string fontFamily = "Consolas";
            float baseFontSize = 10f; // базовый размер шрифта

            // Разбиваем JSON на строки
            string[] lines = json.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                // Определяем количество ведущих пробелов, чтобы вычислить уровень вложенности.
                int spaceCount = line.TakeWhile(c => c == ' ').Count();
                // Предполагаем, что один уровень соответствует 4 пробелам.
                int level = spaceCount / 4;
                float fontSize = baseFontSize;
                if (level == 0)
                {
                    fontSize = baseFontSize * 1.4f;
                }
                else if (level == 1)
                {
                    fontSize = baseFontSize * 1.2f;
                }
                // Для уровней 2 и выше оставляем базовый размер.

                Font lineFont = new Font(fontFamily, fontSize);
                // Запоминаем текущую длину текста, чтобы установить форматирование для новой строки.
                int start = richTextBox.TextLength;
                richTextBox.AppendText(line + Environment.NewLine);
                richTextBox.Select(start, line.Length);
                richTextBox.SelectionFont = lineFont;
                // Сбрасываем цвет выделения в стандартный
                richTextBox.SelectionColor = richTextBox.ForeColor;
                // Перемещаем курсор в конец
                richTextBox.SelectionStart = richTextBox.TextLength;
                richTextBox.SelectionLength = 0;
            }

            // Выполняем подсветку синтаксиса без изменения шрифта
            HighlightJson(richTextBox);
        }

        /// <summary>
        /// Осуществляет подсветку синтаксиса JSON в RichTextBox, используя регулярные выражения для выделения:
        /// - ключей (строки перед двоеточием), 
        /// - строковых значений, чисел, логических значений и null.
        /// Метод изменяет только цвет выделения, не затрагивая размер шрифта.
        /// </summary>
        /// <param name="richTextBox">Элемент RichTextBox с текстом JSON.</param>
        private static void HighlightJson(RichTextBox richTextBox)
        {
            // Сохраняем текущую позицию курсора
            int originalSelectionStart = richTextBox.SelectionStart;
            int originalSelectionLength = richTextBox.SelectionLength;

            // Определяем регулярные выражения для различных элементов JSON.
            // Ключи: строки, за которыми следует двоеточие
            string keyPattern = @"(""(\\[uU][0-9a-fA-F]{4}|\\[^u]|[^\\\""])*""(?=\s*:))";
            // Строковые значения
            string stringPattern = @"(?<=:\s*)(""(\\[uU][0-9a-fA-F]{4}|\\[^u]|[^\\\""])*"")";
            // Числа (целые, дробные, с экспонентой)
            string numberPattern = @"(?<=:\s*)(-?\d+(\.\d+)?([eE][+\-]?\d+)?)";
            // Логические значения
            string boolPattern = @"(?<=:\s*)(true|false)";
            // null
            string nullPattern = @"(?<=:\s*)(null)";

            // Применяем подсветку
            ApplyRegexHighlighting(richTextBox, keyPattern, Color.Blue);
            ApplyRegexHighlighting(richTextBox, stringPattern, Color.Brown);
            ApplyRegexHighlighting(richTextBox, numberPattern, Color.Magenta);
            ApplyRegexHighlighting(richTextBox, boolPattern, Color.DarkCyan);
            ApplyRegexHighlighting(richTextBox, nullPattern, Color.Gray);

            // Восстанавливаем исходное выделение.
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
