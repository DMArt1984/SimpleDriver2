using Connector.SGT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogCodeMessage
{
    public static class CodeMessageFactory
    {
        const int version = 1100;
        public static CodeMessage FromEnum(Enum value)
        {
            int code = Convert.ToInt32(value);
            string message = value.ToString();

            return new CodeMessage(code, message);
        }
        public static CodeMessage FromEnumX(Enum value)
        {
            int code = Convert.ToInt32(value);
            string message = value.ToString();

            if (value is eTagCode tagCode)
            {
                message = tagCode.GetText();
            } else if (value is eSourceStatus sourceStatus)
            {
                message = sourceStatus.GetText();
            }

            return new CodeMessage(code, message);
        }

        public static CodeMessage FromException(Exception ex, string template = "")
        {
            int code = ex.HResult;
            string message = (String.IsNullOrWhiteSpace(template)) ? ex.Message : template.Replace("#", ex.Message);
            return new CodeMessage(code, message);
        }

    }
}
