using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using mshtml;
using System.Diagnostics;

namespace WinHtmlEditor
{
    [Docking(DockingBehavior.Ask), ComVisible(true), ClassInterface(ClassInterfaceType.AutoDispatch), ToolboxBitmap(typeof(HtmlEditor), "Resources.HTML.bmp")]
    public partial class HtmlEditor : UserControl
    {
        public HtmlEditor()
        {
            InitializeComponent();
        }

        public string BodyInnerHTML
        {
            get
            {
                try
                {
                    return this.wb.Document.Body.InnerHtml;
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    this.wb.Document.Body.InnerHtml = value;
                }
                catch
                {
                }
            }
        }

        [Description("设置或获取无格式的内容"), Category("Appearance")]
        public string BodyInnerText
        {
            get
            {
                try
                {
                    return this.wb.Document.Body.InnerHtml;
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    this.wb.Document.Body.InnerHtml = value;
                }
                catch
                {
                }
            }
        }

        [Category("Appearance"), DefaultValue("false"), Description("设置或获取一个值，决定按下回车键的时候是插入BR还是P")]
        public bool EnterToBR { get; set; }

        [Category("Appearance"), DefaultValue(""), Description("设置或获取HTML代码")]
        public string HTML
        {
            get
            {
                return this.wb.DocumentText;
            }
            set
            {
                try
                {
                    this.wb.Document.Body.InnerHtml = value;
                }
                catch
                {
                }
            }
        }

        [Category("Appearance"), Description("设置或获一个值，是否显示工具条"), DefaultValue("true")]
        public bool ShowToolBar
        {
            get
            {
                return this.topToolBar.Visible;
            }
            set
            {
                this.topToolBar.Visible = value;
            }
        }

        [Category("Appearance"), Description("设置或获一个值，是否显示html编辑器"), DefaultValue("true")]
        public bool ShowWb
        {
            get
            {
                return this.wb.Visible;
            }
            set
            {
                this.wb.Visible = value;
            }
        }

        [Category("Appearance"), Description("设置或获一个值，是否启用快捷键"), DefaultValue("true")]
        public bool WebBrowserShortcutsEnabled
        {
            get
            {
                return this.wb.WebBrowserShortcutsEnabled;
            }
            set
            {
                this.wb.WebBrowserShortcutsEnabled = value;
            }
        }

        #region 公开方法
        public bool AddToobarButtom(ToolStripButton Buttom)
        {
            Buttom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            return (this.topToolBar.Items.Add(Buttom) > 0);
        }

        public void RemoveToobarButtom(int index)
        {
            if (index >= 1)
            {
                try
                {
                    this.topToolBar.Items.RemoveAt(index + 0x20);
                }
                catch
                {
                }
            }
        }

        public void Navigate()
        {
            this.Navigate("about:blank");
        }

        public void Navigate(string url)
        {
            this.wb.Navigate(url);
        }
        #endregion

