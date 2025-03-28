using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DML.Log
{
    /// <summary>
    /// Структура, содержащая код и сообщение
    /// </summary>
    public struct CodeMessage
    {
        public readonly int сode;
        public readonly string message;

        public CodeMessage(int code = 0, string message = "")
        {
            сode = code;
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
