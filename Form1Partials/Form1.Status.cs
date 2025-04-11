using Connector;
using DML;
using DML.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinSimpleIDriver
{
    public partial class Form1
    {

        #region Status
        // Установить сообщение 1
        private string SetLeftLabelMessage1(string message = "")
        {
            toolStripStatusLabelMessage1.Text = message;
            //ProcessMaster _tempLog = new ProcessMaster(LogTarget.FileOnly);
            //_tempLog.Info(message);
            loggerB.Info(message);
            return message;
        }
        // Установить сообщение 2
        private string SetMidLabelMessage2(string message = "")
        {
            toolStripStatusLabelMessage2.Text = message;
            //ProcessMaster _tempLog = new ProcessMaster(LogTarget.FileOnly);
            //_tempLog.Info(message);
            loggerB.Info(message);
            return message;
        }
        // Установить сообщение 3
        private string SetRightLabelMessage3(string message = "")
        {
            toolStripStatusLabelMessage3.Text = message;
            //ProcessMaster _tempLog = new ProcessMaster(LogTarget.FileOnly);
            //_tempLog.Info(message);
            loggerB.Info(message);
            return message;
        }

        #endregion


        #region LabelAndText
        private void OnlyLabel(eMessageType mtype, string message, ToolStripStatusLabel labelType = null, ToolStripStatusLabel labelMessage = null)
        {
            if (labelType != null)
            {
                labelType.ForeColor = LogHelper.GetColorForMessage(mtype);
                labelType.Text = FormText(LogHelper.TypeMessage(mtype));
            }

            if (labelMessage != null)
            {
                if (labelType == null)
                    labelMessage.ForeColor = LogHelper.GetColorForMessage(mtype);
                labelMessage.Text = FormText(message);
            }
        }

        private string FormText(string value)
        {
            int w = this.Width / 8;
            if (value.Length > w)
                return value.Substring(0, w) + "...";

            return value;
        }
        #endregion
        #region Draw Label
        private void DrawLabelLeft(eMessageType messageType, string message)
        {
            OnlyLabel(messageType, message, toolStripStatusLabelMessage1, null);
        }
        private void DrawLabelRight(eMessageType messageType, string message)
        {
            OnlyLabel(messageType, message, toolStripStatusLabelMessage2, toolStripStatusLabelMessage3);
        }

        #endregion

    }
}
