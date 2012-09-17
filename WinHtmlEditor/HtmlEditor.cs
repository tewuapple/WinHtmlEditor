using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using WinHtmlEditor.Properties;
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

        [Description("设置或获取有格式的内容"), Category("Appearance")]
        public string BodyInnerHTML
        {
            get
            {
                try
                {
                    if (wb.Document != null && wb.Document.Body != null) return wb.Document.Body.InnerHtml;
                }
                catch
                {
                    return "";
                }
                return null;
            }
            set
            {
                try
                {
                    if (wb.Document != null && wb.Document.Body != null) wb.Document.Body.InnerHtml = value;
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
                    if (wb.Document != null && wb.Document.Body != null) return wb.Document.Body.InnerText;
                }
                catch
                {
                    return "";
                }
                return null;
            }
            set
            {
                try
                {
                    if (wb.Document != null && wb.Document.Body != null) wb.Document.Body.InnerText = value;
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
                return wb.DocumentText;
            }
            set
            {
                try
                {
                    if (wb.Document != null && wb.Document.Body != null) wb.Document.Body.InnerHtml = value;
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
                return topToolBar.Visible;
            }
            set
            {
                topToolBar.Visible = value;
            }
        }

        [Category("Appearance"), Description("设置或获一个值，是否显示html编辑器"), DefaultValue("true")]
        public bool ShowWb
        {
            get
            {
                return wb.Visible;
            }
            set
            {
                wb.Visible = value;
            }
        }

        [Category("Appearance"), Description("设置或获一个值，是否启用快捷键"), DefaultValue("true")]
        public bool WebBrowserShortcutsEnabled
        {
            get
            {
                return wb.WebBrowserShortcutsEnabled;
            }
            set
            {
                wb.WebBrowserShortcutsEnabled = value;
            }
        }

        #region 公开方法
        /// <summary>
        /// 在当前工具栏上新加一个按钮
        /// </summary>
        /// <param name="buttom">按钮</param>
        /// <returns></returns>
        public bool AddToobarButtom(ToolStripButton buttom)
        {
            buttom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            return (topToolBar.Items.Add(buttom) > 0);
        }

        /// <summary>
        /// 去除指定位置上的按钮
        /// </summary>
        /// <param name="index">位置索引</param>
        public void RemoveToobarButtom(int index)
        {
            if (index >= 1)
            {
                try
                {
                    topToolBar.Items.RemoveAt(index + 0x20);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 重置为空白页
        /// </summary>
        public void Navigate()
        {
            Navigate("about:blank");
        }

        /// <summary>
        /// 将指定的统一资源定位符 (URL) 处的文档加载到 System.Windows.Forms.WebBrowser 控件中，替换上一个文档。
        /// </summary>
        /// <param name="url">指定的统一资源定位符 (URL)</param>
        public void Navigate(string url)
        {
            wb.Navigate(url);
        }

        /// <summary>
        /// The currently selected text/controls will be replaced by the given HTML code.
        /// If nothing is selected, the HTML code is inserted at the cursor position
        /// </summary>
        /// <param name="sHtml"></param>
        public void PasteIntoSelection(string sHtml)
        {
            HTMLEditHelper.PasteIntoSelection(sHtml);
        }

        /// <summary>
        /// 统计Html编辑器内容的字数
        /// </summary>
        /// <returns>字数</returns>
        public int WordCount()
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            Debug.Assert(wb.Document.Body != null, "wb.Document.Body != null");
            return wb.Document.Body.InnerText == null
                       ? 0
                       : Regex.Split(wb.Document.Body.InnerText, @"\s").
                             Sum(si => Regex.Matches(si, @"[\u0000-\u00ff]+").Count + si.Count(c => (int) c > 0x00FF));
        }

        /// <summary>
        /// 源码和设计视图相互切换
        /// </summary>
        public void ShowHTML()
        {
            bool visible = wb.Visible;
            for (int i = 0; i < topToolBar.Items.Count; i++)
            {
                topToolBar.Items[i].Enabled = !visible;
            }
            if (visible)
            {
                var box = new TextBox
                {
                    Name = "textHTMLCode",
                    Dock = DockStyle.Fill,
                    Text = HTML,
                    ScrollBars = ScrollBars.Vertical,
                    Multiline = true
                };
                tscMain.ContentPanel.Controls.Add(box);
                box.Select(0, 0);
                box.Visible = true;
                wb.Visible = false;
                tsbShowHTML.Enabled = true;
                box.KeyUp -= textHTMLCode_KeyUp;
                box.KeyUp += textHTMLCode_KeyUp;
            }
            else
            {
                var box2 = tscMain.ContentPanel.Controls["textHTMLCode"] as TextBox;
                if (box2 != null)
                {
                    HTML = box2.Text;
                    tscMain.ContentPanel.Controls.Remove(box2);
                    box2.Dispose();
                }
                wb.Visible = true;
            }
        }

        #endregion

        private void tsbNew_Click(object sender, EventArgs e)
        {
            HTML = "";
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                FileName = "",
                Filter = Resources.OpenFilter,
                Title = Resources.OpenTitle
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var reader = new StreamReader(dialog.FileName, Encoding.Default);
                HTML = reader.ReadToEnd();
                reader.Close();
            }
            dialog.Dispose();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                FileName = "",
                Filter = Resources.SaveFilter,
                Title = Resources.SaveTitle
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var writer = new StreamWriter(dialog.FileName);
                writer.Write(wb.DocumentText);
                writer.Close();
            }
            dialog.Dispose();
        }

        private void tsbShowHTML_Click(object sender, EventArgs e)
        {
            ShowHTML();
        }

        private void textHTMLCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Copy", false, null);
        }

        private void tsbCut_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Cut", false, null);
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Paste", false, null);
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Delete", false, null);
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            wb.Focus();
            SendKeys.SendWait("^f");
        }

        private void tsbClearFormat_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("RemoveFormat", false, null);
        }

        private void tsbCenter_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("JustifyCenter", false, null);
        }

        private void tsbLeft_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("JustifyLeft", false, null);
        }

        private void tsbRight_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("JustifyRight", false, null);
        }

        private void tsbUnderline_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Underline", false, null);
        }

        private void tsbItalic_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Italic", false, null);
        }

        private void tsbBlod_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Bold", false, null);
        }

        private void tsbBgcolor_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = string.Format("#{0:X2}{1:X2}{2:X2}", dialog.Color.R, dialog.Color.G, dialog.Color.B);
                Debug.Assert(wb.Document != null, "wb.Document != null");
                wb.Document.ExecCommand("BackColor", false, str);
            }
        }

        private void tsbFontColor_Click(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = string.Format("#{0:X2}{1:X2}{2:X2}", dialog.Color.R, dialog.Color.G, dialog.Color.B);
                Debug.Assert(wb.Document != null, "wb.Document != null");
                wb.Document.ExecCommand("ForeColor", false, str);
            }
        }

        private void tsbSetFont_Click(object sender, EventArgs e)
        {
            var dialog = new FontDialog
            {
                ShowEffects = true,
                ShowColor = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Debug.Assert(wb.Document != null, "wb.Document != null");
                wb.Document.ExecCommand("FontSize", false, dialog.Font.Size);
                wb.Document.ExecCommand("FontName", false, dialog.Font.Name);
                if (dialog.Font.Underline)
                {
                    wb.Document.ExecCommand("Underline", false, null);
                }
                if (dialog.Font.Italic)
                {
                    wb.Document.ExecCommand("Italic", false, null);
                }
                if (dialog.Font.Bold)
                {
                    wb.Document.ExecCommand("Bold", false, null);
                }
                Color color = dialog.Color;
                if (dialog.Color != Color.Black)
                {
                    string str = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
                    wb.Document.ExecCommand("ForeColor", false, str);
                }
            }
            dialog.Dispose();
        }

        private void tsbLink_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("CreateLink", true, null);
        }

        private void tsbUnlink_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Unlink", false, null);
        }

        private void tsbImg_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("InsertImage", true, null);
        }

        private void tsbInsertHorizontalRule_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("InsertHorizontalRule", true, null);
        }

        private void tsbOutdent_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Outdent", false, null);
        }

        private void tsbIndent_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Indent", false, null);
        }

        private void tsbUL_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("InsertUnorderedList", false, null);
        }

        private void tsbOL_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("InsertOrderedList", false, null);
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.AboutInfo, Resources.AboutText, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Process.Start("https://github.com/tewuapple/WinHtmlEditor");
            }
        }

        private void wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var document = wb.Document;
            Debug.Assert(document != null, "document != null");
            Debug.Assert(document.Body != null, "document.Body != null");
            document.Body.KeyDown -= Body_KeyDown;
            document.Body.KeyDown += Body_KeyDown;
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
                }
            }
            if ((e.KeyPressedCode == 13) && EnterToBR)
            {
                Debug.Assert(wb.Document != null, "wb.Document != null");
                var domDocument = wb.Document.DomDocument as IHTMLDocument2;
                Debug.Assert(domDocument != null, "domDocument != null");
                var range = domDocument.selection.createRange() as IHTMLTxtRange;
                Debug.Assert(range != null, "range != null");
                range.pasteHTML(!e.ShiftKeyPressed ? "<br>" : "<P>&nbsp;</P>");
                range.collapse();
                range.select();
                e.ReturnValue = true;
            }
        }

        private void HtmlEditor_Load(object sender, EventArgs e)
        {
            topToolBar.Dock = DockStyle.Top;
            wb.Navigate("about:blank");
            Debug.Assert(wb.Document != null, "wb.Document != null");
            var domDocument = wb.Document.DomDocument as IHTMLDocument2;
            wb.IsWebBrowserContextMenuEnabled = false;
            Debug.Assert(domDocument != null, "domDocument != null");
            domDocument.designMode = "on";
            SelectAllMenu.Click += SelectAllMenu_Click;
            DeleteMenu.Click += tsbDelete_Click;
            FindMenu.Click += tsbFind_Click;
            CopyMenu.Click += tsbCopy_Click;
            CutMenu.Click += tsbCut_Click;
            PasteMenu.Click += tsbPaste_Click;
            SaveToFileMenu.Click += tsbSave_Click;
            RemoveFormatMenu.Click += tsbClearFormat_Click;
            HTMLEditHelper.DOMDocument = domDocument;
        }

        private void SelectAll()
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("SelectAll", false, null);
        }

        private void SelectAllMenu_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        private void tsbFull_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("JustifyFull", false, null);
        }

        private void tsbInsertTable_Click(object sender, EventArgs e)
        {
            var mFrmTable = new FrmTable();
            mFrmTable.ShowDialog(this);
            if (mFrmTable.MResult == DialogResult.OK)
            {
                HTMLEditHelper.AppendTable(mFrmTable.MCols, mFrmTable.MRows,
                    mFrmTable.MBorderSize, mFrmTable.MAlignment,
                    mFrmTable.MCellPadding, mFrmTable.MCellSpacing,
                    mFrmTable.MWidthPercent, mFrmTable.MWidthPixels);
            }
        }

        private void tsbWordCount_Click(object sender, EventArgs e)
        {
            int wordCount = WordCount();
            MessageBox.Show(string.Format("字数：{0}", wordCount));
        }

        private void tsbSuperscript_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Superscript", false, null);
        }

        private void tsbSubscript_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Subscript", false, null);
        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Print", false, null);
        }

        private void tsbDate_Click(object sender, EventArgs e)
        {
            HTMLEditHelper.PasteIntoSelection(DateTime.Now.ToLongDateString());
        }

        private void tsbTime_Click(object sender, EventArgs e)
        {
            HTMLEditHelper.PasteIntoSelection(DateTime.Now.ToLongTimeString());
        }

        private void tsbSpellCheck_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            Debug.Assert(wb.Document.Body != null, "wb.Document.Body != null");
            spellCheck.Text = wb.Document.Body.InnerHtml;
            spellCheck.SpellCheck();
        }

        public static string ClearWord(string sourceText, bool bIgnoreFont = true, bool bRemoveStyles = true, bool cleanWordKeepsStructure = true)
        {
            sourceText = Regex.Replace(sourceText, @"<o:p>\s*<\/o:p>", "");
            sourceText = Regex.Replace(sourceText, @"<o:p>.*?<\/o:p>", " ");
            // Remove mso-xxx styles.
            sourceText = Regex.Replace(sourceText, @"\s*mso-[^:]+:[^;""]+;?", "", RegexOptions.IgnoreCase);
            // Remove margin styles.
            sourceText = Regex.Replace(sourceText, @"\s*MARGIN: 0cm 0cm 0pt\s*;", "", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"\s*MARGIN: 0cm 0cm 0pt\s*""", "\"", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"\s*TEXT-INDENT: 0cm\s*;", "", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"\s*TEXT-INDENT: 0cm\s*""", "\"", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"\s*TEXT-ALIGN: [^\s;]+;?""", "\"", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"\s*PAGE-BREAK-BEFORE: [^\s;]+;?""", "\"", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"\s*FONT-VARIANT: [^\s;]+;?""", "\"", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"\s*tab-stops:[^;""]*;?", "", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"\s*tab-stops:[^""]*", "", RegexOptions.IgnoreCase);
            // Remove FONT face attributes.
            if (bIgnoreFont)
            {
                sourceText = Regex.Replace(sourceText, @"\s*face=""[^""]*""", "", RegexOptions.IgnoreCase);
                sourceText = Regex.Replace(sourceText, @"\s*face=[^ >]*", "", RegexOptions.IgnoreCase);
                sourceText = Regex.Replace(sourceText, @"\s*FONT-FAMILY:[^;""]*;?", "", RegexOptions.IgnoreCase);
            }

            // Remove Class attributes
            sourceText = Regex.Replace(sourceText, @"<(\w[^>]*) class=([^ |>]*)([^>]*)", "<$1$3", RegexOptions.IgnoreCase);
            // Remove styles.
            if (bRemoveStyles)
                sourceText = Regex.Replace(sourceText, @"<(\w[^>]*) style=""([^\""]*)""([^>]*)", "<$1$3", RegexOptions.IgnoreCase);
            // Remove empty styles.
            sourceText = Regex.Replace(sourceText, @"\s*style=""\s*""", "", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"<SPAN\s*[^>]*>\s* \s*<\/SPAN>", " ", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"<SPAN\s*[^>]*><\/SPAN>", "", RegexOptions.IgnoreCase);
            // Remove Lang attributes
            sourceText = Regex.Replace(sourceText, @"<(\w[^>]*) lang=([^ |>]*)([^>]*)", "<$1$3", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"<SPAN\s*>(.*?)<\/SPAN>", "$1", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"<FONT\s*>(.*?)<\/FONT>", "$1", RegexOptions.IgnoreCase);
            // Remove XML elements and declarations
            sourceText = Regex.Replace(sourceText, @"<\\?\?xml[^>]*>", "", RegexOptions.IgnoreCase);

            // Remove Tags with XML namespace declarations: <o:p><\/o:p>
            sourceText = Regex.Replace(sourceText, @"<\/?\w+:[^>]*>", "", RegexOptions.IgnoreCase);
            // Remove comments [SF BUG-1481861].
            sourceText = Regex.Replace(sourceText, @"<\!--.*?-->/", "");
            sourceText = Regex.Replace(sourceText, @"<(U|I|STRIKE)> <\/\1>", " ");
            sourceText = Regex.Replace(sourceText, @"<H\d>\s*<\/H\d>", "", RegexOptions.IgnoreCase);

            // Remove "display:none" tags.
            sourceText = Regex.Replace(sourceText, @"<(\w+)[^>]*\sstyle=""[^""]*DISPLAY\s?:\s?none(.*?)<\/\1>", "", RegexOptions.IgnoreCase);

            // Remove language tags
            sourceText = Regex.Replace(sourceText, @"<(\w[^>]*) language=([^ |>]*)([^>]*)", "<$1$3", RegexOptions.IgnoreCase);

            // Remove onmouseover and onmouseout events (from MS Word comments effect)
            sourceText = Regex.Replace(sourceText, @"<(\w[^>]*) onmouseover=""([^\""]*)""([^>]*)", "<$1$3", RegexOptions.IgnoreCase);
            sourceText = Regex.Replace(sourceText, @"<(\w[^>]*) onmouseout=""([^\""]*)""([^>]*)", "<$1$3", RegexOptions.IgnoreCase);

            if (cleanWordKeepsStructure)
            {
                // The original <Hn> tag send from Word is something like this: <Hn style="margin-top:0px;margin-bottom:0px">
                sourceText = Regex.Replace(sourceText, @"<H(\d)([^>]*)>", "<h$1>", RegexOptions.IgnoreCase);

                // Word likes to insert extra <font> tags, when using MSIE. (Wierd).
                sourceText = Regex.Replace(sourceText, @"<(H\d)><FONT[^>]*>(.*?)<\/FONT><\/\1>", @"<$1>$2<\/$1>", RegexOptions.IgnoreCase);
                sourceText = Regex.Replace(sourceText, @"<(H\d)><EM>(.*?)<\/EM><\/\1>", @"<$1>$2<\/$1>", RegexOptions.IgnoreCase);
            }
            else
            {
                sourceText = Regex.Replace(sourceText, @"<H1([^>]*)>", @"<div$1><b><font size=""6"">", RegexOptions.IgnoreCase);
                sourceText = Regex.Replace(sourceText, @"<H2([^>]*)>", @"<div$1><b><font size=""5"">", RegexOptions.IgnoreCase);
                sourceText = Regex.Replace(sourceText, @"<H3([^>]*)>", @"<div$1><b><font size=""4"">", RegexOptions.IgnoreCase);
                sourceText = Regex.Replace(sourceText, @"<H4([^>]*)>", @"<div$1><b><font size=""3"">", RegexOptions.IgnoreCase);
                sourceText = Regex.Replace(sourceText, @"<H5([^>]*)>", @"<div$1><b><font size=""2"">", RegexOptions.IgnoreCase);
                sourceText = Regex.Replace(sourceText, @"<H6([^>]*)>", @"<div$1><b><font size=""1"">", RegexOptions.IgnoreCase);

                sourceText = Regex.Replace(sourceText, @"<\/H\d>", @"<\/font><\/b><\/div>", RegexOptions.IgnoreCase);

                // Transform <P> to <DIV>
                //var re = new Regex(@"(<P)([^>]*>.*?)(<\/P>)", RegexOptions.IgnoreCase); // Different because of a IE 5.0 error
                sourceText = Regex.Replace(sourceText, @"(<P)([^>]*>.*?)(<\/P>)", @"<div$2<\/div>", RegexOptions.IgnoreCase);

                // Remove empty tags (three times, just to be sure).
                // This also removes any empty anchor
                sourceText = Regex.Replace(sourceText, @"<([^\s>]+)(\s[^>]*)?>\s*<\/\1>", "");
                sourceText = Regex.Replace(sourceText, @"<([^\s>]+)(\s[^>]*)?>\s*<\/\1>", "");
                sourceText = Regex.Replace(sourceText, @"<([^\s>]+)(\s[^>]*)?>\s*<\/\1>", "");
            }
            return sourceText;
        }

        private void tsbWordClean_Click(object sender, EventArgs e)
        {
            if (BodyInnerHTML != null)
            {
                Debug.Assert(wb.Document != null, "wb.Document != null");
                Debug.Assert(wb.Document.Body != null, "wb.Document.Body != null");
                wb.Document.Body.InnerHtml = ClearWord(BodyInnerHTML);
            }
        }

        private void spellCheck_DeletedWord(object sender, NetSpell.SpellChecker.SpellingEventArgs e)
        {
            var htmlDocument = wb.Document;
            if (htmlDocument != null) if (htmlDocument.Body != null) htmlDocument.Body.InnerHtml = e.Text;
        }

        private void spellCheck_ReplacedWord(object sender, NetSpell.SpellChecker.ReplaceWordEventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            Debug.Assert(wb.Document.Body != null, "wb.Document.Body != null");
            wb.Document.Body.InnerHtml = e.Text;
        }
    }
}