        private void tsbNew_Click(object sender, EventArgs e)
        {
            this.HTML = "";
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                FileName = "",
                Filter = "支持文件|*.html;*.shtml;*.txt;*.htm|所有文件|*.*",
                Title = "请选择文件"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(dialog.FileName, Encoding.Default);
                this.HTML = reader.ReadToEnd();
                reader.Close();
            }
            dialog.Dispose();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = "",
                Filter = "HTML文件|*.html|SHTML文件|*.shtml|HTM文件|*.htm|文本文件|*.txt",
                Title = "保存文件"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(dialog.FileName);
                writer.Write(this.wb.DocumentText);
                writer.Close();
            }
            dialog.Dispose();
        }

        private void tsbShowHTML_Click(object sender, EventArgs e)
        {
            ShowHTML();
        }

        public void ShowHTML()
        {
            bool visible = this.wb.Visible;
            for (int i = 0; i < this.topToolBar.Items.Count; i++)
            {
                this.topToolBar.Items[i].Enabled = !visible;
            }
            if (visible)
            {
                TextBox box = new TextBox
                {
                    Name = "textHTMLCode",
                    Dock = DockStyle.Fill,
                    Text = this.HTML,
                    ScrollBars = ScrollBars.Vertical,
                    Multiline = true
                };
                this.tscMain.ContentPanel.Controls.Add(box);
                box.Select(0, 0);
                box.Visible = true;
                this.wb.Visible = false;
                this.tsbShowHTML.Enabled = true;
            }
            else
            {
                TextBox box2 = this.tscMain.ContentPanel.Controls["textHTMLCode"] as TextBox;
                this.HTML = box2.Text;
                this.tscMain.ContentPanel.Controls.Remove(box2);
                this.wb.Visible = true;
                box2.Dispose();
            }
        }

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Copy", false, null);
        }

        private void tsbCut_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Cut", false, null);
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Paste", false, null);
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Delete", false, null);
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            this.wb.Focus();
            SendKeys.SendWait("^f");
        }

        private void tsbClearFormat_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("RemoveFormat", false, null);
        }

        private void tsbCenter_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("JustifyCenter", false, null);
        }

        private void tsbLeft_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("JustifyLeft", false, null);
        }

        private void tsbRight_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("JustifyRight", false, null);
        }

        private void tsbUnderline_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Underline", false, null);
        }

        private void tsbItalic_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Italic", false, null);
        }

        private void tsbBlod_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Bold", false, null);
        }

        private void tsbBgcolor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = string.Format("#{0:X2}{1:X2}{2:X2}", dialog.Color.R, dialog.Color.G, dialog.Color.B);
                this.wb.Document.ExecCommand("BackColor", false, str);
            }
        }

        private void tsbFontColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = string.Format("#{0:X2}{1:X2}{2:X2}", dialog.Color.R, dialog.Color.G, dialog.Color.B);
                this.wb.Document.ExecCommand("ForeColor", false, str);
            }
        }

        private void tsbSetFont_Click(object sender, EventArgs e)
        {
            FontDialog dialog = new FontDialog
            {
                ShowEffects = true,
                ShowColor = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.wb.Document.ExecCommand("FontSize", false, dialog.Font.Size);
                this.wb.Document.ExecCommand("FontName", false, dialog.Font.Name);
                if (dialog.Font.Underline)
                {
                    this.wb.Document.ExecCommand("Underline", false, null);
                }
                if (dialog.Font.Italic)
                {
                    this.wb.Document.ExecCommand("Italic", false, null);
                }
                if (dialog.Font.Bold)
                {
                    this.wb.Document.ExecCommand("Bold", false, null);
                }
                Color color = dialog.Color;
                if (dialog.Color != Color.Black)
                {
                    string str = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
                    this.wb.Document.ExecCommand("ForeColor", false, str);
                }
            }
            dialog.Dispose();
        }

        private void tsbLink_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("CreateLink", true, null);
        }

        private void tsbUnlink_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Unlink", false, null);
        }

        private void tsbImg_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("InsertImage", true, null);
        }

        private void tsbInsertHorizontalRule_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("InsertHorizontalRule", true, null);
        }

        private void tsbOutdent_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Outdent", false, null);
        }

        private void tsbIndent_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("Indent", false, null);
        }

        private void tsbUL_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("InsertUnorderedList", false, null);
        }

        private void tsbOL_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("InsertOrderedList", false, null);
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("这是一个免费的HTML控件，你可以自由使用。\r\n如有任何问题或建议请加入QQ群：217478320，本人博客请访问：hi.baidu.com/new/tewuapple,是否现在就访问？\r\n", "关于", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Process.Start("http://hi.baidu.com/new/tewuapple");
            }
        }

        private void wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            HtmlDocument document = this.wb.Document;
            document.Body.KeyDown -= new HtmlElementEventHandler(this.Body_KeyDown);
            document.Body.KeyDown += new HtmlElementEventHandler(this.Body_KeyDown);
        }

        private void Body_KeyDown(object sender, HtmlElementEventArgs e)
        {
            if (e.CtrlKeyPressed)
            {
                switch (e.KeyPressedCode)
                {
                    case 65:
                        SelectAll();
                        e.ReturnValue = true;
                        break;
                    default:
                        break;
                }
            }
            if ((e.KeyPressedCode == 13) && this.EnterToBR)
            {
                IHTMLDocument2 domDocument = this.wb.Document.DomDocument as IHTMLDocument2;
                IHTMLTxtRange range = domDocument.selection.createRange() as IHTMLTxtRange;
                if (!e.ShiftKeyPressed)
                {
                    range.pasteHTML("<br>");
                }
                else
                {
                    range.pasteHTML("<P>&nbsp;</P>");
                }
                range.collapse(true);
                range.select();
                e.ReturnValue = true;
            }
        }

        private void HtmlEditor_Load(object sender, EventArgs e)
        {
            this.topToolBar.Dock = DockStyle.Top;
            this.wb.Navigate("about:blank");
            IHTMLDocument2 domDocument = this.wb.Document.DomDocument as IHTMLDocument2;
            this.wb.IsWebBrowserContextMenuEnabled = false;
            domDocument.designMode = "on";
            this.SelectAllMenu.Click += new EventHandler(this.SelectAllMenu_Click);
            this.DeleteMenu.Click += new EventHandler(this.tsbDelete_Click);
            this.FindMenu.Click += new EventHandler(this.tsbFind_Click);
            this.CopyMenu.Click += new EventHandler(this.tsbCopy_Click);
            this.CutMenu.Click += new EventHandler(this.tsbCut_Click);
            this.PasteMenu.Click += new EventHandler(this.tsbPaste_Click);
            this.SaveToFileMenu.Click += new EventHandler(this.tsbSave_Click);
            this.RemoveFormatMenu.Click += new EventHandler(this.tsbClearFormat_Click);
            htmledit.DOMDocument = domDocument;
        }

        private void SelectAll()
        {
            this.wb.Document.ExecCommand("SelectAll", false, null);
        }
        private void SelectAllMenu_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        private void tsbFull_Click(object sender, EventArgs e)
        {
            this.wb.Document.ExecCommand("JustifyFull", false, null);
        }

        private HTMLEditHelper htmledit = new HTMLEditHelper();

        private void tsbInsertTable_Click(object sender, EventArgs e)
        {
            frmTable m_frmTable = new frmTable();
            m_frmTable.ShowDialog(this);
            if (m_frmTable.m_Result == DialogResult.OK)
            {
                htmledit.AppendTable(m_frmTable.m_Cols, m_frmTable.m_Rows,
                    m_frmTable.m_BorderSize, m_frmTable.m_Alignment,
                    m_frmTable.m_CellPadding, m_frmTable.m_CellSpacing,
                    m_frmTable.m_WidthPercent, m_frmTable.m_WidthPixels);
            }
        }

        private void tsbWordCount_Click(object sender, EventArgs e)
        {
            var iSumWords = 0;
            IHTMLDocument2 document = wb.Document.DomDocument as IHTMLDocument2;
            IHTMLBodyElement body = document.body as IHTMLBodyElement;
            IHTMLTxtRange rng = body.createTextRange() as IHTMLTxtRange;
            rng.collapse(true);
            while (rng.move("word", 1) > 0)
            {
                iSumWords++;
            }
            MessageBox.Show("大约" + iSumWords.ToString() + "字");
        }
    }
}
