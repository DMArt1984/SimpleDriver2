namespace WinSimpleIDriver.UserControl
{
    partial class SourceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageNew = new System.Windows.Forms.TabPage();
            this.textBoxNewConnection = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonNew = new System.Windows.Forms.Button();
            this.checkBoxNewOff = new System.Windows.Forms.CheckBox();
            this.checkBoxNewAutoRequestAfterOpen = new System.Windows.Forms.CheckBox();
            this.checkBoxNewAutoOpenAfterFail = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxNewDriver = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxNewTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageControl = new System.Windows.Forms.TabPage();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonOne = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxConnection = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.textBoxDriver = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPageSetting = new System.Windows.Forms.TabPage();
            this.buttonCheckedFalse = new System.Windows.Forms.Button();
            this.groupBoxSett = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoRequestAfterOpen = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoOpenAfterFail = new System.Windows.Forms.CheckBox();
            this.tabPageDescription = new System.Windows.Forms.TabPage();
            this.richTextBoxDesc = new System.Windows.Forms.RichTextBox();
            this.tabPageError = new System.Windows.Forms.TabPage();
            this.buttonTestPing = new System.Windows.Forms.Button();
            this.buttonTestHost = new System.Windows.Forms.Button();
            this.buttonResetError = new System.Windows.Forms.Button();
            this.richTextBoxActiveError = new System.Windows.Forms.RichTextBox();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.checkBoxEnableLog = new System.Windows.Forms.CheckBox();
            this.buttonResetLog = new System.Windows.Forms.Button();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageNew.SuspendLayout();
            this.tabPageControl.SuspendLayout();
            this.tabPageSetting.SuspendLayout();
            this.groupBoxSett.SuspendLayout();
            this.tabPageDescription.SuspendLayout();
            this.tabPageError.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageNew);
            this.tabControl1.Controls.Add(this.tabPageControl);
            this.tabControl1.Controls.Add(this.tabPageSetting);
            this.tabControl1.Controls.Add(this.tabPageDescription);
            this.tabControl1.Controls.Add(this.tabPageError);
            this.tabControl1.Controls.Add(this.tabPageLog);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(978, 545);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.Visible = false;
            // 
            // tabPageNew
            // 
            this.tabPageNew.Controls.Add(this.textBoxNewConnection);
            this.tabPageNew.Controls.Add(this.label6);
            this.tabPageNew.Controls.Add(this.buttonNew);
            this.tabPageNew.Controls.Add(this.checkBoxNewOff);
            this.tabPageNew.Controls.Add(this.checkBoxNewAutoRequestAfterOpen);
            this.tabPageNew.Controls.Add(this.checkBoxNewAutoOpenAfterFail);
            this.tabPageNew.Controls.Add(this.button2);
            this.tabPageNew.Controls.Add(this.comboBoxNewDriver);
            this.tabPageNew.Controls.Add(this.label4);
            this.tabPageNew.Controls.Add(this.textBoxNewTitle);
            this.tabPageNew.Controls.Add(this.label2);
            this.tabPageNew.Location = new System.Drawing.Point(4, 24);
            this.tabPageNew.Name = "tabPageNew";
            this.tabPageNew.Size = new System.Drawing.Size(970, 517);
            this.tabPageNew.TabIndex = 5;
            this.tabPageNew.Text = "Настройки";
            this.tabPageNew.UseVisualStyleBackColor = true;
            // 
            // textBoxNewConnection
            // 
            this.textBoxNewConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNewConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxNewConnection.Location = new System.Drawing.Point(11, 115);
            this.textBoxNewConnection.Margin = new System.Windows.Forms.Padding(8);
            this.textBoxNewConnection.Multiline = true;
            this.textBoxNewConnection.Name = "textBoxNewConnection";
            this.textBoxNewConnection.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxNewConnection.Size = new System.Drawing.Size(951, 57);
            this.textBoxNewConnection.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 98);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 15);
            this.label6.TabIndex = 28;
            this.label6.Text = "Строка подключения";
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonNew.Location = new System.Drawing.Point(403, 465);
            this.buttonNew.Margin = new System.Windows.Forms.Padding(2);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(166, 24);
            this.buttonNew.TabIndex = 27;
            this.buttonNew.Text = "Добавить новый";
            this.buttonNew.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewOff
            // 
            this.checkBoxNewOff.AutoSize = true;
            this.checkBoxNewOff.Location = new System.Drawing.Point(11, 180);
            this.checkBoxNewOff.Name = "checkBoxNewOff";
            this.checkBoxNewOff.Size = new System.Drawing.Size(76, 19);
            this.checkBoxNewOff.TabIndex = 24;
            this.checkBoxNewOff.Text = "Включен";
            this.checkBoxNewOff.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewAutoRequestAfterOpen
            // 
            this.checkBoxNewAutoRequestAfterOpen.AutoSize = true;
            this.checkBoxNewAutoRequestAfterOpen.Location = new System.Drawing.Point(11, 236);
            this.checkBoxNewAutoRequestAfterOpen.Name = "checkBoxNewAutoRequestAfterOpen";
            this.checkBoxNewAutoRequestAfterOpen.Size = new System.Drawing.Size(324, 19);
            this.checkBoxNewAutoRequestAfterOpen.TabIndex = 22;
            this.checkBoxNewAutoRequestAfterOpen.Text = "Автоматический запуск опроса после подключения";
            this.checkBoxNewAutoRequestAfterOpen.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewAutoOpenAfterFail
            // 
            this.checkBoxNewAutoOpenAfterFail.AutoSize = true;
            this.checkBoxNewAutoOpenAfterFail.Location = new System.Drawing.Point(11, 208);
            this.checkBoxNewAutoOpenAfterFail.Name = "checkBoxNewAutoOpenAfterFail";
            this.checkBoxNewAutoOpenAfterFail.Size = new System.Drawing.Size(314, 19);
            this.checkBoxNewAutoOpenAfterFail.TabIndex = 23;
            this.checkBoxNewAutoOpenAfterFail.Text = "Автоматическое переподключение после ошибки";
            this.checkBoxNewAutoOpenAfterFail.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(858, 35);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 27);
            this.button2.TabIndex = 19;
            this.button2.Text = "Справка";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // comboBoxNewDriver
            // 
            this.comboBoxNewDriver.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxNewDriver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNewDriver.FormattingEnabled = true;
            this.comboBoxNewDriver.Location = new System.Drawing.Point(78, 36);
            this.comboBoxNewDriver.Name = "comboBoxNewDriver";
            this.comboBoxNewDriver.Size = new System.Drawing.Size(775, 23);
            this.comboBoxNewDriver.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 39);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 15);
            this.label4.TabIndex = 17;
            this.label4.Text = "Драйвер";
            // 
            // textBoxNewTitle
            // 
            this.textBoxNewTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNewTitle.Location = new System.Drawing.Point(78, 9);
            this.textBoxNewTitle.Name = "textBoxNewTitle";
            this.textBoxNewTitle.Size = new System.Drawing.Size(884, 21);
            this.textBoxNewTitle.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "Название";
            // 
            // tabPageControl
            // 
            this.tabPageControl.Controls.Add(this.textBoxTitle);
            this.tabPageControl.Controls.Add(this.button1);
            this.tabPageControl.Controls.Add(this.textBoxMessage);
            this.tabPageControl.Controls.Add(this.labelMessage);
            this.tabPageControl.Controls.Add(this.buttonStop);
            this.tabPageControl.Controls.Add(this.buttonStart);
            this.tabPageControl.Controls.Add(this.buttonOne);
            this.tabPageControl.Controls.Add(this.buttonDisconnect);
            this.tabPageControl.Controls.Add(this.buttonConnect);
            this.tabPageControl.Controls.Add(this.textBoxConnection);
            this.tabPageControl.Controls.Add(this.label1);
            this.tabPageControl.Controls.Add(this.buttonHelp);
            this.tabPageControl.Controls.Add(this.textBoxDriver);
            this.tabPageControl.Controls.Add(this.label3);
            this.tabPageControl.Location = new System.Drawing.Point(4, 24);
            this.tabPageControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageControl.Name = "tabPageControl";
            this.tabPageControl.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageControl.Size = new System.Drawing.Size(970, 517);
            this.tabPageControl.TabIndex = 0;
            this.tabPageControl.Text = "Управление";
            this.tabPageControl.UseVisualStyleBackColor = true;
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxTitle.Location = new System.Drawing.Point(448, 9);
            this.textBoxTitle.Margin = new System.Windows.Forms.Padding(8, 2, 2, 2);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.ReadOnly = true;
            this.textBoxTitle.Size = new System.Drawing.Size(162, 21);
            this.textBoxTitle.TabIndex = 27;
            this.textBoxTitle.Text = "нет";
            this.textBoxTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(850, 468);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 24);
            this.button1.TabIndex = 26;
            this.button1.Text = "Очистить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessage.Location = new System.Drawing.Point(12, 172);
            this.textBoxMessage.Margin = new System.Windows.Forms.Padding(8);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ReadOnly = true;
            this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessage.Size = new System.Drawing.Size(950, 287);
            this.textBoxMessage.TabIndex = 25;
            // 
            // labelMessage
            // 
            this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(14, 151);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(73, 15);
            this.labelMessage.TabIndex = 24;
            this.labelMessage.Text = "Сообщения";
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStop.Location = new System.Drawing.Point(880, 119);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(81, 24);
            this.buttonStop.TabIndex = 22;
            this.buttonStop.Text = "Стоп";
            this.buttonStop.UseVisualStyleBackColor = true;
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(709, 119);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(2);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(81, 24);
            this.buttonStart.TabIndex = 21;
            this.buttonStart.Text = "Пуск";
            this.buttonStart.UseVisualStyleBackColor = true;
            // 
            // buttonOne
            // 
            this.buttonOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOne.Location = new System.Drawing.Point(794, 119);
            this.buttonOne.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOne.Name = "buttonOne";
            this.buttonOne.Size = new System.Drawing.Size(81, 24);
            this.buttonOne.TabIndex = 23;
            this.buttonOne.Text = "Один запрос";
            this.buttonOne.UseVisualStyleBackColor = true;
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(128, 119);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(111, 24);
            this.buttonDisconnect.TabIndex = 20;
            this.buttonDisconnect.Text = "Отключить";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            // 
            // buttonConnect
            // 
            this.buttonConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonConnect.Location = new System.Drawing.Point(12, 119);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(111, 24);
            this.buttonConnect.TabIndex = 19;
            this.buttonConnect.Text = "Подключить";
            this.buttonConnect.UseVisualStyleBackColor = true;
            // 
            // textBoxConnection
            // 
            this.textBoxConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxConnection.Location = new System.Drawing.Point(12, 54);
            this.textBoxConnection.Margin = new System.Windows.Forms.Padding(8);
            this.textBoxConnection.Multiline = true;
            this.textBoxConnection.Name = "textBoxConnection";
            this.textBoxConnection.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxConnection.Size = new System.Drawing.Size(950, 57);
            this.textBoxConnection.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Строка подключения";
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(857, 7);
            this.buttonHelp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(104, 23);
            this.buttonHelp.TabIndex = 16;
            this.buttonHelp.Text = "Справка";
            this.buttonHelp.UseVisualStyleBackColor = true;
            // 
            // textBoxDriver
            // 
            this.textBoxDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDriver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDriver.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxDriver.Location = new System.Drawing.Point(690, 9);
            this.textBoxDriver.Margin = new System.Windows.Forms.Padding(8, 2, 2, 2);
            this.textBoxDriver.Name = "textBoxDriver";
            this.textBoxDriver.ReadOnly = true;
            this.textBoxDriver.Size = new System.Drawing.Size(162, 21);
            this.textBoxDriver.TabIndex = 15;
            this.textBoxDriver.Text = "нет";
            this.textBoxDriver.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(630, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Драйвер";
            // 
            // tabPageSetting
            // 
            this.tabPageSetting.Controls.Add(this.buttonCheckedFalse);
            this.tabPageSetting.Controls.Add(this.groupBoxSett);
            this.tabPageSetting.Location = new System.Drawing.Point(4, 24);
            this.tabPageSetting.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageSetting.Name = "tabPageSetting";
            this.tabPageSetting.Size = new System.Drawing.Size(970, 517);
            this.tabPageSetting.TabIndex = 2;
            this.tabPageSetting.Text = "Параметры";
            this.tabPageSetting.UseVisualStyleBackColor = true;
            // 
            // buttonCheckedFalse
            // 
            this.buttonCheckedFalse.Location = new System.Drawing.Point(7, 116);
            this.buttonCheckedFalse.Name = "buttonCheckedFalse";
            this.buttonCheckedFalse.Size = new System.Drawing.Size(136, 29);
            this.buttonCheckedFalse.TabIndex = 4;
            this.buttonCheckedFalse.Text = "Снять автомат";
            this.buttonCheckedFalse.UseVisualStyleBackColor = true;
            // 
            // groupBoxSett
            // 
            this.groupBoxSett.Controls.Add(this.checkBoxAutoRequestAfterOpen);
            this.groupBoxSett.Controls.Add(this.checkBoxAutoOpenAfterFail);
            this.groupBoxSett.Location = new System.Drawing.Point(7, 5);
            this.groupBoxSett.Name = "groupBoxSett";
            this.groupBoxSett.Size = new System.Drawing.Size(522, 105);
            this.groupBoxSett.TabIndex = 3;
            this.groupBoxSett.TabStop = false;
            // 
            // checkBoxAutoRequestAfterOpen
            // 
            this.checkBoxAutoRequestAfterOpen.AutoSize = true;
            this.checkBoxAutoRequestAfterOpen.Location = new System.Drawing.Point(20, 65);
            this.checkBoxAutoRequestAfterOpen.Name = "checkBoxAutoRequestAfterOpen";
            this.checkBoxAutoRequestAfterOpen.Size = new System.Drawing.Size(324, 19);
            this.checkBoxAutoRequestAfterOpen.TabIndex = 0;
            this.checkBoxAutoRequestAfterOpen.Text = "Автоматический запуск опроса после подключения";
            this.checkBoxAutoRequestAfterOpen.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoOpenAfterFail
            // 
            this.checkBoxAutoOpenAfterFail.AutoSize = true;
            this.checkBoxAutoOpenAfterFail.Location = new System.Drawing.Point(20, 30);
            this.checkBoxAutoOpenAfterFail.Name = "checkBoxAutoOpenAfterFail";
            this.checkBoxAutoOpenAfterFail.Size = new System.Drawing.Size(314, 19);
            this.checkBoxAutoOpenAfterFail.TabIndex = 1;
            this.checkBoxAutoOpenAfterFail.Text = "Автоматическое переподключение после ошибки";
            this.checkBoxAutoOpenAfterFail.UseVisualStyleBackColor = true;
            // 
            // tabPageDescription
            // 
            this.tabPageDescription.Controls.Add(this.richTextBoxDesc);
            this.tabPageDescription.Location = new System.Drawing.Point(4, 24);
            this.tabPageDescription.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageDescription.Name = "tabPageDescription";
            this.tabPageDescription.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageDescription.Size = new System.Drawing.Size(970, 517);
            this.tabPageDescription.TabIndex = 1;
            this.tabPageDescription.Text = "Описание";
            this.tabPageDescription.UseVisualStyleBackColor = true;
            // 
            // richTextBoxDesc
            // 
            this.richTextBoxDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxDesc.Location = new System.Drawing.Point(6, 12);
            this.richTextBoxDesc.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxDesc.Name = "richTextBoxDesc";
            this.richTextBoxDesc.ReadOnly = true;
            this.richTextBoxDesc.Size = new System.Drawing.Size(957, 467);
            this.richTextBoxDesc.TabIndex = 4;
            this.richTextBoxDesc.Text = "";
            // 
            // tabPageError
            // 
            this.tabPageError.Controls.Add(this.buttonTestPing);
            this.tabPageError.Controls.Add(this.buttonTestHost);
            this.tabPageError.Controls.Add(this.buttonResetError);
            this.tabPageError.Controls.Add(this.richTextBoxActiveError);
            this.tabPageError.Location = new System.Drawing.Point(4, 24);
            this.tabPageError.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageError.Name = "tabPageError";
            this.tabPageError.Size = new System.Drawing.Size(970, 517);
            this.tabPageError.TabIndex = 3;
            this.tabPageError.Text = "Сообщение/Ошибка";
            this.tabPageError.UseVisualStyleBackColor = true;
            // 
            // buttonTestPing
            // 
            this.buttonTestPing.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonTestPing.Location = new System.Drawing.Point(636, 459);
            this.buttonTestPing.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTestPing.Name = "buttonTestPing";
            this.buttonTestPing.Size = new System.Drawing.Size(118, 28);
            this.buttonTestPing.TabIndex = 8;
            this.buttonTestPing.Text = "Тест ping";
            this.buttonTestPing.UseVisualStyleBackColor = true;
            // 
            // buttonTestHost
            // 
            this.buttonTestHost.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonTestHost.Location = new System.Drawing.Point(232, 459);
            this.buttonTestHost.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTestHost.Name = "buttonTestHost";
            this.buttonTestHost.Size = new System.Drawing.Size(118, 28);
            this.buttonTestHost.TabIndex = 7;
            this.buttonTestHost.Text = "Тест хоста";
            this.buttonTestHost.UseVisualStyleBackColor = true;
            // 
            // buttonResetError
            // 
            this.buttonResetError.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonResetError.Location = new System.Drawing.Point(393, 459);
            this.buttonResetError.Margin = new System.Windows.Forms.Padding(2);
            this.buttonResetError.Name = "buttonResetError";
            this.buttonResetError.Size = new System.Drawing.Size(189, 28);
            this.buttonResetError.TabIndex = 6;
            this.buttonResetError.Text = "Сброс";
            this.buttonResetError.UseVisualStyleBackColor = true;
            // 
            // richTextBoxActiveError
            // 
            this.richTextBoxActiveError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxActiveError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxActiveError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxActiveError.Location = new System.Drawing.Point(6, 11);
            this.richTextBoxActiveError.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxActiveError.Name = "richTextBoxActiveError";
            this.richTextBoxActiveError.ReadOnly = true;
            this.richTextBoxActiveError.Size = new System.Drawing.Size(957, 431);
            this.richTextBoxActiveError.TabIndex = 5;
            this.richTextBoxActiveError.Text = "";
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.checkBoxEnableLog);
            this.tabPageLog.Controls.Add(this.buttonResetLog);
            this.tabPageLog.Controls.Add(this.richTextBoxLog);
            this.tabPageLog.Location = new System.Drawing.Point(4, 24);
            this.tabPageLog.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Size = new System.Drawing.Size(970, 517);
            this.tabPageLog.TabIndex = 4;
            this.tabPageLog.Text = "Лог";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnableLog
            // 
            this.checkBoxEnableLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.checkBoxEnableLog.AutoSize = true;
            this.checkBoxEnableLog.Location = new System.Drawing.Point(271, 460);
            this.checkBoxEnableLog.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxEnableLog.Name = "checkBoxEnableLog";
            this.checkBoxEnableLog.Size = new System.Drawing.Size(105, 19);
            this.checkBoxEnableLog.TabIndex = 8;
            this.checkBoxEnableLog.Text = "Включить лог";
            this.checkBoxEnableLog.UseVisualStyleBackColor = true;
            // 
            // buttonResetLog
            // 
            this.buttonResetLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonResetLog.Location = new System.Drawing.Point(399, 455);
            this.buttonResetLog.Margin = new System.Windows.Forms.Padding(2);
            this.buttonResetLog.Name = "buttonResetLog";
            this.buttonResetLog.Size = new System.Drawing.Size(189, 28);
            this.buttonResetLog.TabIndex = 7;
            this.buttonResetLog.Text = "Очистить";
            this.buttonResetLog.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxLog.Location = new System.Drawing.Point(6, 11);
            this.richTextBoxLog.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(957, 432);
            this.richTextBoxLog.TabIndex = 6;
            this.richTextBoxLog.Text = "";
            // 
            // SourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 545);
            this.Controls.Add(this.tabControl1);
            this.Name = "SourceForm";
            this.Text = "SourceForm";
            this.Load += new System.EventHandler(this.SourceForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageNew.ResumeLayout(false);
            this.tabPageNew.PerformLayout();
            this.tabPageControl.ResumeLayout(false);
            this.tabPageControl.PerformLayout();
            this.tabPageSetting.ResumeLayout(false);
            this.groupBoxSett.ResumeLayout(false);
            this.groupBoxSett.PerformLayout();
            this.tabPageDescription.ResumeLayout(false);
            this.tabPageError.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.tabPageLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageNew;
        private System.Windows.Forms.TextBox textBoxNewConnection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.CheckBox checkBoxNewOff;
        private System.Windows.Forms.CheckBox checkBoxNewAutoRequestAfterOpen;
        private System.Windows.Forms.CheckBox checkBoxNewAutoOpenAfterFail;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxNewDriver;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxNewTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPageControl;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonOne;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxConnection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.TextBox textBoxDriver;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPageSetting;
        private System.Windows.Forms.Button buttonCheckedFalse;
        private System.Windows.Forms.GroupBox groupBoxSett;
        private System.Windows.Forms.CheckBox checkBoxAutoRequestAfterOpen;
        private System.Windows.Forms.CheckBox checkBoxAutoOpenAfterFail;
        private System.Windows.Forms.TabPage tabPageDescription;
        private System.Windows.Forms.RichTextBox richTextBoxDesc;
        private System.Windows.Forms.TabPage tabPageError;
        private System.Windows.Forms.Button buttonTestPing;
        private System.Windows.Forms.Button buttonTestHost;
        private System.Windows.Forms.Button buttonResetError;
        private System.Windows.Forms.RichTextBox richTextBoxActiveError;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.CheckBox checkBoxEnableLog;
        private System.Windows.Forms.Button buttonResetLog;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
    }
}