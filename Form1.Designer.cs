
namespace WinSimpleIDriver
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Источники/Группы/Теги");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Блоки");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Структуры");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Классы");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Страницы");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemImport = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.вилToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemViewTree = new System.Windows.Forms.ToolStripMenuItem();
            this.пускСтопToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поискToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemVer = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerForm = new System.Windows.Forms.SplitContainer();
            this.labelProject = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSource = new System.Windows.Forms.TabPage();
            this.buttonSourceFilter = new System.Windows.Forms.Button();
            this.buttonSourceHelp = new System.Windows.Forms.Button();
            this.checkBoxSourceStatistic = new System.Windows.Forms.CheckBox();
            this.checkBoxSourceDesc = new System.Windows.Forms.CheckBox();
            this.checkBoxSourceRuntime = new System.Windows.Forms.CheckBox();
            this.checkBoxSourceEditor = new System.Windows.Forms.CheckBox();
            this.textBoxSourceFilter = new System.Windows.Forms.TextBox();
            this.buttonSourceView = new System.Windows.Forms.Button();
            this.buttonSourceDel = new System.Windows.Forms.Button();
            this.buttonSourceCopy = new System.Windows.Forms.Button();
            this.dataGridViewSource = new System.Windows.Forms.DataGridView();
            this.sourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceCalc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sourceTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceON = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sourceAutoRestart = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sourceDriver = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.sourceAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceStatistic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageGroup = new System.Windows.Forms.TabPage();
            this.buttonGroupFilter = new System.Windows.Forms.Button();
            this.checkBoxGroupSource = new System.Windows.Forms.CheckBox();
            this.checkBoxGroupRuntime = new System.Windows.Forms.CheckBox();
            this.comboBoxGroupFilterSource = new System.Windows.Forms.ComboBox();
            this.checkBoxGroupStatistic = new System.Windows.Forms.CheckBox();
            this.checkBoxGroupDesc = new System.Windows.Forms.CheckBox();
            this.checkBoxGroupEditor = new System.Windows.Forms.CheckBox();
            this.textBoxGroupFilter = new System.Windows.Forms.TextBox();
            this.buttonGroupView = new System.Windows.Forms.Button();
            this.buttonGroupDel = new System.Windows.Forms.Button();
            this.buttonGroupCopy = new System.Windows.Forms.Button();
            this.dataGridViewGroup = new System.Windows.Forms.DataGridView();
            this.groupID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupCalc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupOn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupStatistic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageTag = new System.Windows.Forms.TabPage();
            this.buttonTagFilter = new System.Windows.Forms.Button();
            this.checkBoxTagSG = new System.Windows.Forms.CheckBox();
            this.checkBoxTagBP = new System.Windows.Forms.CheckBox();
            this.comboBoxTagFilterPage = new System.Windows.Forms.ComboBox();
            this.comboBoxTagFilterBlock = new System.Windows.Forms.ComboBox();
            this.comboBoxTagFilterGroup = new System.Windows.Forms.ComboBox();
            this.comboBoxTagFilterSource = new System.Windows.Forms.ComboBox();
            this.buttonTagHelp = new System.Windows.Forms.Button();
            this.checkBoxTagStatistic = new System.Windows.Forms.CheckBox();
            this.checkBoxTagDesc = new System.Windows.Forms.CheckBox();
            this.checkBoxTagRuntime = new System.Windows.Forms.CheckBox();
            this.checkBoxTagEditor = new System.Windows.Forms.CheckBox();
            this.textBoxTagFilter = new System.Windows.Forms.TextBox();
            this.buttonTagView = new System.Windows.Forms.Button();
            this.buttonTagDel = new System.Windows.Forms.Button();
            this.buttonTagCopy = new System.Windows.Forms.Button();
            this.dataGridViewTag = new System.Windows.Forms.DataGridView();
            this.tagID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagCalc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tagTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagON = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tagValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tagAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagCommand = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tagWriteValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagWriteTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagBlock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagPage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tagStatistic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageStructures = new System.Windows.Forms.TabPage();
            this.splitContainerStructure = new System.Windows.Forms.SplitContainer();
            this.buttonStructureFilter = new System.Windows.Forms.Button();
            this.textBoxStructureFilter = new System.Windows.Forms.TextBox();
            this.dataGridViewStructure = new System.Windows.Forms.DataGridView();
            this.structureID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureOn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.structureConnector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.structureTagSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureTemplate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxTargetFilterSource = new System.Windows.Forms.ComboBox();
            this.buttonTargetFilter = new System.Windows.Forms.Button();
            this.dataGridViewTarget = new System.Windows.Forms.DataGridView();
            this.targetID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetStructure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetOn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.targetAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxTargetFilter = new System.Windows.Forms.TextBox();
            this.tabPageIncludes = new System.Windows.Forms.TabPage();
            this.splitContainerInclude = new System.Windows.Forms.SplitContainer();
            this.buttonIncludeFilter = new System.Windows.Forms.Button();
            this.textBoxIncludeFilter = new System.Windows.Forms.TextBox();
            this.buttonIncludeLeft = new System.Windows.Forms.Button();
            this.dataGridViewInclude = new System.Windows.Forms.DataGridView();
            this.includeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includePrefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includeFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includeChanges = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxChangeFilterInclude = new System.Windows.Forms.ComboBox();
            this.buttonChangeFilter = new System.Windows.Forms.Button();
            this.textBoxChangeFilter = new System.Windows.Forms.TextBox();
            this.buttonIncludeRight = new System.Windows.Forms.Button();
            this.dataGridViewChange = new System.Windows.Forms.DataGridView();
            this.changeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.changePrefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.changeFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.changeTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPagePage = new System.Windows.Forms.TabPage();
            this.buttonStructureLeft = new System.Windows.Forms.Button();
            this.buttonStructureRight = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerForm)).BeginInit();
            this.splitContainerForm.Panel1.SuspendLayout();
            this.splitContainerForm.Panel2.SuspendLayout();
            this.splitContainerForm.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSource)).BeginInit();
            this.tabPageGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroup)).BeginInit();
            this.tabPageTag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTag)).BeginInit();
            this.tabPageStructures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStructure)).BeginInit();
            this.splitContainerStructure.Panel1.SuspendLayout();
            this.splitContainerStructure.Panel2.SuspendLayout();
            this.splitContainerStructure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStructure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTarget)).BeginInit();
            this.tabPageIncludes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInclude)).BeginInit();
            this.splitContainerInclude.Panel1.SuspendLayout();
            this.splitContainerInclude.Panel2.SuspendLayout();
            this.splitContainerInclude.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInclude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChange)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.вилToolStripMenuItem,
            this.пускСтопToolStripMenuItem,
            this.поискToolStripMenuItem,
            this.ToolStripMenuItemHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1415, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemNew,
            this.ToolStripMenuItemOpen,
            this.ToolStripMenuItemSave,
            this.ToolStripMenuItemSaveAs,
            this.toolStripMenuItem2,
            this.ToolStripMenuItemImport,
            this.ToolStripMenuItemExport,
            this.toolStripMenuItem3,
            this.ToolStripMenuItemExit});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 19);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // ToolStripMenuItemNew
            // 
            this.ToolStripMenuItemNew.Name = "ToolStripMenuItemNew";
            this.ToolStripMenuItemNew.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItemNew.Text = "Новый";
            this.ToolStripMenuItemNew.Click += new System.EventHandler(this.ToolStripMenuItemNew_Click);
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItemOpen.Text = "Открыть";
            // 
            // ToolStripMenuItemSave
            // 
            this.ToolStripMenuItemSave.Name = "ToolStripMenuItemSave";
            this.ToolStripMenuItemSave.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItemSave.Text = "Сохранить";
            // 
            // ToolStripMenuItemSaveAs
            // 
            this.ToolStripMenuItemSaveAs.Name = "ToolStripMenuItemSaveAs";
            this.ToolStripMenuItemSaveAs.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItemSaveAs.Text = "Сохранить как...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(159, 6);
            // 
            // ToolStripMenuItemImport
            // 
            this.ToolStripMenuItemImport.Name = "ToolStripMenuItemImport";
            this.ToolStripMenuItemImport.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItemImport.Text = "Импорт";
            // 
            // ToolStripMenuItemExport
            // 
            this.ToolStripMenuItemExport.Name = "ToolStripMenuItemExport";
            this.ToolStripMenuItemExport.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItemExport.Text = "Экспорт";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(159, 6);
            // 
            // ToolStripMenuItemExit
            // 
            this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
            this.ToolStripMenuItemExit.Size = new System.Drawing.Size(162, 22);
            this.ToolStripMenuItemExit.Text = "Выход";
            this.ToolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click);
            // 
            // вилToolStripMenuItem
            // 
            this.вилToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemViewTree});
            this.вилToolStripMenuItem.Name = "вилToolStripMenuItem";
            this.вилToolStripMenuItem.Size = new System.Drawing.Size(39, 19);
            this.вилToolStripMenuItem.Text = "Вид";
            // 
            // ToolStripMenuItemViewTree
            // 
            this.ToolStripMenuItemViewTree.Checked = true;
            this.ToolStripMenuItemViewTree.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripMenuItemViewTree.Name = "ToolStripMenuItemViewTree";
            this.ToolStripMenuItemViewTree.Size = new System.Drawing.Size(114, 22);
            this.ToolStripMenuItemViewTree.Text = "Дерево";
            this.ToolStripMenuItemViewTree.Click += new System.EventHandler(this.ToolStripMenuItemViewTree_Click);
            // 
            // пускСтопToolStripMenuItem
            // 
            this.пускСтопToolStripMenuItem.Name = "пускСтопToolStripMenuItem";
            this.пускСтопToolStripMenuItem.Size = new System.Drawing.Size(78, 19);
            this.пускСтопToolStripMenuItem.Text = "Пуск/Стоп";
            // 
            // поискToolStripMenuItem
            // 
            this.поискToolStripMenuItem.Name = "поискToolStripMenuItem";
            this.поискToolStripMenuItem.Size = new System.Drawing.Size(54, 19);
            this.поискToolStripMenuItem.Text = "Поиск";
            // 
            // ToolStripMenuItemHelp
            // 
            this.ToolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemVer});
            this.ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp";
            this.ToolStripMenuItemHelp.Size = new System.Drawing.Size(65, 19);
            this.ToolStripMenuItemHelp.Text = "Справка";
            // 
            // ToolStripMenuItemVer
            // 
            this.ToolStripMenuItemVer.Name = "ToolStripMenuItemVer";
            this.ToolStripMenuItemVer.Size = new System.Drawing.Size(113, 22);
            this.ToolStripMenuItemVer.Text = "Версия";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 525);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1415, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabel1.Text = "-";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabel2.Text = "-";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabel3.Text = "-";
            // 
            // splitContainerForm
            // 
            this.splitContainerForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerForm.Location = new System.Drawing.Point(0, 25);
            this.splitContainerForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerForm.Name = "splitContainerForm";
            // 
            // splitContainerForm.Panel1
            // 
            this.splitContainerForm.Panel1.Controls.Add(this.labelProject);
            this.splitContainerForm.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainerForm.Panel2
            // 
            this.splitContainerForm.Panel2.Controls.Add(this.tabControl1);
            this.splitContainerForm.Size = new System.Drawing.Size(1415, 500);
            this.splitContainerForm.SplitterDistance = 225;
            this.splitContainerForm.SplitterWidth = 5;
            this.splitContainerForm.TabIndex = 2;
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Location = new System.Drawing.Point(10, 7);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(51, 17);
            this.labelProject.TabIndex = 1;
            this.labelProject.Text = "Проект";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeView1.Location = new System.Drawing.Point(7, 29);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView1.Name = "treeView1";
            treeNode16.Name = "Sources";
            treeNode16.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode16.Tag = "Source";
            treeNode16.Text = "Источники/Группы/Теги";
            treeNode17.Name = "Blocks";
            treeNode17.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode17.Tag = "Block";
            treeNode17.Text = "Блоки";
            treeNode18.Name = "Structures";
            treeNode18.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode18.Text = "Структуры";
            treeNode19.Name = "Includes";
            treeNode19.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode19.Tag = "Include";
            treeNode19.Text = "Классы";
            treeNode20.Name = "Pages";
            treeNode20.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode20.Tag = "Page";
            treeNode20.Text = "Страницы";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20});
            this.treeView1.Size = new System.Drawing.Size(213, 461);
            this.treeView1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSource);
            this.tabControl1.Controls.Add(this.tabPageGroup);
            this.tabControl1.Controls.Add(this.tabPageTag);
            this.tabControl1.Controls.Add(this.tabPageStructures);
            this.tabControl1.Controls.Add(this.tabPageIncludes);
            this.tabControl1.Controls.Add(this.tabPagePage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1185, 500);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageSource
            // 
            this.tabPageSource.Controls.Add(this.buttonSourceFilter);
            this.tabPageSource.Controls.Add(this.buttonSourceHelp);
            this.tabPageSource.Controls.Add(this.checkBoxSourceStatistic);
            this.tabPageSource.Controls.Add(this.checkBoxSourceDesc);
            this.tabPageSource.Controls.Add(this.checkBoxSourceRuntime);
            this.tabPageSource.Controls.Add(this.checkBoxSourceEditor);
            this.tabPageSource.Controls.Add(this.textBoxSourceFilter);
            this.tabPageSource.Controls.Add(this.buttonSourceView);
            this.tabPageSource.Controls.Add(this.buttonSourceDel);
            this.tabPageSource.Controls.Add(this.buttonSourceCopy);
            this.tabPageSource.Controls.Add(this.dataGridViewSource);
            this.tabPageSource.Location = new System.Drawing.Point(4, 26);
            this.tabPageSource.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageSource.Name = "tabPageSource";
            this.tabPageSource.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageSource.Size = new System.Drawing.Size(1177, 470);
            this.tabPageSource.TabIndex = 0;
            this.tabPageSource.Text = "Источники данных";
            this.tabPageSource.UseVisualStyleBackColor = true;
            // 
            // buttonSourceFilter
            // 
            this.buttonSourceFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSourceFilter.Location = new System.Drawing.Point(1099, 38);
            this.buttonSourceFilter.Name = "buttonSourceFilter";
            this.buttonSourceFilter.Size = new System.Drawing.Size(70, 24);
            this.buttonSourceFilter.TabIndex = 11;
            this.buttonSourceFilter.Text = "Фильтр";
            this.buttonSourceFilter.UseVisualStyleBackColor = true;
            this.buttonSourceFilter.Click += new System.EventHandler(this.buttonSourceFilter_Click);
            // 
            // buttonSourceHelp
            // 
            this.buttonSourceHelp.Location = new System.Drawing.Point(265, 7);
            this.buttonSourceHelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSourceHelp.Name = "buttonSourceHelp";
            this.buttonSourceHelp.Size = new System.Drawing.Size(80, 24);
            this.buttonSourceHelp.TabIndex = 10;
            this.buttonSourceHelp.Text = "Справка";
            this.buttonSourceHelp.UseVisualStyleBackColor = true;
            // 
            // checkBoxSourceStatistic
            // 
            this.checkBoxSourceStatistic.AutoSize = true;
            this.checkBoxSourceStatistic.Location = new System.Drawing.Point(653, 10);
            this.checkBoxSourceStatistic.Name = "checkBoxSourceStatistic";
            this.checkBoxSourceStatistic.Size = new System.Drawing.Size(95, 21);
            this.checkBoxSourceStatistic.TabIndex = 9;
            this.checkBoxSourceStatistic.Text = "Статистика";
            this.checkBoxSourceStatistic.UseVisualStyleBackColor = true;
            this.checkBoxSourceStatistic.CheckedChanged += new System.EventHandler(this.checkBoxSourceStatistic_CheckedChanged);
            // 
            // checkBoxSourceDesc
            // 
            this.checkBoxSourceDesc.AutoSize = true;
            this.checkBoxSourceDesc.Checked = true;
            this.checkBoxSourceDesc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSourceDesc.Location = new System.Drawing.Point(543, 10);
            this.checkBoxSourceDesc.Name = "checkBoxSourceDesc";
            this.checkBoxSourceDesc.Size = new System.Drawing.Size(89, 21);
            this.checkBoxSourceDesc.TabIndex = 8;
            this.checkBoxSourceDesc.Text = "Описание";
            this.checkBoxSourceDesc.UseVisualStyleBackColor = true;
            this.checkBoxSourceDesc.CheckedChanged += new System.EventHandler(this.checkBoxSourceDesc_CheckedChanged);
            // 
            // checkBoxSourceRuntime
            // 
            this.checkBoxSourceRuntime.AutoSize = true;
            this.checkBoxSourceRuntime.Location = new System.Drawing.Point(449, 10);
            this.checkBoxSourceRuntime.Name = "checkBoxSourceRuntime";
            this.checkBoxSourceRuntime.Size = new System.Drawing.Size(75, 21);
            this.checkBoxSourceRuntime.TabIndex = 7;
            this.checkBoxSourceRuntime.Text = "Runtime";
            this.checkBoxSourceRuntime.UseVisualStyleBackColor = true;
            this.checkBoxSourceRuntime.CheckedChanged += new System.EventHandler(this.checkBoxSourceRuntime_CheckedChanged);
            // 
            // checkBoxSourceEditor
            // 
            this.checkBoxSourceEditor.AutoSize = true;
            this.checkBoxSourceEditor.Checked = true;
            this.checkBoxSourceEditor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSourceEditor.Location = new System.Drawing.Point(356, 10);
            this.checkBoxSourceEditor.Name = "checkBoxSourceEditor";
            this.checkBoxSourceEditor.Size = new System.Drawing.Size(84, 21);
            this.checkBoxSourceEditor.TabIndex = 6;
            this.checkBoxSourceEditor.Text = "Редактор";
            this.checkBoxSourceEditor.UseVisualStyleBackColor = true;
            this.checkBoxSourceEditor.CheckedChanged += new System.EventHandler(this.checkBoxSourceEditor_CheckedChanged);
            // 
            // textBoxSourceFilter
            // 
            this.textBoxSourceFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSourceFilter.Location = new System.Drawing.Point(7, 39);
            this.textBoxSourceFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxSourceFilter.Name = "textBoxSourceFilter";
            this.textBoxSourceFilter.Size = new System.Drawing.Size(1086, 22);
            this.textBoxSourceFilter.TabIndex = 5;
            this.textBoxSourceFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxSourceFilter.TextChanged += new System.EventHandler(this.textBoxSourceFilter_TextChanged);
            // 
            // buttonSourceView
            // 
            this.buttonSourceView.Location = new System.Drawing.Point(179, 7);
            this.buttonSourceView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSourceView.Name = "buttonSourceView";
            this.buttonSourceView.Size = new System.Drawing.Size(80, 24);
            this.buttonSourceView.TabIndex = 4;
            this.buttonSourceView.Text = "Детали";
            this.buttonSourceView.UseVisualStyleBackColor = true;
            // 
            // buttonSourceDel
            // 
            this.buttonSourceDel.Location = new System.Drawing.Point(93, 7);
            this.buttonSourceDel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSourceDel.Name = "buttonSourceDel";
            this.buttonSourceDel.Size = new System.Drawing.Size(80, 24);
            this.buttonSourceDel.TabIndex = 3;
            this.buttonSourceDel.Text = "Удалить";
            this.buttonSourceDel.UseVisualStyleBackColor = true;
            // 
            // buttonSourceCopy
            // 
            this.buttonSourceCopy.Location = new System.Drawing.Point(7, 7);
            this.buttonSourceCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSourceCopy.Name = "buttonSourceCopy";
            this.buttonSourceCopy.Size = new System.Drawing.Size(80, 24);
            this.buttonSourceCopy.TabIndex = 2;
            this.buttonSourceCopy.Text = "Копия";
            this.buttonSourceCopy.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSource
            // 
            this.dataGridViewSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSource.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sourceID,
            this.sourceCalc,
            this.sourceTitle,
            this.sourceON,
            this.sourceAutoRestart,
            this.sourceDriver,
            this.sourceAddress,
            this.sourceDesc,
            this.sourceStatus,
            this.sourceMessage,
            this.sourceTags,
            this.sourceStatistic});
            this.dataGridViewSource.Location = new System.Drawing.Point(7, 67);
            this.dataGridViewSource.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewSource.MultiSelect = false;
            this.dataGridViewSource.Name = "dataGridViewSource";
            this.dataGridViewSource.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSource.Size = new System.Drawing.Size(1162, 395);
            this.dataGridViewSource.TabIndex = 1;
            this.dataGridViewSource.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewSource_CellBeginEdit);
            this.dataGridViewSource.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSource_CellEndEdit);
            this.dataGridViewSource.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSource_CellValueChanged);
            this.dataGridViewSource.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridViewSource_RowStateChanged);
            this.dataGridViewSource.SelectionChanged += new System.EventHandler(this.dataGridViewSource_SelectionChanged);
            this.dataGridViewSource.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewSource_UserAddedRow);
            // 
            // sourceID
            // 
            this.sourceID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle22.Format = "N2";
            dataGridViewCellStyle22.NullValue = null;
            this.sourceID.DefaultCellStyle = dataGridViewCellStyle22;
            this.sourceID.HeaderText = "ID";
            this.sourceID.Name = "sourceID";
            this.sourceID.ReadOnly = true;
            this.sourceID.Width = 45;
            // 
            // sourceCalc
            // 
            this.sourceCalc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sourceCalc.HeaderText = "Р";
            this.sourceCalc.Name = "sourceCalc";
            this.sourceCalc.ReadOnly = true;
            this.sourceCalc.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sourceCalc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sourceCalc.Width = 40;
            // 
            // sourceTitle
            // 
            this.sourceTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sourceTitle.HeaderText = "Название";
            this.sourceTitle.Name = "sourceTitle";
            // 
            // sourceON
            // 
            this.sourceON.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sourceON.HeaderText = "ВКЛ";
            this.sourceON.Name = "sourceON";
            this.sourceON.Width = 37;
            // 
            // sourceAutoRestart
            // 
            this.sourceAutoRestart.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sourceAutoRestart.HeaderText = "АПП";
            this.sourceAutoRestart.Name = "sourceAutoRestart";
            this.sourceAutoRestart.Width = 39;
            // 
            // sourceDriver
            // 
            this.sourceDriver.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sourceDriver.HeaderText = "Драйвер";
            this.sourceDriver.Name = "sourceDriver";
            // 
            // sourceAddress
            // 
            this.sourceAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sourceAddress.HeaderText = "Адрес";
            this.sourceAddress.Name = "sourceAddress";
            // 
            // sourceDesc
            // 
            this.sourceDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sourceDesc.HeaderText = "Описание";
            this.sourceDesc.Name = "sourceDesc";
            // 
            // sourceStatus
            // 
            this.sourceStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sourceStatus.HeaderText = "Статус";
            this.sourceStatus.Name = "sourceStatus";
            this.sourceStatus.ReadOnly = true;
            // 
            // sourceMessage
            // 
            this.sourceMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sourceMessage.HeaderText = "Сообщение";
            this.sourceMessage.Name = "sourceMessage";
            this.sourceMessage.ReadOnly = true;
            // 
            // sourceTags
            // 
            this.sourceTags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sourceTags.HeaderText = "Теги";
            this.sourceTags.Name = "sourceTags";
            this.sourceTags.ReadOnly = true;
            this.sourceTags.Width = 57;
            // 
            // sourceStatistic
            // 
            this.sourceStatistic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sourceStatistic.HeaderText = "Статистика";
            this.sourceStatistic.Name = "sourceStatistic";
            this.sourceStatistic.Width = 101;
            // 
            // tabPageGroup
            // 
            this.tabPageGroup.Controls.Add(this.buttonGroupFilter);
            this.tabPageGroup.Controls.Add(this.checkBoxGroupSource);
            this.tabPageGroup.Controls.Add(this.checkBoxGroupRuntime);
            this.tabPageGroup.Controls.Add(this.comboBoxGroupFilterSource);
            this.tabPageGroup.Controls.Add(this.checkBoxGroupStatistic);
            this.tabPageGroup.Controls.Add(this.checkBoxGroupDesc);
            this.tabPageGroup.Controls.Add(this.checkBoxGroupEditor);
            this.tabPageGroup.Controls.Add(this.textBoxGroupFilter);
            this.tabPageGroup.Controls.Add(this.buttonGroupView);
            this.tabPageGroup.Controls.Add(this.buttonGroupDel);
            this.tabPageGroup.Controls.Add(this.buttonGroupCopy);
            this.tabPageGroup.Controls.Add(this.dataGridViewGroup);
            this.tabPageGroup.Location = new System.Drawing.Point(4, 22);
            this.tabPageGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageGroup.Name = "tabPageGroup";
            this.tabPageGroup.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageGroup.Size = new System.Drawing.Size(1177, 474);
            this.tabPageGroup.TabIndex = 1;
            this.tabPageGroup.Text = "Группы опроса";
            this.tabPageGroup.UseVisualStyleBackColor = true;
            // 
            // buttonGroupFilter
            // 
            this.buttonGroupFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGroupFilter.Location = new System.Drawing.Point(1099, 38);
            this.buttonGroupFilter.Name = "buttonGroupFilter";
            this.buttonGroupFilter.Size = new System.Drawing.Size(70, 24);
            this.buttonGroupFilter.TabIndex = 20;
            this.buttonGroupFilter.Text = "Фильтр";
            this.buttonGroupFilter.UseVisualStyleBackColor = true;
            this.buttonGroupFilter.Click += new System.EventHandler(this.buttonGroupFilter_Click);
            // 
            // checkBoxGroupSource
            // 
            this.checkBoxGroupSource.AutoSize = true;
            this.checkBoxGroupSource.Checked = true;
            this.checkBoxGroupSource.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGroupSource.Location = new System.Drawing.Point(663, 10);
            this.checkBoxGroupSource.Name = "checkBoxGroupSource";
            this.checkBoxGroupSource.Size = new System.Drawing.Size(83, 21);
            this.checkBoxGroupSource.TabIndex = 19;
            this.checkBoxGroupSource.Text = "Источник";
            this.checkBoxGroupSource.UseVisualStyleBackColor = true;
            this.checkBoxGroupSource.CheckedChanged += new System.EventHandler(this.checkBoxGroupSource_CheckedChanged);
            // 
            // checkBoxGroupRuntime
            // 
            this.checkBoxGroupRuntime.AutoSize = true;
            this.checkBoxGroupRuntime.Location = new System.Drawing.Point(364, 10);
            this.checkBoxGroupRuntime.Name = "checkBoxGroupRuntime";
            this.checkBoxGroupRuntime.Size = new System.Drawing.Size(75, 21);
            this.checkBoxGroupRuntime.TabIndex = 18;
            this.checkBoxGroupRuntime.Text = "Runtime";
            this.checkBoxGroupRuntime.UseVisualStyleBackColor = true;
            this.checkBoxGroupRuntime.CheckedChanged += new System.EventHandler(this.checkBoxGroupRuntime_CheckedChanged);
            // 
            // comboBoxGroupFilterSource
            // 
            this.comboBoxGroupFilterSource.FormattingEnabled = true;
            this.comboBoxGroupFilterSource.Location = new System.Drawing.Point(7, 37);
            this.comboBoxGroupFilterSource.Name = "comboBoxGroupFilterSource";
            this.comboBoxGroupFilterSource.Size = new System.Drawing.Size(199, 25);
            this.comboBoxGroupFilterSource.Sorted = true;
            this.comboBoxGroupFilterSource.TabIndex = 17;
            this.comboBoxGroupFilterSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxGroupFilterSource_SelectedIndexChanged);
            this.comboBoxGroupFilterSource.TextChanged += new System.EventHandler(this.comboBoxGroupFilterSource_TextChanged);
            // 
            // checkBoxGroupStatistic
            // 
            this.checkBoxGroupStatistic.AutoSize = true;
            this.checkBoxGroupStatistic.Location = new System.Drawing.Point(562, 10);
            this.checkBoxGroupStatistic.Name = "checkBoxGroupStatistic";
            this.checkBoxGroupStatistic.Size = new System.Drawing.Size(95, 21);
            this.checkBoxGroupStatistic.TabIndex = 16;
            this.checkBoxGroupStatistic.Text = "Статистика";
            this.checkBoxGroupStatistic.UseVisualStyleBackColor = true;
            this.checkBoxGroupStatistic.CheckedChanged += new System.EventHandler(this.checkBoxGroupStatistic_CheckedChanged);
            // 
            // checkBoxGroupDesc
            // 
            this.checkBoxGroupDesc.AutoSize = true;
            this.checkBoxGroupDesc.Checked = true;
            this.checkBoxGroupDesc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGroupDesc.Location = new System.Drawing.Point(453, 10);
            this.checkBoxGroupDesc.Name = "checkBoxGroupDesc";
            this.checkBoxGroupDesc.Size = new System.Drawing.Size(89, 21);
            this.checkBoxGroupDesc.TabIndex = 15;
            this.checkBoxGroupDesc.Text = "Описание";
            this.checkBoxGroupDesc.UseVisualStyleBackColor = true;
            this.checkBoxGroupDesc.CheckedChanged += new System.EventHandler(this.checkBoxGroupDesc_CheckedChanged);
            // 
            // checkBoxGroupEditor
            // 
            this.checkBoxGroupEditor.AutoSize = true;
            this.checkBoxGroupEditor.Checked = true;
            this.checkBoxGroupEditor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGroupEditor.Location = new System.Drawing.Point(268, 10);
            this.checkBoxGroupEditor.Name = "checkBoxGroupEditor";
            this.checkBoxGroupEditor.Size = new System.Drawing.Size(84, 21);
            this.checkBoxGroupEditor.TabIndex = 14;
            this.checkBoxGroupEditor.Text = "Редактор";
            this.checkBoxGroupEditor.UseVisualStyleBackColor = true;
            this.checkBoxGroupEditor.CheckedChanged += new System.EventHandler(this.checkBoxGroupEditor_CheckedChanged);
            // 
            // textBoxGroupFilter
            // 
            this.textBoxGroupFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGroupFilter.Location = new System.Drawing.Point(212, 39);
            this.textBoxGroupFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxGroupFilter.Name = "textBoxGroupFilter";
            this.textBoxGroupFilter.Size = new System.Drawing.Size(881, 22);
            this.textBoxGroupFilter.TabIndex = 13;
            this.textBoxGroupFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxGroupFilter.TextChanged += new System.EventHandler(this.textBoxGroupFilter_TextChanged);
            // 
            // buttonGroupView
            // 
            this.buttonGroupView.Location = new System.Drawing.Point(179, 7);
            this.buttonGroupView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonGroupView.Name = "buttonGroupView";
            this.buttonGroupView.Size = new System.Drawing.Size(80, 24);
            this.buttonGroupView.TabIndex = 12;
            this.buttonGroupView.Text = "Детали";
            this.buttonGroupView.UseVisualStyleBackColor = true;
            // 
            // buttonGroupDel
            // 
            this.buttonGroupDel.Location = new System.Drawing.Point(93, 7);
            this.buttonGroupDel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonGroupDel.Name = "buttonGroupDel";
            this.buttonGroupDel.Size = new System.Drawing.Size(80, 24);
            this.buttonGroupDel.TabIndex = 11;
            this.buttonGroupDel.Text = "Удалить";
            this.buttonGroupDel.UseVisualStyleBackColor = true;
            // 
            // buttonGroupCopy
            // 
            this.buttonGroupCopy.Location = new System.Drawing.Point(7, 7);
            this.buttonGroupCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonGroupCopy.Name = "buttonGroupCopy";
            this.buttonGroupCopy.Size = new System.Drawing.Size(80, 24);
            this.buttonGroupCopy.TabIndex = 10;
            this.buttonGroupCopy.Text = "Копия";
            this.buttonGroupCopy.UseVisualStyleBackColor = true;
            // 
            // dataGridViewGroup
            // 
            this.dataGridViewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupID,
            this.groupCalc,
            this.groupTitle,
            this.groupOn,
            this.groupSource,
            this.groupPeriod,
            this.groupDesc,
            this.groupStatus,
            this.groupTags,
            this.groupStatistic});
            this.dataGridViewGroup.Location = new System.Drawing.Point(7, 67);
            this.dataGridViewGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewGroup.MultiSelect = false;
            this.dataGridViewGroup.Name = "dataGridViewGroup";
            this.dataGridViewGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGroup.Size = new System.Drawing.Size(1162, 391);
            this.dataGridViewGroup.TabIndex = 9;
            this.dataGridViewGroup.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewGroup_CellBeginEdit);
            this.dataGridViewGroup.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGroup_CellEndEdit);
            this.dataGridViewGroup.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGroup_CellValueChanged);
            this.dataGridViewGroup.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridViewGroup_RowStateChanged);
            this.dataGridViewGroup.SelectionChanged += new System.EventHandler(this.dataGridViewGroup_SelectionChanged);
            this.dataGridViewGroup.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewGroup_UserAddedRow);
            // 
            // groupID
            // 
            this.groupID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle23.Format = "N2";
            dataGridViewCellStyle23.NullValue = null;
            this.groupID.DefaultCellStyle = dataGridViewCellStyle23;
            this.groupID.HeaderText = "ID";
            this.groupID.Name = "groupID";
            this.groupID.ReadOnly = true;
            this.groupID.Width = 45;
            // 
            // groupCalc
            // 
            this.groupCalc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.groupCalc.HeaderText = "Р";
            this.groupCalc.Name = "groupCalc";
            this.groupCalc.ReadOnly = true;
            this.groupCalc.Width = 21;
            // 
            // groupTitle
            // 
            this.groupTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupTitle.HeaderText = "Название";
            this.groupTitle.Name = "groupTitle";
            // 
            // groupOn
            // 
            this.groupOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.groupOn.HeaderText = "ВКЛ";
            this.groupOn.Name = "groupOn";
            this.groupOn.Width = 37;
            // 
            // groupSource
            // 
            this.groupSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupSource.HeaderText = "Источник";
            this.groupSource.Name = "groupSource";
            // 
            // groupPeriod
            // 
            this.groupPeriod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupPeriod.HeaderText = "Опрос (мсек)";
            this.groupPeriod.Name = "groupPeriod";
            // 
            // groupDesc
            // 
            this.groupDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupDesc.HeaderText = "Описание";
            this.groupDesc.Name = "groupDesc";
            // 
            // groupStatus
            // 
            this.groupStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupStatus.HeaderText = "Статус";
            this.groupStatus.Name = "groupStatus";
            this.groupStatus.ReadOnly = true;
            // 
            // groupTags
            // 
            this.groupTags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.groupTags.HeaderText = "Теги";
            this.groupTags.Name = "groupTags";
            this.groupTags.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.groupTags.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.groupTags.Width = 38;
            // 
            // groupStatistic
            // 
            this.groupStatistic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.groupStatistic.HeaderText = "Статистика";
            this.groupStatistic.Name = "groupStatistic";
            this.groupStatistic.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.groupStatistic.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.groupStatistic.Width = 82;
            // 
            // tabPageTag
            // 
            this.tabPageTag.Controls.Add(this.buttonTagFilter);
            this.tabPageTag.Controls.Add(this.checkBoxTagSG);
            this.tabPageTag.Controls.Add(this.checkBoxTagBP);
            this.tabPageTag.Controls.Add(this.comboBoxTagFilterPage);
            this.tabPageTag.Controls.Add(this.comboBoxTagFilterBlock);
            this.tabPageTag.Controls.Add(this.comboBoxTagFilterGroup);
            this.tabPageTag.Controls.Add(this.comboBoxTagFilterSource);
            this.tabPageTag.Controls.Add(this.buttonTagHelp);
            this.tabPageTag.Controls.Add(this.checkBoxTagStatistic);
            this.tabPageTag.Controls.Add(this.checkBoxTagDesc);
            this.tabPageTag.Controls.Add(this.checkBoxTagRuntime);
            this.tabPageTag.Controls.Add(this.checkBoxTagEditor);
            this.tabPageTag.Controls.Add(this.textBoxTagFilter);
            this.tabPageTag.Controls.Add(this.buttonTagView);
            this.tabPageTag.Controls.Add(this.buttonTagDel);
            this.tabPageTag.Controls.Add(this.buttonTagCopy);
            this.tabPageTag.Controls.Add(this.dataGridViewTag);
            this.tabPageTag.Location = new System.Drawing.Point(4, 22);
            this.tabPageTag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageTag.Name = "tabPageTag";
            this.tabPageTag.Size = new System.Drawing.Size(1177, 474);
            this.tabPageTag.TabIndex = 2;
            this.tabPageTag.Text = "Теги";
            this.tabPageTag.UseVisualStyleBackColor = true;
            // 
            // buttonTagFilter
            // 
            this.buttonTagFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTagFilter.Location = new System.Drawing.Point(1099, 38);
            this.buttonTagFilter.Name = "buttonTagFilter";
            this.buttonTagFilter.Size = new System.Drawing.Size(70, 24);
            this.buttonTagFilter.TabIndex = 25;
            this.buttonTagFilter.Text = "Фильтр";
            this.buttonTagFilter.UseVisualStyleBackColor = true;
            this.buttonTagFilter.Click += new System.EventHandler(this.buttonTagFilter_Click);
            // 
            // checkBoxTagSG
            // 
            this.checkBoxTagSG.AutoSize = true;
            this.checkBoxTagSG.Checked = true;
            this.checkBoxTagSG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTagSG.Location = new System.Drawing.Point(841, 10);
            this.checkBoxTagSG.Name = "checkBoxTagSG";
            this.checkBoxTagSG.Size = new System.Drawing.Size(130, 21);
            this.checkBoxTagSG.TabIndex = 24;
            this.checkBoxTagSG.Text = "Источник/Группа";
            this.checkBoxTagSG.UseVisualStyleBackColor = true;
            this.checkBoxTagSG.CheckedChanged += new System.EventHandler(this.checkBoxTagSG_CheckedChanged);
            // 
            // checkBoxTagBP
            // 
            this.checkBoxTagBP.AutoSize = true;
            this.checkBoxTagBP.Location = new System.Drawing.Point(616, 10);
            this.checkBoxTagBP.Name = "checkBoxTagBP";
            this.checkBoxTagBP.Size = new System.Drawing.Size(119, 21);
            this.checkBoxTagBP.TabIndex = 23;
            this.checkBoxTagBP.Text = "Блок/Страница";
            this.checkBoxTagBP.UseVisualStyleBackColor = true;
            this.checkBoxTagBP.CheckedChanged += new System.EventHandler(this.checkBoxTagBP_CheckedChanged);
            // 
            // comboBoxTagFilterPage
            // 
            this.comboBoxTagFilterPage.FormattingEnabled = true;
            this.comboBoxTagFilterPage.Location = new System.Drawing.Point(460, 37);
            this.comboBoxTagFilterPage.Name = "comboBoxTagFilterPage";
            this.comboBoxTagFilterPage.Size = new System.Drawing.Size(145, 25);
            this.comboBoxTagFilterPage.Sorted = true;
            this.comboBoxTagFilterPage.TabIndex = 22;
            this.comboBoxTagFilterPage.SelectedIndexChanged += new System.EventHandler(this.comboBoxTagFilterPage_SelectedIndexChanged);
            this.comboBoxTagFilterPage.TextChanged += new System.EventHandler(this.comboBoxTagFilterPage_TextChanged);
            // 
            // comboBoxTagFilterBlock
            // 
            this.comboBoxTagFilterBlock.FormattingEnabled = true;
            this.comboBoxTagFilterBlock.Location = new System.Drawing.Point(309, 37);
            this.comboBoxTagFilterBlock.Name = "comboBoxTagFilterBlock";
            this.comboBoxTagFilterBlock.Size = new System.Drawing.Size(145, 25);
            this.comboBoxTagFilterBlock.Sorted = true;
            this.comboBoxTagFilterBlock.TabIndex = 21;
            this.comboBoxTagFilterBlock.SelectedIndexChanged += new System.EventHandler(this.comboBoxTagFilterBlock_SelectedIndexChanged);
            this.comboBoxTagFilterBlock.TextChanged += new System.EventHandler(this.comboBoxTagFilterBlock_TextChanged);
            // 
            // comboBoxTagFilterGroup
            // 
            this.comboBoxTagFilterGroup.FormattingEnabled = true;
            this.comboBoxTagFilterGroup.Location = new System.Drawing.Point(158, 37);
            this.comboBoxTagFilterGroup.Name = "comboBoxTagFilterGroup";
            this.comboBoxTagFilterGroup.Size = new System.Drawing.Size(145, 25);
            this.comboBoxTagFilterGroup.Sorted = true;
            this.comboBoxTagFilterGroup.TabIndex = 20;
            this.comboBoxTagFilterGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxTagFilterGroup_SelectedIndexChanged);
            this.comboBoxTagFilterGroup.TextChanged += new System.EventHandler(this.comboBoxTagFilterGroup_TextChanged);
            // 
            // comboBoxTagFilterSource
            // 
            this.comboBoxTagFilterSource.FormattingEnabled = true;
            this.comboBoxTagFilterSource.Location = new System.Drawing.Point(7, 37);
            this.comboBoxTagFilterSource.Name = "comboBoxTagFilterSource";
            this.comboBoxTagFilterSource.Size = new System.Drawing.Size(145, 25);
            this.comboBoxTagFilterSource.Sorted = true;
            this.comboBoxTagFilterSource.TabIndex = 19;
            this.comboBoxTagFilterSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxTagFilterSource_SelectedIndexChanged);
            this.comboBoxTagFilterSource.TextChanged += new System.EventHandler(this.comboBoxTagFilterSource_TextChanged);
            // 
            // buttonTagHelp
            // 
            this.buttonTagHelp.Location = new System.Drawing.Point(265, 7);
            this.buttonTagHelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTagHelp.Name = "buttonTagHelp";
            this.buttonTagHelp.Size = new System.Drawing.Size(80, 24);
            this.buttonTagHelp.TabIndex = 18;
            this.buttonTagHelp.Text = "Справка";
            this.buttonTagHelp.UseVisualStyleBackColor = true;
            // 
            // checkBoxTagStatistic
            // 
            this.checkBoxTagStatistic.AutoSize = true;
            this.checkBoxTagStatistic.Location = new System.Drawing.Point(740, 10);
            this.checkBoxTagStatistic.Name = "checkBoxTagStatistic";
            this.checkBoxTagStatistic.Size = new System.Drawing.Size(95, 21);
            this.checkBoxTagStatistic.TabIndex = 17;
            this.checkBoxTagStatistic.Text = "Статистика";
            this.checkBoxTagStatistic.UseVisualStyleBackColor = true;
            this.checkBoxTagStatistic.CheckedChanged += new System.EventHandler(this.checkBoxTagStatistic_CheckedChanged);
            // 
            // checkBoxTagDesc
            // 
            this.checkBoxTagDesc.AutoSize = true;
            this.checkBoxTagDesc.Checked = true;
            this.checkBoxTagDesc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTagDesc.Location = new System.Drawing.Point(524, 10);
            this.checkBoxTagDesc.Name = "checkBoxTagDesc";
            this.checkBoxTagDesc.Size = new System.Drawing.Size(89, 21);
            this.checkBoxTagDesc.TabIndex = 16;
            this.checkBoxTagDesc.Text = "Описание";
            this.checkBoxTagDesc.UseVisualStyleBackColor = true;
            this.checkBoxTagDesc.CheckedChanged += new System.EventHandler(this.checkBoxTagDesc_CheckedChanged);
            // 
            // checkBoxTagRuntime
            // 
            this.checkBoxTagRuntime.AutoSize = true;
            this.checkBoxTagRuntime.Location = new System.Drawing.Point(442, 10);
            this.checkBoxTagRuntime.Name = "checkBoxTagRuntime";
            this.checkBoxTagRuntime.Size = new System.Drawing.Size(75, 21);
            this.checkBoxTagRuntime.TabIndex = 15;
            this.checkBoxTagRuntime.Text = "Runtime";
            this.checkBoxTagRuntime.UseVisualStyleBackColor = true;
            this.checkBoxTagRuntime.CheckedChanged += new System.EventHandler(this.checkBoxTagRuntime_CheckedChanged);
            // 
            // checkBoxTagEditor
            // 
            this.checkBoxTagEditor.AutoSize = true;
            this.checkBoxTagEditor.Checked = true;
            this.checkBoxTagEditor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTagEditor.Location = new System.Drawing.Point(354, 10);
            this.checkBoxTagEditor.Name = "checkBoxTagEditor";
            this.checkBoxTagEditor.Size = new System.Drawing.Size(84, 21);
            this.checkBoxTagEditor.TabIndex = 14;
            this.checkBoxTagEditor.Text = "Редактор";
            this.checkBoxTagEditor.UseVisualStyleBackColor = true;
            this.checkBoxTagEditor.CheckedChanged += new System.EventHandler(this.checkBoxTagEditor_CheckedChanged);
            // 
            // textBoxTagFilter
            // 
            this.textBoxTagFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTagFilter.Location = new System.Drawing.Point(611, 39);
            this.textBoxTagFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxTagFilter.Name = "textBoxTagFilter";
            this.textBoxTagFilter.Size = new System.Drawing.Size(482, 22);
            this.textBoxTagFilter.TabIndex = 13;
            this.textBoxTagFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxTagFilter.TextChanged += new System.EventHandler(this.textBoxTagFilter_TextChanged);
            // 
            // buttonTagView
            // 
            this.buttonTagView.Location = new System.Drawing.Point(179, 7);
            this.buttonTagView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTagView.Name = "buttonTagView";
            this.buttonTagView.Size = new System.Drawing.Size(80, 24);
            this.buttonTagView.TabIndex = 12;
            this.buttonTagView.Text = "Детали";
            this.buttonTagView.UseVisualStyleBackColor = true;
            // 
            // buttonTagDel
            // 
            this.buttonTagDel.Location = new System.Drawing.Point(93, 7);
            this.buttonTagDel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTagDel.Name = "buttonTagDel";
            this.buttonTagDel.Size = new System.Drawing.Size(80, 24);
            this.buttonTagDel.TabIndex = 11;
            this.buttonTagDel.Text = "Удалить";
            this.buttonTagDel.UseVisualStyleBackColor = true;
            // 
            // buttonTagCopy
            // 
            this.buttonTagCopy.Location = new System.Drawing.Point(7, 7);
            this.buttonTagCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonTagCopy.Name = "buttonTagCopy";
            this.buttonTagCopy.Size = new System.Drawing.Size(80, 24);
            this.buttonTagCopy.TabIndex = 10;
            this.buttonTagCopy.Text = "Копия";
            this.buttonTagCopy.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTag
            // 
            this.dataGridViewTag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTag.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tagID,
            this.tagCalc,
            this.tagTitle,
            this.tagON,
            this.tagValue,
            this.tagSource,
            this.tagGroup,
            this.tagDataType,
            this.tagAddress,
            this.tagCommand,
            this.tagWriteValue,
            this.tagWriteTag,
            this.tagDesc,
            this.tagBlock,
            this.tagPage,
            this.tagStatus,
            this.tagMessage,
            this.tagStatistic});
            this.dataGridViewTag.Location = new System.Drawing.Point(7, 67);
            this.dataGridViewTag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewTag.MultiSelect = false;
            this.dataGridViewTag.Name = "dataGridViewTag";
            this.dataGridViewTag.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTag.Size = new System.Drawing.Size(1162, 391);
            this.dataGridViewTag.TabIndex = 9;
            this.dataGridViewTag.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewTag_CellBeginEdit);
            this.dataGridViewTag.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTag_CellEndEdit);
            this.dataGridViewTag.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTag_CellValueChanged);
            this.dataGridViewTag.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridViewTag_RowStateChanged);
            this.dataGridViewTag.SelectionChanged += new System.EventHandler(this.dataGridViewTag_SelectionChanged);
            this.dataGridViewTag.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewTag_UserAddedRow);
            // 
            // tagID
            // 
            this.tagID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle24.Format = "N2";
            dataGridViewCellStyle24.NullValue = null;
            this.tagID.DefaultCellStyle = dataGridViewCellStyle24;
            this.tagID.HeaderText = "ID";
            this.tagID.Name = "tagID";
            this.tagID.ReadOnly = true;
            this.tagID.Width = 45;
            // 
            // tagCalc
            // 
            this.tagCalc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tagCalc.HeaderText = "Р";
            this.tagCalc.Name = "tagCalc";
            this.tagCalc.ReadOnly = true;
            this.tagCalc.Width = 21;
            // 
            // tagTitle
            // 
            this.tagTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagTitle.HeaderText = "Название";
            this.tagTitle.Name = "tagTitle";
            // 
            // tagON
            // 
            this.tagON.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tagON.HeaderText = "ВКЛ";
            this.tagON.Name = "tagON";
            this.tagON.Width = 37;
            // 
            // tagValue
            // 
            this.tagValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagValue.HeaderText = "Значение";
            this.tagValue.Name = "tagValue";
            this.tagValue.ReadOnly = true;
            // 
            // tagSource
            // 
            this.tagSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagSource.HeaderText = "Источник";
            this.tagSource.Name = "tagSource";
            this.tagSource.ReadOnly = true;
            // 
            // tagGroup
            // 
            this.tagGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagGroup.HeaderText = "Группа";
            this.tagGroup.Name = "tagGroup";
            this.tagGroup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tagGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tagDataType
            // 
            this.tagDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagDataType.HeaderText = "Тип данных";
            this.tagDataType.Name = "tagDataType";
            // 
            // tagAddress
            // 
            this.tagAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagAddress.HeaderText = "Адрес";
            this.tagAddress.Name = "tagAddress";
            this.tagAddress.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tagAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tagCommand
            // 
            this.tagCommand.HeaderText = "Команда";
            this.tagCommand.Name = "tagCommand";
            // 
            // tagWriteValue
            // 
            this.tagWriteValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagWriteValue.HeaderText = "Значение записи";
            this.tagWriteValue.Name = "tagWriteValue";
            this.tagWriteValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tagWriteValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tagWriteTag
            // 
            this.tagWriteTag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagWriteTag.HeaderText = "Тег записи";
            this.tagWriteTag.Name = "tagWriteTag";
            this.tagWriteTag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tagWriteTag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tagDesc
            // 
            this.tagDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagDesc.HeaderText = "Описание";
            this.tagDesc.Name = "tagDesc";
            // 
            // tagBlock
            // 
            this.tagBlock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tagBlock.HeaderText = "Блок";
            this.tagBlock.Name = "tagBlock";
            this.tagBlock.Width = 60;
            // 
            // tagPage
            // 
            this.tagPage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tagPage.HeaderText = "Страница";
            this.tagPage.Name = "tagPage";
            this.tagPage.Width = 93;
            // 
            // tagStatus
            // 
            this.tagStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tagStatus.HeaderText = "Статус";
            this.tagStatus.Name = "tagStatus";
            this.tagStatus.ReadOnly = true;
            this.tagStatus.Width = 74;
            // 
            // tagMessage
            // 
            this.tagMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagMessage.HeaderText = "Сообщение";
            this.tagMessage.Name = "tagMessage";
            this.tagMessage.ReadOnly = true;
            // 
            // tagStatistic
            // 
            this.tagStatistic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tagStatistic.HeaderText = "Статистика";
            this.tagStatistic.Name = "tagStatistic";
            this.tagStatistic.ReadOnly = true;
            this.tagStatistic.Width = 101;
            // 
            // tabPageStructures
            // 
            this.tabPageStructures.Controls.Add(this.splitContainerStructure);
            this.tabPageStructures.Location = new System.Drawing.Point(4, 26);
            this.tabPageStructures.Name = "tabPageStructures";
            this.tabPageStructures.Size = new System.Drawing.Size(1177, 470);
            this.tabPageStructures.TabIndex = 5;
            this.tabPageStructures.Text = "Структуры";
            this.tabPageStructures.UseVisualStyleBackColor = true;
            // 
            // splitContainerStructure
            // 
            this.splitContainerStructure.Location = new System.Drawing.Point(3, 43);
            this.splitContainerStructure.Name = "splitContainerStructure";
            this.splitContainerStructure.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerStructure.Panel1
            // 
            this.splitContainerStructure.Panel1.Controls.Add(this.buttonStructureLeft);
            this.splitContainerStructure.Panel1.Controls.Add(this.buttonStructureFilter);
            this.splitContainerStructure.Panel1.Controls.Add(this.textBoxStructureFilter);
            this.splitContainerStructure.Panel1.Controls.Add(this.dataGridViewStructure);
            // 
            // splitContainerStructure.Panel2
            // 
            this.splitContainerStructure.Panel2.Controls.Add(this.buttonStructureRight);
            this.splitContainerStructure.Panel2.Controls.Add(this.comboBoxTargetFilterSource);
            this.splitContainerStructure.Panel2.Controls.Add(this.buttonTargetFilter);
            this.splitContainerStructure.Panel2.Controls.Add(this.dataGridViewTarget);
            this.splitContainerStructure.Panel2.Controls.Add(this.textBoxTargetFilter);
            this.splitContainerStructure.Size = new System.Drawing.Size(1171, 424);
            this.splitContainerStructure.SplitterDistance = 194;
            this.splitContainerStructure.TabIndex = 0;
            // 
            // buttonStructureFilter
            // 
            this.buttonStructureFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStructureFilter.Location = new System.Drawing.Point(486, 6);
            this.buttonStructureFilter.Name = "buttonStructureFilter";
            this.buttonStructureFilter.Size = new System.Drawing.Size(70, 24);
            this.buttonStructureFilter.TabIndex = 19;
            this.buttonStructureFilter.Text = "Фильтр";
            this.buttonStructureFilter.UseVisualStyleBackColor = true;
            this.buttonStructureFilter.Click += new System.EventHandler(this.buttonStructureFilter_Click);
            // 
            // textBoxStructureFilter
            // 
            this.textBoxStructureFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStructureFilter.Location = new System.Drawing.Point(3, 7);
            this.textBoxStructureFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxStructureFilter.Name = "textBoxStructureFilter";
            this.textBoxStructureFilter.Size = new System.Drawing.Size(477, 22);
            this.textBoxStructureFilter.TabIndex = 18;
            this.textBoxStructureFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxStructureFilter.TextChanged += new System.EventHandler(this.textBoxStructureFilter_TextChanged);
            // 
            // dataGridViewStructure
            // 
            this.dataGridViewStructure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStructure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStructure.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.structureID,
            this.structureTitle,
            this.structureOn,
            this.structureConnector,
            this.structureDataType,
            this.structureTagSource,
            this.structureTemplate,
            this.structureGroup});
            this.dataGridViewStructure.Location = new System.Drawing.Point(3, 34);
            this.dataGridViewStructure.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewStructure.MultiSelect = false;
            this.dataGridViewStructure.Name = "dataGridViewStructure";
            this.dataGridViewStructure.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStructure.Size = new System.Drawing.Size(1163, 156);
            this.dataGridViewStructure.TabIndex = 10;
            this.dataGridViewStructure.SelectionChanged += new System.EventHandler(this.dataGridViewStructure_SelectionChanged);
            // 
            // structureID
            // 
            this.structureID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle25.Format = "N2";
            dataGridViewCellStyle25.NullValue = null;
            this.structureID.DefaultCellStyle = dataGridViewCellStyle25;
            this.structureID.HeaderText = "ID";
            this.structureID.Name = "structureID";
            this.structureID.ReadOnly = true;
            this.structureID.Width = 45;
            // 
            // structureTitle
            // 
            this.structureTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.structureTitle.HeaderText = "Название";
            this.structureTitle.Name = "structureTitle";
            // 
            // structureOn
            // 
            this.structureOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.structureOn.HeaderText = "ВКЛ";
            this.structureOn.Name = "structureOn";
            this.structureOn.Width = 37;
            // 
            // structureConnector
            // 
            this.structureConnector.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.structureConnector.HeaderText = "Соединитель";
            this.structureConnector.Name = "structureConnector";
            // 
            // structureDataType
            // 
            this.structureDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.structureDataType.HeaderText = "Тип данных";
            this.structureDataType.Name = "structureDataType";
            this.structureDataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.structureDataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // structureTagSource
            // 
            this.structureTagSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.structureTagSource.HeaderText = "Тег-источник";
            this.structureTagSource.Name = "structureTagSource";
            // 
            // structureTemplate
            // 
            this.structureTemplate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.structureTemplate.HeaderText = "Шаблон адреса";
            this.structureTemplate.Name = "structureTemplate";
            // 
            // structureGroup
            // 
            this.structureGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.structureGroup.HeaderText = "Группа";
            this.structureGroup.Name = "structureGroup";
            // 
            // comboBoxTargetFilterSource
            // 
            this.comboBoxTargetFilterSource.FormattingEnabled = true;
            this.comboBoxTargetFilterSource.Location = new System.Drawing.Point(3, 4);
            this.comboBoxTargetFilterSource.Name = "comboBoxTargetFilterSource";
            this.comboBoxTargetFilterSource.Size = new System.Drawing.Size(199, 25);
            this.comboBoxTargetFilterSource.Sorted = true;
            this.comboBoxTargetFilterSource.TabIndex = 19;
            this.comboBoxTargetFilterSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetFilterSource_SelectedIndexChanged);
            // 
            // buttonTargetFilter
            // 
            this.buttonTargetFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTargetFilter.Location = new System.Drawing.Point(486, 4);
            this.buttonTargetFilter.Name = "buttonTargetFilter";
            this.buttonTargetFilter.Size = new System.Drawing.Size(70, 24);
            this.buttonTargetFilter.TabIndex = 17;
            this.buttonTargetFilter.Text = "Фильтр";
            this.buttonTargetFilter.UseVisualStyleBackColor = true;
            this.buttonTargetFilter.Click += new System.EventHandler(this.buttonTargetFilter_Click);
            // 
            // dataGridViewTarget
            // 
            this.dataGridViewTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTarget.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.targetID,
            this.targetStructure,
            this.targetOn,
            this.targetAddress,
            this.targetTitle,
            this.targetDesc});
            this.dataGridViewTarget.Location = new System.Drawing.Point(3, 32);
            this.dataGridViewTarget.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewTarget.MultiSelect = false;
            this.dataGridViewTarget.Name = "dataGridViewTarget";
            this.dataGridViewTarget.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTarget.Size = new System.Drawing.Size(1163, 190);
            this.dataGridViewTarget.TabIndex = 11;
            this.dataGridViewTarget.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewTarget_UserAddedRow);
            // 
            // targetID
            // 
            this.targetID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle26.Format = "N2";
            dataGridViewCellStyle26.NullValue = null;
            this.targetID.DefaultCellStyle = dataGridViewCellStyle26;
            this.targetID.HeaderText = "ID";
            this.targetID.Name = "targetID";
            this.targetID.ReadOnly = true;
            this.targetID.Width = 45;
            // 
            // targetStructure
            // 
            this.targetStructure.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetStructure.HeaderText = "Структура";
            this.targetStructure.Name = "targetStructure";
            // 
            // targetOn
            // 
            this.targetOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.targetOn.HeaderText = "ВКЛ";
            this.targetOn.Name = "targetOn";
            this.targetOn.Width = 37;
            // 
            // targetAddress
            // 
            this.targetAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetAddress.HeaderText = "Адрес";
            this.targetAddress.Name = "targetAddress";
            // 
            // targetTitle
            // 
            this.targetTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetTitle.HeaderText = "Название тега";
            this.targetTitle.Name = "targetTitle";
            // 
            // targetDesc
            // 
            this.targetDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetDesc.HeaderText = "Описание тега";
            this.targetDesc.Name = "targetDesc";
            // 
            // textBoxTargetFilter
            // 
            this.textBoxTargetFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTargetFilter.Location = new System.Drawing.Point(208, 5);
            this.textBoxTargetFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxTargetFilter.Name = "textBoxTargetFilter";
            this.textBoxTargetFilter.Size = new System.Drawing.Size(272, 22);
            this.textBoxTargetFilter.TabIndex = 16;
            this.textBoxTargetFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxTargetFilter.TextChanged += new System.EventHandler(this.textBoxTargetFilter_TextChanged);
            // 
            // tabPageIncludes
            // 
            this.tabPageIncludes.Controls.Add(this.splitContainerInclude);
            this.tabPageIncludes.Location = new System.Drawing.Point(4, 26);
            this.tabPageIncludes.Name = "tabPageIncludes";
            this.tabPageIncludes.Size = new System.Drawing.Size(1177, 470);
            this.tabPageIncludes.TabIndex = 4;
            this.tabPageIncludes.Text = "Классы";
            this.tabPageIncludes.UseVisualStyleBackColor = true;
            // 
            // splitContainerInclude
            // 
            this.splitContainerInclude.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerInclude.Location = new System.Drawing.Point(3, 69);
            this.splitContainerInclude.Name = "splitContainerInclude";
            // 
            // splitContainerInclude.Panel1
            // 
            this.splitContainerInclude.Panel1.Controls.Add(this.buttonIncludeFilter);
            this.splitContainerInclude.Panel1.Controls.Add(this.textBoxIncludeFilter);
            this.splitContainerInclude.Panel1.Controls.Add(this.buttonIncludeLeft);
            this.splitContainerInclude.Panel1.Controls.Add(this.dataGridViewInclude);
            // 
            // splitContainerInclude.Panel2
            // 
            this.splitContainerInclude.Panel2.Controls.Add(this.comboBoxChangeFilterInclude);
            this.splitContainerInclude.Panel2.Controls.Add(this.buttonChangeFilter);
            this.splitContainerInclude.Panel2.Controls.Add(this.textBoxChangeFilter);
            this.splitContainerInclude.Panel2.Controls.Add(this.buttonIncludeRight);
            this.splitContainerInclude.Panel2.Controls.Add(this.dataGridViewChange);
            this.splitContainerInclude.Size = new System.Drawing.Size(1171, 394);
            this.splitContainerInclude.SplitterDistance = 580;
            this.splitContainerInclude.SplitterWidth = 10;
            this.splitContainerInclude.TabIndex = 4;
            // 
            // buttonIncludeFilter
            // 
            this.buttonIncludeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonIncludeFilter.Location = new System.Drawing.Point(486, 3);
            this.buttonIncludeFilter.Name = "buttonIncludeFilter";
            this.buttonIncludeFilter.Size = new System.Drawing.Size(70, 24);
            this.buttonIncludeFilter.TabIndex = 15;
            this.buttonIncludeFilter.Text = "Фильтр";
            this.buttonIncludeFilter.UseVisualStyleBackColor = true;
            this.buttonIncludeFilter.Click += new System.EventHandler(this.buttonIncludeFilter_Click);
            // 
            // textBoxIncludeFilter
            // 
            this.textBoxIncludeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIncludeFilter.Location = new System.Drawing.Point(3, 4);
            this.textBoxIncludeFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxIncludeFilter.Name = "textBoxIncludeFilter";
            this.textBoxIncludeFilter.Size = new System.Drawing.Size(477, 22);
            this.textBoxIncludeFilter.TabIndex = 14;
            this.textBoxIncludeFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxIncludeFilter.TextChanged += new System.EventHandler(this.textBoxIncludeFilter_TextChanged);
            // 
            // buttonIncludeLeft
            // 
            this.buttonIncludeLeft.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonIncludeLeft.Location = new System.Drawing.Point(560, 135);
            this.buttonIncludeLeft.Name = "buttonIncludeLeft";
            this.buttonIncludeLeft.Size = new System.Drawing.Size(17, 118);
            this.buttonIncludeLeft.TabIndex = 3;
            this.buttonIncludeLeft.Text = ">";
            this.buttonIncludeLeft.UseVisualStyleBackColor = true;
            this.buttonIncludeLeft.Click += new System.EventHandler(this.buttonIncludeLeft_Click);
            // 
            // dataGridViewInclude
            // 
            this.dataGridViewInclude.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewInclude.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInclude.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.includeID,
            this.includePrefix,
            this.includeFileName,
            this.includeChanges});
            this.dataGridViewInclude.Location = new System.Drawing.Point(3, 32);
            this.dataGridViewInclude.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewInclude.MultiSelect = false;
            this.dataGridViewInclude.Name = "dataGridViewInclude";
            this.dataGridViewInclude.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInclude.Size = new System.Drawing.Size(553, 358);
            this.dataGridViewInclude.TabIndex = 2;
            this.dataGridViewInclude.SelectionChanged += new System.EventHandler(this.dataGridViewInclude_SelectionChanged);
            // 
            // includeID
            // 
            this.includeID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle27.Format = "N2";
            dataGridViewCellStyle27.NullValue = null;
            this.includeID.DefaultCellStyle = dataGridViewCellStyle27;
            this.includeID.HeaderText = "ID";
            this.includeID.Name = "includeID";
            this.includeID.ReadOnly = true;
            this.includeID.Width = 45;
            // 
            // includePrefix
            // 
            this.includePrefix.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.includePrefix.HeaderText = "Префикс";
            this.includePrefix.Name = "includePrefix";
            // 
            // includeFileName
            // 
            this.includeFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.includeFileName.HeaderText = "Имя файла";
            this.includeFileName.Name = "includeFileName";
            // 
            // includeChanges
            // 
            this.includeChanges.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.includeChanges.HeaderText = "Замены";
            this.includeChanges.Name = "includeChanges";
            this.includeChanges.ReadOnly = true;
            this.includeChanges.Width = 82;
            // 
            // comboBoxChangeFilterInclude
            // 
            this.comboBoxChangeFilterInclude.FormattingEnabled = true;
            this.comboBoxChangeFilterInclude.Location = new System.Drawing.Point(26, 3);
            this.comboBoxChangeFilterInclude.Name = "comboBoxChangeFilterInclude";
            this.comboBoxChangeFilterInclude.Size = new System.Drawing.Size(199, 25);
            this.comboBoxChangeFilterInclude.Sorted = true;
            this.comboBoxChangeFilterInclude.TabIndex = 18;
            this.comboBoxChangeFilterInclude.SelectedIndexChanged += new System.EventHandler(this.comboBoxChangeFilterInclude_SelectedIndexChanged);
            // 
            // buttonChangeFilter
            // 
            this.buttonChangeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeFilter.Location = new System.Drawing.Point(500, 3);
            this.buttonChangeFilter.Name = "buttonChangeFilter";
            this.buttonChangeFilter.Size = new System.Drawing.Size(70, 24);
            this.buttonChangeFilter.TabIndex = 16;
            this.buttonChangeFilter.Text = "Фильтр";
            this.buttonChangeFilter.UseVisualStyleBackColor = true;
            this.buttonChangeFilter.Click += new System.EventHandler(this.buttonChangeFilter_Click);
            // 
            // textBoxChangeFilter
            // 
            this.textBoxChangeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChangeFilter.Location = new System.Drawing.Point(231, 4);
            this.textBoxChangeFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxChangeFilter.Name = "textBoxChangeFilter";
            this.textBoxChangeFilter.Size = new System.Drawing.Size(263, 22);
            this.textBoxChangeFilter.TabIndex = 15;
            this.textBoxChangeFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxChangeFilter.TextChanged += new System.EventHandler(this.textBoxChangeFilter_TextChanged);
            // 
            // buttonIncludeRight
            // 
            this.buttonIncludeRight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonIncludeRight.Location = new System.Drawing.Point(4, 135);
            this.buttonIncludeRight.Name = "buttonIncludeRight";
            this.buttonIncludeRight.Size = new System.Drawing.Size(17, 118);
            this.buttonIncludeRight.TabIndex = 4;
            this.buttonIncludeRight.Text = "<";
            this.buttonIncludeRight.UseVisualStyleBackColor = true;
            this.buttonIncludeRight.Click += new System.EventHandler(this.buttonIncludeRight_Click);
            // 
            // dataGridViewChange
            // 
            this.dataGridViewChange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewChange.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewChange.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.changeID,
            this.changePrefix,
            this.changeFrom,
            this.changeTo});
            this.dataGridViewChange.Location = new System.Drawing.Point(26, 32);
            this.dataGridViewChange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewChange.MultiSelect = false;
            this.dataGridViewChange.Name = "dataGridViewChange";
            this.dataGridViewChange.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewChange.Size = new System.Drawing.Size(544, 358);
            this.dataGridViewChange.TabIndex = 3;
            this.dataGridViewChange.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewChange_UserAddedRow);
            // 
            // changeID
            // 
            this.changeID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle28.Format = "N2";
            dataGridViewCellStyle28.NullValue = null;
            this.changeID.DefaultCellStyle = dataGridViewCellStyle28;
            this.changeID.HeaderText = "ID";
            this.changeID.Name = "changeID";
            this.changeID.ReadOnly = true;
            this.changeID.Width = 45;
            // 
            // changePrefix
            // 
            this.changePrefix.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.changePrefix.HeaderText = "Префикс";
            this.changePrefix.Name = "changePrefix";
            // 
            // changeFrom
            // 
            this.changeFrom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.changeFrom.HeaderText = "Что меняем";
            this.changeFrom.Name = "changeFrom";
            // 
            // changeTo
            // 
            this.changeTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.changeTo.HeaderText = "На что меняем";
            this.changeTo.Name = "changeTo";
            // 
            // tabPagePage
            // 
            this.tabPagePage.Location = new System.Drawing.Point(4, 22);
            this.tabPagePage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPagePage.Name = "tabPagePage";
            this.tabPagePage.Size = new System.Drawing.Size(1177, 474);
            this.tabPagePage.TabIndex = 3;
            this.tabPagePage.Text = "Страницы";
            this.tabPagePage.UseVisualStyleBackColor = true;
            // 
            // buttonStructureLeft
            // 
            this.buttonStructureLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStructureLeft.Location = new System.Drawing.Point(990, 4);
            this.buttonStructureLeft.Name = "buttonStructureLeft";
            this.buttonStructureLeft.Size = new System.Drawing.Size(176, 28);
            this.buttonStructureLeft.TabIndex = 20;
            this.buttonStructureLeft.Text = ">";
            this.buttonStructureLeft.UseVisualStyleBackColor = true;
            this.buttonStructureLeft.Click += new System.EventHandler(this.buttonStructureLeft_Click);
            // 
            // buttonStructureRight
            // 
            this.buttonStructureRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStructureRight.Location = new System.Drawing.Point(990, 2);
            this.buttonStructureRight.Name = "buttonStructureRight";
            this.buttonStructureRight.Size = new System.Drawing.Size(176, 28);
            this.buttonStructureRight.TabIndex = 21;
            this.buttonStructureRight.Text = "<";
            this.buttonStructureRight.UseVisualStyleBackColor = true;
            this.buttonStructureRight.Click += new System.EventHandler(this.buttonStructureRight_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1415, 547);
            this.Controls.Add(this.splitContainerForm);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "WinSimpleDriver";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainerForm.Panel1.ResumeLayout(false);
            this.splitContainerForm.Panel1.PerformLayout();
            this.splitContainerForm.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerForm)).EndInit();
            this.splitContainerForm.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSource.ResumeLayout(false);
            this.tabPageSource.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSource)).EndInit();
            this.tabPageGroup.ResumeLayout(false);
            this.tabPageGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroup)).EndInit();
            this.tabPageTag.ResumeLayout(false);
            this.tabPageTag.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTag)).EndInit();
            this.tabPageStructures.ResumeLayout(false);
            this.splitContainerStructure.Panel1.ResumeLayout(false);
            this.splitContainerStructure.Panel1.PerformLayout();
            this.splitContainerStructure.Panel2.ResumeLayout(false);
            this.splitContainerStructure.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStructure)).EndInit();
            this.splitContainerStructure.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStructure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTarget)).EndInit();
            this.tabPageIncludes.ResumeLayout(false);
            this.splitContainerInclude.Panel1.ResumeLayout(false);
            this.splitContainerInclude.Panel1.PerformLayout();
            this.splitContainerInclude.Panel2.ResumeLayout(false);
            this.splitContainerInclude.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInclude)).EndInit();
            this.splitContainerInclude.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInclude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainerForm;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSource;
        private System.Windows.Forms.TabPage tabPageGroup;
        private System.Windows.Forms.TabPage tabPageTag;
        private System.Windows.Forms.TabPage tabPagePage;
        private System.Windows.Forms.ToolStripMenuItem вилToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поискToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пускСтопToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemViewTree;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemNew;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemImport;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExit;
        private System.Windows.Forms.DataGridView dataGridViewSource;
        private System.Windows.Forms.TextBox textBoxSourceFilter;
        private System.Windows.Forms.Button buttonSourceView;
        private System.Windows.Forms.Button buttonSourceDel;
        private System.Windows.Forms.Button buttonSourceCopy;
        private System.Windows.Forms.CheckBox checkBoxSourceDesc;
        private System.Windows.Forms.CheckBox checkBoxSourceRuntime;
        private System.Windows.Forms.CheckBox checkBoxSourceEditor;
        private System.Windows.Forms.CheckBox checkBoxGroupStatistic;
        private System.Windows.Forms.CheckBox checkBoxGroupDesc;
        private System.Windows.Forms.CheckBox checkBoxGroupEditor;
        private System.Windows.Forms.TextBox textBoxGroupFilter;
        private System.Windows.Forms.Button buttonGroupView;
        private System.Windows.Forms.Button buttonGroupDel;
        private System.Windows.Forms.Button buttonGroupCopy;
        private System.Windows.Forms.DataGridView dataGridViewGroup;
        private System.Windows.Forms.CheckBox checkBoxTagDesc;
        private System.Windows.Forms.CheckBox checkBoxTagRuntime;
        private System.Windows.Forms.CheckBox checkBoxTagEditor;
        private System.Windows.Forms.TextBox textBoxTagFilter;
        private System.Windows.Forms.Button buttonTagView;
        private System.Windows.Forms.Button buttonTagDel;
        private System.Windows.Forms.Button buttonTagCopy;
        private System.Windows.Forms.DataGridView dataGridViewTag;
        private System.Windows.Forms.CheckBox checkBoxSourceStatistic;
        private System.Windows.Forms.Button buttonSourceHelp;
        private System.Windows.Forms.ComboBox comboBoxGroupFilterSource;
        private System.Windows.Forms.CheckBox checkBoxGroupRuntime;
        private System.Windows.Forms.CheckBox checkBoxTagStatistic;
        private System.Windows.Forms.Button buttonTagHelp;
        private System.Windows.Forms.ComboBox comboBoxTagFilterSource;
        private System.Windows.Forms.ComboBox comboBoxTagFilterBlock;
        private System.Windows.Forms.ComboBox comboBoxTagFilterGroup;
        private System.Windows.Forms.ComboBox comboBoxTagFilterPage;
        private System.Windows.Forms.CheckBox checkBoxTagBP;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemVer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.CheckBox checkBoxGroupSource;
        private System.Windows.Forms.CheckBox checkBoxTagSG;
        private System.Windows.Forms.Button buttonSourceFilter;
        private System.Windows.Forms.Button buttonGroupFilter;
        private System.Windows.Forms.Button buttonTagFilter;
        private System.Windows.Forms.TabPage tabPageIncludes;
        private System.Windows.Forms.DataGridView dataGridViewChange;
        private System.Windows.Forms.DataGridView dataGridViewInclude;
        private System.Windows.Forms.SplitContainer splitContainerInclude;
        private System.Windows.Forms.Button buttonIncludeLeft;
        private System.Windows.Forms.Button buttonIncludeRight;
        private System.Windows.Forms.TextBox textBoxIncludeFilter;
        private System.Windows.Forms.TextBox textBoxChangeFilter;
        private System.Windows.Forms.Button buttonIncludeFilter;
        private System.Windows.Forms.Button buttonChangeFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn includeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn includePrefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn includeFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn includeChanges;
        private System.Windows.Forms.DataGridViewTextBoxColumn changeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn changePrefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn changeFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn changeTo;
        private System.Windows.Forms.ComboBox comboBoxChangeFilterInclude;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sourceCalc;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sourceON;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sourceAutoRestart;
        private System.Windows.Forms.DataGridViewComboBoxColumn sourceDriver;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceStatistic;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn groupCalc;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn groupOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupStatistic;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tagCalc;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tagON;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagGroup;
        private System.Windows.Forms.DataGridViewComboBoxColumn tagDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagAddress;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tagCommand;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagWriteValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagWriteTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagBlock;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagPage;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn tagStatistic;
        private System.Windows.Forms.TabPage tabPageStructures;
        private System.Windows.Forms.SplitContainer splitContainerStructure;
        private System.Windows.Forms.DataGridView dataGridViewStructure;
        private System.Windows.Forms.DataGridView dataGridViewTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureID;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn structureOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureConnector;
        private System.Windows.Forms.DataGridViewComboBoxColumn structureDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureTagSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureTemplate;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetID;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetStructure;
        private System.Windows.Forms.DataGridViewCheckBoxColumn targetOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetDesc;
        private System.Windows.Forms.Button buttonStructureFilter;
        private System.Windows.Forms.TextBox textBoxStructureFilter;
        private System.Windows.Forms.ComboBox comboBoxTargetFilterSource;
        private System.Windows.Forms.Button buttonTargetFilter;
        private System.Windows.Forms.TextBox textBoxTargetFilter;
        private System.Windows.Forms.Button buttonStructureLeft;
        private System.Windows.Forms.Button buttonStructureRight;
    }
}

