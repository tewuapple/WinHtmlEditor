namespace WinHtmlEditor
{
    partial class FindReplaceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindReplaceForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabFind = new System.Windows.Forms.TabPage();
            this.tabReplace = new System.Windows.Forms.TabPage();
            this.labelFind = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.textFind = new System.Windows.Forms.TextBox();
            this.bFindNext = new System.Windows.Forms.Button();
            this.labelReplace = new System.Windows.Forms.Label();
            this.textReplace = new System.Windows.Forms.TextBox();
            this.bReplaceAll = new System.Windows.Forms.Button();
            this.bReplace = new System.Windows.Forms.Button();
            this.bOptions = new System.Windows.Forms.Button();
            this.optionMatchCase = new System.Windows.Forms.CheckBox();
            this.optionMatchWhole = new System.Windows.Forms.CheckBox();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.panelInput = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.panelOptions.SuspendLayout();
            this.panelInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabFind);
            this.tabControl.Controls.Add(this.tabReplace);
            this.tabControl.Location = new System.Drawing.Point(8, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowToolTips = true;
            this.tabControl.Size = new System.Drawing.Size(440, 32);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabStop = false;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabFind
            // 
            this.tabFind.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabFind.Location = new System.Drawing.Point(4, 22);
            this.tabFind.Name = "tabFind";
            this.tabFind.Size = new System.Drawing.Size(432, 6);
            this.tabFind.TabIndex = 0;
            this.tabFind.Text = "Find";
            this.tabFind.ToolTipText = "Find Text";
            // 
            // tabReplace
            // 
            this.tabReplace.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabReplace.Location = new System.Drawing.Point(4, 22);
            this.tabReplace.Name = "tabReplace";
            this.tabReplace.Size = new System.Drawing.Size(432, 6);
            this.tabReplace.TabIndex = 1;
            this.tabReplace.Text = "Replace";
            this.tabReplace.ToolTipText = "Find and Replace Text";
            // 
            // labelFind
            // 
            this.labelFind.Location = new System.Drawing.Point(8, 16);
            this.labelFind.Name = "labelFind";
            this.labelFind.Size = new System.Drawing.Size(96, 23);
            this.labelFind.TabIndex = 0;
            this.labelFind.Text = "Find What:";
            // 
            // bCancel
            // 
            this.bCancel.BackColor = System.Drawing.SystemColors.Control;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(344, 80);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = false;
            // 
            // textFind
            // 
            this.textFind.Location = new System.Drawing.Point(112, 16);
            this.textFind.Name = "textFind";
            this.textFind.Size = new System.Drawing.Size(296, 20);
            this.textFind.TabIndex = 1;
            this.textFind.TextChanged += new System.EventHandler(this.textFind_TextChanged);
            // 
            // bFindNext
            // 
            this.bFindNext.BackColor = System.Drawing.SystemColors.Control;
            this.bFindNext.Location = new System.Drawing.Point(264, 80);
            this.bFindNext.Name = "bFindNext";
            this.bFindNext.Size = new System.Drawing.Size(75, 23);
            this.bFindNext.TabIndex = 3;
            this.bFindNext.Text = "Find Next";
            this.bFindNext.UseVisualStyleBackColor = false;
            this.bFindNext.Click += new System.EventHandler(this.bFindNext_Click);
            // 
            // labelReplace
            // 
            this.labelReplace.Location = new System.Drawing.Point(8, 48);
            this.labelReplace.Name = "labelReplace";
            this.labelReplace.Size = new System.Drawing.Size(96, 23);
            this.labelReplace.TabIndex = 0;
            this.labelReplace.Text = "Replace  With:";
            // 
            // textReplace
            // 
            this.textReplace.Location = new System.Drawing.Point(112, 48);
            this.textReplace.Name = "textReplace";
            this.textReplace.Size = new System.Drawing.Size(296, 20);
            this.textReplace.TabIndex = 2;
            this.textReplace.TextChanged += new System.EventHandler(this.textReplace_TextChanged);
            // 
            // bReplaceAll
            // 
            this.bReplaceAll.BackColor = System.Drawing.SystemColors.Control;
            this.bReplaceAll.Location = new System.Drawing.Point(176, 80);
            this.bReplaceAll.Name = "bReplaceAll";
            this.bReplaceAll.Size = new System.Drawing.Size(75, 23);
            this.bReplaceAll.TabIndex = 7;
            this.bReplaceAll.Text = "Replace All";
            this.bReplaceAll.UseVisualStyleBackColor = false;
            this.bReplaceAll.Click += new System.EventHandler(this.bReplaceAll_Click);
            // 
            // bReplace
            // 
            this.bReplace.BackColor = System.Drawing.SystemColors.Control;
            this.bReplace.Location = new System.Drawing.Point(96, 80);
            this.bReplace.Name = "bReplace";
            this.bReplace.Size = new System.Drawing.Size(75, 23);
            this.bReplace.TabIndex = 6;
            this.bReplace.Text = "Replace";
            this.bReplace.UseVisualStyleBackColor = false;
            this.bReplace.Click += new System.EventHandler(this.bReplace_Click);
            // 
            // bOptions
            // 
            this.bOptions.BackColor = System.Drawing.SystemColors.Control;
            this.bOptions.Location = new System.Drawing.Point(8, 80);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(80, 23);
            this.bOptions.TabIndex = 5;
            this.bOptions.Text = "Options";
            this.bOptions.UseVisualStyleBackColor = false;
            this.bOptions.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // optionMatchCase
            // 
            this.optionMatchCase.Location = new System.Drawing.Point(8, 8);
            this.optionMatchCase.Name = "optionMatchCase";
            this.optionMatchCase.Size = new System.Drawing.Size(240, 24);
            this.optionMatchCase.TabIndex = 8;
            this.optionMatchCase.Text = "Match Exact Case";
            // 
            // optionMatchWhole
            // 
            this.optionMatchWhole.Location = new System.Drawing.Point(8, 32);
            this.optionMatchWhole.Name = "optionMatchWhole";
            this.optionMatchWhole.Size = new System.Drawing.Size(240, 24);
            this.optionMatchWhole.TabIndex = 9;
            this.optionMatchWhole.Text = "Match Whole Word Only";
            // 
            // panelOptions
            // 
            this.panelOptions.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelOptions.Controls.Add(this.optionMatchCase);
            this.panelOptions.Controls.Add(this.optionMatchWhole);
            this.panelOptions.Location = new System.Drawing.Point(16, 152);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(424, 64);
            this.panelOptions.TabIndex = 8;
            // 
            // panelInput
            // 
            this.panelInput.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelInput.Controls.Add(this.labelFind);
            this.panelInput.Controls.Add(this.textFind);
            this.panelInput.Controls.Add(this.labelReplace);
            this.panelInput.Controls.Add(this.textReplace);
            this.panelInput.Controls.Add(this.bOptions);
            this.panelInput.Controls.Add(this.bReplace);
            this.panelInput.Controls.Add(this.bReplaceAll);
            this.panelInput.Controls.Add(this.bFindNext);
            this.panelInput.Controls.Add(this.bCancel);
            this.panelInput.Location = new System.Drawing.Point(16, 40);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(424, 112);
            this.panelInput.TabIndex = 9;
            // 
            // FindReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(458, 224);
            this.Controls.Add(this.panelOptions);
            this.Controls.Add(this.panelInput);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindReplaceForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Find and Replace";
            this.tabControl.ResumeLayout(false);
            this.panelOptions.ResumeLayout(false);
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TabPage tabFind;
        private System.Windows.Forms.TabPage tabReplace;
        private System.Windows.Forms.Label labelFind;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TextBox textFind;
        private System.Windows.Forms.Button bFindNext;
        private System.Windows.Forms.Label labelReplace;
        private System.Windows.Forms.Button bReplaceAll;
        private System.Windows.Forms.Button bReplace;
        private System.Windows.Forms.Button bOptions;
        private System.Windows.Forms.CheckBox optionMatchCase;
        private System.Windows.Forms.CheckBox optionMatchWhole;
        private System.Windows.Forms.TextBox textReplace;
        private System.Windows.Forms.Panel panelOptions;
        private System.Windows.Forms.Panel panelInput;
    }
}

