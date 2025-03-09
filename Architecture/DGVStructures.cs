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
        public int CountTags;
    }
    public struct DGVGroupsCol
    {
        public int Calc;
        public int Title;
        public int Source;
        public int Desc;
        public int Status;
        public int CountTags;
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
        //public int TagSource;
        public int TemplateAddress;
        public int Group;
        public int DataType;
    }
    public struct DGVStructTargetCol
    {
        public int Structure;
        public int InnerTitle;
        public int Address;
        public int Desc;
    }
    public struct DGVStructTagCol
    {
        public int Structure;
        public int TagTitle;
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
