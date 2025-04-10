using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WinSimpleIDriver
{
    public static class JsonTreeViewHelper
    {
        public static void PopulateTreeViewFromJson(string jsonString, TreeView treeView)
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            try
            {
                var token = JToken.Parse(jsonString);
                var root = new TreeNode("JSON");
                AddJsonToTree(token, root);
                treeView.Nodes.Add(root);
                treeView.CollapseAll(); // <<< все узлы свернуты
                //treeView.ExpandAll(); // >>> все узлы развернуты
            }
            catch (JsonReaderException ex)
            {
                treeView.Nodes.Add(new TreeNode("Ошибка разбора JSON: " + ex.Message));
            }

            treeView.EndUpdate();
        }

        private static void AddJsonToTree(JToken token, TreeNode parent, int level = 0)
        {
            Font baseFont = parent.TreeView?.Font ?? SystemFonts.DefaultFont;

            // Расчёт масштаба по уровню: 0 → 1.5, 1 → 1.4, ..., минимум 1.0
            float scale = Math.Max(1.0f, 1.5f - level * 0.1f);
            float fontSize = baseFont.Size * scale;
            Font nodeFont = new Font(baseFont.FontFamily, fontSize, baseFont.Style);

            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    var childNode = new TreeNode(property.Name) { NodeFont = nodeFont };
                    AddJsonToTree(property.Value, childNode, level + 1);
                    parent.Nodes.Add(childNode);
                }
            }
            else if (token is JArray array)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    var item = array[i];
                    string nodeText = $"[{i}]";

                    if (item.Type == JTokenType.Object)
                    {
                        var titleProp = item["Title"];
                        if (titleProp != null && titleProp.Type == JTokenType.String)
                            nodeText = titleProp.ToString();
                    }

                    var childNode = new TreeNode(nodeText) { NodeFont = nodeFont };
                    AddJsonToTree(item, childNode, level + 1);
                    parent.Nodes.Add(childNode);
                }
            }
            else if (token is JValue value)
            {
                parent.Text += ": " + value.ToString();
                parent.NodeFont = nodeFont;
            }
            else
            {
                parent.Text += ": " + token.Type.ToString();
                parent.NodeFont = nodeFont;
            }
        }
        private static void AddJsonToTreeOLD(JToken token, TreeNode parent)
        {
            if (token is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    var childNode = new TreeNode(property.Name);
                    AddJsonToTree(property.Value, childNode);
                    parent.Nodes.Add(childNode);
                }
            }
            else if (token is JArray array)
            {
                for (int i = 0; i < array.Count; i++)
                {
                    var item = array[i];
                    string nodeText = $"[{i}]";

                    if (item.Type == JTokenType.Object)
                    {
                        var titleProp = item["Title"];
                        if (titleProp != null && titleProp.Type == JTokenType.String)
                        {
                            nodeText = titleProp.ToString();
                        }
                    }

                    var childNode = new TreeNode(nodeText);
                    AddJsonToTree(item, childNode);
                    parent.Nodes.Add(childNode);
                }
            }
            else if (token is JValue value)
            {
                parent.Text += ": " + value.ToString();
            }
            else
            {
                parent.Text += ": " + token.Type.ToString();
            }
        }
    }
}
