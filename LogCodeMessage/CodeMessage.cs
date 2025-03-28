using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCodeMessage
{
    /// <summary>
    /// Структура, содержащая код и сообщение
    /// </summary>
    public struct CodeMessage
    {
        public readonly int code;
        public readonly string message;

        public CodeMessage(int code = 0, string message = "")
        {
            this.code = code;
            this.message = message;
        }
    }

    public interface ICodeMessage
    {
        ushort Id { get; }
        CodeMessage codeMessage { get; set; }
        bool Good { get; }
    }
}
