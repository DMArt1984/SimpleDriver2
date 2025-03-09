using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSimpleIDriver;

namespace DML
{
    static class TreeLib
    {
        // Дерево проекта
        static public TreeView tree;

        static public TreeNode treeSGT; // Источники/Группы/Теги
        static public TreeNode treeBlock; // Блоки
        static public TreeNode treeStructure; // Структуры
        static public TreeNode treeInclude; // Классы

        static public ContextMenuStrip contextMenuStripTreeProj;

        // Пункты контекстного меню
        static public void TreeProjContexMenu(TreeNode selNode)
        {
            if (selNode.Tag == null)
                return;

            var tpt = (selNode.Tag as TreeProjTag);

            // Контекстное меню в зависимости от категории
            contextMenuStripTreeProj.Items[0].Visible = false;
            contextMenuStripTreeProj.Items[1].Visible = false;
            switch (tpt.category)
            {
                case TreeProjCategory.sourceItem:
                case TreeProjCategory.groupItem:
                case TreeProjCategory.tagItem:
                case TreeProjCategory.blockItem:
                case TreeProjCategory.structureItem:
                case TreeProjCategory.targetItem:
                case TreeProjCategory.includeItem:
                case TreeProjCategory.changeItem:
                    contextMenuStripTreeProj.Items[0].Visible = true;
                    contextMenuStripTreeProj.Items[1].Visible = true;
                    break;

            }
        }

        // Построить дерево для Источники/Группы/Теги
        static public void DrawTreeSGT()
        {
            // Получить списки для дерева
            var collectionSource = MyTree.SetTreeCollection(DataTableLib.dtSource.dgv, DataTableLib.dtSource.col.Title);
            var collectionGroup = MyTree.SetTreeCollection(DataTableLib.dtGroup.dgv, DataTableLib.dtGroup.col.Title, DataTableLib.dtGroup.col.Source);
            var collectionTag = MyTree.SetTreeCollection(DataTableLib.dtTag.dgv, DataTableLib.dtTag.col.Title, DataTableLib.dtTag.col.Group);

            // Источники
            treeSGT.Nodes.Clear();
            //treeSGT = new TreeNode("Источники данных");
            treeSGT.Tag = new TreeProjTag(TreeProjCategory.sources, 0);
            treeSGT.NodeFont = new Font(tree.Font.FontFamily, 12, FontStyle.Regular);
            //treeSGT.ImageIndex = 2;
            //treeSGT.SelectedImageIndex = 1;

            // Список источников
            foreach (var itemSource in collectionSource)
            {
                TreeNode tnSource = new TreeNode($"{itemSource.Title}");
                tnSource.Tag = new TreeProjTag(TreeProjCategory.sourceItem, itemSource.Id, itemSource.Title);
                tnSource.NodeFont = new Font(tree.Font.FontFamily, 12, FontStyle.Regular);
                tnSource.ImageIndex = 0;

                // Список групп
                foreach (var itemGroup in collectionGroup)
                {
                    if (itemGroup.Link != itemSource.Title)
                        continue;

                    TreeNode tnGroup = new TreeNode($"{itemGroup.Title}");
                    tnGroup.Tag = new TreeProjTag(TreeProjCategory.groupItem, itemGroup.Id, itemGroup.Title);
                    tnGroup.NodeFont = new Font(tree.Font.FontFamily, 10, FontStyle.Regular);
                    tnGroup.ImageIndex = 0;

                    // Список тегов
                    foreach (var itemTag in collectionTag)
                    {
                        if (itemTag.Link != itemGroup.Title)
                            continue;

                        TreeNode tnTag = new TreeNode($"{itemTag.Title}");
                        tnTag.Tag = new TreeProjTag(TreeProjCategory.tagItem, itemTag.Id, itemTag.Title);
                        tnTag.NodeFont = new Font(tree.Font.FontFamily, 8, FontStyle.Regular);
                        tnTag.ImageIndex = 0;

                        tnGroup.Nodes.Add(tnTag); // тег
                    }

                    tnSource.Nodes.Add(tnGroup); // группа
                }

                treeSGT.Nodes.Add(tnSource); // источник
            }
            //treeSGT.Text += (EditorControl.sources.Count > 0) ? $" [ {EditorControl.sources.Count} ]" : "";
            //treeViewProject.Nodes.Add(treeSources);
        }

