namespace RQS.GUI
{
    partial class Setup
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
            this.lLocation = new System.Windows.Forms.Label();
            this.linkLocation = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nLimit = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbColor2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbColor1 = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cSecondCopy = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cAutofilter = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nLimit)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // lLocation
            // 
            this.lLocation.Location = new System.Drawing.Point(6, 20);
            this.lLocation.Name = "lLocation";
            this.lLocation.Size = new System.Drawing.Size(388, 36);
            this.lLocation.TabIndex = 0;
            this.lLocation.Text = "Current location:";
            // 
            // linkLocation
            // 
            this.linkLocation.AutoSize = true;
            this.linkLocation.Location = new System.Drawing.Point(6, 58);
            this.linkLocation.Name = "linkLocation";
            this.linkLocation.Size = new System.Drawing.Size(84, 13);
            this.linkLocation.TabIndex = 1;
            this.linkLocation.TabStop = true;
            this.linkLocation.Text = "Change location";
            this.linkLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLocation_LinkClicked);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Controls.Add(this.groupBox3);
            this.flowLayoutPanel1.Controls.Add(this.groupBox4);
            this.flowLayoutPanel1.Controls.Add(this.groupBox5);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(824, 469);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Wheat;
            this.groupBox1.Controls.Add(this.lLocation);
            this.groupBox1.Controls.Add(this.linkLocation);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 77);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Location of .xls files ";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.PaleGreen;
            this.groupBox2.Controls.Add(this.nLimit);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(409, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 77);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Results limit ";
            // 
            // nLimit
            // 
            this.nLimit.Location = new System.Drawing.Point(9, 50);
            this.nLimit.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nLimit.Name = "nLimit";
            this.nLimit.Size = new System.Drawing.Size(120, 21);
            this.nLimit.TabIndex = 1;
            this.nLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nLimit.ValueChanged += new System.EventHandler(this.nLimit_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Limit search results by:";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.PowderBlue;
            this.groupBox3.Controls.Add(this.cmbColor2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cmbColor1);
            this.groupBox3.Location = new System.Drawing.Point(615, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 77);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Table colors ";
            // 
            // cmbColor2
            // 
            this.cmbColor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbColor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColor2.FormattingEnabled = true;
            this.cmbColor2.Location = new System.Drawing.Point(73, 47);
            this.cmbColor2.Name = "cmbColor2";
            this.cmbColor2.Size = new System.Drawing.Size(121, 22);
            this.cmbColor2.TabIndex = 7;
            this.cmbColor2.SelectedIndexChanged += new System.EventHandler(this.cmbColor2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Odd lines";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Even lines";
            // 
            // cmbColor1
            // 
            this.cmbColor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbColor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColor1.FormattingEnabled = true;
            this.cmbColor1.Location = new System.Drawing.Point(73, 19);
            this.cmbColor1.Name = "cmbColor1";
            this.cmbColor1.Size = new System.Drawing.Size(121, 22);
            this.cmbColor1.TabIndex = 6;
            this.cmbColor1.SelectedIndexChanged += new System.EventHandler(this.cmbColor1_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Khaki;
            this.groupBox4.Controls.Add(this.cSecondCopy);
            this.groupBox4.Location = new System.Drawing.Point(3, 86);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 77);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = " Second copy of RQS ";
            // 
            // cSecondCopy
            // 
            this.cSecondCopy.BackColor = System.Drawing.Color.Transparent;
            this.cSecondCopy.Location = new System.Drawing.Point(13, 20);
            this.cSecondCopy.Name = "cSecondCopy";
            this.cSecondCopy.Size = new System.Drawing.Size(181, 51);
            this.cSecondCopy.TabIndex = 0;
            this.cSecondCopy.Text = "Allow running several copies of RQS at the same time";
            this.cSecondCopy.UseVisualStyleBackColor = false;
            this.cSecondCopy.CheckedChanged += new System.EventHandler(this.cSecondCopy_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Plum;
            this.groupBox5.Controls.Add(this.cAutofilter);
            this.groupBox5.Location = new System.Drawing.Point(209, 86);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 77);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Autofilter ";
            // 
            // cAutofilter
            // 
            this.cAutofilter.BackColor = System.Drawing.Color.Transparent;
            this.cAutofilter.Location = new System.Drawing.Point(13, 20);
            this.cAutofilter.Name = "cAutofilter";
            this.cAutofilter.Size = new System.Drawing.Size(181, 51);
            this.cAutofilter.TabIndex = 0;
            this.cAutofilter.Text = "Enable autofilter in Search Results Grid";
            this.cAutofilter.UseVisualStyleBackColor = false;
            this.cAutofilter.CheckedChanged += new System.EventHandler(this.cAutofilter_CheckedChanged);
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "Setup";
            this.Size = new System.Drawing.Size(824, 469);
            this.Tag = " Setup ";
            this.Load += new System.EventHandler(this.Setup_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nLimit)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lLocation;
        private System.Windows.Forms.LinkLabel linkLocation;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbColor2;
        private System.Windows.Forms.ComboBox cmbColor1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cSecondCopy;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cAutofilter;


    }
}
