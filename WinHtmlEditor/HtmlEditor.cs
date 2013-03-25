using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
#if VS2010
using System.Linq;
#endif
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using WinHtmlEditor.Common;
using WinHtmlEditor.Properties;
using mshtml;
using System.Diagnostics;

namespace WinHtmlEditor
{
    [Docking(DockingBehavior.Ask), ComVisible(true), ClassInterface(ClassInterfaceType.AutoDispatch), ToolboxBitmap(typeof(HtmlEditor), "Resources.HTML.bmp")]
    public partial class HtmlEditor : UserControl
    {
        private IHTMLDocument2 _doc;
        private bool _updatingFontName;
        private bool _updatingFontSize;

        public delegate void TickDelegate();
        public event TickDelegate Tick;

        public class EnterKeyEventArgs : EventArgs
        {
            private bool _cancel = false;

            public bool Cancel
            {
                get { return _cancel; }
                set { _cancel = value; }
            }

        }
        public event EventHandler<EnterKeyEventArgs> EnterKeyEvent;

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
                return tsTopToolBar.Visible;
            }
            set
            {
                tsTopToolBar.Visible = value;
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
            return (tsTopToolBar.Items.Add(buttom) > 0);
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
                    tsTopToolBar.Items.RemoveAt(index + 0x20);
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
#if VS2010
            return wb.Document.Body.InnerText == null
                       ? 0
                       : Regex.Split(wb.Document.Body.InnerText, @"\s").
                             Sum(si => Regex.Matches(si, @"[\u0000-\u00ff]+").Count + si.Count(c => (int) c > 0x00FF));
#else
            if (wb.Document.Body.InnerText == null)
                return 0;
            var sec = Regex.Split(wb.Document.Body.InnerText, @"\s");
            int count = 0;
            foreach (var si in sec)
            {
                int ci = Regex.Matches(si, @"[\u0000-\u00ff]+").Count;
                foreach (var c in si)
                    if (c > 0x00FF) ci++;
                count += ci;
            }
            return count;
#endif
        }

        /// <summary>
        /// 源码和设计视图相互切换
        /// </summary>
        public void ShowHTML()
        {
            bool visible = wb.Visible;
            for (int i = 0; i < tsTopToolBar.Items.Count; i++)
            {
                tsTopToolBar.Items[i].Enabled = !visible;
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

        /// <summary>
        /// 预览
        /// </summary>
        public void Preview()
        {
            bool isPreview = (_doc.designMode == "On");
            for (int i = 0; i < tsTopToolBar.Items.Count; i++)
            {
                tsTopToolBar.Items[i].Enabled = !isPreview;
            }
            if(isPreview)
            {
                _doc.designMode = "Off";
                timer.Stop();
                cmsHtml.Enabled = false;
                tsbPreview.Enabled = true;
            }
            else
            {
                timer.Start();
                cmsHtml.Enabled = true;
                _doc.designMode = "On";
            }
        }

        #endregion

        private void tsbNew_Click(object sender, EventArgs e)
        {
            HTML = "";
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                FileName = "",
                Filter = Resources.OpenFilter,
                Title = Resources.OpenTitle
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(dialog.FileName, Encoding.Default);
                    HTML = reader.ReadToEnd();
                    reader.Close();
                }
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog
            {
                FileName = "",
                Filter = Resources.SaveFilter,
                Title = Resources.SaveTitle
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var writer = new StreamWriter(dialog.FileName);
                    writer.Write(wb.DocumentText);
                    writer.Close();
                }
            }
        }

        private void tsbPreview_Click(object sender, EventArgs e)
        {
            Preview();
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

        private void tsbRemoveFormat_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("RemoveFormat", false, null);
        }

