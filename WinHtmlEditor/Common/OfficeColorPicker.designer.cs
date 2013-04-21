namespace WinHtmlEditor
{
    partial class OfficeColorPicker
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
            this.colorToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // colorToolTip
            // 
            this.colorToolTip.AutoPopDelay = 5000;
            this.colorToolTip.InitialDelay = 500;
            this.colorToolTip.ReshowDelay = 1000;
            // 
            // OfficeColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "OfficeColorPicker";
            this.Size = new System.Drawing.Size(146, 120);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip colorToolTip;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}
