using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSimpleIDriver
{
    #region Class Tree
    public enum TreeProjCategory
    {
        sources, // Источники
        sourceItem, // Источник
        groupItem, // Группа
        tagItem, // Тег
        structures, // Структуры
        targetItem, // Элементы структуры
        includes, // Классы
        changeItem, // Элементы класса
        blocks // Блоки
    }
    public class TreeProjTag
    {
        public TreeProjCategory category;
        public ushort Id;
        public TreeProjTag(TreeProjCategory category, ushort Id)
        {
            this.category = category;
            this.Id = Id;
        }
    }
    #endregion

}
