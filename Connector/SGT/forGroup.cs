using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector
{
    public interface IGroupOff
    {
        ushort Id { get; }
        bool Off { get; set; }
    }

    public struct GroupParamStatus
    {
        public readonly ushort Id;
        public uint UpdateRate;
        public bool Off;
        public bool IsStopped;

        public GroupParamStatus(ushort id, uint updateRate, bool off, bool isStopped)
        {
            Id = id;
            UpdateRate = updateRate;
            Off = off;
            IsStopped = isStopped;
        }
    }

    public enum eGroupStatus
    {
        zero = 0, // не определено
        created = 800, // создан
        Off = 300, // Группа отключена
        On = 900, // Группа включена
    }

    public static class eGroupStatusExtensions
    {
        public static string GetText(this eGroupStatus status)
        {
            switch (status)
            {
                case eGroupStatus.created:
                    return "Создан";
                case eGroupStatus.zero:
                    return "Не определено";
                case eGroupStatus.Off:
                    return "Отключено";
                case eGroupStatus.On:
                    return "Включено";
                default:
                    return status.ToString();
            }
        }
    }
}
