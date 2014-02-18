namespace WinHtmlEditorTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.htmlEditor1 = new WinHtmlEditor.HtmlEditor();
            this.htmlEditor2 = new WinHtmlEditor.HtmlEditor();
            this.SuspendLayout();
            // 
            // htmlEditor1
            // 
            this.htmlEditor1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.htmlEditor1.BodyInnerHTML = null;
            this.htmlEditor1.BodyInnerText = null;
            this.htmlEditor1.EnterToBR = false;
            this.htmlEditor1.FontSize = WinHtmlEditor.FontSize.Three;
            this.htmlEditor1.Location = new System.Drawing.Point(0, 0);
            this.htmlEditor1.Name = "htmlEditor1";
            this.htmlEditor1.ShowStatusBar = true;
            this.htmlEditor1.ShowToolBar = true;
            this.htmlEditor1.ShowWb = true;
            this.htmlEditor1.Size = new System.Drawing.Size(1020, 271);
            this.htmlEditor1.TabIndex = 0;
            this.htmlEditor1.WebBrowserShortcutsEnabled = true;
            // 
            // htmlEditor2
            // 
            this.htmlEditor2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.htmlEditor2.BodyInnerHTML = null;
            this.htmlEditor2.BodyInnerText = null;
            this.htmlEditor2.EnterToBR = false;
            this.htmlEditor2.FontSize = WinHtmlEditor.FontSize.Three;
            this.htmlEditor2.Location = new System.Drawing.Point(0, 277);
            this.htmlEditor2.Name = "htmlEditor2";
            this.htmlEditor2.ShowStatusBar = true;
            this.htmlEditor2.ShowToolBar = true;
            this.htmlEditor2.ShowWb = true;
            this.htmlEditor2.Size = new System.Drawing.Size(1020, 304);
            this.htmlEditor2.TabIndex = 1;
            this.htmlEditor2.WebBrowserShortcutsEnabled = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1019, 600);
            this.Controls.Add(this.htmlEditor2);
            this.Controls.Add(this.htmlEditor1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private WinHtmlEditor.HtmlEditor htmlEditor1;
        private WinHtmlEditor.HtmlEditor htmlEditor2;






    }
}

