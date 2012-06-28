namespace WinHtmlEditor
{
    partial class HtmlEditor
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tscMain = new System.Windows.Forms.ToolStripContainer();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.contextMenuWeb = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SelectAllMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.topToolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.FindMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveFormatMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbShowHTML = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.tsbCut = new System.Windows.Forms.ToolStripButton();
            this.tsbPaste = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbFind = new System.Windows.Forms.ToolStripButton();
            this.tsbClearFormat = new System.Windows.Forms.ToolStripButton();
            this.tsbCenter = new System.Windows.Forms.ToolStripButton();
            this.tsbFull = new System.Windows.Forms.ToolStripButton();
            this.tsbLeft = new System.Windows.Forms.ToolStripButton();
            this.tsbRight = new System.Windows.Forms.ToolStripButton();
            this.tsbUnderline = new System.Windows.Forms.ToolStripButton();
            this.tsbItalic = new System.Windows.Forms.ToolStripButton();
            this.tsbBlod = new System.Windows.Forms.ToolStripButton();
            this.tsbBgcolor = new System.Windows.Forms.ToolStripButton();
            this.tsbFontColor = new System.Windows.Forms.ToolStripButton();
            this.tsbSetFont = new System.Windows.Forms.ToolStripButton();
            this.tsbLink = new System.Windows.Forms.ToolStripButton();
            this.tsbUnlink = new System.Windows.Forms.ToolStripButton();
            this.tsbInsertTable = new System.Windows.Forms.ToolStripButton();
            this.tsbImg = new System.Windows.Forms.ToolStripButton();
            this.tsbInsertHorizontalRule = new System.Windows.Forms.ToolStripButton();
            this.tsbOutdent = new System.Windows.Forms.ToolStripButton();
            this.tsbIndent = new System.Windows.Forms.ToolStripButton();
            this.tsbUL = new System.Windows.Forms.ToolStripButton();
            this.tsbOL = new System.Windows.Forms.ToolStripButton();
            this.tsbWordCount = new System.Windows.Forms.ToolStripButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.tscMain.ContentPanel.SuspendLayout();
            this.tscMain.TopToolStripPanel.SuspendLayout();
            this.tscMain.SuspendLayout();
            this.contextMenuWeb.SuspendLayout();
            this.topToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tscMain
            // 
            this.tscMain.BottomToolStripPanelVisible = false;
            // 
            // tscMain.ContentPanel
            // 
            this.tscMain.ContentPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tscMain.ContentPanel.Controls.Add(this.wb);
            this.tscMain.ContentPanel.Size = new System.Drawing.Size(760, 446);
            this.tscMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscMain.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tscMain.LeftToolStripPanelVisible = false;
            this.tscMain.Location = new System.Drawing.Point(0, 0);
            this.tscMain.Name = "tscMain";
            this.tscMain.RightToolStripPanelVisible = false;
            this.tscMain.Size = new System.Drawing.Size(760, 471);
            this.tscMain.TabIndex = 0;
            this.tscMain.Text = "toolStripContainer1";
            // 
            // tscMain.TopToolStripPanel
            // 
            this.tscMain.TopToolStripPanel.Controls.Add(this.topToolBar);
            // 
            // wb
            // 
            this.wb.ContextMenuStrip = this.contextMenuWeb;
            this.wb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb.Location = new System.Drawing.Point(0, 0);
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(756, 442);
            this.wb.TabIndex = 0;
            // 
            // contextMenuWeb
            // 
            this.contextMenuWeb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectAllMenu,
            this.CopyMenu,
            this.CutMenu,
            this.PasteMenu,
            this.DeleteMenu,
            this.FindMenu,
            this.RemoveFormatMenu,
            this.toolStripSeparator5,
            this.SaveToFileMenu});
            this.contextMenuWeb.Name = "contextMenuWeb";
            this.contextMenuWeb.Size = new System.Drawing.Size(125, 186);
            // 
            // SelectAllMenu
            // 
            this.SelectAllMenu.Name = "SelectAllMenu";
            this.SelectAllMenu.Size = new System.Drawing.Size(124, 22);
            this.SelectAllMenu.Text = "全选";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(121, 6);
            // 
            // topToolBar
            // 
            this.topToolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.topToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.topToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbOpen,
            this.tsbSave,
            this.tsbShowHTML,
            this.toolStripSeparator1,
            this.tsbCopy,
            this.tsbCut,
            this.tsbPaste,
            this.tsbDelete,
            this.tsbFind,
            this.tsbClearFormat,
            this.toolStripSeparator2,
            this.tsbCenter,
            this.tsbFull,
            this.tsbLeft,
            this.tsbRight,
            this.tsbUnderline,
            this.tsbItalic,
            this.tsbBlod,
            this.tsbBgcolor,
            this.tsbFontColor,
            this.tsbSetFont,
            this.toolStripSeparator3,
            this.tsbLink,
            this.tsbUnlink,
            this.tsbInsertTable,
            this.tsbImg,
            this.tsbInsertHorizontalRule,
            this.tsbOutdent,
            this.tsbIndent,
            this.tsbUL,
            this.tsbOL,
            this.tsbWordCount,
            this.toolStripSeparator4,
            this.tsbAbout});
            this.topToolBar.Location = new System.Drawing.Point(3, 0);
            this.topToolBar.Name = "topToolBar";
            this.topToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.topToolBar.Size = new System.Drawing.Size(757, 25);
            this.topToolBar.TabIndex = 0;
            this.topToolBar.Text = "topToolBar";
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // CopyMenu
            // 
            this.CopyMenu.Image = global::WinHtmlEditor.Properties.Resources.Copy;
            this.CopyMenu.Name = "CopyMenu";
            this.CopyMenu.Size = new System.Drawing.Size(124, 22);
            this.CopyMenu.Text = "复制";
            // 
            // CutMenu
            // 
            this.CutMenu.Image = global::WinHtmlEditor.Properties.Resources.Cut;
            this.CutMenu.Name = "CutMenu";
            this.CutMenu.Size = new System.Drawing.Size(124, 22);
            this.CutMenu.Text = "剪切";
            // 
            // PasteMenu
            // 
            this.PasteMenu.Image = global::WinHtmlEditor.Properties.Resources.Paste;
            this.PasteMenu.Name = "PasteMenu";
            this.PasteMenu.Size = new System.Drawing.Size(124, 22);
            this.PasteMenu.Text = "粘贴";
            // 
            // DeleteMenu
            // 
            this.DeleteMenu.Image = global::WinHtmlEditor.Properties.Resources.Delete;
            this.DeleteMenu.Name = "DeleteMenu";
            this.DeleteMenu.Size = new System.Drawing.Size(124, 22);
            this.DeleteMenu.Text = "删除";
            // 
            // FindMenu
            // 
            this.FindMenu.Image = global::WinHtmlEditor.Properties.Resources.Find;
            this.FindMenu.Name = "FindMenu";
            this.FindMenu.Size = new System.Drawing.Size(124, 22);
            this.FindMenu.Text = "查找";
            // 
            // RemoveFormatMenu
            // 
            this.RemoveFormatMenu.Image = global::WinHtmlEditor.Properties.Resources.RemoveFormat;
            this.RemoveFormatMenu.Name = "RemoveFormatMenu";
            this.RemoveFormatMenu.Size = new System.Drawing.Size(124, 22);
            this.RemoveFormatMenu.Text = "清除格式";
            // 
            // SaveToFileMenu
            // 
            this.SaveToFileMenu.Image = global::WinHtmlEditor.Properties.Resources.Save;
            this.SaveToFileMenu.Name = "SaveToFileMenu";
            this.SaveToFileMenu.Size = new System.Drawing.Size(124, 22);
            this.SaveToFileMenu.Text = "保存";
            // 
            // tsbNew
            // 
            this.tsbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNew.Image = global::WinHtmlEditor.Properties.Resources.New;
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(23, 22);
            this.tsbNew.Text = "新建";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = global::WinHtmlEditor.Properties.Resources.Open;
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbOpen.Text = "打开文件";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::WinHtmlEditor.Properties.Resources.Save;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "保存为文件";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbShowHTML
            // 
            this.tsbShowHTML.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbShowHTML.Image = global::WinHtmlEditor.Properties.Resources.ShowHTML;
            this.tsbShowHTML.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowHTML.Name = "tsbShowHTML";
            this.tsbShowHTML.Size = new System.Drawing.Size(23, 22);
            this.tsbShowHTML.Text = "查看HTML代码";
            this.tsbShowHTML.Click += new System.EventHandler(this.tsbShowHTML_Click);
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.Image = global::WinHtmlEditor.Properties.Resources.Copy;
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(23, 22);
            this.tsbCopy.Text = "复制";
            this.tsbCopy.Click += new System.EventHandler(this.tsbCopy_Click);
            // 
            // tsbCut
            // 
            this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCut.Image = global::WinHtmlEditor.Properties.Resources.Cut;
            this.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCut.Name = "tsbCut";
            this.tsbCut.Size = new System.Drawing.Size(23, 22);
            this.tsbCut.Text = "剪切";
            this.tsbCut.Click += new System.EventHandler(this.tsbCut_Click);
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.Image = global::WinHtmlEditor.Properties.Resources.Paste;
            this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.Size = new System.Drawing.Size(23, 22);
            this.tsbPaste.Text = "粘贴";
            this.tsbPaste.Click += new System.EventHandler(this.tsbPaste_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete.Image = global::WinHtmlEditor.Properties.Resources.Delete;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(23, 22);
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbFind
            // 
            this.tsbFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFind.Image = global::WinHtmlEditor.Properties.Resources.Find;
            this.tsbFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFind.Name = "tsbFind";
            this.tsbFind.Size = new System.Drawing.Size(23, 22);
            this.tsbFind.Text = "查找";
            this.tsbFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // tsbClearFormat
            // 
            this.tsbClearFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClearFormat.Image = global::WinHtmlEditor.Properties.Resources.RemoveFormat;
            this.tsbClearFormat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClearFormat.Name = "tsbClearFormat";
            this.tsbClearFormat.Size = new System.Drawing.Size(23, 22);
            this.tsbClearFormat.Text = "清除格式";
            this.tsbClearFormat.Click += new System.EventHandler(this.tsbClearFormat_Click);
            // 
            // tsbCenter
            // 
            this.tsbCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCenter.Image = global::WinHtmlEditor.Properties.Resources.JustifyCenter;
            this.tsbCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCenter.Name = "tsbCenter";
            this.tsbCenter.Size = new System.Drawing.Size(23, 22);
            this.tsbCenter.Text = "居中对齐0";
            this.tsbCenter.Click += new System.EventHandler(this.tsbCenter_Click);
            // 
            // tsbFull
            // 
            this.tsbFull.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFull.Image = global::WinHtmlEditor.Properties.Resources.JustifyFull;
            this.tsbFull.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFull.Name = "tsbFull";
            this.tsbFull.Size = new System.Drawing.Size(23, 22);
            this.tsbFull.Text = "两端对齐";
            this.tsbFull.Click += new System.EventHandler(this.tsbFull_Click);
            // 
            // tsbLeft
            // 
            this.tsbLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLeft.Image = global::WinHtmlEditor.Properties.Resources.JustifyLeft;
            this.tsbLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLeft.Name = "tsbLeft";
            this.tsbLeft.Size = new System.Drawing.Size(23, 22);
            this.tsbLeft.Text = "左对齐";
            this.tsbLeft.Click += new System.EventHandler(this.tsbLeft_Click);
            // 
            // tsbRight
            // 
            this.tsbRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRight.Image = global::WinHtmlEditor.Properties.Resources.JustifyRight;
            this.tsbRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRight.Name = "tsbRight";
            this.tsbRight.Size = new System.Drawing.Size(23, 22);
            this.tsbRight.Text = "右对齐";
            this.tsbRight.Click += new System.EventHandler(this.tsbRight_Click);
            // 
            // tsbUnderline
            // 
            this.tsbUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUnderline.Image = global::WinHtmlEditor.Properties.Resources.Underline;
            this.tsbUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUnderline.Name = "tsbUnderline";
            this.tsbUnderline.Size = new System.Drawing.Size(23, 22);
            this.tsbUnderline.Text = "下划线";
            this.tsbUnderline.Click += new System.EventHandler(this.tsbUnderline_Click);
            // 
            // tsbItalic
            // 
            this.tsbItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbItalic.Image = global::WinHtmlEditor.Properties.Resources.Italic;
            this.tsbItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbItalic.Name = "tsbItalic";
            this.tsbItalic.Size = new System.Drawing.Size(23, 22);
            this.tsbItalic.Text = "斜体";
            this.tsbItalic.Click += new System.EventHandler(this.tsbItalic_Click);
            // 
            // tsbBlod
            // 
            this.tsbBlod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbBlod.Image = global::WinHtmlEditor.Properties.Resources.Bold;
            this.tsbBlod.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBlod.Name = "tsbBlod";
            this.tsbBlod.Size = new System.Drawing.Size(23, 22);
            this.tsbBlod.Text = "粗体";
            this.tsbBlod.Click += new System.EventHandler(this.tsbBlod_Click);
            // 
            // tsbBgcolor
            // 
            this.tsbBgcolor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbBgcolor.Image = global::WinHtmlEditor.Properties.Resources.BackColor;
            this.tsbBgcolor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBgcolor.Name = "tsbBgcolor";
            this.tsbBgcolor.Size = new System.Drawing.Size(23, 22);
            this.tsbBgcolor.Text = "背景色";
            this.tsbBgcolor.Click += new System.EventHandler(this.tsbBgcolor_Click);
            // 
            // tsbFontColor
            // 
            this.tsbFontColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFontColor.Image = global::WinHtmlEditor.Properties.Resources.ForeColor;
            this.tsbFontColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFontColor.Name = "tsbFontColor";
            this.tsbFontColor.Size = new System.Drawing.Size(23, 22);
            this.tsbFontColor.Text = "前景色";
            this.tsbFontColor.Click += new System.EventHandler(this.tsbFontColor_Click);
            // 
            // tsbSetFont
            // 
            this.tsbSetFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSetFont.Image = global::WinHtmlEditor.Properties.Resources.Font;
            this.tsbSetFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSetFont.Name = "tsbSetFont";
            this.tsbSetFont.Size = new System.Drawing.Size(23, 22);
            this.tsbSetFont.Text = "设置字体";
            this.tsbSetFont.Click += new System.EventHandler(this.tsbSetFont_Click);
            // 
            // tsbLink
            // 
            this.tsbLink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLink.Image = global::WinHtmlEditor.Properties.Resources.CreateLink;
            this.tsbLink.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLink.Name = "tsbLink";
            this.tsbLink.Size = new System.Drawing.Size(23, 22);
            this.tsbLink.Text = "超链接";
            this.tsbLink.Click += new System.EventHandler(this.tsbLink_Click);
            // 
            // tsbUnlink
            // 
            this.tsbUnlink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUnlink.Image = global::WinHtmlEditor.Properties.Resources.Unlink;
            this.tsbUnlink.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUnlink.Name = "tsbUnlink";
            this.tsbUnlink.Size = new System.Drawing.Size(23, 22);
            this.tsbUnlink.Text = "取消超链接";
            this.tsbUnlink.Click += new System.EventHandler(this.tsbUnlink_Click);
            // 
            // tsbInsertTable
            // 
            this.tsbInsertTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbInsertTable.Image = global::WinHtmlEditor.Properties.Resources.InsertTable;
            this.tsbInsertTable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbInsertTable.Name = "tsbInsertTable";
            this.tsbInsertTable.Size = new System.Drawing.Size(23, 22);
            this.tsbInsertTable.Text = "插入表格";
            this.tsbInsertTable.Click += new System.EventHandler(this.tsbInsertTable_Click);
            // 
            // tsbImg
            // 
            this.tsbImg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbImg.Image = global::WinHtmlEditor.Properties.Resources.InsertImage;
            this.tsbImg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImg.Name = "tsbImg";
            this.tsbImg.Size = new System.Drawing.Size(23, 22);
            this.tsbImg.Text = "插入图片";
            this.tsbImg.Click += new System.EventHandler(this.tsbImg_Click);
            // 
            // tsbInsertHorizontalRule
            // 
            this.tsbInsertHorizontalRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbInsertHorizontalRule.Image = global::WinHtmlEditor.Properties.Resources.InsertHorizontalRule;
            this.tsbInsertHorizontalRule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbInsertHorizontalRule.Name = "tsbInsertHorizontalRule";
            this.tsbInsertHorizontalRule.Size = new System.Drawing.Size(23, 22);
            this.tsbInsertHorizontalRule.Text = "插入水平线";
            this.tsbInsertHorizontalRule.Click += new System.EventHandler(this.tsbInsertHorizontalRule_Click);
            // 
            // tsbOutdent
            // 
            this.tsbOutdent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOutdent.Image = global::WinHtmlEditor.Properties.Resources.Outdent;
            this.tsbOutdent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOutdent.Name = "tsbOutdent";
            this.tsbOutdent.Size = new System.Drawing.Size(23, 22);
            this.tsbOutdent.Text = "减少缩进";
            this.tsbOutdent.Click += new System.EventHandler(this.tsbOutdent_Click);
            // 
            // tsbIndent
            // 
            this.tsbIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbIndent.Image = global::WinHtmlEditor.Properties.Resources.Indent;
            this.tsbIndent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbIndent.Name = "tsbIndent";
            this.tsbIndent.Size = new System.Drawing.Size(23, 22);
            this.tsbIndent.Text = "增加缩进";
            this.tsbIndent.Click += new System.EventHandler(this.tsbIndent_Click);
            // 
            // tsbUL
            // 
            this.tsbUL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUL.Image = global::WinHtmlEditor.Properties.Resources.InsertUnorderedList;
            this.tsbUL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUL.Name = "tsbUL";
            this.tsbUL.Size = new System.Drawing.Size(23, 22);
            this.tsbUL.Text = "插入无序列表";
            this.tsbUL.Click += new System.EventHandler(this.tsbUL_Click);
            // 
            // tsbOL
            // 
            this.tsbOL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOL.Image = global::WinHtmlEditor.Properties.Resources.InsertOrderedList;
            this.tsbOL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOL.Name = "tsbOL";
            this.tsbOL.Size = new System.Drawing.Size(23, 22);
            this.tsbOL.Text = "插入有序列表";
            this.tsbOL.Click += new System.EventHandler(this.tsbOL_Click);
            // 
            // tsbWordCount
            // 
            this.tsbWordCount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWordCount.Image = global::WinHtmlEditor.Properties.Resources.wordcount;
            this.tsbWordCount.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWordCount.Name = "tsbWordCount";
            this.tsbWordCount.Size = new System.Drawing.Size(23, 22);
            this.tsbWordCount.Text = "字数统计";
            this.tsbWordCount.Click += new System.EventHandler(this.tsbWordCount_Click);
            // 
            // tsbAbout
            // 
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAbout.Image = global::WinHtmlEditor.Properties.Resources.About;
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(23, 22);
            this.tsbAbout.Text = "关于";
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // HtmlEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.tscMain);
            this.Name = "HtmlEditor";
            this.Size = new System.Drawing.Size(760, 471);
            this.Load += new System.EventHandler(this.HtmlEditor_Load);
            this.tscMain.ContentPanel.ResumeLayout(false);
            this.tscMain.TopToolStripPanel.ResumeLayout(false);
            this.tscMain.TopToolStripPanel.PerformLayout();
            this.tscMain.ResumeLayout(false);
            this.tscMain.PerformLayout();
            this.contextMenuWeb.ResumeLayout(false);
            this.topToolBar.ResumeLayout(false);
            this.topToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer tscMain;
        private System.Windows.Forms.WebBrowser wb;
        private System.Windows.Forms.ToolStrip topToolBar;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbShowHTML;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripButton tsbPaste;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbFind;
        private System.Windows.Forms.ToolStripButton tsbClearFormat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbCenter;
        private System.Windows.Forms.ToolStripButton tsbFull;
        private System.Windows.Forms.ToolStripButton tsbLeft;
        private System.Windows.Forms.ToolStripButton tsbRight;
        private System.Windows.Forms.ToolStripButton tsbUnderline;
        private System.Windows.Forms.ToolStripButton tsbItalic;
        private System.Windows.Forms.ToolStripButton tsbBlod;
        private System.Windows.Forms.ToolStripButton tsbBgcolor;
        private System.Windows.Forms.ToolStripButton tsbFontColor;
        private System.Windows.Forms.ToolStripButton tsbSetFont;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbLink;
        private System.Windows.Forms.ToolStripButton tsbUnlink;
        private System.Windows.Forms.ToolStripButton tsbImg;
        private System.Windows.Forms.ToolStripButton tsbInsertHorizontalRule;
        private System.Windows.Forms.ToolStripButton tsbOutdent;
        private System.Windows.Forms.ToolStripButton tsbIndent;
        private System.Windows.Forms.ToolStripButton tsbUL;
        private System.Windows.Forms.ToolStripButton tsbOL;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ContextMenuStrip contextMenuWeb;
        private System.Windows.Forms.ToolStripMenuItem SelectAllMenu;
        private System.Windows.Forms.ToolStripMenuItem CopyMenu;
        private System.Windows.Forms.ToolStripMenuItem CutMenu;
        private System.Windows.Forms.ToolStripMenuItem PasteMenu;
        private System.Windows.Forms.ToolStripMenuItem DeleteMenu;
        private System.Windows.Forms.ToolStripMenuItem FindMenu;
        private System.Windows.Forms.ToolStripMenuItem RemoveFormatMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem SaveToFileMenu;
        private System.Windows.Forms.ToolStripButton tsbInsertTable;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.ToolStripButton tsbWordCount;
    }
}
