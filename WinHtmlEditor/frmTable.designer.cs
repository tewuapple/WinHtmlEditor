namespace WinHtmlEditor
{
    partial class frmTable
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UpDownNumberOfCols = new System.Windows.Forms.NumericUpDown();
            this.UpDownNumberOfRows = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.UpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.radioBtnWidthInPercentage = new System.Windows.Forms.RadioButton();
            this.radioBtnWidthInPixels = new System.Windows.Forms.RadioButton();
            this.chkSpecifyWidth = new System.Windows.Forms.CheckBox();
            this.UpDownCellSpacing = new System.Windows.Forms.NumericUpDown();
            this.UpDownCellPadding = new System.Windows.Forms.NumericUpDown();
            this.UpDownBorderSize = new System.Windows.Forms.NumericUpDown();
            this.comboAlignment = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumberOfCols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumberOfRows)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownCellSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownCellPadding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownBorderSize)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UpDownNumberOfCols);
            this.groupBox1.Controls.Add(this.UpDownNumberOfRows);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size:";
            // 
            // UpDownNumberOfCols
            // 
            this.UpDownNumberOfCols.Location = new System.Drawing.Point(230, 15);
            this.UpDownNumberOfCols.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownNumberOfCols.Name = "UpDownNumberOfCols";
            this.UpDownNumberOfCols.Size = new System.Drawing.Size(69, 21);
            this.UpDownNumberOfCols.TabIndex = 3;
            this.UpDownNumberOfCols.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // UpDownNumberOfRows
            // 
            this.UpDownNumberOfRows.Location = new System.Drawing.Point(64, 15);
            this.UpDownNumberOfRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownNumberOfRows.Name = "UpDownNumberOfRows";
            this.UpDownNumberOfRows.Size = new System.Drawing.Size(60, 21);
            this.UpDownNumberOfRows.TabIndex = 1;
            this.UpDownNumberOfRows.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Columns:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Rows:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.UpDownCellSpacing);
            this.groupBox2.Controls.Add(this.UpDownCellPadding);
            this.groupBox2.Controls.Add(this.UpDownBorderSize);
            this.groupBox2.Controls.Add(this.comboAlignment);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(365, 131);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layout:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.UpDownWidth);
            this.panel1.Controls.Add(this.radioBtnWidthInPercentage);
            this.panel1.Controls.Add(this.radioBtnWidthInPixels);
            this.panel1.Controls.Add(this.chkSpecifyWidth);
            this.panel1.Location = new System.Drawing.Point(195, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(163, 95);
            this.panel1.TabIndex = 8;
            // 
            // UpDownWidth
            // 
            this.UpDownWidth.Location = new System.Drawing.Point(13, 25);
            this.UpDownWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownWidth.Name = "UpDownWidth";
            this.UpDownWidth.Size = new System.Drawing.Size(48, 21);
            this.UpDownWidth.TabIndex = 3;
            this.UpDownWidth.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // radioBtnWidthInPercentage
            // 
            this.radioBtnWidthInPercentage.AutoSize = true;
            this.radioBtnWidthInPercentage.Checked = true;
            this.radioBtnWidthInPercentage.Location = new System.Drawing.Point(67, 50);
            this.radioBtnWidthInPercentage.Name = "radioBtnWidthInPercentage";
            this.radioBtnWidthInPercentage.Size = new System.Drawing.Size(83, 16);
            this.radioBtnWidthInPercentage.TabIndex = 3;
            this.radioBtnWidthInPercentage.TabStop = true;
            this.radioBtnWidthInPercentage.Text = "In Percent";
            this.radioBtnWidthInPercentage.UseVisualStyleBackColor = true;
            // 
            // radioBtnWidthInPixels
            // 
            this.radioBtnWidthInPixels.AutoSize = true;
            this.radioBtnWidthInPixels.Location = new System.Drawing.Point(67, 25);
            this.radioBtnWidthInPixels.Name = "radioBtnWidthInPixels";
            this.radioBtnWidthInPixels.Size = new System.Drawing.Size(77, 16);
            this.radioBtnWidthInPixels.TabIndex = 2;
            this.radioBtnWidthInPixels.Text = "In Pixels";
            this.radioBtnWidthInPixels.UseVisualStyleBackColor = true;
            // 
            // chkSpecifyWidth
            // 
            this.chkSpecifyWidth.AutoSize = true;
            this.chkSpecifyWidth.Location = new System.Drawing.Point(13, 2);
            this.chkSpecifyWidth.Name = "chkSpecifyWidth";
            this.chkSpecifyWidth.Size = new System.Drawing.Size(102, 16);
            this.chkSpecifyWidth.TabIndex = 0;
            this.chkSpecifyWidth.Text = "Specify Width";
            this.chkSpecifyWidth.UseVisualStyleBackColor = true;
            // 
            // UpDownCellSpacing
            // 
            this.UpDownCellSpacing.Location = new System.Drawing.Point(110, 95);
            this.UpDownCellSpacing.Name = "UpDownCellSpacing";
            this.UpDownCellSpacing.Size = new System.Drawing.Size(78, 21);
            this.UpDownCellSpacing.TabIndex = 7;
            // 
            // UpDownCellPadding
            // 
            this.UpDownCellPadding.Location = new System.Drawing.Point(110, 71);
            this.UpDownCellPadding.Name = "UpDownCellPadding";
            this.UpDownCellPadding.Size = new System.Drawing.Size(79, 21);
            this.UpDownCellPadding.TabIndex = 5;
            // 
            // UpDownBorderSize
            // 
            this.UpDownBorderSize.Location = new System.Drawing.Point(110, 46);
            this.UpDownBorderSize.Name = "UpDownBorderSize";
            this.UpDownBorderSize.Size = new System.Drawing.Size(79, 21);
            this.UpDownBorderSize.TabIndex = 3;
            this.UpDownBorderSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboAlignment
            // 
            this.comboAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAlignment.FormattingEnabled = true;
            this.comboAlignment.Items.AddRange(new object[] {
            "Default",
            "left",
            "center",
            "right"});
            this.comboAlignment.Location = new System.Drawing.Point(110, 21);
            this.comboAlignment.Name = "comboAlignment";
            this.comboAlignment.Size = new System.Drawing.Size(79, 20);
            this.comboAlignment.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "Cell Spacing:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "Cell Padding:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "Border Size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Alignment:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(77, 209);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 27);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(194, 209);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 27);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmTable
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(382, 247);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("ËÎÌו", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Table";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTable_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumberOfCols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumberOfRows)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownCellSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownCellPadding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownBorderSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown UpDownNumberOfCols;
        private System.Windows.Forms.NumericUpDown UpDownNumberOfRows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboAlignment;
        private System.Windows.Forms.NumericUpDown UpDownCellSpacing;
        private System.Windows.Forms.NumericUpDown UpDownCellPadding;
        private System.Windows.Forms.NumericUpDown UpDownBorderSize;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkSpecifyWidth;
        private System.Windows.Forms.RadioButton radioBtnWidthInPercentage;
        private System.Windows.Forms.RadioButton radioBtnWidthInPixels;
        private System.Windows.Forms.NumericUpDown UpDownWidth;
    }
}