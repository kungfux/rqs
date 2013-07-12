namespace RQS.GUI
{
    partial class Search
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboSearchBy = new System.Windows.Forms.ComboBox();
            this.bSearch = new System.Windows.Forms.Button();
            this.comboSearchText = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextCopyCell = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCopyRows = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextOpenSourceFile = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextCustomize = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.removeSelectedLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(803, 414);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(797, 64);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Search criteria ";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.checkBox1, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboSearchBy, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.bSearch, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboSearchText, 2, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(791, 44);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search by";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox1.Location = new System.Drawing.Point(644, 10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(144, 23);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Limit search results";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboSearchBy
            // 
            this.comboSearchBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboSearchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSearchBy.FormattingEnabled = true;
            this.comboSearchBy.Items.AddRange(new object[] {
            "FR ID",
            "FR TMS Task",
            "FR Text"});
            this.comboSearchBy.Location = new System.Drawing.Point(73, 10);
            this.comboSearchBy.Name = "comboSearchBy";
            this.comboSearchBy.Size = new System.Drawing.Size(144, 21);
            this.comboSearchBy.TabIndex = 4;
            // 
            // bSearch
            // 
            this.bSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSearch.Location = new System.Drawing.Point(543, 10);
            this.bSearch.Name = "bSearch";
            this.bSearch.Size = new System.Drawing.Size(95, 23);
            this.bSearch.TabIndex = 2;
            this.bSearch.Text = "Search";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // comboSearchText
            // 
            this.comboSearchText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboSearchText.FormattingEnabled = true;
            this.comboSearchText.Location = new System.Drawing.Point(223, 10);
            this.comboSearchText.Name = "comboSearchText";
            this.comboSearchText.Size = new System.Drawing.Size(314, 21);
            this.comboSearchText.TabIndex = 1;
            this.comboSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboSearchBy_KeyDown);
            this.comboSearchText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboSearchText_KeyPress);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextCopyCell,
            this.contextCopyRows,
            this.toolStripSeparator1,
            this.contextOpenSourceFile,
            this.toolStripSeparator3,
            this.removeDuplicatesToolStripMenuItem,
            this.removeSelectedLinesToolStripMenuItem,
            this.toolStripSeparator2,
            this.contextCustomize});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(191, 176);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // contextCopyCell
            // 
            this.contextCopyCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.contextCopyCell.Name = "contextCopyCell";
            this.contextCopyCell.Size = new System.Drawing.Size(190, 22);
            this.contextCopyCell.Text = "Copy cell";
            this.contextCopyCell.Click += new System.EventHandler(this.contextCopyCell_Click);
            // 
            // contextCopyRows
            // 
            this.contextCopyRows.Name = "contextCopyRows";
            this.contextCopyRows.Size = new System.Drawing.Size(190, 22);
            this.contextCopyRows.Text = "Copy row(s)";
            this.contextCopyRows.Click += new System.EventHandler(this.contextCopyRows_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // contextOpenSourceFile
            // 
            this.contextOpenSourceFile.Name = "contextOpenSourceFile";
            this.contextOpenSourceFile.Size = new System.Drawing.Size(190, 22);
            this.contextOpenSourceFile.Text = "Open source file";
            this.contextOpenSourceFile.Click += new System.EventHandler(this.contextOpenSourceFile_Click);
            // 
            // removeDuplicatesToolStripMenuItem
            // 
            this.removeDuplicatesToolStripMenuItem.Name = "removeDuplicatesToolStripMenuItem";
            this.removeDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.removeDuplicatesToolStripMenuItem.Text = "Remove duplications";
            this.removeDuplicatesToolStripMenuItem.Click += new System.EventHandler(this.removeDuplicatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(187, 6);
            // 
            // contextCustomize
            // 
            this.contextCustomize.Name = "contextCustomize";
            this.contextCustomize.Size = new System.Drawing.Size(190, 22);
            this.contextCustomize.Text = "Customize...";
            this.contextCustomize.Click += new System.EventHandler(this.contextCustomize_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(187, 6);
            // 
            // removeSelectedLinesToolStripMenuItem
            // 
            this.removeSelectedLinesToolStripMenuItem.Name = "removeSelectedLinesToolStripMenuItem";
            this.removeSelectedLinesToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.removeSelectedLinesToolStripMenuItem.Text = "Remove selected lines";
            this.removeSelectedLinesToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedLinesToolStripMenuItem_Click);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "Search";
            this.Size = new System.Drawing.Size(803, 414);
            this.Tag = " Search ";
            this.Load += new System.EventHandler(this.Search_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboSearchBy;
        private System.Windows.Forms.Button bSearch;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem contextCopyCell;
        private System.Windows.Forms.ToolStripMenuItem contextCopyRows;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem contextCustomize;
        private System.Windows.Forms.ToolStripMenuItem contextOpenSourceFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox comboSearchText;
        private System.Windows.Forms.ToolStripMenuItem removeDuplicatesToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedLinesToolStripMenuItem;
    }
}
