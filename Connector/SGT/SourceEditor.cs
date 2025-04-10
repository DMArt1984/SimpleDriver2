
using DML;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connector
{
    public struct DGVSourcesCol
    {
        public int Calc;
        public int Title;
        public int Driver;
        public int Address;
        public int Desc;
        public int Status;
        public int Message;
        public int CountTags;
    }
    public struct cellSource
    {
        //ED public ushort Id;
        public DataGridViewRow row;
        public DataGridViewCell connection;
        public DataGridViewCell opened;
        public DataGridViewCell code;
        public DataGridViewCell message;
        public DataGridViewCell comment;
        public DataGridViewCell step;
        public DataGridViewCell statistic;
    }

    public class SourceEditor // Редактирование
    {
        public ushort Id; // Уникальный идентификатор (0 - нет Id)
        public eDriverType driver; // Тип драйвера
        public string title; // Название драйвера
        public string address; // Строка подключения
        public bool disableOnStart; // Отключен при старте
        public string description; // Описание
        public bool auto; // Запуск опроса после открытия файла
        public bool reconnect; // Автоматическое переподключение
    }
    
    static public class SourceLib
    {
        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Sources");
        static public void UnpackItemSource(dynamic item, uint forId, out string title, out eDriverType driver, out string connection, out bool disableOnStart, out string description, out bool auto, out bool reopen)
        {
            title = JsonControl.GetString(item, "Title", $"Source #{forId}");
            driver = JsonControl.GetTypeEnum<eDriverType>(item, "Driver", eDriverType.None);
            connection = JsonControl.GetString(item, "Address");
            disableOnStart = JsonControl.GetBool(item, "Off");
            description = JsonControl.GetString(item, "Desc");
            //tags = JsonControl.IsProp(item, "Tags") ? item.Tags : null;
            auto = JsonControl.GetBool(item, "Auto");
            reopen = JsonControl.GetBool(item, "Reconnect");
        }

        // Распаковка источников
        static public List<SourceEditor> UnpackSources(dynamic section)
        {
            List<SourceEditor> items = new List<SourceEditor>();
            ushort sourceId = 0; // ID 
            if (section != null)
            {
                foreach (dynamic item in section)
                {
                    SourceLib.UnpackItemSource(item, ++sourceId, out string title, out eDriverType driver, out string address, out bool disableOnStart, out string description, out bool auto, out bool reconnect);
                    SourceEditor rowSource = new SourceEditor
                    {
                        Id = sourceId,
                        driver = driver,
                        title = title,
                        address = address,
                        disableOnStart = disableOnStart,
                        description = description,
                        auto = auto,
                        reconnect = reconnect
                    };
                    items.Add(rowSource);
                }
            }
            return items;
        }

        // Передача в таблицу
        static public void DataToTable(DataGridView dgv, DGVSourcesCol col, List<SourceEditor> sources)
        {
            // Таблица источников
            dgv.Rows.Clear();
            if (sources == null)
                return;
            foreach (var item in sources)
            {
                DataGridViewRow row = (DataGridViewRow)dgv.Rows[0].Clone();
                row.Cells[0].Value = item.Id;
                row.Cells[3].Value = !item.disableOnStart;
                row.Cells[4].Value = item.auto; // автоматический опрос
                row.Cells[5].Value = item.reconnect; // автоматическое переподключение

                row.Cells[col.Title].Value = item.title;
                row.Cells[col.Driver].Value = item.driver.ToString();
                row.Cells[col.Address].Value = item.address;
                row.Cells[col.Desc].Value = item.description;
                row.Cells[col.CountTags].Value = 0;

                row.Cells[col.Calc].Value = false;
                row.Cells[col.Status].Value = "";
                row.Cells[col.Message].Value = "";

                // -
                dgv.Rows.Add(row);
            }
        }
        static public List<SourceEditor> TableToData(DataGridView dgv, DGVSourcesCol col)
        {
            List<SourceEditor> sources = new List<SourceEditor>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                SourceEditor se = new SourceEditor
                {
                    Id = Convert.ToUInt16(row.Cells[0].Value),
                    disableOnStart = !Convert.ToBoolean(row.Cells[3].Value),
                    auto = Convert.ToBoolean(row.Cells[4].Value),
                    reconnect = Convert.ToBoolean(row.Cells[5].Value),
                    title = row.Cells[col.Title].Value.ToString(),
                    driver = (eDriverType)Enum.Parse(typeof(eDriverType), row.Cells[col.Driver].Value.ToString(), true),
                    address = row.Cells[col.Address].Value.ToString(),
                    description = row.Cells[col.Desc].Value.ToString(),
                };
                sources.Add(se);
            }
            return sources;
        }

        static public DGVSourcesCol GetCols(DataGridView dgv)
        {
            DGVSourcesCol col = new DGVSourcesCol
            {
                Calc = dgv.Columns["sourceCalc"].Index,
                Title = dgv.Columns["sourceTitle"].Index,
                Driver = dgv.Columns["sourceDriver"].Index,
                Address = dgv.Columns["sourceAddress"].Index,
                Desc = dgv.Columns["sourceDesc"].Index,
                Status = dgv.Columns["sourceStatus"].Index,
                Message = dgv.Columns["sourceMessage"].Index,
                CountTags = dgv.Columns["sourceTags"].Index
            };
            return col;
        }

        // Упаковка источников
        static public JArray PackSources(List<SourceEditor> sources)
        {
            // Источники
            JArray arrSources = new JArray();
            if (sources != null)
            {
                foreach (var src in sources)
                {
                    // Собираем свойства источника согласно схеме UnpackProject
                    JObject jSrc = new JObject();
                    //jSrc["Id"] = src.Id;
                    jSrc["Title"] = src.title;
                    jSrc["Driver"] = src.driver.ToString(); // можно изменить вывод драйвера при необходимости
                    jSrc["Address"] = src.address;

                    if (src.disableOnStart)
                        jSrc["Off"] = src.disableOnStart;

                    if (String.IsNullOrWhiteSpace(src.description) == false)
                        jSrc["Desc"] = src.description;

                    if (src.auto)
                        jSrc["Auto"] = src.auto;

                    if (src.reconnect)
                        jSrc["Reconnect"] = src.reconnect;

                    arrSources.Add(jSrc);
                }
            }
            return arrSources;
        }

    }

}
