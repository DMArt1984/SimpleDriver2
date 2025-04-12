using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector
{
    public class TagErrorHandler
    {
        public int ErrorCount { get; private set; }
        public int MaxErrors { get; }

        public TagErrorHandler(int maxErrors)
        {
            MaxErrors = maxErrors;
            ErrorCount = 0;
        }

        /// <summary>
        /// Обрабатывает список тегов. Если обнаружены ошибки (например, breakError) и нет корректных тегов,
        /// увеличивает счетчик ошибок. Если количество ошибок достигает MaxErrors, возвращает true для дальнейшей обработки (например, вызова NewBreak).
        /// В противном случае, сбрасывает счетчик.
        /// </summary>
        /// <param name="tags">Список тегов для проверки</param>
        /// <returns>Возвращает true, если число ошибок превысило порог, иначе false.</returns>
        public bool ProcessErrors(List<TAG> tags)
        {
            bool breakError = tags.Any(x => x.codeMessage.code == TAG.CM.BreakError.code);
            bool anyGood = tags.Any(x => x.Good && x.Command == eCommand.None && x.WriteTagId == 0 && x.WriteTagValue == null);

            if (breakError && !anyGood)
            {
                ErrorCount++;
                if (ErrorCount >= MaxErrors)
                {
                    Reset();
                    return true;
                }
            }
            else
            {
                Reset();
            }
            return false;
        }

        public void Reset()
        {
            ErrorCount = 0;
        }
    }

}
