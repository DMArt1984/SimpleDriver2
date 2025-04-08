
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Источники/Группы/Теги");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Блоки");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Структуры");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Классы");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemLastFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemImport = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.вилToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemViewTree = new System.Windows.Forms.ToolStripMenuItem();
            this.Log2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testTagSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemVer = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessage1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelMessage2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMessage3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerTreeMain = new System.Windows.Forms.SplitContainer();
            this.labelProject = new System.Windows.Forms.Label();
            this.treeViewProject = new System.Windows.Forms.TreeView();
            this.contextMenuStripTreeProj = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tabFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlProject = new System.Windows.Forms.TabControl();
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
            this.sourceAutomation = new System.Windows.Forms.DataGridViewCheckBoxColumn();
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
            this.checkBoxTagAddress = new System.Windows.Forms.CheckBox();
            this.checkBoxTagSave = new System.Windows.Forms.CheckBox();
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
            this.tabPageStructure = new System.Windows.Forms.TabPage();
            this.checkBoxStructCol = new System.Windows.Forms.CheckBox();
            this.buttonStructHelp = new System.Windows.Forms.Button();
            this.buttonStructResult = new System.Windows.Forms.Button();
            this.buttonStructDel = new System.Windows.Forms.Button();
            this.buttonStructCopy = new System.Windows.Forms.Button();
            this.splitContainerStructure = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStructureLeft = new System.Windows.Forms.Button();
            this.buttonStructureFilter = new System.Windows.Forms.Button();
            this.textBoxStructureFilter = new System.Windows.Forms.TextBox();
            this.comboBoxStructureTargetFilterParent = new System.Windows.Forms.ComboBox();
            this.dataGridViewStructure = new System.Windows.Forms.DataGridView();
            this.structureID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureOn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.structureConnector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.structureTemplate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structureGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainerStructTagTarget = new System.Windows.Forms.SplitContainer();
            this.dataGridViewStructureTag = new System.Windows.Forms.DataGridView();
            this.targetTagID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetTagStruct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetTagTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewStructureTarget = new System.Windows.Forms.DataGridView();
            this.targetID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetStructure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonStructureRight = new System.Windows.Forms.Button();
            this.tabPageInclude = new System.Windows.Forms.TabPage();
            this.checkBoxIncludePrefix = new System.Windows.Forms.CheckBox();
            this.buttonIncludeHelp = new System.Windows.Forms.Button();
            this.buttonIncludeResult = new System.Windows.Forms.Button();
            this.buttonIncludeDel = new System.Windows.Forms.Button();
            this.buttonIncludeCopy = new System.Windows.Forms.Button();
            this.splitContainerInclude = new System.Windows.Forms.SplitContainer();
            this.buttonIncludeFilter = new System.Windows.Forms.Button();
            this.textBoxIncludeFilter = new System.Windows.Forms.TextBox();
            this.buttonIncludeLeft = new System.Windows.Forms.Button();
            this.dataGridViewInclude = new System.Windows.Forms.DataGridView();
            this.includeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includePrefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.includeFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxIncludeChildFilterParent = new System.Windows.Forms.ComboBox();
            this.buttonIncludeChildFilter = new System.Windows.Forms.Button();
            this.textBoxIncludeChildFilter = new System.Windows.Forms.TextBox();
            this.buttonIncludeRight = new System.Windows.Forms.Button();
            this.dataGridViewIncludeChild = new System.Windows.Forms.DataGridView();
            this.changeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.changePrefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.changeFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.changeTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewLog = new System.Windows.Forms.DataGridView();
            this.logDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainerLogMain = new System.Windows.Forms.SplitContainer();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabPageJson = new System.Windows.Forms.TabPage();
            this.richTextBoxJsonProject = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLong = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShort = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGroup = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSource = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonInBlock = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOutBlock = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNormalize = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.jsonProjectStatistic = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeMain)).BeginInit();
            this.splitContainerTreeMain.Panel1.SuspendLayout();
            this.splitContainerTreeMain.Panel2.SuspendLayout();
            this.splitContainerTreeMain.SuspendLayout();
            this.contextMenuStripTreeProj.SuspendLayout();
            this.tabControlProject.SuspendLayout();
            this.tabPageSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSource)).BeginInit();
            this.tabPageGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroup)).BeginInit();
            this.tabPageTag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTag)).BeginInit();
            this.tabPageStructure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStructure)).BeginInit();
            this.splitContainerStructure.Panel1.SuspendLayout();
            this.splitContainerStructure.Panel2.SuspendLayout();
            this.splitContainerStructure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStructure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStructTagTarget)).BeginInit();
            this.splitContainerStructTagTarget.Panel1.SuspendLayout();
            this.splitContainerStructTagTarget.Panel2.SuspendLayout();
            this.splitContainerStructTagTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStructureTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStructureTarget)).BeginInit();
            this.tabPageInclude.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInclude)).BeginInit();
            this.splitContainerInclude.Panel1.SuspendLayout();
            this.splitContainerInclude.Panel2.SuspendLayout();
            this.splitContainerInclude.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInclude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIncludeChild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogMain)).BeginInit();
            this.splitContainerLogMain.Panel1.SuspendLayout();
            this.splitContainerLogMain.Panel2.SuspendLayout();
            this.splitContainerLogMain.SuspendLayout();
            this.tabPageJson.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.вилToolStripMenuItem,
            this.testToolStripMenuItem,
            this.testTagSourceToolStripMenuItem,
            this.ToolStripMenuItemDesign,
            this.ToolStripMenuItemHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1361, 25);
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
            this.toolStripMenuItem4,
            this.ToolStripMenuItemLastFiles,
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
            this.ToolStripMenuItemNew.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemNew.Text = "Новый";
            this.ToolStripMenuItemNew.Click += new System.EventHandler(this.ToolStripMenuItemNew_Click);
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemOpen.Text = "Открыть";
            this.ToolStripMenuItemOpen.Click += new System.EventHandler(this.ToolStripMenuItemOpen_Click);
            // 
            // ToolStripMenuItemSave
            // 
            this.ToolStripMenuItemSave.Name = "ToolStripMenuItemSave";
            this.ToolStripMenuItemSave.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemSave.Text = "Сохранить";
            this.ToolStripMenuItemSave.Click += new System.EventHandler(this.ToolStripMenuItemSave_Click);
            // 
            // ToolStripMenuItemSaveAs
            // 
            this.ToolStripMenuItemSaveAs.Name = "ToolStripMenuItemSaveAs";
            this.ToolStripMenuItemSaveAs.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemSaveAs.Text = "Сохранить как...";
            this.ToolStripMenuItemSaveAs.Click += new System.EventHandler(this.ToolStripMenuItemSaveAs_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolStripMenuItemLastFiles
            // 
            this.ToolStripMenuItemLastFiles.Name = "ToolStripMenuItemLastFiles";
            this.ToolStripMenuItemLastFiles.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemLastFiles.Text = "Последние";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolStripMenuItemImport
            // 
            this.ToolStripMenuItemImport.Name = "ToolStripMenuItemImport";
            this.ToolStripMenuItemImport.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemImport.Text = "Импорт";
            this.ToolStripMenuItemImport.Click += new System.EventHandler(this.ToolStripMenuItemImport_Click);
            // 
            // ToolStripMenuItemExport
            // 
            this.ToolStripMenuItemExport.Name = "ToolStripMenuItemExport";
            this.ToolStripMenuItemExport.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemExport.Text = "Экспорт";
            this.ToolStripMenuItemExport.Click += new System.EventHandler(this.ToolStripMenuItemExport_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolStripMenuItemExit
            // 
            this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
            this.ToolStripMenuItemExit.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemExit.Text = "Выход";
            this.ToolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click);
            // 
            // вилToolStripMenuItem
            // 
            this.вилToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemViewTree,
            this.Log2ToolStripMenuItem});
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
            // Log2ToolStripMenuItem
            // 
            this.Log2ToolStripMenuItem.Name = "Log2ToolStripMenuItem";
            this.Log2ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.Log2ToolStripMenuItem.Text = "Лог";
            this.Log2ToolStripMenuItem.Click += new System.EventHandler(this.Log2ToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(63, 19);
            this.testToolStripMenuItem.Text = "Test Tree";
            this.testToolStripMenuItem.Visible = false;
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // testTagSourceToolStripMenuItem
            // 
            this.testTagSourceToolStripMenuItem.Name = "testTagSourceToolStripMenuItem";
            this.testTagSourceToolStripMenuItem.Size = new System.Drawing.Size(96, 19);
            this.testTagSourceToolStripMenuItem.Text = "Test TagSource";
            this.testTagSourceToolStripMenuItem.Visible = false;
            this.testTagSourceToolStripMenuItem.Click += new System.EventHandler(this.testTagSourceToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemDesign
            // 
            this.ToolStripMenuItemDesign.Name = "ToolStripMenuItemDesign";
            this.ToolStripMenuItemDesign.Size = new System.Drawing.Size(104, 19);
            this.ToolStripMenuItemDesign.Text = "Форма дизайна";
            this.ToolStripMenuItemDesign.Click += new System.EventHandler(this.ToolStripMenuItemDesign_Click);
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
            this.toolStripStatusLabelMessage1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabelMessage2,
            this.toolStripStatusLabelMessage3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 580);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1361, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessage1
            // 
            this.toolStripStatusLabelMessage1.Name = "toolStripStatusLabelMessage1";
            this.toolStripStatusLabelMessage1.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabelMessage1.Text = "-";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabelMessage2
            // 
            this.toolStripStatusLabelMessage2.Name = "toolStripStatusLabelMessage2";
            this.toolStripStatusLabelMessage2.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabelMessage2.Text = "-";
            // 
            // toolStripStatusLabelMessage3
            // 
            this.toolStripStatusLabelMessage3.Name = "toolStripStatusLabelMessage3";
            this.toolStripStatusLabelMessage3.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabelMessage3.Text = "-";
            // 
            // splitContainerTreeMain
            // 
            this.splitContainerTreeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTreeMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTreeMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerTreeMain.Name = "splitContainerTreeMain";
            // 
            // splitContainerTreeMain.Panel1
            // 
            this.splitContainerTreeMain.Panel1.Controls.Add(this.labelProject);
            this.splitContainerTreeMain.Panel1.Controls.Add(this.treeViewProject);
            // 
            // splitContainerTreeMain.Panel2
            // 
            this.splitContainerTreeMain.Panel2.Controls.Add(this.tabControlProject);
            this.splitContainerTreeMain.Size = new System.Drawing.Size(1361, 438);
            this.splitContainerTreeMain.SplitterDistance = 226;
            this.splitContainerTreeMain.SplitterWidth = 5;
            this.splitContainerTreeMain.TabIndex = 2;
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Location = new System.Drawing.Point(10, 7);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(50, 15);
            this.labelProject.TabIndex = 1;
            this.labelProject.Text = "Проект";
            // 
            // treeViewProject
            // 
            this.treeViewProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewProject.ContextMenuStrip = this.contextMenuStripTreeProj;
            this.treeViewProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.treeViewProject.Location = new System.Drawing.Point(7, 29);
            this.treeViewProject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeViewProject.Name = "treeViewProject";
            treeNode1.Name = "Sources";
            treeNode1.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode1.Tag = "Source";
            treeNode1.Text = "Источники/Группы/Теги";
            treeNode2.Name = "Blocks";
            treeNode2.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode2.Tag = "Block";
            treeNode2.Text = "Блоки";
            treeNode3.Name = "Structures";
            treeNode3.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode3.Text = "Структуры";
            treeNode4.Name = "Includes";
            treeNode4.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode4.Tag = "Include";
            treeNode4.Text = "Классы";
            this.treeViewProject.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.treeViewProject.Size = new System.Drawing.Size(214, 399);
            this.treeViewProject.TabIndex = 0;
            this.treeViewProject.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProject_AfterSelect);
            this.treeViewProject.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewProject_MouseDoubleClick);
            // 
            // contextMenuStripTreeProj
            // 
            this.contextMenuStripTreeProj.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopy,
            this.tabFilterToolStripMenuItem});
            this.contextMenuStripTreeProj.Name = "contextMenuStripTree";
            this.contextMenuStripTreeProj.Size = new System.Drawing.Size(140, 48);
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemCopy.Text = "Копировать";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            // 
            // tabFilterToolStripMenuItem
            // 
            this.tabFilterToolStripMenuItem.Name = "tabFilterToolStripMenuItem";
            this.tabFilterToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.tabFilterToolStripMenuItem.Text = "Фильтр";
            this.tabFilterToolStripMenuItem.Click += new System.EventHandler(this.tabFilterToolStripMenuItem_Click);
            // 
            // tabControlProject
            // 
            this.tabControlProject.Controls.Add(this.tabPageJson);
            this.tabControlProject.Controls.Add(this.tabPageSource);
            this.tabControlProject.Controls.Add(this.tabPageGroup);
            this.tabControlProject.Controls.Add(this.tabPageTag);
            this.tabControlProject.Controls.Add(this.tabPageStructure);
            this.tabControlProject.Controls.Add(this.tabPageInclude);
            this.tabControlProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProject.Location = new System.Drawing.Point(0, 0);
            this.tabControlProject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlProject.Multiline = true;
            this.tabControlProject.Name = "tabControlProject";
            this.tabControlProject.SelectedIndex = 0;
            this.tabControlProject.Size = new System.Drawing.Size(1130, 438);
            this.tabControlProject.TabIndex = 0;
            this.tabControlProject.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
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
            this.tabPageSource.Location = new System.Drawing.Point(4, 24);
            this.tabPageSource.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageSource.Name = "tabPageSource";
            this.tabPageSource.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageSource.Size = new System.Drawing.Size(1122, 410);
            this.tabPageSource.TabIndex = 0;
            this.tabPageSource.Text = "Источники данных";
            this.tabPageSource.UseVisualStyleBackColor = true;
            // 
            // buttonSourceFilter
            // 
            this.buttonSourceFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSourceFilter.Location = new System.Drawing.Point(1014, 38);
            this.buttonSourceFilter.Name = "buttonSourceFilter";
            this.buttonSourceFilter.Size = new System.Drawing.Size(102, 24);
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
            this.buttonSourceHelp.Click += new System.EventHandler(this.buttonSourceHelp_Click);
            // 
            // checkBoxSourceStatistic
            // 
            this.checkBoxSourceStatistic.AutoSize = true;
            this.checkBoxSourceStatistic.Location = new System.Drawing.Point(653, 10);
            this.checkBoxSourceStatistic.Name = "checkBoxSourceStatistic";
            this.checkBoxSourceStatistic.Size = new System.Drawing.Size(95, 19);
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
            this.checkBoxSourceDesc.Size = new System.Drawing.Size(83, 19);
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
            this.checkBoxSourceRuntime.Size = new System.Drawing.Size(73, 19);
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
            this.checkBoxSourceEditor.Size = new System.Drawing.Size(82, 19);
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
            this.textBoxSourceFilter.Size = new System.Drawing.Size(1001, 21);
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
            this.buttonSourceDel.Click += new System.EventHandler(this.buttonSourceDel_Click);
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
            this.buttonSourceCopy.Click += new System.EventHandler(this.buttonSourceCopy_Click);
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
            this.sourceAutomation,
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
            this.dataGridViewSource.Size = new System.Drawing.Size(1109, 373);
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.sourceID.DefaultCellStyle = dataGridViewCellStyle1;
            this.sourceID.HeaderText = "ID";
            this.sourceID.Name = "sourceID";
            this.sourceID.ReadOnly = true;
            this.sourceID.Width = 44;
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
            // sourceAutomation
            // 
            this.sourceAutomation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sourceAutomation.HeaderText = "АО";
            this.sourceAutomation.Name = "sourceAutomation";
            this.sourceAutomation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.sourceAutomation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.sourceAutomation.Width = 48;
            // 
            // sourceAutoRestart
            // 
            this.sourceAutoRestart.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sourceAutoRestart.HeaderText = "АПП";
            this.sourceAutoRestart.Name = "sourceAutoRestart";
            this.sourceAutoRestart.Width = 38;
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
            this.sourceTags.Width = 58;
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
            this.tabPageGroup.Location = new System.Drawing.Point(4, 24);
            this.tabPageGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageGroup.Name = "tabPageGroup";
            this.tabPageGroup.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageGroup.Size = new System.Drawing.Size(1122, 410);
            this.tabPageGroup.TabIndex = 1;
            this.tabPageGroup.Text = "Группы опроса";
            this.tabPageGroup.UseVisualStyleBackColor = true;
            // 
            // buttonGroupFilter
            // 
            this.buttonGroupFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGroupFilter.Location = new System.Drawing.Point(1005, 38);
            this.buttonGroupFilter.Name = "buttonGroupFilter";
            this.buttonGroupFilter.Size = new System.Drawing.Size(111, 24);
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
            this.checkBoxGroupSource.Size = new System.Drawing.Size(81, 19);
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
            this.checkBoxGroupRuntime.Size = new System.Drawing.Size(73, 19);
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
            this.comboBoxGroupFilterSource.Size = new System.Drawing.Size(199, 23);
            this.comboBoxGroupFilterSource.Sorted = true;
            this.comboBoxGroupFilterSource.TabIndex = 17;
            this.toolTip1.SetToolTip(this.comboBoxGroupFilterSource, "Источник");
            this.comboBoxGroupFilterSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxGroupFilterSource_SelectedIndexChanged);
            this.comboBoxGroupFilterSource.TextChanged += new System.EventHandler(this.comboBoxGroupFilterSource_TextChanged);
            // 
            // checkBoxGroupStatistic
            // 
            this.checkBoxGroupStatistic.AutoSize = true;
            this.checkBoxGroupStatistic.Location = new System.Drawing.Point(562, 10);
            this.checkBoxGroupStatistic.Name = "checkBoxGroupStatistic";
            this.checkBoxGroupStatistic.Size = new System.Drawing.Size(95, 19);
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
            this.checkBoxGroupDesc.Size = new System.Drawing.Size(83, 19);
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
            this.checkBoxGroupEditor.Size = new System.Drawing.Size(82, 19);
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
            this.textBoxGroupFilter.Size = new System.Drawing.Size(787, 21);
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
            this.buttonGroupCopy.Click += new System.EventHandler(this.buttonGroupCopy_Click);
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
            this.dataGridViewGroup.Size = new System.Drawing.Size(1109, 339);
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.groupID.DefaultCellStyle = dataGridViewCellStyle2;
            this.groupID.HeaderText = "ID";
            this.groupID.Name = "groupID";
            this.groupID.ReadOnly = true;
            this.groupID.Width = 44;
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
            this.groupTags.Width = 39;
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
            this.tabPageTag.Controls.Add(this.checkBoxTagAddress);
            this.tabPageTag.Controls.Add(this.checkBoxTagSave);
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
            this.tabPageTag.Location = new System.Drawing.Point(4, 24);
            this.tabPageTag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPageTag.Name = "tabPageTag";
            this.tabPageTag.Size = new System.Drawing.Size(1122, 410);
            this.tabPageTag.TabIndex = 2;
            this.tabPageTag.Text = "Теги";
            this.tabPageTag.UseVisualStyleBackColor = true;
            // 
            // checkBoxTagAddress
            // 
            this.checkBoxTagAddress.AutoSize = true;
            this.checkBoxTagAddress.Location = new System.Drawing.Point(706, 38);
            this.checkBoxTagAddress.Name = "checkBoxTagAddress";
            this.checkBoxTagAddress.Size = new System.Drawing.Size(60, 19);
            this.checkBoxTagAddress.TabIndex = 27;
            this.checkBoxTagAddress.Text = "Адрес";
            this.checkBoxTagAddress.UseVisualStyleBackColor = true;
            this.checkBoxTagAddress.CheckedChanged += new System.EventHandler(this.checkBoxTagAddress_CheckedChanged);
            // 
            // checkBoxTagSave
            // 
            this.checkBoxTagSave.AutoSize = true;
            this.checkBoxTagSave.Location = new System.Drawing.Point(630, 38);
            this.checkBoxTagSave.Name = "checkBoxTagSave";
            this.checkBoxTagSave.Size = new System.Drawing.Size(68, 19);
            this.checkBoxTagSave.TabIndex = 26;
            this.checkBoxTagSave.Text = "Запись";
            this.checkBoxTagSave.UseVisualStyleBackColor = true;
            this.checkBoxTagSave.CheckedChanged += new System.EventHandler(this.checkBoxTagSave_CheckedChanged);
            // 
            // buttonTagFilter
            // 
            this.buttonTagFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTagFilter.Location = new System.Drawing.Point(1044, 65);
            this.buttonTagFilter.Name = "buttonTagFilter";
            this.buttonTagFilter.Size = new System.Drawing.Size(72, 24);
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
            this.checkBoxTagSG.Location = new System.Drawing.Point(494, 38);
            this.checkBoxTagSG.Name = "checkBoxTagSG";
            this.checkBoxTagSG.Size = new System.Drawing.Size(124, 19);
            this.checkBoxTagSG.TabIndex = 24;
            this.checkBoxTagSG.Text = "Источник/Группа";
            this.checkBoxTagSG.UseVisualStyleBackColor = true;
            this.checkBoxTagSG.CheckedChanged += new System.EventHandler(this.checkBoxTagSG_CheckedChanged);
            // 
            // checkBoxTagBP
            // 
            this.checkBoxTagBP.AutoSize = true;
            this.checkBoxTagBP.Location = new System.Drawing.Point(269, 38);
            this.checkBoxTagBP.Name = "checkBoxTagBP";
            this.checkBoxTagBP.Size = new System.Drawing.Size(114, 19);
            this.checkBoxTagBP.TabIndex = 23;
            this.checkBoxTagBP.Text = "Блок/Страница";
            this.checkBoxTagBP.UseVisualStyleBackColor = true;
            this.checkBoxTagBP.CheckedChanged += new System.EventHandler(this.checkBoxTagBP_CheckedChanged);
            // 
            // comboBoxTagFilterPage
            // 
            this.comboBoxTagFilterPage.FormattingEnabled = true;
            this.comboBoxTagFilterPage.Location = new System.Drawing.Point(460, 64);
            this.comboBoxTagFilterPage.Name = "comboBoxTagFilterPage";
            this.comboBoxTagFilterPage.Size = new System.Drawing.Size(145, 23);
            this.comboBoxTagFilterPage.Sorted = true;
            this.comboBoxTagFilterPage.TabIndex = 22;
            this.toolTip1.SetToolTip(this.comboBoxTagFilterPage, "Страница");
            this.comboBoxTagFilterPage.SelectedIndexChanged += new System.EventHandler(this.comboBoxTagFilterPage_SelectedIndexChanged);
            this.comboBoxTagFilterPage.TextChanged += new System.EventHandler(this.comboBoxTagFilterPage_TextChanged);
            // 
            // comboBoxTagFilterBlock
            // 
            this.comboBoxTagFilterBlock.FormattingEnabled = true;
            this.comboBoxTagFilterBlock.Location = new System.Drawing.Point(309, 64);
            this.comboBoxTagFilterBlock.Name = "comboBoxTagFilterBlock";
            this.comboBoxTagFilterBlock.Size = new System.Drawing.Size(145, 23);
            this.comboBoxTagFilterBlock.Sorted = true;
            this.comboBoxTagFilterBlock.TabIndex = 21;
            this.toolTip1.SetToolTip(this.comboBoxTagFilterBlock, "Блок");
            this.comboBoxTagFilterBlock.SelectedIndexChanged += new System.EventHandler(this.comboBoxTagFilterBlock_SelectedIndexChanged);
            this.comboBoxTagFilterBlock.TextChanged += new System.EventHandler(this.comboBoxTagFilterBlock_TextChanged);
            // 
            // comboBoxTagFilterGroup
            // 
            this.comboBoxTagFilterGroup.FormattingEnabled = true;
            this.comboBoxTagFilterGroup.Location = new System.Drawing.Point(158, 64);
            this.comboBoxTagFilterGroup.Name = "comboBoxTagFilterGroup";
            this.comboBoxTagFilterGroup.Size = new System.Drawing.Size(145, 23);
            this.comboBoxTagFilterGroup.Sorted = true;
            this.comboBoxTagFilterGroup.TabIndex = 20;
            this.toolTip1.SetToolTip(this.comboBoxTagFilterGroup, "Группа");
            this.comboBoxTagFilterGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxTagFilterGroup_SelectedIndexChanged);
            this.comboBoxTagFilterGroup.TextChanged += new System.EventHandler(this.comboBoxTagFilterGroup_TextChanged);
            // 
            // comboBoxTagFilterSource
            // 
            this.comboBoxTagFilterSource.FormattingEnabled = true;
            this.comboBoxTagFilterSource.Location = new System.Drawing.Point(7, 64);
            this.comboBoxTagFilterSource.Name = "comboBoxTagFilterSource";
            this.comboBoxTagFilterSource.Size = new System.Drawing.Size(145, 23);
            this.comboBoxTagFilterSource.Sorted = true;
            this.comboBoxTagFilterSource.TabIndex = 19;
            this.toolTip1.SetToolTip(this.comboBoxTagFilterSource, "Источник");
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
            this.buttonTagHelp.Click += new System.EventHandler(this.buttonTagHelp_Click);
            // 
            // checkBoxTagStatistic
            // 
            this.checkBoxTagStatistic.AutoSize = true;
            this.checkBoxTagStatistic.Location = new System.Drawing.Point(393, 38);
            this.checkBoxTagStatistic.Name = "checkBoxTagStatistic";
            this.checkBoxTagStatistic.Size = new System.Drawing.Size(95, 19);
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
            this.checkBoxTagDesc.Location = new System.Drawing.Point(177, 38);
            this.checkBoxTagDesc.Name = "checkBoxTagDesc";
            this.checkBoxTagDesc.Size = new System.Drawing.Size(83, 19);
            this.checkBoxTagDesc.TabIndex = 16;
            this.checkBoxTagDesc.Text = "Описание";
            this.checkBoxTagDesc.UseVisualStyleBackColor = true;
            this.checkBoxTagDesc.CheckedChanged += new System.EventHandler(this.checkBoxTagDesc_CheckedChanged);
            // 
            // checkBoxTagRuntime
            // 
            this.checkBoxTagRuntime.AutoSize = true;
            this.checkBoxTagRuntime.Location = new System.Drawing.Point(95, 38);
            this.checkBoxTagRuntime.Name = "checkBoxTagRuntime";
            this.checkBoxTagRuntime.Size = new System.Drawing.Size(73, 19);
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
            this.checkBoxTagEditor.Location = new System.Drawing.Point(7, 38);
            this.checkBoxTagEditor.Name = "checkBoxTagEditor";
            this.checkBoxTagEditor.Size = new System.Drawing.Size(82, 19);
            this.checkBoxTagEditor.TabIndex = 14;
            this.checkBoxTagEditor.Text = "Редактор";
            this.checkBoxTagEditor.UseVisualStyleBackColor = true;
            this.checkBoxTagEditor.CheckedChanged += new System.EventHandler(this.checkBoxTagEditor_CheckedChanged);
            // 
            // textBoxTagFilter
            // 
            this.textBoxTagFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTagFilter.Location = new System.Drawing.Point(611, 66);
            this.textBoxTagFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxTagFilter.Name = "textBoxTagFilter";
            this.textBoxTagFilter.Size = new System.Drawing.Size(427, 21);
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
            this.buttonTagCopy.Click += new System.EventHandler(this.buttonTagCopy_Click);
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
            this.dataGridViewTag.Location = new System.Drawing.Point(7, 95);
            this.dataGridViewTag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewTag.MultiSelect = false;
            this.dataGridViewTag.Name = "dataGridViewTag";
            this.dataGridViewTag.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTag.Size = new System.Drawing.Size(1109, 309);
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.tagID.DefaultCellStyle = dataGridViewCellStyle3;
            this.tagID.HeaderText = "ID";
            this.tagID.Name = "tagID";
            this.tagID.ReadOnly = true;
            this.tagID.Width = 44;
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
            this.tagBlock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tagBlock.HeaderText = "Блок";
            this.tagBlock.Name = "tagBlock";
            // 
            // tagPage
            // 
            this.tagPage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tagPage.HeaderText = "Страница";
            this.tagPage.Name = "tagPage";
            this.tagPage.Width = 89;
            // 
            // tagStatus
            // 
            this.tagStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tagStatus.HeaderText = "Статус";
            this.tagStatus.Name = "tagStatus";
            this.tagStatus.ReadOnly = true;
            this.tagStatus.Width = 72;
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
            // tabPageStructure
            // 
            this.tabPageStructure.Controls.Add(this.checkBoxStructCol);
            this.tabPageStructure.Controls.Add(this.buttonStructHelp);
            this.tabPageStructure.Controls.Add(this.buttonStructResult);
            this.tabPageStructure.Controls.Add(this.buttonStructDel);
            this.tabPageStructure.Controls.Add(this.buttonStructCopy);
            this.tabPageStructure.Controls.Add(this.splitContainerStructure);
            this.tabPageStructure.Location = new System.Drawing.Point(4, 24);
            this.tabPageStructure.Name = "tabPageStructure";
            this.tabPageStructure.Size = new System.Drawing.Size(1122, 410);
            this.tabPageStructure.TabIndex = 5;
            this.tabPageStructure.Text = "Структуры тегов";
            this.tabPageStructure.UseVisualStyleBackColor = true;
            // 
            // checkBoxStructCol
            // 
            this.checkBoxStructCol.AutoSize = true;
            this.checkBoxStructCol.Checked = true;
            this.checkBoxStructCol.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStructCol.Location = new System.Drawing.Point(352, 9);
            this.checkBoxStructCol.Name = "checkBoxStructCol";
            this.checkBoxStructCol.Size = new System.Drawing.Size(271, 19);
            this.checkBoxStructCol.TabIndex = 26;
            this.checkBoxStructCol.Text = "Название структуры для дочерних таблиц";
            this.checkBoxStructCol.UseVisualStyleBackColor = true;
            this.checkBoxStructCol.CheckedChanged += new System.EventHandler(this.checkBoxStructCol_CheckedChanged);
            // 
            // buttonStructHelp
            // 
            this.buttonStructHelp.Location = new System.Drawing.Point(265, 7);
            this.buttonStructHelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStructHelp.Name = "buttonStructHelp";
            this.buttonStructHelp.Size = new System.Drawing.Size(80, 24);
            this.buttonStructHelp.TabIndex = 22;
            this.buttonStructHelp.Text = "Справка";
            this.buttonStructHelp.UseVisualStyleBackColor = true;
            // 
            // buttonStructResult
            // 
            this.buttonStructResult.Location = new System.Drawing.Point(179, 7);
            this.buttonStructResult.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStructResult.Name = "buttonStructResult";
            this.buttonStructResult.Size = new System.Drawing.Size(80, 24);
            this.buttonStructResult.TabIndex = 21;
            this.buttonStructResult.Text = "Результат";
            this.buttonStructResult.UseVisualStyleBackColor = true;
            // 
            // buttonStructDel
            // 
            this.buttonStructDel.Location = new System.Drawing.Point(93, 7);
            this.buttonStructDel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStructDel.Name = "buttonStructDel";
            this.buttonStructDel.Size = new System.Drawing.Size(80, 24);
            this.buttonStructDel.TabIndex = 20;
            this.buttonStructDel.Text = "Удалить";
            this.buttonStructDel.UseVisualStyleBackColor = true;
            // 
            // buttonStructCopy
            // 
            this.buttonStructCopy.Location = new System.Drawing.Point(7, 7);
            this.buttonStructCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStructCopy.Name = "buttonStructCopy";
            this.buttonStructCopy.Size = new System.Drawing.Size(80, 24);
            this.buttonStructCopy.TabIndex = 19;
            this.buttonStructCopy.Text = "Копия";
            this.buttonStructCopy.UseVisualStyleBackColor = true;
            // 
            // splitContainerStructure
            // 
            this.splitContainerStructure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerStructure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerStructure.Location = new System.Drawing.Point(3, 36);
            this.splitContainerStructure.Name = "splitContainerStructure";
            this.splitContainerStructure.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerStructure.Panel1
            // 
            this.splitContainerStructure.Panel1.Controls.Add(this.label1);
            this.splitContainerStructure.Panel1.Controls.Add(this.buttonStructureLeft);
            this.splitContainerStructure.Panel1.Controls.Add(this.buttonStructureFilter);
            this.splitContainerStructure.Panel1.Controls.Add(this.textBoxStructureFilter);
            this.splitContainerStructure.Panel1.Controls.Add(this.comboBoxStructureTargetFilterParent);
            this.splitContainerStructure.Panel1.Controls.Add(this.dataGridViewStructure);
            // 
            // splitContainerStructure.Panel2
            // 
            this.splitContainerStructure.Panel2.Controls.Add(this.splitContainerStructTagTarget);
            this.splitContainerStructure.Panel2.Controls.Add(this.label3);
            this.splitContainerStructure.Panel2.Controls.Add(this.label2);
            this.splitContainerStructure.Panel2.Controls.Add(this.buttonStructureRight);
            this.splitContainerStructure.Size = new System.Drawing.Size(1116, 371);
            this.splitContainerStructure.SplitterDistance = 163;
            this.splitContainerStructure.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 21;
            this.label1.Text = "Структуры";
            // 
            // buttonStructureLeft
            // 
            this.buttonStructureLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStructureLeft.Location = new System.Drawing.Point(933, 4);
            this.buttonStructureLeft.Name = "buttonStructureLeft";
            this.buttonStructureLeft.Size = new System.Drawing.Size(176, 28);
            this.buttonStructureLeft.TabIndex = 20;
            this.buttonStructureLeft.Text = ">";
            this.buttonStructureLeft.UseVisualStyleBackColor = true;
            this.buttonStructureLeft.Click += new System.EventHandler(this.buttonStructureLeft_Click);
            // 
            // buttonStructureFilter
            // 
            this.buttonStructureFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStructureFilter.Location = new System.Drawing.Point(802, 6);
            this.buttonStructureFilter.Name = "buttonStructureFilter";
            this.buttonStructureFilter.Size = new System.Drawing.Size(106, 24);
            this.buttonStructureFilter.TabIndex = 19;
            this.buttonStructureFilter.Text = "Фильтр";
            this.buttonStructureFilter.UseVisualStyleBackColor = true;
            this.buttonStructureFilter.Click += new System.EventHandler(this.buttonStructureFilter_Click);
            // 
            // textBoxStructureFilter
            // 
            this.textBoxStructureFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStructureFilter.Location = new System.Drawing.Point(289, 7);
            this.textBoxStructureFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxStructureFilter.Name = "textBoxStructureFilter";
            this.textBoxStructureFilter.Size = new System.Drawing.Size(507, 21);
            this.textBoxStructureFilter.TabIndex = 18;
            this.textBoxStructureFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxStructureFilter.TextChanged += new System.EventHandler(this.textBoxStructureFilter_TextChanged);
            // 
            // comboBoxStructureTargetFilterParent
            // 
            this.comboBoxStructureTargetFilterParent.FormattingEnabled = true;
            this.comboBoxStructureTargetFilterParent.Location = new System.Drawing.Point(84, 5);
            this.comboBoxStructureTargetFilterParent.Name = "comboBoxStructureTargetFilterParent";
            this.comboBoxStructureTargetFilterParent.Size = new System.Drawing.Size(199, 23);
            this.comboBoxStructureTargetFilterParent.Sorted = true;
            this.comboBoxStructureTargetFilterParent.TabIndex = 19;
            this.toolTip1.SetToolTip(this.comboBoxStructureTargetFilterParent, "Структура");
            this.comboBoxStructureTargetFilterParent.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetFilterSource_SelectedIndexChanged);
            this.comboBoxStructureTargetFilterParent.TextChanged += new System.EventHandler(this.comboBoxTargetFilterSource_TextChanged);
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
            this.structureTemplate,
            this.structureGroup});
            this.dataGridViewStructure.Location = new System.Drawing.Point(3, 34);
            this.dataGridViewStructure.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewStructure.MultiSelect = false;
            this.dataGridViewStructure.Name = "dataGridViewStructure";
            this.dataGridViewStructure.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStructure.Size = new System.Drawing.Size(1106, 123);
            this.dataGridViewStructure.TabIndex = 10;
            this.dataGridViewStructure.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewStructure_CellEndEdit);
            this.dataGridViewStructure.SelectionChanged += new System.EventHandler(this.dataGridViewStructure_SelectionChanged);
            this.dataGridViewStructure.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewStructure_UserAddedRow);
            // 
            // structureID
            // 
            this.structureID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.structureID.DefaultCellStyle = dataGridViewCellStyle4;
            this.structureID.HeaderText = "ID";
            this.structureID.Name = "structureID";
            this.structureID.ReadOnly = true;
            this.structureID.Width = 44;
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
            // splitContainerStructTagTarget
            // 
            this.splitContainerStructTagTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerStructTagTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerStructTagTarget.Location = new System.Drawing.Point(-4, 36);
            this.splitContainerStructTagTarget.Name = "splitContainerStructTagTarget";
            // 
            // splitContainerStructTagTarget.Panel1
            // 
            this.splitContainerStructTagTarget.Panel1.Controls.Add(this.dataGridViewStructureTag);
            // 
            // splitContainerStructTagTarget.Panel2
            // 
            this.splitContainerStructTagTarget.Panel2.Controls.Add(this.dataGridViewStructureTarget);
            this.splitContainerStructTagTarget.Size = new System.Drawing.Size(1115, 169);
            this.splitContainerStructTagTarget.SplitterDistance = 423;
            this.splitContainerStructTagTarget.TabIndex = 25;
            // 
            // dataGridViewStructureTag
            // 
            this.dataGridViewStructureTag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStructureTag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStructureTag.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.targetTagID,
            this.targetTagStruct,
            this.targetTagTitle});
            this.dataGridViewStructureTag.Location = new System.Drawing.Point(7, 4);
            this.dataGridViewStructureTag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewStructureTag.MultiSelect = false;
            this.dataGridViewStructureTag.Name = "dataGridViewStructureTag";
            this.dataGridViewStructureTag.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStructureTag.Size = new System.Drawing.Size(411, 157);
            this.dataGridViewStructureTag.TabIndex = 23;
            this.dataGridViewStructureTag.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewStructureTag_CellEndEdit);
            this.dataGridViewStructureTag.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewStructureTag_UserAddedRow);
            // 
            // targetTagID
            // 
            this.targetTagID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.targetTagID.DefaultCellStyle = dataGridViewCellStyle5;
            this.targetTagID.HeaderText = "ID";
            this.targetTagID.Name = "targetTagID";
            this.targetTagID.ReadOnly = true;
            this.targetTagID.Width = 44;
            // 
            // targetTagStruct
            // 
            this.targetTagStruct.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetTagStruct.HeaderText = "Структура";
            this.targetTagStruct.Name = "targetTagStruct";
            // 
            // targetTagTitle
            // 
            this.targetTagTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetTagTitle.HeaderText = "Название тега (начало / #Source)";
            this.targetTagTitle.Name = "targetTagTitle";
            // 
            // dataGridViewStructureTarget
            // 
            this.dataGridViewStructureTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStructureTarget.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStructureTarget.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.targetID,
            this.targetStructure,
            this.targetTitle,
            this.targetAddress,
            this.targetDesc});
            this.dataGridViewStructureTarget.Location = new System.Drawing.Point(3, 4);
            this.dataGridViewStructureTarget.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewStructureTarget.MultiSelect = false;
            this.dataGridViewStructureTarget.Name = "dataGridViewStructureTarget";
            this.dataGridViewStructureTarget.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStructureTarget.Size = new System.Drawing.Size(680, 157);
            this.dataGridViewStructureTarget.TabIndex = 11;
            this.dataGridViewStructureTarget.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewStructureTarget_CellEndEdit);
            this.dataGridViewStructureTarget.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewTarget_UserAddedRow);
            // 
            // targetID
            // 
            this.targetID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.targetID.DefaultCellStyle = dataGridViewCellStyle6;
            this.targetID.HeaderText = "ID";
            this.targetID.Name = "targetID";
            this.targetID.ReadOnly = true;
            this.targetID.Width = 44;
            // 
            // targetStructure
            // 
            this.targetStructure.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetStructure.HeaderText = "Структура";
            this.targetStructure.Name = "targetStructure";
            // 
            // targetTitle
            // 
            this.targetTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetTitle.HeaderText = "Название тега (конец)";
            this.targetTitle.Name = "targetTitle";
            // 
            // targetAddress
            // 
            this.targetAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetAddress.HeaderText = "Адрес (#Target)";
            this.targetAddress.Name = "targetAddress";
            // 
            // targetDesc
            // 
            this.targetDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetDesc.HeaderText = "Описание тега";
            this.targetDesc.Name = "targetDesc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 15);
            this.label3.TabIndex = 24;
            this.label3.Text = "Теги";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(773, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "Элементы структуры";
            // 
            // buttonStructureRight
            // 
            this.buttonStructureRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStructureRight.Location = new System.Drawing.Point(933, 5);
            this.buttonStructureRight.Name = "buttonStructureRight";
            this.buttonStructureRight.Size = new System.Drawing.Size(176, 28);
            this.buttonStructureRight.TabIndex = 21;
            this.buttonStructureRight.Text = "<";
            this.buttonStructureRight.UseVisualStyleBackColor = true;
            this.buttonStructureRight.Click += new System.EventHandler(this.buttonStructureRight_Click);
            // 
            // tabPageInclude
            // 
            this.tabPageInclude.Controls.Add(this.checkBoxIncludePrefix);
            this.tabPageInclude.Controls.Add(this.buttonIncludeHelp);
            this.tabPageInclude.Controls.Add(this.buttonIncludeResult);
            this.tabPageInclude.Controls.Add(this.buttonIncludeDel);
            this.tabPageInclude.Controls.Add(this.buttonIncludeCopy);
            this.tabPageInclude.Controls.Add(this.splitContainerInclude);
            this.tabPageInclude.Location = new System.Drawing.Point(4, 24);
            this.tabPageInclude.Name = "tabPageInclude";
            this.tabPageInclude.Size = new System.Drawing.Size(1122, 410);
            this.tabPageInclude.TabIndex = 4;
            this.tabPageInclude.Text = "Классы (внешние проекты)";
            this.tabPageInclude.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludePrefix
            // 
            this.checkBoxIncludePrefix.AutoSize = true;
            this.checkBoxIncludePrefix.Checked = true;
            this.checkBoxIncludePrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIncludePrefix.Location = new System.Drawing.Point(352, 9);
            this.checkBoxIncludePrefix.Name = "checkBoxIncludePrefix";
            this.checkBoxIncludePrefix.Size = new System.Drawing.Size(280, 19);
            this.checkBoxIncludePrefix.TabIndex = 31;
            this.checkBoxIncludePrefix.Text = "Название префикса для дочерней таблицы";
            this.checkBoxIncludePrefix.UseVisualStyleBackColor = true;
            this.checkBoxIncludePrefix.CheckedChanged += new System.EventHandler(this.checkBoxIncludePrefix_CheckedChanged);
            // 
            // buttonIncludeHelp
            // 
            this.buttonIncludeHelp.Location = new System.Drawing.Point(265, 7);
            this.buttonIncludeHelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonIncludeHelp.Name = "buttonIncludeHelp";
            this.buttonIncludeHelp.Size = new System.Drawing.Size(80, 24);
            this.buttonIncludeHelp.TabIndex = 30;
            this.buttonIncludeHelp.Text = "Справка";
            this.buttonIncludeHelp.UseVisualStyleBackColor = true;
            // 
            // buttonIncludeResult
            // 
            this.buttonIncludeResult.Location = new System.Drawing.Point(179, 7);
            this.buttonIncludeResult.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonIncludeResult.Name = "buttonIncludeResult";
            this.buttonIncludeResult.Size = new System.Drawing.Size(80, 24);
            this.buttonIncludeResult.TabIndex = 29;
            this.buttonIncludeResult.Text = "Результат";
            this.buttonIncludeResult.UseVisualStyleBackColor = true;
            // 
            // buttonIncludeDel
            // 
            this.buttonIncludeDel.Location = new System.Drawing.Point(93, 7);
            this.buttonIncludeDel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonIncludeDel.Name = "buttonIncludeDel";
            this.buttonIncludeDel.Size = new System.Drawing.Size(80, 24);
            this.buttonIncludeDel.TabIndex = 28;
            this.buttonIncludeDel.Text = "Удалить";
            this.buttonIncludeDel.UseVisualStyleBackColor = true;
            // 
            // buttonIncludeCopy
            // 
            this.buttonIncludeCopy.Location = new System.Drawing.Point(7, 7);
            this.buttonIncludeCopy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonIncludeCopy.Name = "buttonIncludeCopy";
            this.buttonIncludeCopy.Size = new System.Drawing.Size(80, 24);
            this.buttonIncludeCopy.TabIndex = 27;
            this.buttonIncludeCopy.Text = "Копия";
            this.buttonIncludeCopy.UseVisualStyleBackColor = true;
            // 
            // splitContainerInclude
            // 
            this.splitContainerInclude.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerInclude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerInclude.Location = new System.Drawing.Point(3, 38);
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
            this.splitContainerInclude.Panel2.Controls.Add(this.comboBoxIncludeChildFilterParent);
            this.splitContainerInclude.Panel2.Controls.Add(this.buttonIncludeChildFilter);
            this.splitContainerInclude.Panel2.Controls.Add(this.textBoxIncludeChildFilter);
            this.splitContainerInclude.Panel2.Controls.Add(this.buttonIncludeRight);
            this.splitContainerInclude.Panel2.Controls.Add(this.dataGridViewIncludeChild);
            this.splitContainerInclude.Size = new System.Drawing.Size(1116, 369);
            this.splitContainerInclude.SplitterDistance = 552;
            this.splitContainerInclude.SplitterWidth = 10;
            this.splitContainerInclude.TabIndex = 4;
            // 
            // buttonIncludeFilter
            // 
            this.buttonIncludeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonIncludeFilter.Location = new System.Drawing.Point(456, 3);
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
            this.textBoxIncludeFilter.Size = new System.Drawing.Size(447, 21);
            this.textBoxIncludeFilter.TabIndex = 14;
            this.textBoxIncludeFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxIncludeFilter.TextChanged += new System.EventHandler(this.textBoxIncludeFilter_TextChanged);
            // 
            // buttonIncludeLeft
            // 
            this.buttonIncludeLeft.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonIncludeLeft.Location = new System.Drawing.Point(530, 121);
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
            this.includeFileName});
            this.dataGridViewInclude.Location = new System.Drawing.Point(3, 32);
            this.dataGridViewInclude.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewInclude.MultiSelect = false;
            this.dataGridViewInclude.Name = "dataGridViewInclude";
            this.dataGridViewInclude.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInclude.Size = new System.Drawing.Size(523, 331);
            this.dataGridViewInclude.TabIndex = 2;
            this.dataGridViewInclude.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewInclude_CellEndEdit);
            this.dataGridViewInclude.SelectionChanged += new System.EventHandler(this.dataGridViewInclude_SelectionChanged);
            this.dataGridViewInclude.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewInclude_UserAddedRow);
            // 
            // includeID
            // 
            this.includeID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            dataGridViewCellStyle7.NullValue = null;
            this.includeID.DefaultCellStyle = dataGridViewCellStyle7;
            this.includeID.HeaderText = "ID";
            this.includeID.Name = "includeID";
            this.includeID.ReadOnly = true;
            this.includeID.Width = 44;
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
            // comboBoxIncludeChildFilterParent
            // 
            this.comboBoxIncludeChildFilterParent.FormattingEnabled = true;
            this.comboBoxIncludeChildFilterParent.Location = new System.Drawing.Point(26, 3);
            this.comboBoxIncludeChildFilterParent.Name = "comboBoxIncludeChildFilterParent";
            this.comboBoxIncludeChildFilterParent.Size = new System.Drawing.Size(199, 23);
            this.comboBoxIncludeChildFilterParent.Sorted = true;
            this.comboBoxIncludeChildFilterParent.TabIndex = 18;
            this.toolTip1.SetToolTip(this.comboBoxIncludeChildFilterParent, "Класс");
            this.comboBoxIncludeChildFilterParent.SelectedIndexChanged += new System.EventHandler(this.comboBoxChangeFilterInclude_SelectedIndexChanged);
            this.comboBoxIncludeChildFilterParent.TextChanged += new System.EventHandler(this.comboBoxChangeFilterInclude_TextChanged);
            // 
            // buttonIncludeChildFilter
            // 
            this.buttonIncludeChildFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonIncludeChildFilter.Location = new System.Drawing.Point(451, 3);
            this.buttonIncludeChildFilter.Name = "buttonIncludeChildFilter";
            this.buttonIncludeChildFilter.Size = new System.Drawing.Size(75, 24);
            this.buttonIncludeChildFilter.TabIndex = 16;
            this.buttonIncludeChildFilter.Text = "Фильтр";
            this.buttonIncludeChildFilter.UseVisualStyleBackColor = true;
            this.buttonIncludeChildFilter.Click += new System.EventHandler(this.buttonChangeFilter_Click);
            // 
            // textBoxIncludeChildFilter
            // 
            this.textBoxIncludeChildFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIncludeChildFilter.Location = new System.Drawing.Point(231, 4);
            this.textBoxIncludeChildFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxIncludeChildFilter.Name = "textBoxIncludeChildFilter";
            this.textBoxIncludeChildFilter.Size = new System.Drawing.Size(214, 21);
            this.textBoxIncludeChildFilter.TabIndex = 15;
            this.textBoxIncludeChildFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxIncludeChildFilter.TextChanged += new System.EventHandler(this.textBoxChangeFilter_TextChanged);
            // 
            // buttonIncludeRight
            // 
            this.buttonIncludeRight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonIncludeRight.Location = new System.Drawing.Point(4, 121);
            this.buttonIncludeRight.Name = "buttonIncludeRight";
            this.buttonIncludeRight.Size = new System.Drawing.Size(17, 118);
            this.buttonIncludeRight.TabIndex = 4;
            this.buttonIncludeRight.Text = "<";
            this.buttonIncludeRight.UseVisualStyleBackColor = true;
            this.buttonIncludeRight.Click += new System.EventHandler(this.buttonIncludeRight_Click);
            // 
            // dataGridViewIncludeChild
            // 
            this.dataGridViewIncludeChild.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewIncludeChild.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIncludeChild.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.changeID,
            this.changePrefix,
            this.changeFrom,
            this.changeTo});
            this.dataGridViewIncludeChild.Location = new System.Drawing.Point(26, 32);
            this.dataGridViewIncludeChild.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewIncludeChild.MultiSelect = false;
            this.dataGridViewIncludeChild.Name = "dataGridViewIncludeChild";
            this.dataGridViewIncludeChild.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewIncludeChild.Size = new System.Drawing.Size(500, 331);
            this.dataGridViewIncludeChild.TabIndex = 3;
            this.dataGridViewIncludeChild.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIncludeChild_CellEndEdit);
            this.dataGridViewIncludeChild.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewIncludeChild_UserAddedRow);
            // 
            // changeID
            // 
            this.changeID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.changeID.DefaultCellStyle = dataGridViewCellStyle8;
            this.changeID.HeaderText = "ID";
            this.changeID.Name = "changeID";
            this.changeID.ReadOnly = true;
            this.changeID.Width = 44;
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
            // dataGridViewLog
            // 
            this.dataGridViewLog.AllowUserToAddRows = false;
            this.dataGridViewLog.AllowUserToDeleteRows = false;
            this.dataGridViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.logDT,
            this.logCategory,
            this.logType,
            this.logCode,
            this.logText});
            this.dataGridViewLog.Location = new System.Drawing.Point(7, 0);
            this.dataGridViewLog.Name = "dataGridViewLog";
            this.dataGridViewLog.RowHeadersVisible = false;
            this.dataGridViewLog.Size = new System.Drawing.Size(1344, 110);
            this.dataGridViewLog.TabIndex = 0;
            // 
            // logDT
            // 
            this.logDT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.logDT.HeaderText = "ДатаВремя";
            this.logDT.Name = "logDT";
            // 
            // logCategory
            // 
            this.logCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.logCategory.HeaderText = "Категория";
            this.logCategory.Name = "logCategory";
            this.logCategory.Width = 94;
            // 
            // logType
            // 
            this.logType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.logType.HeaderText = "Тип";
            this.logType.Name = "logType";
            this.logType.ReadOnly = true;
            this.logType.Width = 53;
            // 
            // logCode
            // 
            this.logCode.HeaderText = "Код";
            this.logCode.Name = "logCode";
            // 
            // logText
            // 
            this.logText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.logText.HeaderText = "Текст";
            this.logText.Name = "logText";
            // 
            // splitContainerLogMain
            // 
            this.splitContainerLogMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLogMain.Location = new System.Drawing.Point(0, 25);
            this.splitContainerLogMain.Name = "splitContainerLogMain";
            this.splitContainerLogMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLogMain.Panel1
            // 
            this.splitContainerLogMain.Panel1.Controls.Add(this.splitContainerTreeMain);
            // 
            // splitContainerLogMain.Panel2
            // 
            this.splitContainerLogMain.Panel2.Controls.Add(this.dataGridViewLog);
            this.splitContainerLogMain.Size = new System.Drawing.Size(1361, 555);
            this.splitContainerLogMain.SplitterDistance = 438;
            this.splitContainerLogMain.TabIndex = 3;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "WinSimpleDriver";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // tabPageJson
            // 
            this.tabPageJson.Controls.Add(this.statusStrip2);
            this.tabPageJson.Controls.Add(this.toolStrip1);
            this.tabPageJson.Controls.Add(this.richTextBoxJsonProject);
            this.tabPageJson.Location = new System.Drawing.Point(4, 24);
            this.tabPageJson.Name = "tabPageJson";
            this.tabPageJson.Size = new System.Drawing.Size(1122, 410);
            this.tabPageJson.TabIndex = 6;
            this.tabPageJson.Text = "Проект.json";
            this.tabPageJson.UseVisualStyleBackColor = true;
            // 
            // richTextBoxJsonProject
            // 
            this.richTextBoxJsonProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxJsonProject.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxJsonProject.Location = new System.Drawing.Point(4, 30);
            this.richTextBoxJsonProject.Name = "richTextBoxJsonProject";
            this.richTextBoxJsonProject.Size = new System.Drawing.Size(1111, 353);
            this.richTextBoxJsonProject.TabIndex = 0;
            this.richTextBoxJsonProject.Text = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNormalize,
            this.toolStripButtonLong,
            this.toolStripButtonShort,
            this.toolStripSeparator1,
            this.toolStripButtonGroup,
            this.toolStripButtonSource,
            this.toolStripSeparator2,
            this.toolStripButtonInBlock,
            this.toolStripButtonOutBlock});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1122, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonLong
            // 
            this.toolStripButtonLong.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLong.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLong.Image")));
            this.toolStripButtonLong.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLong.Name = "toolStripButtonLong";
            this.toolStripButtonLong.Size = new System.Drawing.Size(97, 22);
            this.toolStripButtonLong.Text = "Длинный стиль";
            this.toolStripButtonLong.Click += new System.EventHandler(this.toolStripButtonLong_Click);
            // 
            // toolStripButtonShort
            // 
            this.toolStripButtonShort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonShort.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShort.Image")));
            this.toolStripButtonShort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShort.Name = "toolStripButtonShort";
            this.toolStripButtonShort.Size = new System.Drawing.Size(98, 22);
            this.toolStripButtonShort.Text = "Короткий стиль";
            this.toolStripButtonShort.Click += new System.EventHandler(this.toolStripButtonShort_Click);
            // 
            // toolStripButtonGroup
            // 
            this.toolStripButtonGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGroup.Image")));
            this.toolStripButtonGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGroup.Name = "toolStripButtonGroup";
            this.toolStripButtonGroup.Size = new System.Drawing.Size(145, 22);
            this.toolStripButtonGroup.Text = "Групповая вложенность";
            this.toolStripButtonGroup.Click += new System.EventHandler(this.toolStripButtonGroup_Click);
            // 
            // toolStripButtonSource
            // 
            this.toolStripButtonSource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSource.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSource.Image")));
            this.toolStripButtonSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSource.Name = "toolStripButtonSource";
            this.toolStripButtonSource.Size = new System.Drawing.Size(156, 22);
            this.toolStripButtonSource.Text = "Вложенность в источники";
            this.toolStripButtonSource.Click += new System.EventHandler(this.toolStripButtonSource_Click);
            // 
            // toolStripButtonInBlock
            // 
            this.toolStripButtonInBlock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonInBlock.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonInBlock.Image")));
            this.toolStripButtonInBlock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInBlock.Name = "toolStripButtonInBlock";
            this.toolStripButtonInBlock.Size = new System.Drawing.Size(108, 22);
            this.toolStripButtonInBlock.Text = "Упаковка в блоки";
            this.toolStripButtonInBlock.Click += new System.EventHandler(this.toolStripButtonInBlock_Click);
            // 
            // toolStripButtonOutBlock
            // 
            this.toolStripButtonOutBlock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonOutBlock.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOutBlock.Image")));
            this.toolStripButtonOutBlock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOutBlock.Name = "toolStripButtonOutBlock";
            this.toolStripButtonOutBlock.Size = new System.Drawing.Size(132, 22);
            this.toolStripButtonOutBlock.Text = "Распаковка из блоков";
            this.toolStripButtonOutBlock.Click += new System.EventHandler(this.toolStripButtonOutBlock_Click);
            // 
            // toolStripButtonNormalize
            // 
            this.toolStripButtonNormalize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonNormalize.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNormalize.Image")));
            this.toolStripButtonNormalize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNormalize.Name = "toolStripButtonNormalize";
            this.toolStripButtonNormalize.Size = new System.Drawing.Size(94, 22);
            this.toolStripButtonNormalize.Text = "Нормализация";
            this.toolStripButtonNormalize.Click += new System.EventHandler(this.toolStripButtonNormalize_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jsonProjectStatistic});
            this.statusStrip2.Location = new System.Drawing.Point(0, 388);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(1122, 22);
            this.statusStrip2.TabIndex = 2;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // jsonProjectStatistic
            // 
            this.jsonProjectStatistic.Name = "jsonProjectStatistic";
            this.jsonProjectStatistic.Size = new System.Drawing.Size(107, 17);
            this.jsonProjectStatistic.Text = "jsonProjectStatistic";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1361, 602);
            this.Controls.Add(this.splitContainerLogMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "WinSimpleDriver";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainerTreeMain.Panel1.ResumeLayout(false);
            this.splitContainerTreeMain.Panel1.PerformLayout();
            this.splitContainerTreeMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeMain)).EndInit();
            this.splitContainerTreeMain.ResumeLayout(false);
            this.contextMenuStripTreeProj.ResumeLayout(false);
            this.tabControlProject.ResumeLayout(false);
            this.tabPageSource.ResumeLayout(false);
            this.tabPageSource.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSource)).EndInit();
            this.tabPageGroup.ResumeLayout(false);
            this.tabPageGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroup)).EndInit();
            this.tabPageTag.ResumeLayout(false);
            this.tabPageTag.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTag)).EndInit();
            this.tabPageStructure.ResumeLayout(false);
            this.tabPageStructure.PerformLayout();
            this.splitContainerStructure.Panel1.ResumeLayout(false);
            this.splitContainerStructure.Panel1.PerformLayout();
            this.splitContainerStructure.Panel2.ResumeLayout(false);
            this.splitContainerStructure.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStructure)).EndInit();
            this.splitContainerStructure.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStructure)).EndInit();
            this.splitContainerStructTagTarget.Panel1.ResumeLayout(false);
            this.splitContainerStructTagTarget.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStructTagTarget)).EndInit();
            this.splitContainerStructTagTarget.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStructureTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStructureTarget)).EndInit();
            this.tabPageInclude.ResumeLayout(false);
            this.tabPageInclude.PerformLayout();
            this.splitContainerInclude.Panel1.ResumeLayout(false);
            this.splitContainerInclude.Panel1.PerformLayout();
            this.splitContainerInclude.Panel2.ResumeLayout(false);
            this.splitContainerInclude.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInclude)).EndInit();
            this.splitContainerInclude.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInclude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIncludeChild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).EndInit();
            this.splitContainerLogMain.Panel1.ResumeLayout(false);
            this.splitContainerLogMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogMain)).EndInit();
            this.splitContainerLogMain.ResumeLayout(false);
            this.tabPageJson.ResumeLayout(false);
            this.tabPageJson.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage1;
        private System.Windows.Forms.SplitContainer splitContainerTreeMain;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.TreeView treeViewProject;
        private System.Windows.Forms.TabControl tabControlProject;
        private System.Windows.Forms.TabPage tabPageSource;
        private System.Windows.Forms.TabPage tabPageGroup;
        private System.Windows.Forms.TabPage tabPageTag;
        private System.Windows.Forms.ToolStripMenuItem вилToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage3;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.CheckBox checkBoxGroupSource;
        private System.Windows.Forms.CheckBox checkBoxTagSG;
        private System.Windows.Forms.Button buttonSourceFilter;
        private System.Windows.Forms.Button buttonGroupFilter;
        private System.Windows.Forms.Button buttonTagFilter;
        private System.Windows.Forms.TabPage tabPageInclude;
        private System.Windows.Forms.DataGridView dataGridViewIncludeChild;
        private System.Windows.Forms.DataGridView dataGridViewInclude;
        private System.Windows.Forms.SplitContainer splitContainerInclude;
        private System.Windows.Forms.Button buttonIncludeLeft;
        private System.Windows.Forms.Button buttonIncludeRight;
        private System.Windows.Forms.TextBox textBoxIncludeFilter;
        private System.Windows.Forms.TextBox textBoxIncludeChildFilter;
        private System.Windows.Forms.Button buttonIncludeFilter;
        private System.Windows.Forms.Button buttonIncludeChildFilter;
        private System.Windows.Forms.ComboBox comboBoxIncludeChildFilterParent;
        private System.Windows.Forms.TabPage tabPageStructure;
        private System.Windows.Forms.SplitContainer splitContainerStructure;
        private System.Windows.Forms.DataGridView dataGridViewStructure;
        private System.Windows.Forms.DataGridView dataGridViewStructureTarget;
        private System.Windows.Forms.Button buttonStructureFilter;
        private System.Windows.Forms.TextBox textBoxStructureFilter;
        private System.Windows.Forms.ComboBox comboBoxStructureTargetFilterParent;
        private System.Windows.Forms.Button buttonStructureLeft;
        private System.Windows.Forms.Button buttonStructureRight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn changeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn changePrefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn changeFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn changeTo;
        private System.Windows.Forms.ToolStripMenuItem testTagSourceToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewLog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeProj;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.SplitContainer splitContainerLogMain;
        private System.Windows.Forms.ToolStripMenuItem Log2ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem tabFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLastFiles;
        private System.Windows.Forms.CheckBox checkBoxTagSave;
        private System.Windows.Forms.CheckBox checkBoxTagAddress;
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
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDesign;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dataGridViewStructureTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureID;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn structureOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureConnector;
        private System.Windows.Forms.DataGridViewComboBoxColumn structureDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureTemplate;
        private System.Windows.Forms.DataGridViewTextBoxColumn structureGroup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainerStructTagTarget;
        private System.Windows.Forms.CheckBox checkBoxStructCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetID;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetStructure;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetTagID;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetTagStruct;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetTagTitle;
        private System.Windows.Forms.Button buttonStructHelp;
        private System.Windows.Forms.Button buttonStructResult;
        private System.Windows.Forms.Button buttonStructDel;
        private System.Windows.Forms.Button buttonStructCopy;
        private System.Windows.Forms.CheckBox checkBoxIncludePrefix;
        private System.Windows.Forms.Button buttonIncludeHelp;
        private System.Windows.Forms.Button buttonIncludeResult;
        private System.Windows.Forms.Button buttonIncludeDel;
        private System.Windows.Forms.Button buttonIncludeCopy;
        private System.Windows.Forms.DataGridViewTextBoxColumn includeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn includePrefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn includeFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sourceCalc;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sourceON;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sourceAutomation;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sourceAutoRestart;
        private System.Windows.Forms.DataGridViewComboBoxColumn sourceDriver;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceStatistic;
        private System.Windows.Forms.DataGridViewTextBoxColumn logDT;
        private System.Windows.Forms.DataGridViewTextBoxColumn logCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn logType;
        private System.Windows.Forms.DataGridViewTextBoxColumn logCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn logText;
        private System.Windows.Forms.TabPage tabPageJson;
        private System.Windows.Forms.RichTextBox richTextBoxJsonProject;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonLong;
        private System.Windows.Forms.ToolStripButton toolStripButtonShort;
        private System.Windows.Forms.ToolStripButton toolStripButtonGroup;
        private System.Windows.Forms.ToolStripButton toolStripButtonSource;
        private System.Windows.Forms.ToolStripButton toolStripButtonInBlock;
        private System.Windows.Forms.ToolStripButton toolStripButtonOutBlock;
        private System.Windows.Forms.ToolStripButton toolStripButtonNormalize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel jsonProjectStatistic;
    }
}

