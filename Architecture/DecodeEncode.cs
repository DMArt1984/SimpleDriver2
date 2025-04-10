using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver
{
    static class DecodeEncode
    {
        /// <summary>
        /// Если входная строка содержит характерные для неправильной кодировки последовательности (например, "РћРїРёСЃР°РЅРёРµ"),
        /// то метод пытается исправить кодировку, предполагая, что исходный текст был Windows-1251, 
        /// а он прочитан как Windows-1252 (Latin1). В противном случае строка возвращается без изменений.
        /// </summary>
        /// <param name="input">Входная строка, возможно, содержащая неправильную кодировку.</param>
        /// <returns>Строка с исправленной кодировкой, если найдено, или исходная строка, если исправление не требуется.</returns>
        public static string FixGarbledCyrillicEncoding(string input)
        {
            // Простой критерий: если строка содержит часто встречающийся фрагмент "РћРї", считаем, что кодировка нарушена.
            if (input.Contains("РћРї"))
            {
                // Получаем байты, как если бы строка была прочитана в кодировке Windows-1252
                byte[] cp1252Bytes = Encoding.GetEncoding("windows-1252").GetBytes(input);
                // Декодируем байты в Windows-1251 (кодировка для русских текстов)
                string fixedString = Encoding.GetEncoding("windows-1251").GetString(cp1252Bytes);
                return fixedString;
            }
            return input;
        }

        public static Encoding GetEncodingFromString(string encodingName)
        {
            if (string.IsNullOrWhiteSpace(encodingName))
                return Encoding.Default;

            try
            {
                return Encoding.GetEncoding(encodingName);
            }
            catch (ArgumentException)
            {
                // Попробуем дополнительно обработать часто используемые псевдонимы
                switch (encodingName.Trim().ToLowerInvariant())
                {
                    case "utf8":
                    case "utf-8":
                        return Encoding.UTF8;

                    case "utf7":
                    case "utf-7":
                        return Encoding.UTF7;

                    case "utf32":
                    case "utf-32":
                        return Encoding.UTF32;

                    case "ascii":
                        return Encoding.ASCII;

                    case "unicode":
                        return Encoding.Unicode;

                    case "bigendianunicode":
                        return Encoding.BigEndianUnicode;

                    case "default":
                        return Encoding.Default;

                    case "latin1":
                    case "iso-8859-1":
                        return Encoding.GetEncoding("iso-8859-1");

                    case "windows1251":
                    case "windows-1251":
                        return Encoding.GetEncoding(1251); // кириллица

                    default:
                        throw new NotSupportedException($"Неизвестная кодировка: {encodingName}");
                }
            }
        }
    }
}
