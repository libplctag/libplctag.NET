namespace Tag_Browser
{
    partial class BrowseTagsDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseTagsDialog));
            this.backgroundWorkerRefreshData = new System.ComponentModel.BackgroundWorker();
            this.treeViewTags = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.textBoxCurrentSelection = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxTagNameFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxDataTypeFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.showDatatypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAddToSelection = new System.Windows.Forms.ToolStripSplitButton();
            this.addAllBelowCurrentNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonRemoveSelection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOK = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listViewSelection = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripButtonViewValues = new System.Windows.Forms.ToolStripButton();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorkerRefreshData
            // 
            this.backgroundWorkerRefreshData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerRefreshData_DoWork);
            this.backgroundWorkerRefreshData.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerRefreshData_RunWorkerCompleted);
            // 
            // treeViewTags
            // 
            this.treeViewTags.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.treeViewTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTags.ImageIndex = 0;
            this.treeViewTags.ImageList = this.imageList1;
            this.treeViewTags.Location = new System.Drawing.Point(0, 0);
            this.treeViewTags.Name = "treeViewTags";
            this.treeViewTags.PathSeparator = ".";
            this.treeViewTags.SelectedImageIndex = 0;
            this.treeViewTags.Size = new System.Drawing.Size(389, 405);
            this.treeViewTags.TabIndex = 1;
            this.treeViewTags.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTags_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Controller");
            this.imageList1.Images.SetKeyName(1, "Program");
            this.imageList1.Images.SetKeyName(2, "Tag");
            this.imageList1.Images.SetKeyName(3, "Field");
            this.imageList1.Images.SetKeyName(4, "Udt");
            // 
            // textBoxCurrentSelection
            // 
            this.textBoxCurrentSelection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxCurrentSelection.Location = new System.Drawing.Point(0, 430);
            this.textBoxCurrentSelection.Name = "textBoxCurrentSelection";
            this.textBoxCurrentSelection.ReadOnly = true;
            this.textBoxCurrentSelection.Size = new System.Drawing.Size(800, 20);
            this.textBoxCurrentSelection.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRefresh,
            this.toolStripButtonSettings,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.toolStripTextBoxTagNameFilter,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.toolStripTextBoxDataTypeFilter,
            this.toolStripSplitButton1,
            this.toolStripSeparator3,
            this.toolStripButtonAddToSelection,
            this.toolStripButtonRemoveSelection,
            this.toolStripButtonViewValues,
            this.toolStripSeparator4,
            this.toolStripButtonOK,
            this.toolStripButtonCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRefresh.Text = "Refresh";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // toolStripButtonSettings
            // 
            this.toolStripButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSettings.Image")));
            this.toolStripButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSettings.Name = "toolStripButtonSettings";
            this.toolStripButtonSettings.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSettings.Text = "Settings";
            this.toolStripButtonSettings.Click += new System.EventHandler(this.toolStripButtonSettings_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(89, 22);
            this.toolStripLabel1.Text = "Tag Name Filter";
            // 
            // toolStripTextBoxTagNameFilter
            // 
            this.toolStripTextBoxTagNameFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxTagNameFilter.Name = "toolStripTextBoxTagNameFilter";
            this.toolStripTextBoxTagNameFilter.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBoxTagNameFilter.TextChanged += new System.EventHandler(this.toolStripTextBoxTagNameFilter_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(87, 22);
            this.toolStripLabel2.Text = "Data Type Filter";
            // 
            // toolStripTextBoxDataTypeFilter
            // 
            this.toolStripTextBoxDataTypeFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxDataTypeFilter.Name = "toolStripTextBoxDataTypeFilter";
            this.toolStripTextBoxDataTypeFilter.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBoxDataTypeFilter.TextChanged += new System.EventHandler(this.toolStripTextBoxDataTypeFilter_TextChanged);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDatatypesToolStripMenuItem});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButton1.Text = "View Datatypes";
            // 
            // showDatatypesToolStripMenuItem
            // 
            this.showDatatypesToolStripMenuItem.Checked = true;
            this.showDatatypesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showDatatypesToolStripMenuItem.Name = "showDatatypesToolStripMenuItem";
            this.showDatatypesToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.showDatatypesToolStripMenuItem.Text = "Show Datatypes";
            this.showDatatypesToolStripMenuItem.Click += new System.EventHandler(this.showDatatypesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonAddToSelection
            // 
            this.toolStripButtonAddToSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddToSelection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAllBelowCurrentNodeToolStripMenuItem});
            this.toolStripButtonAddToSelection.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddToSelection.Image")));
            this.toolStripButtonAddToSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddToSelection.Name = "toolStripButtonAddToSelection";
            this.toolStripButtonAddToSelection.Size = new System.Drawing.Size(32, 22);
            this.toolStripButtonAddToSelection.Text = "Add to Selection";
            this.toolStripButtonAddToSelection.ButtonClick += new System.EventHandler(this.toolStripButtonAddToSelection_ButtonClick);
            // 
            // addAllBelowCurrentNodeToolStripMenuItem
            // 
            this.addAllBelowCurrentNodeToolStripMenuItem.Name = "addAllBelowCurrentNodeToolStripMenuItem";
            this.addAllBelowCurrentNodeToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.addAllBelowCurrentNodeToolStripMenuItem.Text = "Add All below current node";
            this.addAllBelowCurrentNodeToolStripMenuItem.Click += new System.EventHandler(this.addAllBelowCurrentNodeToolStripMenuItem_Click);
            // 
            // toolStripButtonRemoveSelection
            // 
            this.toolStripButtonRemoveSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveSelection.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRemoveSelection.Image")));
            this.toolStripButtonRemoveSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveSelection.Name = "toolStripButtonRemoveSelection";
            this.toolStripButtonRemoveSelection.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemoveSelection.Text = "Remove From Selection";
            this.toolStripButtonRemoveSelection.Click += new System.EventHandler(this.toolStripButtonRemoveSelection_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonOK
            // 
            this.toolStripButtonOK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOK.Image")));
            this.toolStripButtonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOK.Name = "toolStripButtonOK";
            this.toolStripButtonOK.Size = new System.Drawing.Size(27, 22);
            this.toolStripButtonOK.Text = "OK";
            this.toolStripButtonOK.Click += new System.EventHandler(this.toolStripButtonOK_Click);
            // 
            // toolStripButtonCancel
            // 
            this.toolStripButtonCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCancel.Image")));
            this.toolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCancel.Name = "toolStripButtonCancel";
            this.toolStripButtonCancel.Size = new System.Drawing.Size(47, 22);
            this.toolStripButtonCancel.Text = "Cancel";
            this.toolStripButtonCancel.Click += new System.EventHandler(this.toolStripButtonCancel_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewTags);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewSelection);
            this.splitContainer1.Size = new System.Drawing.Size(800, 405);
            this.splitContainer1.SplitterDistance = 389;
            this.splitContainer1.TabIndex = 4;
            // 
            // listViewSelection
            // 
            this.listViewSelection.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewSelection.HideSelection = false;
            this.listViewSelection.Location = new System.Drawing.Point(0, 0);
            this.listViewSelection.Name = "listViewSelection";
            this.listViewSelection.Size = new System.Drawing.Size(407, 405);
            this.listViewSelection.SmallImageList = this.imageList1;
            this.listViewSelection.TabIndex = 0;
            this.listViewSelection.UseCompatibleStateImageBehavior = false;
            this.listViewSelection.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tag Name";
            this.columnHeader1.Width = 288;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Data Type";
            this.columnHeader2.Width = 111;
            // 
            // toolStripButtonViewValues
            // 
            this.toolStripButtonViewValues.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonViewValues.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonViewValues.Image")));
            this.toolStripButtonViewValues.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonViewValues.Name = "toolStripButtonViewValues";
            this.toolStripButtonViewValues.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonViewValues.Text = "View Values of Selected Variables";
            this.toolStripButtonViewValues.Click += new System.EventHandler(this.toolStripButtonViewValues_Click);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            // 
            // BrowseTagsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.textBoxCurrentSelection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BrowseTagsDialog";
            this.Text = "Browse Tags";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorkerRefreshData;
        private System.Windows.Forms.TreeView treeViewTags;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox textBoxCurrentSelection;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxTagNameFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxDataTypeFilter;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettings;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveSelection;
        private System.Windows.Forms.ListView listViewSelection;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripSplitButton toolStripButtonAddToSelection;
        private System.Windows.Forms.ToolStripMenuItem addAllBelowCurrentNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonOK;
        private System.Windows.Forms.ToolStripButton toolStripButtonCancel;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem showDatatypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonViewValues;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

