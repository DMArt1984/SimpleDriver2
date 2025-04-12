using DML;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connector
{
    public struct DGVTagsCol
    {
        public int Runtime;
        public int Title;
        public int OnOff; // !
        public int Value;
        public int Source;
        public int Group;
        public int DataType;
        public int Address;
        public int RefreshCommand; // !
        public int WriteValue; // !
        public int WriteTagTitle; // !
        public int Desc;
        public int Block;
        public int Page;
        public int Status;
        public int Message;
        public int Statistic; // !
    }

    public struct DGVStructureCol
    {
        public int Title;
        public int Join;
        public int TemplateAddress;
        public int Group;
        public int DataType;
    }
    public struct DGVStructTargetCol
    {
        public int Structure;
        public int InnerTitle;
        public int InnerAddress;
        public int Desc;
    }
    public struct DGVStructTagCol
    {
        public int Structure;
        public int TagTitle;
    }
    public struct cellTag
    {
        public ushort Id;
        public DataGridViewRow row;
        public DataGridViewCell value;
        public DataGridViewCell address;
        public DataGridViewCell dataType;
        public DataGridViewCell writeValue;
        public DataGridViewCell on;
        public DataGridViewCell code;
        public DataGridViewCell message;
        public DataGridViewCell comment;
    }

    #region EDITOR

    public class TagEditor // Редактирование
    {
        public ushort Id; // Уникальный идентификатор (0 - нет Id)
        public string title; // Название
        public eDataType dataType;
        public string groupTitle; // Название группы
        public string address; // адрес
        public bool disableOnStart; // // Отключен при старте
        public bool RefreshCommand; // запрос по команде
        public string writeTagTitle; // Источник новых значений (имя тега)
        public string writeValue; // Записываемое значение
        public string description; // Описание
        public string block; // Блок
        public string page; // Страница
    }

    public struct TargetTag
    {
        public string title;
        public string desc;
        public string address;
    }

    public class StructureEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string title { get; set; } // Название
        public string join { get; set; } // Соединитель
        public eDataType dataType { get; set; } // Тип данных
        public string tagSource { get; set; } // тег-источник
        public string templateAddress { get; set; } // шаблон адреса
        public string group { get; set; } // группа
    }
    public class StructTargetEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string structureTitle { get; set; }  // Название структуры
        public string title { get; set; }  // Название тега
        public string innerAddress { get; set; }  // Адрес
        public string desc { get; set; }  // Описание тега
    }

    public class StructTagEditor
    {
        public uint Id; // Уникальный идентификатор (0 - нет Id)
        public string structureTitle { get; set; }  // Название структуры
        public string title { get; set; }  // Название тега
    }

    static public class TagLib
    {
        static public bool InProject(dynamic output) => JsonControl.IsProp(output, "Tags");
        static public bool IsStructures(dynamic output) => JsonControl.IsProp(output, "Structures");
        static public bool IsTargetTags(dynamic output) => JsonControl.IsProp(output, "TargetTags");
        static public bool IsListBlocks(dynamic output) => JsonControl.IsProp(output, "Blocks");

        // Получение параметров тега
        static public void UnpackItemTag(dynamic item, uint forId, out string title, out eDataType dataType, out bool disableOnStart, out string address, out string description, out string writeTitle, out string groupTitle, out string constValue, out bool isCommand, out string block)
        {
            title = JsonControl.GetString(item, "Title", $"Tag #{forId}");
            groupTitle = JsonControl.GetString(item, "Group");
            dataType = JsonControl.GetTypeEnum<eDataType>(item, "DataType", eDataType.Binary);
            address = JsonControl.GetString(item, "Addr");
            disableOnStart = JsonControl.GetBool(item, "Off");
            description = JsonControl.GetString(item, "Desc");
            writeTitle = JsonControl.GetString(item, "Write");
            constValue = JsonControl.GetString(item, "Value", null);
            isCommand = JsonControl.GetBool(item, "Command");
            block = JsonControl.GetString(item, "Block");
        }
        // Распаковка тегов
        static public List<TagEditor> UnpackTags(dynamic section)
        {
            List<TagEditor> items = new List<TagEditor>();
            ushort tagId = 0; // ID
            if (section != null)
            {
                foreach (dynamic item in section)
                {
                    TagLib.UnpackItemTag(item, ++tagId, out string title, out eDataType dataType, out bool disableOnStart, out string address, out string description, out string writeTitle, out string groupTitle, out string constValue, out bool isCommand, out string block);
                    TagEditor oneTag = new TagEditor
                    {
                        Id = tagId,
                        title = title,
                        dataType = dataType,
                        groupTitle = groupTitle,
                        address = address,
                        disableOnStart = disableOnStart,
                        RefreshCommand = isCommand,
                        writeTagTitle = writeTitle,
                        writeValue = constValue,
                        description = description,
                        block = block
                    };
                    items.Add(oneTag);
                }
            }
            return items;
        }
        // Упаковка
        static public JArray PackTags(List<TagEditor> tags)
        {
            JArray arrTags = new JArray();
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    JObject jTag = new JObject();
                    //jTag["Id"] = tag.Id;
                    jTag["Title"] = tag.title;
                    jTag["Group"] = tag.groupTitle;
                    jTag["DataType"] = tag.dataType.ToString();
                    jTag["Addr"] = tag.address;

                    if (tag.disableOnStart)
                        jTag["Off"] = tag.disableOnStart;

                    if (String.IsNullOrWhiteSpace(tag.description) == false)
                        jTag["Desc"] = tag.description;

                    if (String.IsNullOrWhiteSpace(tag.writeTagTitle) == false)
                        jTag["Write"] = tag.writeTagTitle;

                    if (String.IsNullOrWhiteSpace(tag.writeValue) == false)
                        jTag["Value"] = tag.writeValue;

                    if (tag.RefreshCommand)
                        jTag["Command"] = tag.RefreshCommand;

                    if (String.IsNullOrWhiteSpace(tag.block) == false)
                        jTag["Block"] = tag.block;

                    if (String.IsNullOrWhiteSpace(tag.page) == false)
                        jTag["Page"] = tag.page;

                    arrTags.Add(jTag);
                }
            }
            return arrTags;
        }

        // Получение параметров структуры
        static public void UnpackItemStructure(dynamic item, out string title, out string join, out string templateAddress, out eDataType dataType, out string tagSource, out string group, out string[] sourceTags, out List<TargetTag> targetTags)
        {
            title = JsonControl.GetString(item, "Title", $"noname #{DateTime.Now.Millisecond}");
            join = JsonControl.GetString(item, "Join", ".");
            templateAddress = JsonControl.GetString(item, "Address", "{#Source.[#Target]}");
            dataType = JsonControl.GetTypeEnum<eDataType>(item, "DataType", eDataType.Binary);
            tagSource = JsonControl.GetString(item, "Source", "");
            group = JsonControl.GetString(item, "Group", "");
            sourceTags = JsonControl.GetArrayString(item, "SourceTags");
            targetTags = new List<TargetTag>();
            if (IsTargetTags(item))
            {
                foreach (var target in item.TargetTags)
                {
                    UnpackTargetTag(target, out string ttitle, out string taddress, out string tdesc);
                    if (String.IsNullOrWhiteSpace(taddress) == false)
                    {
                        targetTags.Add(new TargetTag { title = ttitle, address = taddress, desc = tdesc });
                    }
                }
            }
        }
        static public void UnpackTargetTag(dynamic item, out string title, out string address, out string desc)
        {
            address = JsonControl.GetString(item, "Address", "");
            title = JsonControl.GetString(item, "Title", $"index{address}");
            desc = JsonControl.GetString(item, "Desc", title);
        }


        //
        static public void DataToTable(DataGridView dgv, DGVTagsCol col, DataTable tagTable, BindingSource bindingSource, List<TagEditor> tags)
        {
            // Проверка на null DataGridView
            if (dgv == null)
            {
                MessageBox.Show("DataGridView не инициализирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Инициализация DataTable, если DataSource отсутствует
            if (dgv.DataSource == null)
            {
                tagTable = new DataTable();

                // Добавляем только те столбцы, которые уже существуют в DataGridView
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    Type columnType = column.ValueType ?? typeof(string); // Если тип null, используем string
                    tagTable.Columns.Add(column.Name, columnType);
                }

                // Создание BindingSource и привязка к DataGridView
                bindingSource = new BindingSource { DataSource = tagTable };
                dgv.DataSource = bindingSource;
            }
            else
            {
                // Получаем существующую DataTable из DataSource
                if (dgv.DataSource is BindingSource binding && binding.DataSource is DataTable existingTable)
                {
                    tagTable = existingTable;
                    tagTable.Clear(); // Очищаем таблицу перед добавлением новых данных
                }
                else
                {
                    MessageBox.Show("Не удалось получить DataTable из DataSource!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (tags != null)
            {
                // Добавляем данные в DataTable
                foreach (var item in tags)
                {
                    var row = tagTable.NewRow();
                    row[0] = item.Id;
                    row[col.Runtime] = false;
                    row[col.Title] = item.title;
                    row[col.OnOff] = !item.disableOnStart;
                    row[col.Value] = "";
                    row[col.Source] = "";
                    row[col.Group] = item.groupTitle;
                    row[col.DataType] = item.dataType.ToString();
                    row[col.Address] = item.address;
                    row[col.RefreshCommand] = item.RefreshCommand;
                    row[col.WriteValue] = item.writeValue;
                    row[col.WriteTagTitle] = item.writeTagTitle;
                    row[col.Desc] = item.description;
                    row[col.Block] = item.block;
                    row[col.Page] = item.page;
                    row[col.Status] = "";
                    row[col.Message] = "";
                    row[col.Statistic] = "";
                    // ---
                    tagTable.Rows.Add(row);
                }
            }

            // Обновляем данные в DataGridView
            bindingSource.ResetBindings(false);
        }
        static public List<TagEditor> TableToData(DataGridView dgv, DGVTagsCol col)
        {
            List<TagEditor> tags = new List<TagEditor>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                TagEditor te = new TagEditor
                {
                    Id = Convert.ToUInt16(row.Cells[0].Value),
                    // Runtime не используется
                    title = row.Cells[col.Title].Value.ToString(),
                    disableOnStart = !Convert.ToBoolean(row.Cells[col.OnOff].Value),
                    // value не используется
                    // source не используется
                    groupTitle = row.Cells[col.Group].Value.ToString(),
                    dataType = (eDataType)Enum.Parse(typeof(eDataType), row.Cells[col.DataType].Value.ToString(), true),
                    address = row.Cells[col.Address].Value.ToString(),
                    RefreshCommand = Convert.ToBoolean(row.Cells[col.RefreshCommand].Value),
                    writeValue = row.Cells[col.WriteValue].Value.ToString(),
                    writeTagTitle = row.Cells[col.WriteTagTitle].Value.ToString(),
                    description = row.Cells[col.Desc].Value.ToString(),
                    block = row.Cells[col.Block].Value.ToString(),
                    page = row.Cells[col.Page].Value.ToString(),
                    // остальные не используются
                };
                tags.Add(te);
            }
            return tags;
        }


        // Получение адреса тега с подстановками
        static public string ExpTagAddress(string address, out bool success, List<TAG> items, ITagClient tag = null)
        {
            success = true;

            // Если tag задан, но у него не заполнены внутренние теги, возвращаем адрес без изменений.
            if (tag != null && (tag is TAG tTag && (tTag.InnerTags == null || !tTag.InnerTags.Any())))
                return address;

            if (address.Contains("{") && address.Contains("}"))
            {
                // Если tag не null, отбираем только те теги, которые присутствуют в его InnerTags.
                HashSet<ushort> innerIds = null;
                if (tag != null && tag is TAG t)
                {
                    innerIds = new HashSet<ushort>(t.InnerTags.Select(x => x.Id));
                }

                foreach (var item in tag == null ? items : items.Where(x => innerIds.Contains(x.Id)))
                {
                    string nameValue = "{" + item.title + "}"; // значение
                    string goodValue = "{" + item.title + ".good}"; // тег хорошего качества
                    string codeValue = "{" + item.title + ".code}"; // код тега
                    string titleValue = "{" + item.title + ".title}"; // имя тега
                    string descValue = "{" + item.title + ".desc}"; // описание тега
                    string nameIndex = "{" + item.title + "["; // значение из списка

                    bool good = item.Good; // || item.SimEnable; // new item.SimEnable
                    int code = item.codeMessage.code;
                    dynamic lastValue = item.LastGoodValue;
                    dynamic actualValue = item.value;

                    if (address.Contains(goodValue)) // качество тега
                    {
                        address = address.Replace(goodValue, Convert.ToString(good));
                    }
                    if (address.Contains(codeValue)) // код тега
                    {
                        address = address.Replace(codeValue, Convert.ToString(code));
                    }
                    if (address.Contains(titleValue)) // имя тега
                    {
                        address = address.Replace(titleValue, item.title);
                    }
                    if (address.Contains(descValue)) // описание тега
                    {
                        address = address.Replace(descValue, item.description);
                    }
                    if (address.Contains(nameValue)) // значение тега
                    {
                        //
                        if (code < 0 && tag?.title != item.title) // если тег с ошибкой и это не тот же тег
                            success = false;

                        //
                        if (lastValue != null) // && good
                        {
                            if (TagLib.IsArray(lastValue))
                            {
                                address = address.Replace(nameValue, string.Join(";", lastValue));
                            }
                            else
                            {
                                address = address.Replace(nameValue, Convert.ToString(lastValue));
                            }
                        }
                        else
                        {
                            if (tag?.title != item.title) // если это не тот же тег
                                success = false;

                            if (item.DataType == eDataType.STRING || item.DataType == eDataType.Char)
                            {
                                address = address.Replace(nameValue, "");
                            }
                            else
                            {
                                address = address.Replace(nameValue, "0");
                            }
                        }
                    }
                    if (address.Contains(nameIndex)) // значение индекса тега
                    {
                        //
                        if (code < 0 && tag?.title != item.title) // если тег с ошибкой и это не тот же тег
                            success = false;

                        var iter = 0;
                        while (address.IndexOf(nameIndex) >= 0 && iter <= 1024)
                        {
                            var index1 = address.IndexOf(nameIndex);
                            if (index1 >= 0)
                            {
                                var index2 = address.IndexOf("]", index1);
                                if (index2 > 0)
                                {
                                    int start = index1 + nameIndex.Length;
                                    string num = address.Substring(start, index2 - start);
                                    bool ok = int.TryParse(num, out int indexArr);
                                    // Если индекс текстовый
                                    if (ok == false || indexArr == 0)
                                    {
                                        if (TagLib.IsArray(lastValue))
                                        {
                                            foreach (string element in lastValue)
                                            {
                                                var partElement = element.Split('~'); // было =
                                                if (partElement.Length > 1)
                                                {
                                                    if (partElement[0] == num)
                                                    {
                                                        var partElementAt2 = partElement.Where(x => x != partElement[0]).ToArray(); // удаляем первый элемент из массива
                                                        //address = address.Replace(nameIndex + num + "]}", partElement[1]);
                                                        address = address.Replace(nameIndex + num + "]}", String.Join("~", partElementAt2));
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {

                                            if (lastValue != null)
                                            {
                                                var partElement = lastValue.Split('~'); // было =
                                                if (partElement.Length > 1)
                                                {
                                                    if (partElement[0] == num)
                                                    {
                                                        address = address.Replace(nameIndex + num + "]}", partElement[1]);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (tag?.title != item.title) // если это не тот же тег
                                                    success = false;
                                            }
                                        }
                                    }
                                    // Если индекс числовой
                                    if (ok)
                                    {
                                        try
                                        {
                                            if (lastValue != null) // && good
                                            {
                                                if (TagLib.IsArray(lastValue))
                                                {
                                                    address = address.Replace(nameIndex + num + "]}", Convert.ToString(lastValue[indexArr - 1]));
                                                }
                                                else
                                                {
                                                    if (lastValue.GetType() == typeof(string))
                                                    {
                                                        string s = lastValue as string;
                                                        address = address.Replace(nameIndex + num + "]}", s[indexArr - 1].ToString());
                                                    }
                                                    else if (lastValue.GetType() == typeof(int))
                                                    {
                                                        BitArray b = new BitArray(new int[] { lastValue });
                                                        address = address.Replace(nameIndex + num + "]}", b[indexArr - 1].ToString());
                                                    }
                                                    else if (lastValue.GetType() == typeof(byte))
                                                    {
                                                        BitArray b = new BitArray(new byte[] { lastValue });
                                                        address = address.Replace(nameIndex + num + "]}", b[indexArr - 1].ToString());
                                                    }
                                                    else
                                                    {
                                                        address = address.Replace(nameIndex + num + "]}", Convert.ToString(lastValue));
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                if (tag?.title != item.title) // если это не тот же тег
                                                    success = false;

                                                if (item.DataType == eDataType.STRING || item.DataType == eDataType.Char)
                                                {
                                                    address = address.Replace(nameIndex + num + "]}", "");
                                                }
                                                else
                                                {
                                                    address = address.Replace(nameIndex + num + "]}", "0");
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            success = false;
                                            address = address.Replace(nameIndex + num + "]}", ex.Message);
                                        }
                                    }


                                }
                            }
                            iter++;
                        }

                    }
                }
            }
            return address;
        }

        #region View
        static public string ValueToSimpleText(ITagResult item)
        {
            try
            {
                if (item == null)
                    return "";

                dynamic value = item.value;
                string text = "";

                if (value != null && item.Good)
                {
                    if (TagLib.IsArray(value))
                    {
                        int count = 0;
                        foreach (var itemVal in value)
                        {
                            count++;
                        }
                        if (count >= 1)
                        {
                            text = $"[{count}]..{value[0]}";
                        }
                        else
                        {
                            text = $"[{value}]";
                        }
                    }
                    else
                    {
                        text = Convert.ToString(value);
                    }

                }
                else
                {
                    text = "";
                }

                return text;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        static public string ValueToAdvText(TAG item, string valueTrue = "True", string valueFalse = "False", string[] valueList = null)
        {
            var tagValue = item.LastGoodValue; // последнее достоверное значение
            if (tagValue != null)
            {
                if (TagLib.IsArray(tagValue)) // tagValue is IEnumerable && tagValue.GetType() != typeof(string)
                {
                    return String.Join(GetSepatator(item.DataType), tagValue);
                }
                else
                {
                    if (item.DataType == eDataType.Bool)
                    {
                        tagValue = (tagValue == true) ? valueTrue : valueFalse;
                    }
                    else if (IndexDataType(item.DataType) && valueList != null && valueList.Length > 0)
                    {
                        if (tagValue >= 0 && tagValue < valueList.Length)
                        {
                            tagValue = valueList[tagValue];
                        }
                    }
                    return Convert.ToString(tagValue);
                }
            }
            else
            {
                return "";
            }
        }
        static public string GetSepatator(eDataType dataType)
        {
            if (dataType == eDataType.ArrayA)
                return ";";
            if (dataType == eDataType.ArrayB)
                return " ";
            if (dataType == eDataType.ArrayC)
                return "~";

            return "; ";
        }
        #endregion

        #region Info

        // Зависимости адреса
        static public string AddressLinks(string address, List<TAG> items, string tagTitle = "")
        {
            string retval = "";
            if (address.Contains("{") && address.Contains("}"))
            {
                foreach (var item in items)
                {
                    if (item.title != tagTitle) // защита от бесконечного цикла (?)
                    {
                        string nameValue = "{" + item.title + "}"; // значение
                        string goodValue = "{" + item.title + ".good}"; // тег хорошего качества
                        string codeValue = "{" + item.title + ".code}"; // код тега
                        string titleValue = "{" + item.title + ".title}"; // имя тега
                        string descValue = "{" + item.title + ".desc}"; // описание тега
                        string nameIndex = "{" + item.title + "["; // значение из списка


                        if (address.Contains(goodValue) ||  // качество тега
                             address.Contains(codeValue) || // код тега
                             address.Contains(titleValue) || // имя тега
                             address.Contains(descValue) || // описание тега
                             address.Contains(nameValue) || // значение тега
                             address.Contains(nameIndex)) // значение индекса тега
                        {
                            string sk = AddressLinks(item.Address, items, item.title);
                            if (String.IsNullOrWhiteSpace(sk) == false)
                            {
                                sk = $"<-({sk})";
                            }
                            retval += $" {item.title}={item.Address}{sk} ";
                        }
                    }
                }
            }
            return retval;
        }

        // Зависимости тега
        static public List<string> TagLinks(List<TAG> tagItems, string address, string tagTitle = "", bool levels = true)
        {
            List<string> retval = new List<string>();
            if (address.Contains("{") && address.Contains("}"))
            {
                foreach (var item in tagItems)
                {
                    if (item.title != tagTitle) // защита от бесконечного цикла (?)
                    {
                        string nameValue = "{" + item.title + "}"; // значение
                        string goodValue = "{" + item.title + ".good}"; // тег хорошего качества
                        string codeValue = "{" + item.title + ".code}"; // код тега
                        string titleValue = "{" + item.title + ".title}"; // имя тега
                        string descValue = "{" + item.title + ".desc}"; // описание тега
                        string nameIndex = "{" + item.title + "["; // значение из списка

                        if (address.Contains(goodValue) ||  // качество тега
                             address.Contains(codeValue) || // код тега
                             address.Contains(titleValue) || // имя тега
                             address.Contains(descValue) || // описание тега
                             address.Contains(nameValue) || // значение тега
                             address.Contains(nameIndex)) // значение индекса тега
                        {
                            retval.Add(item.title);

                            if (levels)
                                retval.AddRange(TagLinks(tagItems, item.Address, item.title));

                        }
                    }
                }
            }
            return retval;
        }
        #endregion

        #region Converter

        // Преобразование типов
        static public dynamic ConvertValue(dynamic value, eDataType dataType)
        {
            if (value == null)
            {
                if (dataType == eDataType.STRING)
                    return "";
                return 0;
            }

            switch (dataType)
            {
                case eDataType.Bool:
                    if (value is string)
                    {
                        var svalue = ((string)value).ToLower();
                        if (svalue == "1" || svalue == "true" || svalue == "yes")
                            return true;
                        return (bool.TryParse(value, out bool result)) ? result : false;
                    }
                    return Convert.ToBoolean(value);

                case eDataType.Byte:
                    if (value is string)
                    {
                        bool boolOK = byte.TryParse(value, out byte result);
                        if (boolOK)
                            return result;

                        byte[] arr = Encoding.ASCII.GetBytes(value);
                        if (arr.Length == 1)
                            return arr[0];
                        return arr;
                    }
                    if (value < 0)
                        value = 0;
                    if (value > 255)
                        value = 255;
                    return Convert.ToByte(value);

                case eDataType.Binary:
                    if ((value is string) == false)
                    {
                        value = Convert.ToInt16(value);
                        BitArray b = new BitArray(new int[] { value });
                        bool[] bits = new bool[b.Count];
                        b.CopyTo(bits, 0);
                        return bits;
                    }
                    break;

                case eDataType.ArrayA:
                    if ((value is string))
                    {
                        string[] arr = ((string)value).Split(';');
                        return arr;
                    }
                    else
                    {
                        return new string[] { Convert.ToString(value) };
                    }

                case eDataType.ArrayB:
                    if ((value is string))
                    {
                        string[] arr = ((string)value).Split(' ');
                        return arr;
                    }
                    else
                    {
                        return new string[] { Convert.ToString(value) };
                    }

                case eDataType.ArrayC:
                    if ((value is string))
                    {
                        string[] arr = ((string)value).Split('~');
                        return arr;
                    }
                    else
                    {
                        return new string[] { Convert.ToString(value) };
                    }

                case eDataType.HEX:
                    if ((value is string) == false)
                    {
                        return Convert.ToInt32(value).ToString("X");
                    }
                    break;

                case eDataType.Char:
                    if (value is string)
                    {
                        dynamic SValue = (string)value;
                        if (SValue.Length > 1)
                            return ((string)SValue).ToCharArray();
                        return (char.TryParse(SValue, out char result)) ? result : ' ';
                    }
                    return Convert.ToChar(Convert.ToInt32(value));

                case eDataType.Short:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return short.TryParse(value, out short retvalInt1) ? retvalInt1 : 0;
                    }
                    return Convert.ToInt16(value);

                case eDataType.UShort:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return ushort.TryParse(value, out ushort retvalInt2) ? retvalInt2 : 0;
                    }
                    return Convert.ToUInt16(value);

                case eDataType.Int:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return int.TryParse(value, out int retvalInt3) ? retvalInt3 : 0;
                    }
                    return Convert.ToInt32(value);

                case eDataType.UInt:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return uint.TryParse(value, out uint retvalInt4) ? retvalInt4 : 0;
                    }
                    return Convert.ToUInt32(value);

                case eDataType.Long:
                    if (value is string)
                    {
                        value = DecimalString(value);
                        return long.TryParse(value, out long retvalInt5) ? retvalInt5 : 0;
                    }
                    return Convert.ToInt64(value);

                case eDataType.Float:
                    if (value is string)
                    {
                        return ToFloat(DecimalString(value));
                    }
                    return Convert.ToSingle(value);

                case eDataType.Double:
                    if (value is string)
                    {
                        return ToDouble(DecimalString(value));
                    }
                    return Convert.ToDouble(value);

                case eDataType.STRING:
                    return Convert.ToString(value);
            }

            return value;
        }

        // ---------------------------------------------------
        static private float ToFloat(dynamic value)
        {
            string SValue = DecimalString(value);
            bool floatOK1 = float.TryParse(SValue.Replace(",", "."), out float result);
            if (floatOK1)
            {
                return result;
            }
            else
            {
                bool floatOK2 = float.TryParse(SValue.Replace(".", ","), out float result2);
                if (floatOK2)
                {
                    return result2;
                }
                else
                {
                    return 0;
                }
            }
        }

        static private double ToDouble(dynamic value)
        {
            string SValue = DecimalString(value);
            bool doubleOK1 = double.TryParse(SValue.Replace(",", "."), out double result);
            if (doubleOK1)
            {
                return result;
            }
            else
            {
                bool doubleOK2 = double.TryParse(SValue.Replace(".", ","), out double result2);
                if (doubleOK2)
                {
                    return result2;
                }
                else
                {
                    return 0;
                }
            }
        }

        // ---------------------------------------------------
        // Преобразование типов с поддержкой массивов
        static public dynamic ConvertValueWithArray(dynamic value, eDataType dataType)
        {
            if (TagLib.IsArray(value)) // && dataType != eDataType.STRING && dataType != eDataType.Char
            {
                List<dynamic> arrValues = new List<dynamic>();
                foreach (var item in value)
                {
                    arrValues.Add(TagLib.ConvertValue(item, dataType));
                }
                value = arrValues;
            }
            else
            {
                value = TagLib.ConvertValue(value, dataType);
            }

            return value;
        }

        // Получение строки, содержащей только числовые символы
        static private string DecimalString(dynamic value)
        {
            string SValue = value;
            SValue = String.Join("", SValue.Where(x => x == '-' || x == '.' || x == ',' || x == '-' || (x >= '0' && x <= '9')).ToArray());
            if (SValue[0] == '-')
            {
                SValue = "-" + SValue.Replace("-", "");
            }
            return SValue;
        }

        // Значение или хначения в строку
        static public string ValuesString(dynamic value)
        {
            if (TagLib.IsArray(value))
            {
                return String.Join($";", value);
            }
            else
            {
                return Convert.ToString(value);
            }
        }

        #endregion

        #region Check types
        // Является ли тип числовым?
        static public bool NumberDataType(eDataType dataType)
        {
            switch (dataType)
            {
                case eDataType.Byte:
                case eDataType.Short:
                case eDataType.UShort:
                case eDataType.Int:
                case eDataType.UInt:
                case eDataType.Long:
                case eDataType.Float:
                case eDataType.Double:
                    return true;
            }

            return false;
        }
        // Подходит ли тип для индекса?
        static public bool IndexDataType(eDataType dataType)
        {
            switch (dataType)
            {
                case eDataType.Byte:
                case eDataType.Short:
                case eDataType.UShort:
                case eDataType.Int:
                case eDataType.UInt:
                case eDataType.Long:
                    return true;
            }

            return false;
        }
        static public bool IsArray(dynamic value)
        {
            if (value == null)
                return false;

            if (value is IEnumerable && value.GetType() != typeof(string))
                return true;

            return false;
        }

        #endregion

    }

    #endregion
}