        // Построить дерево для Блоков
        static public void DrawTreeBlock()
        {
            // Получение списка путей групп из таблицы тегов
            List<string> blocks = new List<string>();
            foreach (DataGridViewRow row in DataTableLib.dtTag.dgv.Rows)
            {
                var block = row.Cells[DataTableLib.dtTag.col.Block].Value;
                if (block == null)
                    continue;

                if (String.IsNullOrWhiteSpace(block.ToString()))
                    continue;

                blocks.Add(block.ToString());
            }
            // Убираем повторения
            blocks = blocks.Distinct().OrderBy(x => x).ToList();

            // Блоки
            TreeLib.treeBlock.Nodes.Clear();
            //treeBlock = new TreeNode("Блоки");
            TreeLib.treeBlock.Tag = new TreeProjTag(TreeProjCategory.blocks, 0);
            TreeLib.treeBlock.NodeFont = new Font(tree.Font.FontFamily, 12, FontStyle.Regular);
            //treeBlock.ImageIndex = 2;
            //treeBlock.SelectedImageIndex = 1;

            // Перебор всех путей
            foreach (string pathBlock in blocks)
            {
                if (String.IsNullOrWhiteSpace(pathBlock))
                    continue;

                TreeNode tn = TreeLib.treeBlock;
                string[] parts = pathBlock.Split('.'); // A.B.C.D
                foreach (string item in parts)
                {
                    if (String.IsNullOrWhiteSpace(item))
                        continue;

                    var exist = tn.Nodes.Find(item, false);
                    if (exist == null || exist.Length == 0)
                    {
                        TreeNode next = new TreeNode(item);
                        next.Name = item;
                        next.ToolTipText = pathBlock;
                        next.Tag = new TreeProjTag(TreeProjCategory.blockItem, 0, pathBlock);
                        next.NodeFont = new Font(tree.Font.FontFamily, 10, FontStyle.Regular);
                        tn.Nodes.Add(next);
                        tn = next;
                    }
                    else
                    {
                        tn = exist[0];
                    }

                }

            }

        }

        // Построить дерево для Структур
        static public void DrawTreeStructure()
        {
            // Получить списки для дерева
            var collectionStructure = MyTree.SetTreeCollection(DataTableLib.dtStructure.dgv, DataTableLib.dtStructure.col.Title);
            var collectionTarget = MyTree.SetTreeCollection(DataTableLib.dtTarget.dgv, DataTableLib.dtTarget.col.Tag, DataTableLib.dtTarget.col.Structure);

            // Структуры
            TreeLib.treeStructure.Nodes.Clear();
            //treeStructure = new TreeNode("Структуры");
            TreeLib.treeStructure.Tag = new TreeProjTag(TreeProjCategory.structures, 0);
            TreeLib.treeStructure.NodeFont = new Font(tree.Font.FontFamily, 12, FontStyle.Regular);
            //treeStructure.ImageIndex = 2;
            //treeStructure.SelectedImageIndex = 1;

            // Список структур
            foreach (var itemStructure in collectionStructure)
            {
                TreeNode tnStructure = new TreeNode($"{itemStructure.Title}");
                tnStructure.Tag = new TreeProjTag(TreeProjCategory.structureItem, itemStructure.Id);
                tnStructure.NodeFont = new Font(tree.Font.FontFamily, 12, FontStyle.Regular);
                tnStructure.ImageIndex = 0;

                // Список тегов структур
                foreach (var itemTarget in collectionTarget)
                {
                    if (itemTarget.Link != itemStructure.Title)
                        continue;

                    TreeNode tnTarget = new TreeNode($"{itemTarget.Title}");
                    tnTarget.Tag = new TreeProjTag(TreeProjCategory.targetItem, itemTarget.Id);
                    tnTarget.NodeFont = new Font(tree.Font.FontFamily, 10, FontStyle.Regular);
                    tnTarget.ImageIndex = 0;

                    tnStructure.Nodes.Add(tnTarget); // тег структуры
                }
                TreeLib.treeStructure.Nodes.Add(tnStructure); // структура
            }

        }

        // Построить дерево для Классов
        static public void DrawTreeInclude()
        {
            // Получить списки для дерева
            var collectionInclude = MyTree.SetTreeCollection(DataTableLib.dtInclude.dgv, DataTableLib.dtInclude.col.Prefix);
            var collectionChange = MyTree.SetTreeCollection(DataTableLib.dtIncludeChild.dgv, DataTableLib.dtIncludeChild.col.ChangeFrom, DataTableLib.dtIncludeChild.col.Prefix);

            // Классы
            TreeLib.treeInclude.Nodes.Clear();
            //treeInclude = new TreeNode("Классы");
            TreeLib.treeInclude.Tag = new TreeProjTag(TreeProjCategory.includes, 0);
            TreeLib.treeInclude.NodeFont = new Font(tree.Font.FontFamily, 12, FontStyle.Regular);
            //treeInclude.ImageIndex = 2;
            //treeInclude.SelectedImageIndex = 1;

            // Список классов
            foreach (var itemInclude in collectionInclude)
            {
                TreeNode tnInclude = new TreeNode($"{itemInclude.Title}");
                tnInclude.Tag = new TreeProjTag(TreeProjCategory.includeItem, itemInclude.Id);
                tnInclude.NodeFont = new Font(tree.Font.FontFamily, 12, FontStyle.Regular);
                tnInclude.ImageIndex = 0;

                // Список замен
                foreach (var itemChange in collectionChange)
                {
                    if (itemChange.Link != itemInclude.Title)
                        continue;

                    TreeNode tnChange = new TreeNode($"{itemChange.Title}");
                    tnChange.Tag = new TreeProjTag(TreeProjCategory.changeItem, itemChange.Id);
                    tnChange.NodeFont = new Font(tree.Font.FontFamily, 10, FontStyle.Regular);
                    tnChange.ImageIndex = 0;

                    tnInclude.Nodes.Add(tnChange); // замена
                }
                TreeLib.treeInclude.Nodes.Add(tnInclude); // классы
            }
        }

    }
}
