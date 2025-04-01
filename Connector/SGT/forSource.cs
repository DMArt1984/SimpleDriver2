using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector
{
    public enum eSourceStatus
    {
        zero = 1000, // не определено
        noClient = 1, // нет клиента
        closing = 2, // закрытие...
        closed = 3, // закрыт
        opening = 4, // открытие...
        openedNoCycle = 5, // открыт, но нет опроса
        cycle = 0, // циклический опрос
        breaking = 6, // закрытие по ошибке
        wait = 7, // ожидание перезапуска...
        errOpen = -56, // ошибка открытия
        errClose = -57, // ошибка закрытия
    }

    public static class eSourceStatusExtensions
    {
        public static string GetText(this eSourceStatus status)
        {
            switch (status)
            {
                case eSourceStatus.cycle:
                    return "Работает";
                case eSourceStatus.breaking:
                    return "Закрытие по ошибке...";
                case eSourceStatus.closed:
                    return "Закрыто";
                case eSourceStatus.closing:
                    return "Закрытие...";
                case eSourceStatus.noClient:
                    return "Ошибка создания клиента";
                case eSourceStatus.openedNoCycle:
                    return "Открыто, но нет опроса";
                case eSourceStatus.opening:
                    return "Открытие...";
                case eSourceStatus.wait:
                    return "Ожидание...";
                case eSourceStatus.errOpen:
                    return "Ошибка открытия";
                case eSourceStatus.errClose:
                    return "Ошибка закрытия";
                default:
                    return status.ToString();
            }
        }
    }
}
