using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector.SGT
{
    public enum eTagCode // Коды тегов
    {
        good = 0,
        created = 1, // новый тег
        sourceOpened = 100,
        sourceClosed = 200,

        emptyRequest = -30, // пустой запрос

        sourceFail = -200,
        groupOff = 300,
        groupOn = 301,
        noPing = -400,
        newValueIsNull = 404,
        tagOff = 50,
        tagOn = 51,
        IsNotSupport = -60, // тип данных не поддерживается
        connectionTimedOut = -70,
        tagTimeout = -71,
        noWrite = -80,
        noData = -31,
        breakError = -600, // возможно ошибка источника
        inconsistency = -90, // не соответствие типа данных

        notReliableA = -700, // нет достоверных данных в адресе
        notReliableTW = -701, // нет достоверных данных в теге для записи
        noTagForWrite = -702 // нет тега для записи

    }

    public static class eTagCodeExtensions
    {
        public static string GetText(this eTagCode code)
        {
            switch (code)
            {
                case eTagCode.good:
                    return "Норма";

                case eTagCode.created:
                    return "Создан";

                case eTagCode.emptyRequest:
                    return "Пустой запрос";

                case eTagCode.tagOff:
                    return "Тег отключен";

                case eTagCode.tagOn:
                    return "Тег включен, нет опроса";

                case eTagCode.groupOff:
                    return "Группа отключена";

                case eTagCode.groupOn:
                    return "Группа включена, нет опроса";

                case eTagCode.sourceClosed:
                    return "Источник закрыт";

                case eTagCode.sourceOpened:
                    return "Источник открыт, нет опроса";

                case eTagCode.inconsistency:
                    return "Не соответствут типу данных";

                case eTagCode.notReliableA:
                    return "Не достоверны данные в адресе {?}";

                case eTagCode.notReliableTW:
                    return "Нет достоверных данных в теге для записи";

                case eTagCode.noTagForWrite:
                    return "нет тега для записи";

                case eTagCode.breakError:
                    return "Вероятна ошибка подключения";

                case eTagCode.connectionTimedOut:
                case eTagCode.IsNotSupport:
                case eTagCode.newValueIsNull:
                case eTagCode.noData:
                case eTagCode.noPing:
                case eTagCode.noWrite:
                case eTagCode.sourceFail:
                case eTagCode.tagTimeout:
                    return code.ToString();
                default:
                    return code.ToString();
            }
        }
    }
    
}
