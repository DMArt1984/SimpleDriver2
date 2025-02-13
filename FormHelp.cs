using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver
{
    public partial class FormHelp : Form
    {
        public Dictionary<string, string> dic;

        public FormHelp()
        {
            InitializeComponent();
        }

        private void FormHelp_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectionIndent = 8;
            richTextBox1.SelectionHangingIndent = 3;
            richTextBox1.SelectionRightIndent = 12;

            foreach (var item in dic)
            {
                richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 11, FontStyle.Bold);
                richTextBox1.AppendText(item.Key);
                richTextBox1.AppendText("\r\n");

                richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 4, FontStyle.Bold);
                richTextBox1.AppendText("\r\n");

                richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, 10, FontStyle.Regular);

                bool col = false;
                foreach (var ch in item.Value)
                {
                    if (ch == '{')
                        col = true;

                    if (col)
                    {
                        richTextBox1.SelectionColor = Color.Blue;
                    }
                    else
                    {
                        richTextBox1.SelectionColor = Color.Black;
                    }

                    richTextBox1.AppendText(ch.ToString());

                    if (ch == '}')
                        col = false;
                }

                richTextBox1.AppendText("\r\n");
                richTextBox1.AppendText("\r\n");
            }
        }
    }
}
