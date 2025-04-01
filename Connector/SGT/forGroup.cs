using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector
{
    public enum eGroupStatus
    {
        sourceOpened = 100, // Источник открыт – тег получает данные от открытого источника
        sourceClosed = 200, // Источник закрыт – тег не может получать данные, так как источник закрыт
        sourceFail = -200, // Ошибка источника – возникла проблема с источником, из-за которой тег не обновляется
        groupOff = 300, // Группа отключена – теги в данной группе не активны
        groupOn = 301, // Группа включена – теги в группе активны, но опрос может быть не запущен
    }

    public static class eGroupStatusExtensions
    {
        public static string GetText(this eGroupStatus status)
        {
            switch (status)
            {
                case eGroupStatus.sourceOpened:
                    return "Источник открыт – тег получает данные от открытого источника";
                case eGroupStatus.sourceClosed:
                    return "Источник закрыт – тег не может получать данные, так как источник закрыт";
                case eGroupStatus.sourceFail:
                    return "Источник в ошибке – возникла проблема с источником, из-за которой тег не обновляется";
                case eGroupStatus.groupOff:
                    return "Группа отключена – теги в данной группе не активны";
                case eGroupStatus.groupOn:
                    return "Группа включена – теги в группе активны, но опрос может быть не запущен";
                default:
                    return status.ToString();
            }
        }
    }
}
