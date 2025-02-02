using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver
{
    #region DGV
    public struct DGVSourcesCol
    {
        public int Calc;
        public int Title;
        public int Driver;
        public int Address;
        public int Desc;
        public int Status;
        public int Message;
    }
    public struct DGVGroupsCol
    {
        public int Calc;
        public int Title;
        public int Source;
        public int Desc;
        public int Status;
    }
    public struct DGVTagsCol
    {
        public int Calc;
        public int Title;
        public int Value;
        public int DataType;
        public int Address;
        public int Desc;
        public int Status;
        public int Message;
        public int Source;
        public int Group;
        public int Block;
        public int Page;
    }


    public struct DGVStructureCol
    {
        public int Title;
        public int Connector;
        public int TagSource;
        public int Template;
        public int Group;
    }
    public struct DGVTargetCol
    {
        public int Structure;
        public int Address;
        public int Tag;
        public int Desc;
    }


    public struct DGVIncludeCol
    {
        public int Prefix;
        public int FileName;
    }
    public struct DGVChangeCol
    {
        public int Prefix;
        public int ChangeFrom;
        public int ChangeTo;
    }
    #endregion
}
