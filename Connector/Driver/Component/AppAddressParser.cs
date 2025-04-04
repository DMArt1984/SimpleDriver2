using LogCodeMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.Driver.Component
{
    public static class AddressParser
    {
        // Словарь, где ключ – префикс (например, "app"), а значение – делегат, обрабатывающий адрес.
        private static readonly Dictionary<string, Func<string[], eDataType, TagResult>> Parsers =
            new Dictionary<string, Func<string[], eDataType, TagResult>>(StringComparer.OrdinalIgnoreCase)
            {
            { "app", ParseApp },
            { "license", ParseLicense },
            { "webserver", ParseWebserver },
            { "source", ParseSource },
            { "group", ParseGroup },
            { "tag", ParseTag },
                // Добавьте другие префиксы по необходимости.
            };

        public static TagResult Parse(string address, eDataType dataType)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return new TagResult(null, new CodeMessage(-1, "Пустой адрес"));
            }

            string[] parts = address.Split('@');
            if (parts.Length < 2)
            {
                return new TagResult(null, new CodeMessage(-1, "Неверный формат адреса"));
            }

            string prefix = parts[0];
            if (Parsers.TryGetValue(prefix, out Func<string[], eDataType, TagResult> parser))
            {
                return parser(parts, dataType);
            }
            else
            {
                return new TagResult(null, new CodeMessage(-1, $"Неизвестный префикс '{prefix}'"));
            }
        }

        private static TagResult ParseApp(string[] parts, eDataType dataType)
        {
            // Пример: "app@title" – возвращает название приложения.
            if (parts.Length < 2)
            {
                return new TagResult(null, new CodeMessage(-1, "Неверный формат для 'app'"));
            }
            switch (parts[1].ToLower())
            {
                case "title":
                    // Здесь можно вернуть, например, значение из централизованного хранилища.
                    return new TagResult("Название приложения", Tag.CM.Good);
                case "time":
                    return new TagResult(DateTime.Now.TimeOfDay.ToString(), Tag.CM.Good);
                case "date":
                    return new TagResult(DateTime.Now.Date.ToString(), Tag.CM.Good);
                default:
                    return new TagResult(null, new CodeMessage(-1, $"Неизвестная команда для 'app': {parts[1]}"));
            }
        }

        private static TagResult ParseLicense(string[] parts, eDataType dataType)
        {
            // Аналогично реализуем логику для лицензии.
            if (parts.Length < 2)
            {
                return new TagResult(null, new CodeMessage(-1, "Неверный формат для 'license'"));
            }
            switch (parts[1].ToLower())
            {
                case "enable":
                    // Возвращаем значение включения лицензии
                    return new TagResult(true, Tag.CM.Good);
                case "check":
                    // Возвращаем результат проверки лицензии
                    return new TagResult("Проверено", Tag.CM.Good);
                default:
                    return new TagResult(null, new CodeMessage(-1, $"Неизвестная команда для 'license': {parts[1]}"));
            }
        }

        private static TagResult ParseWebserver(string[] parts, eDataType dataType)
        {
            // Реализуйте логику для webserver аналогично.
            return new TagResult(null, new CodeMessage(-1, "Функция webserver не реализована"));
        }

        private static TagResult ParseSource(string[] parts, eDataType dataType)
        {
            // Здесь можно обрабатывать запросы типа "source@count", "source@list" и т.д.
            if (parts.Length < 2)
                return new TagResult(null, new CodeMessage(-1, "Неверный формат для 'source'"));

            switch (parts[1].ToLower())
            {
                case "count":
                    return new TagResult(Source.items.Count, Tag.CM.Good);
                case "list":
                    return new TagResult(Source.items.Select(x => x.title).ToArray(), Tag.CM.Good);
                // Можно добавить и другие команды для источника.
                default:
                    return new TagResult(null, new CodeMessage(-1, $"Неизвестная команда для 'source': {parts[1]}"));
            }
        }

        private static TagResult ParseGroup(string[] parts, eDataType dataType)
        {
            // Обработка групп, например, "group@count", "group@list" и т.д.
            if (parts.Length < 2)
                return new TagResult(null, new CodeMessage(-1, "Неверный формат для 'group'"));

            switch (parts[1].ToLower())
            {
                case "count":
                    return new TagResult(Group.items.Count, Tag.CM.Good);
                case "list":
                    return new TagResult(Group.items.Select(x => x.title).ToArray(), Tag.CM.Good);
                default:
                    return new TagResult(null, new CodeMessage(-1, $"Неизвестная команда для 'group': {parts[1]}"));
            }
        }

        private static TagResult ParseTag(string[] parts, eDataType dataType)
        {
            // Обработка тегов, например, "tag@count", "tag@list", "tag@<имя>@code" и т.д.
            if (parts.Length < 2)
                return new TagResult(null, new CodeMessage(-1, "Неверный формат для 'tag'"));

            switch (parts[1].ToLower())
            {
                case "count":
                    return new TagResult(Tag.items.Count, Tag.CM.Good);
                case "list":
                    return new TagResult(Tag.items.Select(x => x.title).ToArray(), Tag.CM.Good);
                default:
                    // Здесь можно добавить более тонкую обработку, например, поиск по Id или по названию и обращение к свойствам
                    return new TagResult(null, new CodeMessage(-1, $"Неизвестная команда для 'tag': {parts[1]}"));
            }
        }
    }

}
