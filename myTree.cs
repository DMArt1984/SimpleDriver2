using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        structureItem, // Структура
        targetItem, // Тег структуры
        includes, // Классы
        includeItem, // Класс
        changeItem, // Замены класса
        blocks, // Блоки
        blockItem // Блок
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

    public class MyTree
    {

        // Установить количество по спискам
        static public void SetCountTreeNode(TreeNode tn)
        {
            foreach (TreeNode child in tn.Nodes)
            {
                SetCountTreeNodeChild(child);
            }
        }

        // Установить количество 
        static public void SetCountTreeNodeChild(TreeNode tn)
        {
            var count = tn.Nodes.Count;
            if (count <= 1)
            {
                tn.Text = tn.Text.Split('[')[0].Trim();
            }
            else
            {
                tn.Text = tn.Text.Split('[')[0].Trim() + " [" + count.ToString() + "]";
            }

        }

        // Получить список для дерева без ссылок
        static public List<TableIdentity> SetTreeCollection(DataGridView dgv, int colTitle, int colParentTitle = 0)
        {
            List<TableIdentity> collections = new List<TableIdentity>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                var cellValue = row.Cells[colTitle].Value;
                if (cellValue == null)
                    continue;

                string title = cellValue.ToString();
                if (String.IsNullOrWhiteSpace(title))
                    continue;

                string parentTitle = (colParentTitle > 0) ? (row.Cells[colParentTitle].Value != null) ? row.Cells[colParentTitle].Value.ToString() : "" : "";

                var cellId = row.Cells[0].Value;
                if (cellId == null)
                    continue;

                collections.Add(new TableIdentity { Id = Convert.ToUInt16(cellId), Title = title, Link = parentTitle });
            }
            return collections;
        }
    }

    #endregion

}
