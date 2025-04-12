using DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver
{
    public partial class FormMain
    {

        #region Tree

        #region Tree.Event
        private void tabFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteTabFilter();
        }
        // Двойное нажатие на ветку из дерева
        private void treeViewProject_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selNode = treeViewProject.SelectedNode;
            TreeProjectSelector(selNode);
            OpenTab(selNode);
            SetTreeFilter(selNode);
        }

        // Выбрана ветка из дерева
        private void treeViewProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selNode = treeViewProject.SelectedNode;
            TreeLib.TreeProjContexMenu(selNode);
        }
        #endregion




        // Добавить в фильт тегов источник, группу и т.п.
        public void SetTreeFilter(TreeNode selNode)
        {
            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);
            switch (tpt.category)
            {
                case TreeProjCategory.sourceItem:
                    //SetTreeSourceFilter(selNode);
                    break;

                case TreeProjCategory.groupItem:
                    //SetTreeGroupFilter(selNode);
                    break;

                case TreeProjCategory.blocks:
                    //SetTreeBlockFilter(selNode);
                    break;
            }
        }

        // Редактирование ветки из дерева
        private void TreeProjectSelector(TreeNode selNode)
        {
            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);

            // Категория из дерева
            switch (tpt.category)
            {
                case TreeProjCategory.sources:
                    break;

                case TreeProjCategory.sourceItem:
                    break;

                case TreeProjCategory.groupItem:
                    break;

                case TreeProjCategory.tagItem:
                    break;

                case TreeProjCategory.blockItem:
                    break;

                case TreeProjCategory.structures:
                    break;

                case TreeProjCategory.structTagItem:
                    break;

                case TreeProjCategory.includes:
                    break;

                case TreeProjCategory.includeItem:
                    break;

            }

        }

        #region Tree.lib

        // Установить значения из дерева в соответствующие поля фильтра
        private void WriteTabFilter()
        {
            var selNode = treeViewProject.SelectedNode;

            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);
            string text = tpt.text; // selNode.Text;

            // Категория из дерева
            switch (tpt.category)
            {
                case TreeProjCategory.sources:
                    break;

                case TreeProjCategory.sourceItem:
                    comboBoxGroupFilterSource.Text = text;
                    comboBoxTagFilterSource.Text = text;
                    //DataTableLib.dtGroup.TextFilter();
                    //DataTableLib.dtTag.TextFilter();
                    GroupFilter();
                    TagFilter();
                    break;

                case TreeProjCategory.groupItem:
                    comboBoxTagFilterGroup.Text = text;
                    //DataTableLib.dtTag.TextFilter();
                    GroupFilter();
                    break;

                case TreeProjCategory.tagItem:
                    break;

                case TreeProjCategory.blockItem:
                    comboBoxTagFilterBlock.Text = text;
                    //DataTableLib.dtTag.TextFilter();
                    TagFilter();
                    break;

                case TreeProjCategory.structures:
                    comboBoxStructureTargetFilterParent.Text = text;
                    TargetAndTagFilter();
                    break;

                case TreeProjCategory.structTagItem:
                    break;

                case TreeProjCategory.includes:
                    comboBoxIncludeChildFilterParent.Text = text;
                    IncludeChildFilter();
                    break;

                case TreeProjCategory.includeItem:
                    break;

            }

        }

        #endregion


        #endregion


    }
}
