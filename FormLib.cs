using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver
{
    static class FormLib
    {



        // Добавление в текста в Combobox
        static public void SaveTextComboBox(ComboBox comboBox, string text = "")
        {
            if (text == "")
                text = comboBox.Text;

            if (String.IsNullOrWhiteSpace(text) == false)
            {
                if (comboBox.Items.Contains(text) == false)
                {
                    comboBox.Items.Add(text);
                }
            }
        }

        static public class SourceForm
        {
            static public DataGridView dgv;

            static public CheckBox cbEditor;
            static public CheckBox cbDesc;
            static public CheckBox cbRuntime;
            static public CheckBox cbStatistic;

            static public TextBox tbFilter;

            #region DGV.Columns
            static public void CheckColumns()
            {
                bool checkE = cbEditor.Checked;
                dgv.Columns["sourceID"].Visible = checkE;
                dgv.Columns["sourceAutoRestart"].Visible = checkE;
                dgv.Columns["sourceDriver"].Visible = checkE;
                dgv.Columns["sourceAddress"].Visible = checkE;
                dgv.RowHeadersVisible = checkE;
                dgv.ReadOnly = !checkE;

                bool checkD = cbDesc.Checked;
                dgv.Columns["sourceDesc"].Visible = checkD;

                bool checkR = cbRuntime.Checked;
                dgv.Columns["sourceCalc"].Visible = checkR;
                dgv.Columns["sourceStatus"].Visible = checkR;
                dgv.Columns["sourceMessage"].Visible = checkR;

                bool checkS = cbStatistic.Checked;
                dgv.Columns["sourceTags"].Visible = checkS;
                dgv.Columns["sourceStatistic"].Visible = checkS;

            }

            #endregion

            #region Filter
            static public void TextFilter()
            {
                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtSource.GetColumnIndexFilter(), DTLib.dtSource.GetPairFilter());
            }
            #endregion

        }

        static public class GroupForm
        {
            static public DataGridView dgv;

            static public CheckBox cbEditor;
            static public CheckBox cbDesc;
            static public CheckBox cbRuntime;
            static public CheckBox cbStatistic;
            static public CheckBox cbGroupSource;

            static public TextBox tbFilter;
            static public ComboBox coFilterSource;

            #region DGV.Columns
            static public void CheckColumns()
            {
                bool checkE = cbEditor.Checked;
                dgv.Columns["groupID"].Visible = checkE;
                dgv.Columns["groupPeriod"].Visible = checkE;
                dgv.RowHeadersVisible = checkE;
                dgv.ReadOnly = !checkE;

                bool checkD = cbDesc.Checked;
                dgv.Columns["groupDesc"].Visible = checkD;

                bool checkR = cbRuntime.Checked;
                dgv.Columns["groupCalc"].Visible = checkR;
                dgv.Columns["groupStatus"].Visible = checkR;

                bool checkST = cbStatistic.Checked;
                dgv.Columns["groupTags"].Visible = checkST;
                dgv.Columns["groupStatistic"].Visible = checkST;

                bool checkSource = cbGroupSource.Checked;
                dgv.Columns["groupSource"].Visible = checkSource;
            }
            #endregion

            #region Filter
            static public void TextFilter()
            {
                string text = coFilterSource.Text;
                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtGroup.GetColumnIndexFilter(), DTLib.dtGroup.GetPairFilter(text));
            }
            #endregion

        }

        static public class TagForm
        {
            static public DataGridView dgv;

            static public CheckBox cbEditor;
            static public CheckBox cbDesc;
            static public CheckBox cbRuntime;
            static public CheckBox cbStatistic;
            static public CheckBox cbBP;
            static public CheckBox cbSG;

            static public TextBox tbFilter;
            static public ComboBox coFilterSource;
            static public ComboBox coFilterGroup;
            static public ComboBox coFilterBlock;
            static public ComboBox coFilterPage;

            #region DGV.Columns
            static public void CheckColumns()
            {
                bool checkE = cbEditor.Checked;
                dgv.Columns["tagID"].Visible = checkE;
                dgv.Columns["tagAddress"].Visible = checkE;
                dgv.Columns["tagCommand"].Visible = checkE;
                dgv.Columns["tagWriteValue"].Visible = checkE;
                dgv.Columns["tagWriteTag"].Visible = checkE;
                dgv.RowHeadersVisible = checkE;
                dgv.ReadOnly = !checkE;

                bool checkD = cbDesc.Checked;
                dgv.Columns["tagDesc"].Visible = checkD;

                bool checkR = cbRuntime.Checked;
                dgv.Columns["tagCalc"].Visible = checkR;
                dgv.Columns["tagValue"].Visible = checkR;
                dgv.Columns["tagStatus"].Visible = checkR;
                dgv.Columns["tagMessage"].Visible = checkR;

                bool checkS = cbStatistic.Checked;
                dgv.Columns["tagStatistic"].Visible = checkS;

                bool checkBP = cbBP.Checked;
                dgv.Columns["tagBlock"].Visible = checkBP;
                dgv.Columns["tagPage"].Visible = checkBP;

                bool checkSG = cbSG.Checked;
                dgv.Columns["tagSource"].Visible = checkSG;
                dgv.Columns["tagGroup"].Visible = checkSG;
            }
            #endregion

            #region Filter
            static public void TextFilter()
            {
                string text1 = coFilterSource.Text;
                string text2 = coFilterGroup.Text;
                string text3 = coFilterBlock.Text;
                string text4 = coFilterPage.Text;

                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtTag.GetColumnIndexFilter(), DTLib.dtTag.GetPairFilter(text1, text2, text3, text4));
            }
            #endregion

            // Обновить связи тегов к источникам от групп
            static public void UpdateDGVTagSourceLink()
            {
                // Получить списки для...
                var collectionGroup = MyTree.SetTreeCollection(GroupForm.dgv, DTLib.dtGroup.col.Title, DTLib.dtGroup.col.Source);

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string sourceTitle = "";

                    var group = row.Cells[DTLib.dtTag.col.Group].Value;
                    if (group != null)
                    {
                        string groupTitle = group.ToString();
                        if (String.IsNullOrWhiteSpace(groupTitle) == false)
                        {
                            var groupItem = collectionGroup.FirstOrDefault(x => x.Title == groupTitle);
                            if (groupItem.Id > 0)
                            {
                                sourceTitle = (String.IsNullOrWhiteSpace(groupItem.Link)) ? "" : groupItem.Link;
                            }
                        }
                    }

                    row.Cells[DTLib.dtTag.col.Source].Value = sourceTitle;
                }

            }

        }

        static public class StructureForm
        {
            static public DataGridView dgv;

            static public TextBox tbFilter;

            #region Filter
            static public void StructureFilter()
            {
                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtStructure.GetColumnIndexFilter(), DTLib.dtStructure.GetPairFilter());
            }
            #endregion

        }

        static public class StructureTargetForm
        {
            static public DataGridView dgv;

            static public TextBox tbFilter;
            static public ComboBox coFilterParent;

            #region Filter
            static public void StructureTargetFilter()
            {
                string text = coFilterParent.Text;

                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtTarget.GetColumnIndexFilter(), DTLib.dtTarget.GetPairFilter(text));
            }
            #endregion

        }

        static public class IncludeForm
        {
            static public DataGridView dgv;

            static public TextBox tbFilter;

            #region Filter
            static public void IncludeFilter()
            {
                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtInclude.GetColumnIndexFilter(), DTLib.dtInclude.GetPairFilter());
            }
            #endregion
        }

        static public class IncludeChildForm
        {
            static public DataGridView dgv;

            static public TextBox tbFilter;
            static public ComboBox coFilterParent;

            #region Filter
            static public void IncludeChildFilter()
            {
                string text = coFilterParent.Text;

                DTLib.TableFilter(tbFilter.Text, dgv,
                    DTLib.dtIncludeChild.GetColumnIndexFilter(), DTLib.dtIncludeChild.GetPairFilter(text));
            }
            #endregion


        }

        

    }
}
