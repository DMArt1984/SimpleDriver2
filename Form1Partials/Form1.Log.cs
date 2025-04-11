using Connector;
using DML;
using DML.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver
{
    public partial class Form1
    {
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

    }
}