        private void tsbJustifyCenter_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("JustifyCenter", false, null);
        }

        private void tsbJustifyLeft_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("JustifyLeft", false, null);
        }

        private void tsbJustifyRight_Click(object sender, EventArgs e)
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

        private void tsbBold_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Bold", false, null);
        }

        private void tsbBackColor_Click(object sender, EventArgs e)
        {
            using (var dialog = new ColorDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string str = string.Format("#{0:X2}{1:X2}{2:X2}", dialog.Color.R, dialog.Color.G, dialog.Color.B);
                    Debug.Assert(wb.Document != null, "wb.Document != null");
                    wb.Document.ExecCommand("BackColor", false, str);
                }
            }
        }

        private void tsbForeColor_Click(object sender, EventArgs e)
        {
            using (var dialog = new ColorDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string str = string.Format("#{0:X2}{1:X2}{2:X2}", dialog.Color.R, dialog.Color.G, dialog.Color.B);
                    Debug.Assert(wb.Document != null, "wb.Document != null");
                    wb.Document.ExecCommand("ForeColor", false, str);
                }
            }
        }

        private void tsbStrikeThrough_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("StrikeThrough", false, null);
        }

        private void tsbCreateLink_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("CreateLink", true, null);
        }

        private void tsbUnlink_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Unlink", false, null);
        }

        private void tsbInsertImage_Click(object sender, EventArgs e)
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

        private void tsbInsertUnorderedList_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("InsertUnorderedList", false, null);
        }

        private void tsbInsertOrderedList_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("InsertOrderedList", false, null);
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.AboutInfo, Resources.AboutText, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                Process.Start("http://tewuapple.github.com/WinHtmlEditor/");
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
            if ((e.KeyPressedCode != 13)) return;
            if (EnterToBR)
            {
                Debug.Assert(wb.Document != null, "wb.Document != null");
                var domDocument = wb.Document.DomDocument as IHTMLDocument2;
                Debug.Assert(domDocument != null, "domDocument != null");
                var range = domDocument.selection.createRange() as IHTMLTxtRange;
                Debug.Assert(range != null, "range != null");
                range.pasteHTML(!e.ShiftKeyPressed ? "<br>" : "<P>&nbsp;</P>");
#if VS2010
                range.collapse();
#else
                range.collapse(true);
#endif
                range.@select();
            }
            if (!e.ShiftKeyPressed)
            {
                e.ReturnValue = SetupKeyListener();
            }
            e.ReturnValue = true;
        }

        private static void Selector_TableSizeSelected(object sender, TableSizeEventArgs e)
        {
            HTMLEditHelper.AppendTable(e.SelectedSize.Width, e.SelectedSize.Height, 1, "", 0, 0, "", 0);
        }

        private void DropDown_Opening(object sender, CancelEventArgs e)
        {
            var c = tsddbInsertTable.DropDown as ToolStripTableSizeSelector;
            if (c != null)
            {
                c.Selector.SelectedSize = new Size(0, 0);
                c.Selector.VisibleRange = new Size(10, 10);
            }
        }

        /// <summary>
        /// 初始化工具栏和邮件菜单
        /// </summary>
        private void InitUi()
        {
            //初始化插入表格部分
            var dropDown = new ToolStripTableSizeSelector();
            dropDown.Opening += DropDown_Opening;
            dropDown.Selector.TableSizeSelected += Selector_TableSizeSelected;
            tsddbInsertTable.DropDown = dropDown;
            var tsmiInsertTable = new ToolStripMenuItem
                {
                    Image = Resources.InsertTable,
                    Name = "tsmiInsertTable",
                    Size = new Size(152, 22),
                    Text = Resources.strInsertTable
                };
            tsmiInsertTable.Click += tsmiInsertTable_Click;
            tsddbInsertTable.DropDownItems.Add(tsmiInsertTable);

            string removeButton = ConfigurationManager.AppSettings["removeButtons"];
            if (!string.IsNullOrEmpty(removeButton))
            {
                var removeButtons = removeButton.Split(',');
                foreach (var item in tsTopToolBar.Items)
                {
                    if (item is ToolStripButton)
                    {
                        var tsb = item as ToolStripButton;
                        foreach (var button in removeButtons)
                        {
                            if (String.CompareOrdinal(tsb.Tag.ToString(), button) == 0)
                            {
                                tsb.Visible = false;
                            }
                        }
                    }
                    else if (item is ToolStripDropDownButton)
                    {
                        var tsddb = item as ToolStripDropDownButton;
                        foreach (var button in removeButtons)
                        {
                            if (String.CompareOrdinal(tsddb.Tag.ToString(), button) == 0)
                            {
                                tsddb.Visible = false;
                            }
                        }
                    }
                }
            }
            string removeMenu = ConfigurationManager.AppSettings["removeMenus"];
            if (!string.IsNullOrEmpty(removeMenu))
            {
                var removeMenus = removeMenu.Split(',');
                foreach (var item in cmsHtml.Items)
                {
                    if (item is ToolStripMenuItem)
                    {
                        var tsmi = item as ToolStripMenuItem;
                        foreach (var menu in removeMenus)
                        {
                            if (String.CompareOrdinal(tsmi.Tag.ToString(), menu) == 0)
                            {
                                tsmi.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        private void HtmlEditor_Load(object sender, EventArgs e)
        {
            SynchFont(string.Empty);
            SetupTimer();
            SetupComboFontSize();
            tsTopToolBar.Dock = DockStyle.Top;
            wb.Navigate("about:blank");
            Debug.Assert(wb.Document != null, "wb.Document != null");
            _doc = wb.Document.DomDocument as IHTMLDocument2;
            wb.IsWebBrowserContextMenuEnabled = false;
            Debug.Assert(_doc != null, "domDocument != null");
            _doc.designMode = "On";
            tsmiSelectAll.Click += tsmiSelectAll_Click;
            tsmiDelete.Click += tsbDelete_Click;
            tsmiFind.Click += tsbFind_Click;
            tsmiCopy.Click += tsbCopy_Click;
            tsmiCut.Click += tsbCut_Click;
            tsmiPaste.Click += tsbPaste_Click;
            tsmiSave.Click += tsbSave_Click;
            tsmiRemoveFormat.Click += tsbRemoveFormat_Click;
            InitUi();
            HTMLEditHelper.DOMDocument = _doc;
        }

        private void SelectAll()
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("SelectAll", false, null);
        }

        private void tsmiSelectAll_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        private void tsbJustifyFull_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("JustifyFull", false, null);
        }

        private void tsmiInsertTable_Click(object sender, EventArgs e)
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

#if VS2010
        public static string ClearWord(string sourceText, bool bIgnoreFont = true, bool bRemoveStyles = true, bool cleanWordKeepsStructure = true)
        {
            return ClearWordNoDefult(sourceText, bIgnoreFont, bRemoveStyles, cleanWordKeepsStructure);
        }
#endif
        public static string ClearWordNoDefult(string sourceText, bool bIgnoreFont, bool bRemoveStyles, bool cleanWordKeepsStructure)
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


        private void tsbClearWord_Click(object sender, EventArgs e)
        {
            if (BodyInnerHTML != null)
            {
                Debug.Assert(wb.Document != null, "wb.Document != null");
                Debug.Assert(wb.Document.Body != null, "wb.Document.Body != null");
#if VS2010
                wb.Document.Body.InnerHtml = ClearWord(BodyInnerHTML);
#else
                wb.Document.Body.InnerHtml = ClearWordNoDefult(BodyInnerHTML, true, true, true);
#endif
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

        #region Fonts and Color Handling

        //To handle the fact that setting SelectedIndex property calls SelectedIndexChanged event
        //We set this value to true whenever SelectedIndex is set. 
        private bool _internalCall;
        private void SetupComboFontSize()
        {
            tscbFontSize.DropDownStyle = ComboBoxStyle.DropDownList;
            tscbFontSize.Items.Add("1 (8 pt)");
            tscbFontSize.Items.Add("2 (10 pt)");
            tscbFontSize.Items.Add("3 (12 pt)");
            tscbFontSize.Items.Add("4 (14 pt)");
            tscbFontSize.Items.Add("5 (18 pt)");
            tscbFontSize.Items.Add("6 (24 pt)");
            tscbFontSize.Items.Add("7 (36 pt)");
            tscbFontSize.Click += tscbFontSize_Click;
            tscbFontSize.SelectedIndexChanged += tscbFontSize_SelectedIndexChanged;
        }

        private void tscbFontSize_Click(object sender, EventArgs e)
        {
            _internalCall = false;
        }

        private void tscbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Fontsize changed 1 to 7
                if ((tscbFontSize.SelectedIndex > -1) && (!_internalCall))
                {
                    if (_updatingFontSize) return;
                    int obj = tscbFontSize.SelectedIndex + 1;
                    switch (obj)
                    {
                        case 1:
                            FontSize = FontSize.One;
                            break;
                        case 2:
                            FontSize = FontSize.Two;
                            break;
                        case 3:
                            FontSize = FontSize.Three;
                            break;
                        case 4:
                            FontSize = FontSize.Four;
                            break;
                        case 5:
                            FontSize = FontSize.Five;
                            break;
                        case 6:
                            FontSize = FontSize.Six;
                            break;
                        case 7:
                            FontSize = FontSize.Seven;
                            break;
                        default:
                            FontSize = FontSize.Seven;
                            break;
                    }
                    wb.Focus();
                }
                _internalCall = false;
            }
            catch
            {
            }
        }

        /// <summary>
        /// Called when the font combo box has changed.
        /// Ignores the event when the timer is updating the font combo Box 
        /// to synchronize the editor selection with the font combo box.
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">EventArgs</param>
        private void tsfcbFontName_Leave(object sender, EventArgs e)
        {
            if (_updatingFontName) return;
            FontFamily ff;
            try
            {
                ff = new FontFamily(tsfcbFontName.Text);
            }
            catch (Exception)
            {
                _updatingFontName = true;
                tsfcbFontName.Text = FontName.GetName(0);
                _updatingFontName = false;
                return;
            }
            FontName = ff;
        }


        private void tsfcbFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((tsfcbFontName.SelectedIndex > -1) &&
                    (!tsfcbFontName.InternalCall))
                {
                    if (_updatingFontName) return;
                    Font f = tsfcbFontName.SelectedFontItem;
                    FontName = f.FontFamily;
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Private variables and methods

        private void SynchFont(string sTagName)
        {
            //Times Roman New
            string fontname = string.Empty;
            if (_doc != null)
            {
                object obj = _doc.queryCommandValue("FontName");
                if (obj == null)
                    return;
                fontname = obj.ToString();
                obj = _doc.queryCommandValue("FontSize");
                if (obj == null)
                    return;
                //Could indicate a headingxxx, P, or BODY
                _internalCall = true;
                if (obj.ToString().Length > 0)
                    tscbFontSize.SelectedIndex = Convert.ToInt32(obj) - 1; //x (x - 1)
                else
                    AdjustForHeading(sTagName);
            }
            tsfcbFontName.SelectedFontNameItem = fontname;
        }

        private void AdjustForHeading(string sTag)
        {
            if (string.IsNullOrEmpty(sTag))
                return;
            int index;
            if (sTag == "H1")
                index = 5; //24pt
            else if (sTag == "H2")
                index = 4; //18pt
            else if (sTag == "H3")
                index = 3; //14pt
            else if (sTag == "H4")
                index = 2; //12pt
            else if (sTag == "H5")
                index = 1; //10pt
            else if (sTag == "H6")
                index = 0; //8pt
            else
                return; //do nothing
            _internalCall = true;
            tscbFontSize.SelectedIndex = index;
        }

        /// <summary>
        /// Setup timer with 200ms interval
        /// </summary>
        private void SetupTimer()
        {
            timer.Interval = 200;
            timer.Tick += timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Get the ready state of the internal browser component.
        /// </summary>
        public ReadyState ReadyState
        {
            get
            {
                if (_doc == null)
                    return ReadyState.Uninitialized;
                switch (_doc.readyState.ToLower())
                {
                    case "uninitialized":
                        return ReadyState.Uninitialized;
                    case "loading":
                        return ReadyState.Loading;
                    case "loaded":
                        return ReadyState.Loaded;
                    case "interactive":
                        return ReadyState.Interactive;
                    case "complete":
                        return ReadyState.Complete;
                    default:
                        return ReadyState.Uninitialized;
                }
            }
        }

        /// <summary>
        /// Called when the timer fires to synchronize the format buttons 
        /// with the text editor current selection.
        /// SetupKeyListener if necessary.
        /// Set bold, italic, underline and link buttons as based on editor state.
        /// Synchronize the font combo box and the font size combo box.
        /// Finally, fire the Tick event to allow external components to synchronize 
        /// their state with the editor.
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">EventArgs</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // don't process until browser is in ready state.
            if (ReadyState != ReadyState.Complete)
                return;
            // don't process until bowser is create.
            if (wb.IsHandleCreated == false)
                return;
            SetupKeyListener();
            tsbBold.Checked = IsBold();
            tsbItalic.Checked = IsItalic();
            tsbUnderline.Checked = IsUnderline();
            tsbStrikeThrough.Checked = IsStrikeThrough();
            tsbInsertOrderedList.Checked = IsOrderedList();
            tsbInsertUnorderedList.Checked = IsUnorderedList();
            tsbJustifyLeft.Checked = IsJustifyLeft();
            tsbJustifyCenter.Checked = IsJustifyCenter();
            tsbJustifyRight.Checked = IsJustifyRight();
            tsbJustifyFull.Checked = IsJustifyFull();
            tsbUndo.Enabled = IsUndo();
            tsbRedo.Enabled = IsRedo();
            UpdateFontComboBox();
            UpdateFontSizeComboBox();

            if (Tick != null)
                Tick();
        }

        /// <summary>
        /// Set up a key listener on the body once.
        /// The key listener checks for specific key strokes and takes 
        /// special action in certain cases.
        /// </summary>
        /// <returns></returns>
        private bool SetupKeyListener()
        {
            // handle enter code cancellation
            bool cancel = false;
            if (EnterKeyEvent != null)
            {
                var args = new EnterKeyEventArgs();
                EnterKeyEvent(this, args);
                cancel = args.Cancel;
            }
            return cancel;
        }

        /// <summary>
        /// Determine whether the current block is left justified.
        /// </summary>
        /// <returns>true if left justified, otherwise false</returns>
        public bool IsJustifyLeft()
        {
            return _doc.queryCommandState("JustifyLeft");
        }

        /// <summary>
        /// Determine whether the current block is right justified.
        /// </summary>
        /// <returns>true if right justified, otherwise false</returns>
        public bool IsJustifyRight()
        {
            return _doc.queryCommandState("JustifyRight");
        }

        /// <summary>
        /// Determine whether the current block is center justified.
        /// </summary>
        /// <returns>true if center justified, false otherwise</returns>
        public bool IsJustifyCenter()
        {
            return _doc.queryCommandState("JustifyCenter");
        }

        /// <summary>
        /// Determine whether the current block is full justified.
        /// </summary>
        /// <returns>true if full justified, false otherwise</returns>
        public bool IsJustifyFull()
        {
            return _doc.queryCommandState("JustifyFull");
        }

        /// <summary>
        /// Determine whether the current selection is in Bold mode.
        /// </summary>
        /// <returns>whether or not the current selection is Bold</returns>
        public bool IsBold()
        {
            return _doc.queryCommandState("Bold");
        }

        /// <summary>
        /// Determine whether the current selection is in Italic mode.
        /// </summary>
        /// <returns>whether or not the current selection is Italicized</returns>
        public bool IsItalic()
        {
            return _doc.queryCommandState("Italic");
        }

        /// <summary>
        /// Determine whether the current selection is in Underline mode.
        /// </summary>
        /// <returns>whether or not the current selection is Underlined</returns>
        public bool IsUnderline()
        {
            return _doc.queryCommandState("Underline");
        }

        /// <summary>
        /// Determine whether the current selection is in StrikeThrough mode.
        /// </summary>
        /// <returns>whether or not the current selection is StrikeThrough</returns>
        public bool IsStrikeThrough()
        {
            return _doc.queryCommandState("StrikeThrough");
        }

        /// <summary>
        /// Determine whether the current paragraph is an ordered list.
        /// </summary>
        /// <returns>true if current paragraph is ordered, false otherwise</returns>
        public bool IsOrderedList()
        {
            return _doc.queryCommandState("InsertOrderedList");
        }

        /// <summary>
        /// Determine whether the current paragraph is an unordered list.
        /// </summary>
        /// <returns>true if current paragraph is ordered, false otherwise</returns>
        public bool IsUnorderedList()
        {
            return _doc.queryCommandState("InsertUnorderedList");
        }

        /// <summary>
        /// Determine whether the current block can undo.
        /// </summary>
        /// <returns>true if current block can undo, false otherwise</returns>
        public bool IsUndo()
        {
            return _doc.queryCommandEnabled("Undo");
        }

        /// <summary>
        /// Determine whether the current block can redo.
        /// </summary>
        /// <returns>true if current block can redo, false otherwise</returns>
        public bool IsRedo()
        {
            return _doc.queryCommandEnabled("Redo");
        }

        /// <summary>
        /// Update the font size combo box.
        /// Sets a flag to indicate that the combo box is updating, and should 
        /// not update the editor's selection.
        /// </summary>
        private void UpdateFontSizeComboBox()
        {
            if (!tscbFontSize.Focused)
            {
                int foo;
                switch (FontSize)
                {
                    case FontSize.One:
                        foo = 0;
                        break;
                    case FontSize.Two:
                        foo = 1;
                        break;
                    case FontSize.Three:
                        foo = 2;
                        break;
                    case FontSize.Four:
                        foo = 3;
                        break;
                    case FontSize.Five:
                        foo = 4;
                        break;
                    case FontSize.Six:
                        foo = 5;
                        break;
                    case FontSize.Seven:
                        foo = 6;
                        break;
                    case FontSize.NA:
                        foo = -1;
                        break;
                    default:
                        foo = 2;
                        break;
                }
                //string fontsize = Convert.ToString(foo);
                if (foo != tscbFontSize.SelectedIndex)
                {
                    _updatingFontSize = true;
                    tscbFontSize.SelectedIndex = foo;
                    _updatingFontSize = false;
                }
            }
        }

        /// <summary>
        /// Update the font combo box.
        /// Sets a flag to indicate that the combo box is updating, and should 
        /// not update the editor's selection.
        /// </summary>
        private void UpdateFontComboBox()
        {
            if (!tsfcbFontName.Focused)
            {
                FontFamily fam = FontName;
                if (fam != null)
                {
                    string fontname = fam.Name;
                    if (fontname != tsfcbFontName.Text)
                    {
                        _updatingFontName = true;
                        tsfcbFontName.Text = fontname;
                        _updatingFontName = false;
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set the current font size.
        /// </summary>
        [Browsable(false)]
        public FontSize FontSize
        {
            get
            {
                if (ReadyState != ReadyState.Complete)
                    return FontSize.NA;
                switch (_doc.queryCommandValue("FontSize").ToString())
                {
                    case "1":
                        return FontSize.One;
                    case "2":
                        return FontSize.Two;
                    case "3":
                        return FontSize.Three;
                    case "4":
                        return FontSize.Four;
                    case "5":
                        return FontSize.Five;
                    case "6":
                        return FontSize.Six;
                    case "7":
                        return FontSize.Seven;
                    default:
                        return FontSize.NA;
                }
            }
            set
            {
                int sz;
                switch (value)
                {
                    case FontSize.One:
                        sz = 1;
                        break;
                    case FontSize.Two:
                        sz = 2;
                        break;
                    case FontSize.Three:
                        sz = 3;
                        break;
                    case FontSize.Four:
                        sz = 4;
                        break;
                    case FontSize.Five:
                        sz = 5;
                        break;
                    case FontSize.Six:
                        sz = 6;
                        break;
                    case FontSize.Seven:
                        sz = 7;
                        break;
                    default:
                        sz = 7;
                        break;
                }
                if (wb.Document != null) wb.Document.ExecCommand("FontSize", false, sz.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Get/Set the current font name.
        /// </summary>
        [Browsable(false)]
        public FontFamily FontName
        {
            get
            {
                if (ReadyState != ReadyState.Complete)
                    return null;
                var name = _doc.queryCommandValue("FontName") as string;
                if (name == null) return null;
                return new FontFamily(name);
            }
            set
            {
                if (value != null)
                    if (wb.Document != null) wb.Document.ExecCommand("FontName", false, value.Name);
            }
        }

        #endregion

        private void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_doc.designMode == "Off")
                e.Cancel = true;
        }

        private void tsbAutoLayout_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(BodyInnerHTML))
                BodyInnerHTML = Regex.Replace(BodyInnerHTML, "(<P class=Para>)[\\s]*(&nbsp;){0,}[\\s]*", "$1　　",
                                              RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        private void tsbUndo_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Undo", false, null);
        }

        private void tsbRedo_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            wb.Document.ExecCommand("Redo", false, null);
        }

    }

    /// <summary>
    /// Enumeration of possible font sizes for the Editor component
    /// </summary>
    public enum FontSize
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        NA
    }

    public enum ReadyState
    {
        Uninitialized,
        Loading,
        Loaded,
        Interactive,
        Complete
    }
}
