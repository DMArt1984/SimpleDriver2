using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsIDevice.Connector.SGT
{

    public struct CodeMessage // Код и Сообщение
    {
        public readonly int code;
        public readonly string message;
        public CodeMessage(int code, string message = "")
        {
            this.code = code;
            this.message = message;
        }
        public CodeMessage(eSourceStatus status)
        {
            this.code = (int)status;
            this.message = status.GetText();
        }
        public CodeMessage(eTagCode tagCode)
        {
            this.code = (int)tagCode;
            this.message = tagCode.GetText();
        }
    }

    public interface ICodeMessage
    {
        ushort Id { get; }
        CodeMessage codeMessage { get; set; }
        bool Good { get; }
    }

    

    

}
