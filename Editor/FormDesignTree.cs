using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver.Editor
{
    public partial class FormDesignTree : Form
    {
        private FormDesign design;

        public FormDesignTree(FormDesign design)
        {
            InitializeComponent();
            this.design = design;
        }

        private void FormDesignTree_Load(object sender, EventArgs e)
        {

        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildTree();
        }

        private void BuildTree()
        {
            // Очищаем текущее дерево
            treeView1.Nodes.Clear();

            // Группируем элементы
            var groupedElements = design.GetAppElements()
                .GroupBy(e => string.IsNullOrEmpty(e.Page) ? "Без страницы" : e.Page)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(e => string.IsNullOrEmpty(e.Template) ? "Без шаблона" : e.Template)
                          .ToDictionary(
                              t => t.Key,
                              t => t.GroupBy(e => e.Group == 0 ? "Без группы" : e.Group.ToString())
                                    .ToDictionary(
                                        gr => gr.Key,
                                        gr => gr.ToList()
                                    )
                          )
                );

            // Строим дерево
            foreach (var pageGroup in groupedElements)
            {
                TreeNode pageNode = new TreeNode(pageGroup.Key);

                foreach (var templateGroup in pageGroup.Value)
                {
                    TreeNode templateNode = new TreeNode(templateGroup.Key);

                    foreach (var group in templateGroup.Value)
                    {
                        TreeNode groupNode = new TreeNode(group.Key);

                        foreach (var element in group.Value)
                        {
                            TreeNode elementNode = new TreeNode(element.Control.Name);

                            // Если у элемента нет Page, Template и Group = 0, добавляем его на верхний уровень
                            if (string.IsNullOrEmpty(element.Page) && string.IsNullOrEmpty(element.Template) && element.Group == 0)
                            {
                                treeView1.Nodes.Add(elementNode);
                            }
                            // Если нет Template и Group = 0, добавляем его сразу в Page
                            else if (string.IsNullOrEmpty(element.Template) && element.Group == 0)
                            {
                                pageNode.Nodes.Add(elementNode);
                            }
                            // Если нет Group (или 0), добавляем в Template
                            else if (element.Group == 0)
                            {
                                templateNode.Nodes.Add(elementNode);
                            }
                            else
                            {
                                groupNode.Nodes.Add(elementNode);
                            }
                        }

                        if (groupNode.Nodes.Count > 0)
                        {
                            templateNode.Nodes.Add(groupNode);
                        }
                    }

                    if (templateNode.Nodes.Count > 0)
                    {
                        pageNode.Nodes.Add(templateNode);
                    }
                }

                if (pageNode.Nodes.Count > 0)
                {
                    treeView1.Nodes.Add(pageNode);
                }
            }

            // Разворачиваем дерево после загрузки
            treeView1.ExpandAll();
        }


        private void BuildTree1()
        {
            // Очищаем текущее дерево
            treeView1.Nodes.Clear();

            // Группируем элементы
            var groupedElements = design.GetAppElements()
                .GroupBy(e => e.Page ?? "Без страницы")
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(e => e.Template ?? "Без шаблона")
                          .ToDictionary(
                              t => t.Key,
                              t => t.GroupBy(e => e.Group.ToString() ?? "Без группы")
                                    .ToDictionary(
                                        gr => gr.Key,
                                        gr => gr.ToList()
                                    )
                          )
                );

            // Строим дерево
            foreach (var pageGroup in groupedElements)
            {
                TreeNode pageNode = new TreeNode(pageGroup.Key);

                foreach (var templateGroup in pageGroup.Value)
                {
                    TreeNode templateNode = new TreeNode(templateGroup.Key);

                    foreach (var group in templateGroup.Value)
                    {
                        TreeNode groupNode = new TreeNode(group.Key);

                        foreach (var element in group.Value)
                        {
                            TreeNode elementNode = new TreeNode(element.Control.Name);
                            groupNode.Nodes.Add(elementNode);
                        }

                        templateNode.Nodes.Add(groupNode);
                    }

                    pageNode.Nodes.Add(templateNode);
                }

                treeView1.Nodes.Add(pageNode);
            }

            // Разворачиваем дерево после загрузки
            treeView1.ExpandAll();
        }

    }
}
