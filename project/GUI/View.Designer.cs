namespace RQS.GUI
{
    partial class View
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lFRID = new System.Windows.Forms.Label();
            this.txtFRID = new System.Windows.Forms.TextBox();
            this.lTMSTask = new System.Windows.Forms.Label();
            this.txtTMSTask = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(589, 276);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.txtTMSTask);
            this.groupBox1.Controls.Add(this.lTMSTask);
            this.groupBox1.Controls.Add(this.txtFRID);
            this.groupBox1.Controls.Add(this.lFRID);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(583, 44);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lFRID
            // 
            this.lFRID.AutoSize = true;
            this.lFRID.Location = new System.Drawing.Point(9, 16);
            this.lFRID.Name = "lFRID";
            this.lFRID.Size = new System.Drawing.Size(34, 13);
            this.lFRID.TabIndex = 0;
            this.lFRID.Text = "FR ID";
            // 
            // txtFRID
            // 
            this.txtFRID.Location = new System.Drawing.Point(59, 13);
            this.txtFRID.Name = "txtFRID";
            this.txtFRID.ReadOnly = true;
            this.txtFRID.Size = new System.Drawing.Size(100, 21);
            this.txtFRID.TabIndex = 1;
            // 
            // lTMSTask
            // 
            this.lTMSTask.AutoSize = true;
            this.lTMSTask.Location = new System.Drawing.Point(184, 16);
            this.lTMSTask.Name = "lTMSTask";
            this.lTMSTask.Size = new System.Drawing.Size(52, 13);
            this.lTMSTask.TabIndex = 2;
            this.lTMSTask.Text = "TMS Task";
            // 
            // txtTMSTask
            // 
            this.txtTMSTask.Location = new System.Drawing.Point(253, 13);
            this.txtTMSTask.Name = "txtTMSTask";
            this.txtTMSTask.ReadOnly = true;
            this.txtTMSTask.Size = new System.Drawing.Size(100, 21);
            this.txtTMSTask.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBox1, 2);
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 53);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(583, 220);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 276);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "View";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.View_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFRID;
        private System.Windows.Forms.Label lFRID;
        private System.Windows.Forms.TextBox txtTMSTask;
        private System.Windows.Forms.Label lTMSTask;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}