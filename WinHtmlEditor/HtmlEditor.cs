using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
#if VS2010
using System.Linq;
#endif
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WinHtmlEditor.Properties;
using System.Diagnostics;
using mshtmlTextRange = mshtml.IHTMLTxtRange;
using mshtmlSelection = mshtml.IHTMLSelectionObject;
using mshtmlDocument = mshtml.HTMLDocument;
using mshtmlBody = mshtml.HTMLBody;
using mshtmlControlRange = mshtml.IHTMLControlRange;
using mshtmlElement = mshtml.IHTMLElement;
using mshtmlEventObject = mshtml.IHTMLEventObj;
using mshtmlElementCollection = mshtml.IHTMLElementCollection;
using mshtmlAnchorElement = mshtml.IHTMLAnchorElement;
using mshtmlDomNode = mshtml.IHTMLDOMNode;
using mshtmlStyle = mshtml.IHTMLStyle;
using mshtmlIHTMLDocument2 = mshtml.IHTMLDocument2;

using mshtmlTable = mshtml.IHTMLTable;
using mshtmlTableCaption = mshtml.IHTMLTableCaption;
using mshtmlTableRow = mshtml.IHTMLTableRow;
using Pavonis.COM;
using Pavonis.COM.IOleCommandTarget;
using System.IO.Compression;

namespace WinHtmlEditor
{
    [Docking(DockingBehavior.Ask), ComVisible(true), ClassInterface(ClassInterfaceType.AutoDispatch), ToolboxBitmap(typeof(HtmlEditor), "Resources.HTML.bmp")]
    public sealed partial class HtmlEditor : UserControl
    {
        /// <summary>
        /// Public event that is raised if an internal processing exception is found
        /// </summary>
        [Category("Exception"), Description("An Internal Processing Exception was encountered")]
        public event HtmlExceptionEventHandler HtmlException;

        /// <summary>
        /// Public event that is raised if navigation event is captured
        /// </summary>
        [Category("Navigation"), Description("A Navigation Event was encountered")]
        public event HtmlNavigationEventHandler HtmlNavigation;

        // browser html constan expressions
        private const string BLANK_HTML_PAGE = "about:blank";

        // define the tags being used by the application
        private const string ANCHOR_TAG = "A";
        private const string TABLE_TAG = "TABLE";
        private const string SELECT_TYPE_TEXT = "text";
        private const string SELECT_TYPE_CONTROL = "control";
        private const string SELECT_TYPE_NONE = "none";

        // define commands for mshtml execution execution
        private const string HTML_COMMAND_FONTNAME = "FontName";
        private const string HTML_COMMAND_FONTSIZE = "FontSize";
        private const string HTML_COMMAND_PRINT = "Print"; 
        private const string HTML_COMMAND_COPY = "Copy";
        private const string HTML_COMMAND_PASTE = "Paste";
        private const string HTML_COMMAND_CUT = "Cut";
        private const string HTML_COMMAND_DELETE = "Delete";
        private const string HTML_COMMAND_UNDO = "Undo";
        private const string HTML_COMMAND_REDO = "Redo";
        private const string HTML_COMMAND_REMOVE_FORMAT = "RemoveFormat";
        private const string HTML_COMMAND_JUSTIFY_CENTER = "JustifyCenter";
        private const string HTML_COMMAND_JUSTIFY_FULL = "JustifyFull";
        private const string HTML_COMMAND_JUSTIFY_LEFT = "JustifyLeft";
        private const string HTML_COMMAND_JUSTIFY_RIGHT = "JustifyRight";
        private const string HTML_COMMAND_UNDERLINE = "Underline";
        private const string HTML_COMMAND_ITALIC = "Italic";
        private const string HTML_COMMAND_BOLD = "Bold";
        private const string HTML_COMMAND_BACK_COLOR = "BackColor";
        private const string HTML_COMMAND_FORE_COLOR = "ForeColor";
        private const string HTML_COMMAND_STRIKE_THROUGH = "StrikeThrough";
        private const string HTML_COMMAND_CREATE_LINK = "CreateLink";
        private const string HTML_COMMAND_UNLINK = "Unlink";
        private const string HTML_COMMAND_INSERT_HORIZONTAL_RULE = "InsertHorizontalRule";
        private const string HTML_COMMAND_INSERT_IMAGE = "InsertImage";
        private const string HTML_COMMAND_OUTDENT = "Outdent";
        private const string HTML_COMMAND_INDENT = "Indent";
        private const string HTML_COMMAND_INSERT_UNORDERED_LIST = "InsertUnorderedList";
        private const string HTML_COMMAND_INSERT_ORDERED_LIST = "InsertOrderedList";
        private const string HTML_COMMAND_SUPERSCRIPT = "Superscript";
        private const string HTML_COMMAND_SUBSCRIPT = "Subscript";
        private const string HTML_COMMAND_SELECT_ALL = "SelectAll";

        // internal command constants
        private const string INTERNAL_COMMAND_NEW = "New";
        private const string INTERNAL_COMMAND_OPEN = "Open";
        private const string INTERNAL_COMMAND_PRINT = "Print";
        private const string INTERNAL_COMMAND_SAVE = "Save";
        private const string INTERNAL_COMMAND_PREVIEW = "Preview";
        private const string INTERNAL_COMMAND_SHOWHTML = "ShowHTML";
        private const string INTERNAL_COMMAND_COPY = "Copy";
        private const string INTERNAL_COMMAND_PASTE = "Paste";
        private const string INTERNAL_COMMAND_CUT = "Cut";
        private const string INTERNAL_COMMAND_DELETE = "Delete";
        private const string INTERNAL_COMMAND_UNDO = "Undo";
        private const string INTERNAL_COMMAND_REDO = "Redo";
        private const string INTERNAL_COMMAND_FIND = "Find";
        private const string INTERNAL_COMMAND_REMOVE_FORMAT = "RemoveFormat";
        private const string INTERNAL_COMMAND_JUSTIFYCENTER = "JustifyCenter";
        private const string INTERNAL_COMMAND_JUSTIFYFULL = "JustifyFull";
        private const string INTERNAL_COMMAND_JUSTIFYLEFT = "JustifyLeft";
        private const string INTERNAL_COMMAND_JUSTIFYRIGHT = "JustifyRight";
        private const string INTERNAL_COMMAND_UNDERLINE = "Underline";
        private const string INTERNAL_COMMAND_ITALIC = "Italic";
        private const string INTERNAL_COMMAND_BOLD = "Bold";
        private const string INTERNAL_COMMAND_BACKCOLOR = "BackColor";
        private const string INTERNAL_COMMAND_FORECOLOR = "ForeColor";
        private const string INTERNAL_COMMAND_STRIKETHROUGH = "StrikeThrough";
        private const string INTERNAL_COMMAND_CREATELINK = "CreateLink";
        private const string INTERNAL_COMMAND_UNLINK = "Unlink";
        private const string INTERNAL_COMMAND_INSERTTABLE = "InsertTable";
        private const string INTERNAL_COMMAND_TABLEPROPERTIES = "TableModify";
        private const string INTERNAL_COMMAND_TABLEINSERTROW = "TableInsertRow";
        private const string INTERNAL_COMMAND_TABLEDELETEROW = "TableDeleteRow";
        private const string INTERNAL_COMMAND_INSERTIMAGE = "InsertImage";
        private const string INTERNAL_COMMAND_INSERTHORIZONTALRULE = "InsertHorizontalRule";
        private const string INTERNAL_COMMAND_OUTDENT = "Outdent";
        private const string INTERNAL_COMMAND_INDENT = "Indent";
        private const string INTERNAL_COMMAND_INSERTUNORDEREDLIST = "InsertUnorderedList";
        private const string INTERNAL_COMMAND_INSERTORDEREDLIST = "InsertOrderedList";
        private const string INTERNAL_COMMAND_SUPERSCRIPT = "Superscript";
        private const string INTERNAL_COMMAND_SUBSCRIPT = "Subscript";
        private const string INTERNAL_COMMAND_SELECTALL = "SelectAll";
        private const string INTERNAL_COMMAND_WORDCOUNT = "WordCount";
        private const string INTERNAL_COMMAND_INSERTDATE = "InsertDate";
        private const string INTERNAL_COMMAND_INSERTTIME = "InsertTime";
        private const string INTERNAL_COMMAND_CLEARWORD = "ClearWord";
        private const string INTERNAL_COMMAND_AUTOLAYOUT = "AutoLayout";
        private const string INTERNAL_COMMAND_ABOUT = "About";

        // constants for displaying the HTML dialog
        private const string CONTENT_EDITABLE_INHERIT = "inherit";
        private const string HTML_TITLE_EDIT = "Edit Html";
        private const string DEFAULT_HTML_TEXT = "";

        private const string BODY_PARSE_PRE_EXPRESSION = @"(<body).*?(</body)";
        private const string BODY_PARSE_EXPRESSION = @"(?<bodyOpen>(<body).*?>)(?<innerBody>.*?)(?<bodyClose>(</body\s*>))";
        private const string BODY_DEFAULT_TAG = @"<Body></Body>";
        private const string BODY_TAG_PARSE_MATCH = @"${bodyOpen}${bodyClose}";
        private const string BODY_INNER_PARSE_MATCH = @"${innerBody}";

        // browser constants and commands
        private object EMPTY_PARAMETER;

        // internal body property values
        private bool _readOnly;
        private HtmlFontProperty _bodyFont;
        private Color _bodyBackColor;
        private Color _bodyForeColor;
        private int[] _customColors;
        private readonly NavigateActionOption _navigateWindow;
        private DisplayScrollBarOption _scrollBars;
        private bool _autoWordWrap;

        // default values used to reset values
        private readonly Color _defaultBodyBackColor;
        private readonly Color _defaultBodyForeColor;
        private readonly HtmlFontProperty _defaultFont;

        // internal property values
        private string _bodyText;
        private string _bodyHtml;
        private string _bodyUrl;
        private bool _toolbarVisible;
        private bool _statusbarVisible;
        private bool _wbVisible;

        // document and body elements
        private mshtmlDocument document;
        private mshtmlIHTMLDocument2 _doc;
        // find and replace internal text range
        private mshtmlTextRange _findRange;
        private volatile bool rebaseUrlsNeeded;
        private volatile bool loading;
        private volatile bool codeNavigate;
        private mshtmlBody body;
        private bool _updatingFontName;
        private bool _updatingFontSize;

        public class EnterKeyEventArgs : EventArgs
        {
            public EnterKeyEventArgs()
            {
                Cancel = false;
            }

            public bool Cancel { get; set; }
        }
        public event EventHandler<EnterKeyEventArgs> EnterKeyEvent;

        public HtmlEditor()
        {
            InitializeComponent();
            // define the default values
            // browser constants and commands
            EMPTY_PARAMETER = System.Reflection.Missing.Value;

            // default values used to reset values
            _defaultBodyBackColor = Color.White;
            _defaultBodyForeColor = Color.Black;
            _defaultFont = new HtmlFontProperty(Font);
            _navigateWindow = NavigateActionOption.Default;
            _scrollBars = DisplayScrollBarOption.Auto;
            _autoWordWrap = true;
            _toolbarVisible = true;
            _statusbarVisible = true;
            _wbVisible = true;
            _bodyText = DEFAULT_HTML_TEXT;
            _bodyHtml = DEFAULT_HTML_TEXT;

            _bodyBackColor = _defaultBodyBackColor;
            _bodyForeColor = _defaultBodyForeColor;
            _bodyFont = _defaultFont;

            // load the blank Html page to load the MsHtml object model
            BrowserCodeNavigate(BLANK_HTML_PAGE);

            // after load ensure document marked as editable
            ReadOnly = _readOnly;
            ScrollBars = _scrollBars;

            SetupComboFontSize();
            SynchFont(string.Empty);
        }

        /// <summary>
        /// Method used to navigate to the required page
        /// Call made sync using a loading variable
        /// </summary>
        private void BrowserCodeNavigate(string url)
        {
            // once navigated to the href page wait until successful
            // need to do this to ensure properties are all correctly set
            codeNavigate = true;
            loading = true;

            // perform the navigation
            wb.Navigate(url);

            // wait for the navigate to complete using the loading variable
            // DoEvents needs to be called to enable the DocumentComplete to execute
            while (loading)
            {
                Application.DoEvents();
                Thread.Sleep(0);
            }

        } //BrowserCodeNavigate

        [Description("The Inner HTML of the contents"), Category("Textual")]
        public string BodyInnerHTML
        {
            get
            {
                _bodyText = body.innerText;
                _bodyHtml = body.innerHTML;
                return _bodyHtml;
            }
            set
            {
                try
                {
                    // clear the defined body url
                    _bodyUrl = string.Empty;
                    if (value.IsNull()) value = string.Empty;
                    // set the body property
                    body.innerHTML = value;
                    // set the body text and html
                    _bodyText = body.innerText;
                    _bodyHtml = body.innerHTML;
                    // if needed rebase urls
                    RebaseAnchorUrl();
                }
                catch (Exception ex)
                {
                    throw new HtmlEditorException("Inner Html for the body cannot be set.", "SetInnerHtml", ex);
                }
            }
        }

        /// <summary>
        /// Method to remove references to about:blank from the anchors
        /// </summary>
        private void RebaseAnchorUrl()
        {
            if (rebaseUrlsNeeded)
            {
                // review the anchors and remove any references to about:blank
                mshtmlElementCollection anchors = body.getElementsByTagName(ANCHOR_TAG);
                foreach (mshtmlElement element in anchors)
                {
                    try
                    {
                        var anchor = (mshtmlAnchorElement)element;
                        string href = anchor.href;
                        // see if href need updating
                        if (!href.IsNull() && Regex.IsMatch(href, BLANK_HTML_PAGE, RegexOptions.IgnoreCase))
                        {
                            anchor.href = href.Replace(BLANK_HTML_PAGE, string.Empty);
                        }
                    }
                    catch (Exception)
                    {
                        // ignore any errors
                    }
                }
            }

        } //RebaseAnchorUrl

        /// <summary>
        /// Property defining the base text for the body
        /// The HTML value can be used at runtime
        /// </summary>
        [Category("Textual"), Description("Set the initial Body Text")]
        [DefaultValue(DEFAULT_HTML_TEXT)]
        public string BodyInnerText
        {
            get
            {
                _bodyText = body.innerText;
                _bodyHtml = body.innerHTML;
                return _bodyText;
            }
            set
            {
                try
                {
                    // clear the defined body url
                    _bodyUrl = string.Empty;
                    if (value.IsNull()) value = string.Empty;
                    // set the body property
                    body.innerText = value;
                    // set the body text and html
                    _bodyText = body.innerText;
                    _bodyHtml = body.innerHTML;
                }
                catch (Exception ex)
                {
                    throw new HtmlEditorException("Inner Text for the body cannot be set.", "SetInnerText", ex);
                }

            }

        } //BodyInnerText

        /// <summary>
        /// Property returning any Url that was used to load the current document
        /// </summary>
        [Category("Textual"), Description("Url used to load the Document")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string DocumentUrl
        {
            get
            {
                //return document.baseUrl;
                return _bodyUrl;
            }
        }

        [Category("Appearance"), DefaultValue("false"), Description("设置或获取一个值，决定按下回车键的时候是插入BR还是P")]
        public bool EnterToBR { get; set; }

        /// <summary>
        /// Property defining the editable status of the text
        /// </summary>
        [Category("RuntimeDisplay"), Description("Marks the content as ReadOnly")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                // define the document editable property
                body.contentEditable = (!_readOnly).ToString();
                // define the menu bar state
                tsTopToolBar.Enabled = (!_readOnly);
                // define whether the IE cntext menu should be shown
                wb.IsWebBrowserContextMenuEnabled = _readOnly;
            }

        } //ReadOnly

        /// <summary>
        /// Property defining the body tag of the html
        /// On set operation the body attributes are redefined
        /// </summary>
        [Category("Textual"), Description("Complete Document including Body Tag")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string BodyHtml
        {
            get
            {
                // set the read only property before return
                body.contentEditable = CONTENT_EDITABLE_INHERIT;
                string html = _doc.body.outerHTML.Trim();
                ReadOnly = _readOnly;
                return html;

            }
            set
            {
                // clear the defined body url
                _bodyUrl = string.Empty;

                // define some local working variables
                string bodyElement = string.Empty;
                string innerHtml = string.Empty;

                try
                {
                    // ensure have body open and close tags
                    if (Regex.IsMatch(value, BODY_PARSE_PRE_EXPRESSION, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline))
                    {
                        // define a regular expression for the Html Body parsing and obtain the match expression
                        var expression = new Regex(BODY_PARSE_EXPRESSION, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
                        var match = expression.Match(value);
                        // see if a match was found
                        if (match.Success)
                        {
                            // extract the body tag and the inner html
                            bodyElement = match.Result(BODY_TAG_PARSE_MATCH);
                            innerHtml = match.Result(BODY_INNER_PARSE_MATCH);
                            // remove whitespaces from the body and inner html tags
                            bodyElement = bodyElement.Trim();
                            innerHtml = innerHtml.Trim();
                        }
                    }
                    // ensure body was set
                    if (bodyElement == string.Empty)
                    {
                        // assume the Html given is an inner html with no body
                        bodyElement = BODY_DEFAULT_TAG;
                        innerHtml = value.Trim();
                    }

                    // first navigate to a blank page to reset the html header
                    BrowserCodeNavigate(BLANK_HTML_PAGE);

                    // replace the body tag with the one passed in
                    var oldBodyNode = (mshtmlDomNode)document.body;
                    var newBodyNode = (mshtmlDomNode)document.createElement(bodyElement);
                    oldBodyNode.replaceNode(newBodyNode);

                    // define the new inner html and body objects
                    body = (mshtmlBody)document.body;
                    body.innerHTML = innerHtml;

                    // now all successfully loaded need to review the body attributes
                    _bodyText = body.innerText;
                    _bodyHtml = body.innerHTML;

                    // set and define the appropriate properties
                    // this will set the appropriate read only property
                    DefineBodyAttributes();

                    // if needed rebase urls
                    RebaseAnchorUrl();
                }
                catch (Exception ex)
                {
                    throw new HtmlEditorException("Outer Html for the body cannot be set.", "SetBodyHtml", ex);
                }
            }

        } //BodyHtml

        /// <summary>
        /// Defines all the body attributes once a document has been loaded
        /// </summary>
        private void DefineBodyAttributes()
        {
            // define the body colors based on the new body html
            _bodyBackColor = GeneralUtil.IsNull(body.bgColor) ? _defaultBodyBackColor : ColorTranslator.FromHtml((string)body.bgColor);
            _bodyForeColor = GeneralUtil.IsNull(body.text) ? _defaultBodyForeColor : ColorTranslator.FromHtml((string)body.text);

            // define the font object based on current font of new document
            // deafult used unless a style on the body modifies the value
            mshtmlStyle bodyStyle = body.style;
            if (!bodyStyle.IsNull())
            {
                string fontName = _bodyFont.Name;
                HtmlFontSize fontSize = _bodyFont.Size;
                bool fontBold = _bodyFont.Bold;
                bool fontItalic = _bodyFont.Italic;
                // define the font name if defined in the style
                if (!bodyStyle.fontFamily.IsNull()) fontName = bodyStyle.fontFamily;
                if (!GeneralUtil.IsNull(bodyStyle.fontSize)) fontSize = HtmlFontConversion.StyleSizeToHtml(bodyStyle.fontSize.ToString());
                if (!bodyStyle.fontWeight.IsNull()) fontBold = HtmlFontConversion.IsStyleBold(bodyStyle.fontWeight);
                if (!bodyStyle.fontStyle.IsNull()) fontItalic = HtmlFontConversion.IsStyleItalic(bodyStyle.fontStyle);
                bool fontUnderline = bodyStyle.textDecorationUnderline;
                // define the new font object and set the property
                _bodyFont = new HtmlFontProperty(fontName, fontSize, fontBold, fontItalic, fontUnderline);
                BodyFont = _bodyFont;
            }

            // define the content based on the current value
            ReadOnly = _readOnly;
            ScrollBars = _scrollBars;
            AutoWordWrap = _autoWordWrap;

        } //DefineBodyAttributes

        /// <summary>
        /// Property defining whether scroll bars should be displayed
        /// </summary>
        [Category("RuntimeDisplay"), Description("Controls the Display of Scrolls Bars")]
        [DefaultValue(DisplayScrollBarOption.Auto)]
        public DisplayScrollBarOption ScrollBars
        {
            get
            {
                return _scrollBars;
            }
            set
            {
                _scrollBars = value;
                // define the document scroll bar visibility
                body.scroll = _scrollBars.ToString();
            }

        } //ScrollBars

        /// <summary>
        /// Property defining whether words will be auto wrapped
        /// </summary>
        [Category("RuntimeDisplay"), Description("Controls the auto wrapping of content")]
        [DefaultValue(true)]
        public bool AutoWordWrap
        {
            get
            {
                return _autoWordWrap;
            }
            set
            {
                _autoWordWrap = value;
                // define the document word wrap property
                body.noWrap = !_autoWordWrap;
            }

        } //AutoWordWrap

        /// <summary>
        /// Property for the base font to use for text editing
        /// Reset and Serialize values defined
        /// </summary>
        [Category("Textual"), Description("Defines the base Font object for the Body")]
        [RefreshProperties(RefreshProperties.All)]
        public HtmlFontProperty BodyFont
        {
            get
            {
                return _bodyFont;
            }
            set
            {
                // set the new value using the default if set to null
                _bodyFont = HtmlFontProperty.IsNotNull(value) ? value : _defaultFont;
                // set the font attributes based on any body styles
                mshtmlStyle bodyStyle = body.style;
                if (!bodyStyle.IsNull())
                {
                    if (HtmlFontProperty.IsEqual(_bodyFont, _defaultFont))
                    {
                        // ensure no values are set in the Body style
                        if (!bodyStyle.fontFamily.IsNull()) bodyStyle.fontFamily = string.Empty;
                        if (!GeneralUtil.IsNull(bodyStyle.fontSize)) bodyStyle.fontSize = string.Empty;
                        if (!bodyStyle.fontWeight.IsNull()) bodyStyle.fontWeight = string.Empty;
                    }
                    else
                    {
                        // set the body styles based on the defined value
                        bodyStyle.fontFamily = _bodyFont.Name;
                        bodyStyle.fontSize = HtmlFontConversion.HtmlFontSizeString(_bodyFont.Size);
                        bodyStyle.fontWeight = HtmlFontConversion.HtmlFontBoldString(_bodyFont.Bold);
                    }
                }
            }

        } //BodyFont

        public bool ShouldSerializeBodyFont()
        {
            return (HtmlFontProperty.IsNotEqual(_bodyFont, _defaultFont));

        } //ShouldSerializeBodyFont

        public void ResetBodyFont()
        {
            this.BodyFont = _defaultFont;

        } //ResetBodyFont

        [Category("Appearance"), Description("Controls the visible of ToolBar"), DefaultValue("true")]
        public bool ShowToolBar
        {
            get
            {
                return _toolbarVisible;
            }
            set
            {
                _toolbarVisible = value;
                this.tsTopToolBar.Visible = _toolbarVisible;
            }
        }

        [Category("Appearance"), Description("Controls the visible of StatusStrip"), DefaultValue("true")]
        public bool ShowStatusBar
        {
            get
            {
                return _statusbarVisible;
            }
            set
            {
                _statusbarVisible = value;
                ssHtml.Visible = _statusbarVisible;
            }
        }

        [Category("Appearance"), Description("Controls the visible of html editor"), DefaultValue("true")]
        public bool ShowWb
        {
            get
            {
                return _wbVisible;
            }
            set
            {
                _wbVisible = value;
                wb.Visible = _wbVisible;
            }
        }

        [Category("Appearance"), Description("Controls the enable of shortcut key"), DefaultValue("true")]
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

        #region public method

        #region Find and Replace Operations

        /// <summary>
        /// Dialog to allow the user to perform a find and replace
        /// </summary>
        public void FindReplacePrompt(bool findOrReplace)
        {
            // define a default value for the text to find
            mshtmlTextRange range = GetTextRange();
            string initText = string.Empty;
            if (!range.IsNull())
            {
                string findText = range.text;
                if (!findText.IsNull()) initText = findText.Trim();
            }

            // prompt the user for the new href
            using (var dialog = new FindReplaceForm(initText,
                       FindReplaceReset,
                       FindFirst,
                       FindNext,
                       FindReplaceOne,
                       FindReplaceAll, findOrReplace))
            {
                DefineDialogProperties(dialog);
                dialog.ShowDialog(ParentForm);
            }
        } //FindReplacePrompt


        /// <summary>
        /// Method to reset the find and replace options to initialize a new search
        /// </summary>
        public void FindReplaceReset()
        {
            // reset the range being worked with
            _findRange = body.createTextRange();
            document.selection.empty();

        } //FindReplaceReset


        /// <summary>
        /// Method to find the first occurrence of the given text string
        /// Uses false case for the search options
        /// </summary>
        public bool FindFirst(string findText)
        {
            return FindFirst(findText, false, false);

        } //FindFirst

        /// <summary>
        /// Method to find the first occurrence of the given text string
        /// </summary>
        public bool FindFirst(string findText, bool matchWhole, bool matchCase)
        {
            // reset the text search range prior to making any calls
            FindReplaceReset();

            // calls the Find Next once search has been initialized
            return FindNext(findText, matchWhole, matchCase);

        } //FindFirst


        /// <summary>
        /// Method to find the next occurrence of a given text string
        /// Assumes a previous search was made
        /// Uses false case for the search options
        /// </summary>
        public bool FindNext(string findText)
        {
            return FindNext(findText, false, false);

        } //FindNext

        /// <summary>
        /// Method to find the next occurrence of a given text string
        /// Assumes a previous search was made
        /// </summary>
        public bool FindNext(string findText, bool matchWhole, bool matchCase)
        {
            return (!FindText(findText, matchWhole, matchCase).IsNull());

        } //FindNext


        /// <summary>
        /// Replace the first occurrence of the given string with the other
        /// Uses false case for the search options
        /// </summary>
        public bool FindReplaceOne(string findText, string replaceText)
        {
            return FindReplaceOne(findText, replaceText);

        } //FindReplaceOne

        /// <summary>
        /// Method to replace the first occurrence of the given string with the other
        /// </summary>
        public bool FindReplaceOne(string findText, string replaceText, bool matchWhole, bool matchCase)
        {
            // find the given text within the find range
            mshtmlTextRange replaceRange = FindText(findText, matchWhole, matchCase);
            if (!replaceRange.IsNull())
            {
                // if text found perform a replace
                replaceRange.text = replaceText;
                replaceRange.select();
                // replace made to return success
                return true;
            }
            // no replace was made so return false
            return false;
        } //FindReplaceOne


        /// <summary>
        /// Method to replace all the occurrence of the given string with the other
        /// Uses false case for the search options
        /// </summary>
        public int FindReplaceAll(string findText, string replaceText)
        {
            return FindReplaceAll(findText, replaceText, false, false);

        } //FindReplaceAll

        /// <summary>
        /// Method to replace all the occurrences of the given string with the other
        /// </summary>
        public int FindReplaceAll(string findText, string replaceText, bool matchWhole, bool matchCase)
        {
            int found = 0;
            mshtmlTextRange replaceRange;

            do
            {
                // find the given text within the find range
                replaceRange = FindText(findText, matchWhole, matchCase);
                // if found perform a replace
                if (!replaceRange.IsNull())
                {
                    replaceRange.text = replaceText;
                    found++;
                }
            } while (!replaceRange.IsNull());

            // return count of items repalced
            return found;

        } //FindReplaceAll


        /// <summary>
        /// Method to perform the actual find of the given text
        /// </summary>
        private mshtmlTextRange FindText(string findText, bool matchWhole, bool matchCase)
        {
            // define the search options
            int searchOptions = 0;
            if (matchWhole) searchOptions = searchOptions + 2;
            if (matchCase) searchOptions = searchOptions + 4;

            // perform the search operation
            if (!_findRange.text.IsNull() && _findRange.findText(findText, _findRange.text.Length, searchOptions))
            {
                // select the found text within the document
                _findRange.select();
                // limit the new find range to be from the newly found text
                var foundRange = (mshtmlTextRange)document.selection.createRange();
                _findRange = body.createTextRange();
                _findRange.setEndPoint("StartToEnd", foundRange);
                // text found so return this selection
                return foundRange;
            }
            // reset the find ranges
            FindReplaceReset();
            // no text found so return null range
            return null;
        } //FindText 

        #endregion

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
        /// 重置为空白页
        /// </summary>
        public void Navigate()
        {
            NavigateToUrl("about:blank");
        }

        /// <summary>
        /// 将指定的统一资源定位符 (URL) 处的文档加载到 System.Windows.Forms.WebBrowser 控件中，替换上一个文档。
        /// </summary>
        /// <param name="url">指定的统一资源定位符 (URL)</param>
        public void NavigateToUrl(string url)
        {
            wb.Navigate(url);
        }

        /// <summary>
        /// Method to allow the user to load a document by navigation
        /// A new window can optionally be specified
        /// </summary>
        public void NavigateToUrl(string url, bool newWindow)
        {
            if (newWindow)
            {
                // open the Url in a new window
                wb.Navigate(url, true);
            }
            else
            {
                // if no new window required call the normal navigate method
                NavigateToUrl(url);
            }

        } //NavigateToUrl

        /// <summary>
        /// The currently selected text/controls will be replaced by the given HTML code.
        /// If nothing is selected, the HTML code is inserted at the cursor position
        /// </summary>
        /// <param name="sHtml"></param>
        public void PasteIntoSelection(string sHtml)
        {
            HTMLEditHelper.DOMDocument = _doc;
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
            return wb.Document.Body.InnerText.IsNull()
                       ? 0
                       : Regex.Split(wb.Document.Body.InnerText, @"\s").
                             Sum(si => Regex.Matches(si, @"[\u0000-\u00ff]+").Count + si.Count(c => (int) c > 0x00FF));
#else
            if (wb.Document.Body.InnerText.IsNull())
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
        /// Method to allow the user to view the html contents
        /// </summary>
        public void ShowHTML()
        {
            using (var dialog = new EditHtmlForm())
            {
                dialog.HTML = BodyInnerHTML;
                dialog.ReadOnly = false;
                dialog.SetCaption(HTML_TITLE_EDIT);
                DefineDialogProperties(dialog);
                if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
                {
                    BodyInnerHTML = dialog.HTML;
                }
            }
        }

        /// <summary>
        /// Method to allow the user to preview the html page
        /// </summary>
        public void Preview()
        {
            bool isPreview = (body.contentEditable == "true");
            for (int i = 0; i < tsTopToolBar.Items.Count; i++)
            {
                tsTopToolBar.Items[i].Enabled = !isPreview;
            }
            if(isPreview)
            {
                body.contentEditable = "false";
                cmsHtml.Enabled = false;
                tsbPreview.Enabled = true;
            }
            else
            {
                cmsHtml.Enabled = true;
                body.contentEditable = "true";
            }
        }

        /// <summary>
        /// New document
        /// </summary>
        public void New()
        {
            BodyHtml = "";
        } //New

        /// <summary>
        /// Method to allow the user to persist the Html stream to a file
        /// </summary>
        public void Save()
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
                    var writer = new StreamWriter(dialog.FileName,false,Encoding.UTF8);
                    writer.Write(BodyHtml);
                    writer.Close();
                }
            }
        } //New

        /// <summary>
        /// Method to allow the user to select a file and read the contents into the Html stream
        /// </summary>
        public void Open()
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
                    BodyHtml = reader.ReadToEnd();
                    reader.Close();
                }
            }
        } //Open

        /// <summary>
        /// Method to print the html text using the document print command
        /// Print preview is not supported
        /// </summary>
        public void Print()
        {
            ExecuteCommandDocument(HTML_COMMAND_PRINT);

        } //Print

        /// <summary>
        /// Method to copy the currently selected text to the clipboard
        /// </summary>
        public void Copy()
        {
            ExecuteCommandDocument(HTML_COMMAND_COPY);
        } //Copy

        /// <summary>
        /// Method to cut the currently selected text to the clipboard
        /// </summary>
        public void Cut()
        {
            ExecuteCommandDocument(HTML_COMMAND_CUT);
        } //Cut

        /// <summary>
        /// Method to paste the currently selected text from the clipboard
        /// </summary>
        public void Paste()
        {
            ExecuteCommandDocument(HTML_COMMAND_PASTE);
        } //Paste

        /// <summary>
        /// Method to delete the currently selected text from the screen
        /// </summary>
        public void Delete()
        {
            ExecuteCommandDocument(HTML_COMMAND_DELETE);
        } //Delete

        /// <summary>
        /// Method to undo former commands
        /// </summary>
        public void Undo()
        {
            ExecuteCommandDocument(HTML_COMMAND_UNDO);
            FormatSelectionChange();
        } //Undo

        /// <summary>
        /// Method to redo former undo
        /// </summary>
        public void Redo()
        {
            ExecuteCommandDocument(HTML_COMMAND_REDO);
            FormatSelectionChange();
        } //Redo

        /// <summary>
        /// Method to find string
        /// </summary>
        public void Find()
        {
            Focus();
            SendKeys.SendWait("^f");
        }

        /// <summary>
        /// Method to remove format
        /// </summary>
        public void RemoveFormat()
        {
            ExecuteCommandDocument(HTML_COMMAND_REMOVE_FORMAT);
        } //RemoveFormat

        /// <summary>
        /// Method to define the font justification as CENTER
        /// </summary>
        public void JustifyCenter()
        {
            ExecuteCommandRange(HTML_COMMAND_JUSTIFY_CENTER, null);
            FormatSelectionChange();
        } //JustifyCenter

        /// <summary>
        /// Method to define the font justification as FULL
        /// </summary>
        public void JustifyFull()
        {
            ExecuteCommandRange(HTML_COMMAND_JUSTIFY_FULL, null);
            FormatSelectionChange();
        } //JustifyFull

        /// <summary>
        /// Method to define the font justification as LEFT
        /// </summary>
        public void JustifyLeft()
        {
            ExecuteCommandRange(HTML_COMMAND_JUSTIFY_LEFT, null);
            FormatSelectionChange();
        } //JustifyLeft

        /// <summary>
        /// Method to define the font justification as RIGHT
        /// </summary>
        public void JustifyRight()
        {
            ExecuteCommandRange(HTML_COMMAND_JUSTIFY_RIGHT, null);
            FormatSelectionChange();
        } //JustifyRight

        /// <summary>
        /// Method using the document to toggle the selection with a underline tag
        /// </summary>
        public void Underline()
        {
            ExecuteCommandRange(HTML_COMMAND_UNDERLINE, null);
            FormatSelectionChange();
        } //Underline

        /// <summary>
        /// Method using the document to toggle the selection with a italic tag
        /// </summary>
        public void Italic()
        {
            ExecuteCommandRange(HTML_COMMAND_ITALIC, null);
            FormatSelectionChange();
        } //Italic

        /// <summary>
        /// Method using the document to toggle the selection with a bold tag
        /// </summary>
        public void Bold()
        {
            ExecuteCommandRange(HTML_COMMAND_BOLD, null);
            FormatSelectionChange();

        } //Bold

        /// <summary>
        /// Method using the exec command to define the back color properties for the selected tag
        /// </summary>
        public void FormatBackColor(Color color)
        {
            // Use the COLOR object to set the property BackColor
            string colorHtml = color != Color.Empty ? ColorTranslator.ToHtml(color) : null;
            ExecuteCommandRange(HTML_COMMAND_BACK_COLOR, colorHtml);
        }

        /// <summary>
        /// Method to display the system color dialog
        /// Use use to set the selected text Back Color
        /// </summary>
        public void FormatBackColorPrompt()
        {
            // display the Color dialog and use the selected color to modify text
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.AnyColor = true;
                colorDialog.SolidColorOnly = true;
                colorDialog.AllowFullOpen = true;
                colorDialog.Color = GetBackColor();
                colorDialog.CustomColors = _customColors;
                if (colorDialog.ShowDialog(ParentForm) == DialogResult.OK)
                {
                    _customColors = colorDialog.CustomColors;
                    FormatBackColor(colorDialog.Color);
                }
            }

        } //FormatBackColorPrompt

        /// <summary>
        /// Method using the exec command to define the color properties for the selected tag
        /// </summary>
        public void FormatFontColor(Color color)
        {
            // Use the COLOR object to set the property ForeColor
            string colorHtml = color != Color.Empty ? ColorTranslator.ToHtml(color) : null;
            ExecuteCommandRange(HTML_COMMAND_FORE_COLOR, colorHtml);
        } //FormatFontColor

        /// <summary>
        /// Method to display the system color dialog
        /// Use use to set the selected text Color
        /// </summary>
        public void FormatFontColorPrompt()
        {
            // display the Color dialog and use the selected color to modify text
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.AnyColor = true;
                colorDialog.SolidColorOnly = true;
                colorDialog.AllowFullOpen = true;
                colorDialog.Color = GetFontColor();
                colorDialog.CustomColors = _customColors;
                if (colorDialog.ShowDialog(ParentForm) == DialogResult.OK)
                {
                    _customColors = colorDialog.CustomColors;
                    FormatFontColor(colorDialog.Color);
                }
            }

        } //FormatFontColorPrompt

        /// <summary>
        /// Method using the document to toggle the selection with a StrikeThrough tag
        /// </summary>
        public void StrikeThrough()
        {
            ExecuteCommandRange(HTML_COMMAND_STRIKE_THROUGH, null);
            FormatSelectionChange();
        } //StrikeThrough

        /// <summary>
        /// Method using the document to create link
        /// </summary>
        public void CreateLink()
        {
            ExecuteCommandRange(HTML_COMMAND_CREATE_LINK, null);
        } //CreateLink

        /// <summary>
        /// Method using the document to remove link
        /// </summary>
        public void UnLink()
        {
            ExecuteCommandRange(HTML_COMMAND_UNLINK, null);
            FormatSelectionChange();
        } //UnLink

        /// <summary>
        /// Method to insert a image and prompt a user for the link
        /// Calls the public InsertImage method
        /// </summary>
        public void InsertImage()
        {
            ExecuteCommandDocumentPrompt(HTML_COMMAND_INSERT_IMAGE);
        } //InsertImage

        /// <summary>
        /// 
        /// </summary>
        public void AutoLayout()
        {
            if (!BodyInnerHTML.IsNullOrEmpty())
                BodyInnerHTML = Regex.Replace(BodyInnerHTML, "(<P.*?>)[\\s]*(&nbsp;){0,}[\\s]*", "$1　　",
                                              RegexOptions.IgnoreCase | RegexOptions.Multiline);
        } //AutoLayout

        /// <summary>
        /// Create a new focus method that ensure the body gets the focus
        /// Should be called when text processing command are called
        /// </summary>
        public new bool Focus()
        {
            // have the return value be the focus return from the user control
            bool focus = base.Focus();
            // try to set the focus to the web browser
            try
            {
                wb.Focus();
                if (!body.IsNull()) body.focus();
            }
            catch (Exception)
            {
                // ignore errors
            }
            return focus;

        } //Focus

        #region Table Processing Operations

        /// <summary>
        /// Method to create a table class
        /// Insert method then works on this table
        /// </summary>
        public void TableInsert(HtmlTableProperty tableProperties)
        {
            // call the private insert table method with a null table entry
            ProcessTable(null, tableProperties);

        } //TableInsert

        /// <summary>
        /// Method to modify a tables properties
        /// Ensure a table is currently selected or insertion point is within a table
        /// </summary>
        public bool TableModify(HtmlTableProperty tableProperties)
        {
            // define the Html Table element
            mshtmlTable table = GetTableElement();

            // if a table has been selected then process
            if (!table.IsNull())
            {
                ProcessTable(table, tableProperties);
                return true;
            }
            return false;
        } //TableModify

        /// <summary>
        /// Method to present to the user the table properties dialog
        /// Uses all the default properties for the table based on an insert operation
        /// </summary>
        public void TableInsertPrompt()
        {
            // if user has selected a table create a reference
            var table = GetFirstControl() as mshtmlTable;
            ProcessTablePrompt(table);

        } //TableInsertPrompt

        /// <summary>
        /// Method to present to the user the table properties dialog
        /// Ensure a table is currently selected or insertion point is within a table
        /// </summary>
        public bool TableModifyPrompt()
        {
            // define the Html Table element
            mshtmlTable table = GetTableElement();

            // if a table has been selected then process
            if (!table.IsNull())
            {
                ProcessTablePrompt(table);
                return true;
            }
            return false;
        } //TableModifyPrompt

        /// <summary>
        /// Method to insert a new row into the table
        /// Based on the current user row and insertion after
        /// </summary>
        public void TableInsertRow()
        {
            // see if a table selected or insertion point inside a table
            mshtmlTable table;
            mshtmlTableRow row;
            GetTableElement(out table, out row);

            // process according to table being defined
            if (!table.IsNull() && !row.IsNull())
            {
                try
                {
                    // find the existing row the user is on and perform the insertion
                    int index = row.rowIndex + 1;
                    var insertedRow = (mshtmlTableRow)table.insertRow(index);
                    // add the new columns to the end of each row
                    int numberCols = row.cells.length;
                    for (int idxCol = 0; idxCol < numberCols; idxCol++)
                    {
                        insertedRow.insertCell();
                    }
                }
                catch (Exception ex)
                {
                    throw new HtmlEditorException("Unable to insert a new Row", "TableinsertRow", ex);
                }
            }
            else
            {
                throw new HtmlEditorException("Row not currently selected within the table", "TableInsertRow");
            }

        } //TableInsertRow

        /// <summary>
        /// Method to delete the currently selected row
        /// Operation based on the current user row location
        /// </summary>
        public void TableDeleteRow()
        {
            // see if a table selected or insertion point inside a table
            mshtmlTable table;
            mshtmlTableRow row;
            GetTableElement(out table, out row);

            // process according to table being defined
            if (!table.IsNull() && !row.IsNull())
            {
                try
                {
                    // find the existing row the user is on and perform the deletion
                    int index = row.rowIndex;
                    table.deleteRow(index);
                }
                catch (Exception ex)
                {
                    throw new HtmlEditorException("Unable to delete the selected Row", "TableDeleteRow", ex);
                }
            }
            else
            {
                throw new HtmlEditorException("Row not currently selected within the table", "TableDeleteRow");
            }

        } //TableDeleteRow

        /// <summary>
        /// Method to present to the user the table properties dialog
        /// Uses all the default properties for the table based on an insert operation
        /// </summary>
        private void ProcessTablePrompt(mshtmlTable table)
        {
            using (var dialog = new TablePropertyForm())
            {
                // define the base set of table properties
                HtmlTableProperty tableProperties = GetTableProperties(table);

                // set the dialog properties
                dialog.TableProperties = tableProperties;
                DefineDialogProperties(dialog);
                // based on the user interaction perform the neccessary action
                if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
                {
                    tableProperties = dialog.TableProperties;
                    if (table.IsNull()) TableInsert(tableProperties);
                    else ProcessTable(table, tableProperties);
                }
            }

        } // ProcessTablePrompt

        /// <summary>
        /// Method to insert a basic table
        /// Will honour the existing table if passed in
        /// </summary>
        private void ProcessTable(mshtmlTable table, HtmlTableProperty tableProperties)
        {
            try
            {
                // obtain a reference to the body node and indicate table present
                var bodyNode = (mshtmlDomNode)document.body;
                bool tableCreated = false;

                // ensure a table node has been defined to work with
                if (table.IsNull())
                {
                    // create the table and indicate it was created
                    table = (mshtmlTable)document.createElement(TABLE_TAG);
                    tableCreated = true;
                }

                // define the table border, width, cell padding and spacing
                table.border = tableProperties.BorderSize;
                if (tableProperties.TableWidth > 0) table.width = (tableProperties.TableWidthMeasurement == MeasurementOption.Pixel) ? string.Format("{0}", tableProperties.TableWidth) : string.Format("{0}%", tableProperties.TableWidth);
                else table.width = string.Empty;
                table.align = tableProperties.TableAlignment != HorizontalAlignOption.Default ? tableProperties.TableAlignment.ToString().ToLower() : string.Empty;
                table.cellPadding = tableProperties.CellPadding.ToString(CultureInfo.InvariantCulture);
                table.cellSpacing = tableProperties.CellSpacing.ToString(CultureInfo.InvariantCulture);

                // define the given table caption and alignment
                string caption = tableProperties.CaptionText;
                mshtmlTableCaption tableCaption = table.caption;
                if (!caption.IsNullOrEmpty())
                {
                    // ensure table caption correctly defined
                    if (tableCaption.IsNull()) tableCaption = table.createCaption();
                    ((mshtmlElement)tableCaption).innerText = caption;
                    if (tableProperties.CaptionAlignment != HorizontalAlignOption.Default) tableCaption.align = tableProperties.CaptionAlignment.ToString().ToLower();
                    if (tableProperties.CaptionLocation != VerticalAlignOption.Default) tableCaption.vAlign = tableProperties.CaptionLocation.ToString().ToLower();
                }
                else
                {
                    // if no caption specified remove the existing one
                    if (!tableCaption.IsNull())
                    {
                        // prior to deleting the caption the contents must be cleared
                        ((mshtmlElement)tableCaption).innerText = null;
                        table.deleteCaption();
                    }
                }

                // determine the number of rows one has to insert
                int numberRows, numberCols;
                if (tableCreated)
                {
                    numberRows = Math.Max((int)tableProperties.TableRows, 1);
                }
                else
                {
                    numberRows = Math.Max((int)tableProperties.TableRows, 1) - table.rows.length;
                }

                // layout the table structure in terms of rows and columns
                table.cols = tableProperties.TableColumns;
                if (tableCreated)
                {
                    // this section is an optimization based on creating a new table
                    // the section below works but not as efficiently
                    numberCols = Math.Max((int)tableProperties.TableColumns, 1);
                    // insert the appropriate number of rows
                    mshtmlTableRow tableRow;
                    for (int idxRow = 0; idxRow < numberRows; idxRow++)
                    {
                        tableRow = (mshtmlTableRow)table.insertRow();
                        // add the new columns to the end of each row
                        for (int idxCol = 0; idxCol < numberCols; idxCol++)
                        {
                            tableRow.insertCell();
                        }
                    }
                }
                else
                {
                    // if the number of rows is increasing insert the decrepency
                    if (numberRows > 0)
                    {
                        // insert the appropriate number of rows
                        for (int idxRow = 0; idxRow < numberRows; idxRow++)
                        {
                            table.insertRow();
                        }
                    }
                    else
                    {
                        // remove the extra rows from the table
                        for (int idxRow = numberRows; idxRow < 0; idxRow++)
                        {
                            table.deleteRow(table.rows.length - 1);
                        }
                    }
                    // have the rows constructed
                    // now ensure the columns are correctly defined for each row
                    mshtmlElementCollection rows = table.rows;
                    foreach (mshtmlTableRow tableRow in rows)
                    {
                        numberCols = Math.Max((int)tableProperties.TableColumns, 1) - tableRow.cells.length;
                        if (numberCols > 0)
                        {
                            // add the new column to the end of each row
                            for (int idxCol = 0; idxCol < numberCols; idxCol++)
                            {
                                tableRow.insertCell();
                            }
                        }
                        else
                        {
                            // reduce the number of cells in the given row
                            // remove the extra rows from the table
                            for (int idxCol = numberCols; idxCol < 0; idxCol++)
                            {
                                tableRow.deleteCell(tableRow.cells.length - 1);
                            }
                        }
                    }
                }

                // if the table was created then it requires insertion into the DOM
                // otherwise property changes are sufficient
                if (tableCreated)
                {
                    // table processing all complete so insert into the DOM
                    var tableNode = (mshtmlDomNode)table;
                    var tableElement = (mshtmlElement)table;
                    mshtmlTextRange textRange = GetTextRange();
                    // final insert dependant on what user has selected
                    if (!textRange.IsNull())
                    {
                        // text range selected so overwrite with a table
                        try
                        {
                            string selectedText = textRange.text;
                            if (!selectedText.IsNull())
                            {
                                // place selected text into first cell
                                var tableRow = (mshtmlTableRow)table.rows.item(0, null);
                                ((mshtmlElement)tableRow.cells.item(0, null)).innerText = selectedText;
                            }
                            textRange.pasteHTML(tableElement.outerHTML);
                        }
                        catch (Exception ex)
                        {
                            throw new HtmlEditorException("Invalid Text selection for the Insertion of a Table.", "ProcessTable", ex);
                        }
                    }
                    else
                    {
                        mshtmlControlRange controlRange = GetAllControls();
                        if (!controlRange.IsNull())
                        {
                            // overwrite any controls the user has selected
                            try
                            {
                                // clear the selection and insert the table
                                // only valid if multiple selection is enabled
                                for (int idx = 1; idx < controlRange.length; idx++)
                                {
                                    controlRange.remove(idx);
                                }
                                controlRange.item(0).outerHTML = tableElement.outerHTML;
                                // this should work with initial count set to zero
                                // controlRange.add((mshtmlControlElement)table);
                            }
                            catch (Exception ex)
                            {
                                throw new HtmlEditorException("Cannot Delete all previously Controls selected.", "ProcessTable", ex);
                            }
                        }
                        else
                        {
                            // insert the table at the end of the HTML
                            bodyNode.appendChild(tableNode);
                        }
                    }
                }
                else
                {
                    // table has been correctly defined as being the first selected item
                    // need to remove other selected items
                    mshtmlControlRange controlRange = GetAllControls();
                    if (!controlRange.IsNull())
                    {
                        // clear the controls selected other than than the first table
                        // only valid if multiple selection is enabled
                        for (int idx = 1; idx < controlRange.length; idx++)
                        {
                            controlRange.remove(idx);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // throw an exception indicating table structure change error
                throw new HtmlEditorException("Unable to modify Html Table properties.", "ProcessTable", ex);
            }

        } //ProcessTable

        /// <summary>
        /// Method to determine if the current selection is a table
        /// If found will return the table element
        /// </summary>
        private void GetTableElement(out mshtmlTable table, out mshtmlTableRow row)
        {
            row = null;
            mshtmlTextRange range = GetTextRange();

            try
            {
                // first see if the table element is selected
                table = GetFirstControl() as mshtmlTable;
                // if table not selected then parse up the selection tree
                if (table.IsNull() && !range.IsNull())
                {
                    var element = range.parentElement();
                    // parse up the tree until the table element is found
                    while (!element.IsNull() && table.IsNull())
                    {
                        element = element.parentElement;
                        // extract the Table properties
                        var htmlTable = element as mshtmlTable;
                        if (!htmlTable.IsNull())
                        {
                            table = htmlTable;
                        }
                        // extract the Row  properties
                        var htmlTableRow = element as mshtmlTableRow;
                        if (!htmlTableRow.IsNull())
                        {
                            row = htmlTableRow;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // have unknown error so set return to null
                table = null;
                row = null;
            }

        } //GetTableElement

        /// <summary>
        /// Method to return the currently selected Html Table Element
        /// </summary>
        private mshtmlTable GetTableElement()
        {
            // define the table and row elements and obtain there values
            mshtmlTable table;
            mshtmlTableRow row;
            GetTableElement(out table, out row);

            // return the defined table element
            return table;

        }

        /// <summary>
        /// Given an Html Table Element determines the table properties
        /// Returns the properties as an HtmlTableProperty class
        /// </summary>
        private HtmlTableProperty GetTableProperties(mshtmlTable table)
        {
            // define a set of base table properties
            var tableProperties = new HtmlTableProperty(true);

            // if user has selected a table extract those properties
            if (!table.IsNull())
            {
                try
                {
                    // have a table so extract the properties
                    mshtmlTableCaption caption = table.caption;
                    // if have a caption persist the values
                    if (!caption.IsNull())
                    {
                        tableProperties.CaptionText = ((mshtmlElement)table.caption).innerText;
                        if (!caption.align.IsNull()) tableProperties.CaptionAlignment = (HorizontalAlignOption)typeof(HorizontalAlignOption).TryParseEnum(caption.align, HorizontalAlignOption.Default);
                        if (!caption.vAlign.IsNull()) tableProperties.CaptionLocation = (VerticalAlignOption)typeof(VerticalAlignOption).TryParseEnum(caption.vAlign, VerticalAlignOption.Default);
                    }
                    // look at the table properties
                    if (!GeneralUtil.IsNull(table.border)) tableProperties.BorderSize =GeneralUtil.TryParseByte(table.border.ToString(),tableProperties.BorderSize);
                    if (!table.align.IsNull()) tableProperties.TableAlignment = (HorizontalAlignOption)typeof(HorizontalAlignOption).TryParseEnum(table.align, HorizontalAlignOption.Default);
                    // define the table rows and columns
                    int rows = Math.Min(table.rows.length, Byte.MaxValue);
                    int cols = Math.Min(table.cols, Byte.MaxValue);
                    if (cols == 0 && rows > 0)
                    {
                        // cols value not set to get the maxiumn number of cells in the rows
                        foreach (mshtmlTableRow tableRow in table.rows)
                        {
                            cols = Math.Max(cols, tableRow.cells.length);
                        }
                    }
                    tableProperties.TableRows = (byte)Math.Min(rows, byte.MaxValue);
                    tableProperties.TableColumns = (byte)Math.Min(cols, byte.MaxValue);
                    // define the remaining table properties
                    if (!GeneralUtil.IsNull(table.cellPadding)) tableProperties.CellPadding = GeneralUtil.TryParseByte(table.cellPadding.ToString(),tableProperties.CellPadding);
                    if (!GeneralUtil.IsNull(table.cellSpacing)) tableProperties.CellSpacing = GeneralUtil.TryParseByte(table.cellSpacing.ToString(),tableProperties.CellSpacing);
                    if (!GeneralUtil.IsNull(table.width))
                    {
                        string tableWidth = table.width.ToString();
                        if (tableWidth.TrimEnd(null).EndsWith("%"))
                        {
                            tableProperties.TableWidth = tableWidth.Remove(tableWidth.LastIndexOf("%", StringComparison.Ordinal), 1).TryParseUshort(tableProperties.TableWidth);
                            tableProperties.TableWidthMeasurement = MeasurementOption.Percent;
                        }
                        else
                        {
                            tableProperties.TableWidth = tableWidth.TryParseUshort(tableProperties.TableWidth);
                            tableProperties.TableWidthMeasurement = MeasurementOption.Pixel;
                        }
                    }
                    else
                    {
                        tableProperties.TableWidth = 0;
                        tableProperties.TableWidthMeasurement = MeasurementOption.Pixel;
                    }
                }
                catch (Exception ex)
                {
                    // throw an exception indicating table structure change be determined
                    throw new HtmlEditorException("Unable to determine Html Table properties.", "GetTableProperties", ex);
                }
            }

            // return the table properties
            return tableProperties;

        } //GetTableProperties

        /// <summary>
        /// Method to return  a table defintion based on the user selection
        /// If table selected (or insertion point within table) returns these values
        /// </summary>
        public void GetTableDefinition(out HtmlTableProperty table, out bool tableFound)
        {
            // see if a table selected or insertion point inside a table
            mshtmlTable htmlTable = GetTableElement();

            // process according to table being defined
            if (htmlTable.IsNull())
            {
                table = new HtmlTableProperty(true);
                tableFound = false;
            }
            else
            {
                table = GetTableProperties(htmlTable);
                tableFound = true;
            }

        } //GetTableDefinition

        /// <summary>
        /// Method to determine if the insertion point or selection is a table
        /// </summary>
        private bool IsParentTable()
        {
            // see if a table selected or insertion point inside a table
            mshtmlTable htmlTable = GetTableElement();

            // process according to table being defined
            if (htmlTable.IsNull())
            {
                return false;
            }
            return true;
        } //IsParentTable

        /// <summary>
        /// Method to insert a horizontal line in the body
        /// If have a control range rather than text range one could overwrite the controls with the line
        /// </summary>
        public void InsertLine()
        {
            mshtmlTextRange range = GetTextRange();
            if (range != null)
            {
                ExecuteCommandRange(range, HTML_COMMAND_INSERT_HORIZONTAL_RULE, null);
            }
            else
            {
                throw new HtmlEditorException("Invalid Selection for Line insertion.", "InsertLine");
            }

        } //InsertLine

        /// <summary>
        /// Method to Tab the current line to the left
        /// </summary>
        public void Outdent()
        {
            ExecuteCommandRange(HTML_COMMAND_OUTDENT, null);

        } //Outdent

        /// <summary>
        /// Method to Tab the current line to the right
        /// </summary>
        public void Indent()
        {
            ExecuteCommandRange(HTML_COMMAND_INDENT, null);

        } //Indent

        /// <summary>
        /// Method to insert an unordered list
        /// </summary>
        public void InsertUnorderedList()
        {
            ExecuteCommandRange(HTML_COMMAND_INSERT_UNORDERED_LIST, null);
            FormatSelectionChange();
        } //InsertUnorderedList

        /// <summary>
        /// Method to insert an ordered list
        /// </summary>
        public void InsertOrderedList()
        {
            ExecuteCommandRange(HTML_COMMAND_INSERT_ORDERED_LIST, null);
            FormatSelectionChange();
        } //InsertOrderedList

        /// <summary>
        /// Method using the document to toggle the selection with a Superscript tag
        /// </summary>
        public void Superscript()
        {
            ExecuteCommandRange(HTML_COMMAND_SUPERSCRIPT, null);
            FormatSelectionChange();

        } //Superscript

        /// <summary>
        /// Method using the document to toggle the selection with a Subscript tag
        /// </summary>
        public void Subscript()
        {
            ExecuteCommandRange(HTML_COMMAND_SUBSCRIPT, null);
            FormatSelectionChange();

        } //Subscript

        /// <summary>
        /// Method to select the entire document contents
        /// </summary>
        public void SelectAll()
        {
            ExecuteCommandDocument(HTML_COMMAND_SELECT_ALL);

        } //SelectAll

        #endregion

        #endregion

        private void tsbNew_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbPreview_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbShowHTML_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbCopy_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbCut_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        /// <summary>
        /// Method to ensure dialog resembles the user form characteristics
        /// </summary>
        private void DefineDialogProperties(Form dialog)
        {
            // set ambient control properties
            if (!ParentForm.IsNull())
            {
                dialog.Font = ParentForm.Font;
                dialog.ForeColor = ParentForm.ForeColor;
                dialog.BackColor = ParentForm.BackColor;
                dialog.Cursor = ParentForm.Cursor;
                dialog.RightToLeft = ParentForm.RightToLeft;
            }

            // define location and control style as system
            dialog.StartPosition = FormStartPosition.CenterParent;

        } //DefineDialogProperties

        /// <summary>
        /// Method to determine if the tag name is of the correct type
        /// A string comparision is made whilst ignoring case
        /// </summary>
        private bool IsStringEqual(string tagText, string tagType)
        {
            return (String.Compare(tagText, tagType, StringComparison.OrdinalIgnoreCase) == 0);

        } //IsStringEqual

        private void tsbRemoveFormat_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbJustifyCenter_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbJustifyLeft_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbJustifyRight_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbUnderline_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbItalic_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbBold_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbStrikeThrough_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbCreateLink_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbUnlink_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbInsertImage_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbInsertHorizontalRule_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbOutdent_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbIndent_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbInsertUnorderedList_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbInsertOrderedList_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbJustifyFull_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsmiInsertTable_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var command = (string)menuItem.Tag;
            ProcessCommand(command);
        }

        private void tsbWordCount_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbSuperscript_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbSubscript_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbDate_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbTime_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbSpellCheck_Click(object sender, EventArgs e)
        {
            Debug.Assert(wb.Document != null, "wb.Document != null");
            Debug.Assert(wb.Document.Body != null, "wb.Document.Body != null");
            spellCheck.Text = wb.Document.Body.InnerHtml;
            spellCheck.SpellCheck();
        }

        private void tsbAutoLayout_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbUndo_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbRedo_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        private void tsbClearWord_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            var command = (string)button.Tag;
            ProcessCommand(command);
        }

        /// <summary>
        /// Method to perform the process of showing the context menus
        /// </summary>
        private void DocumentContextMenu(object sender, HtmlElementEventArgs e)
        {
            // if in readonly mode display the standard context menu
            // otherwise display the editing context menu
            if (!_readOnly)
            {
                // should disable inappropriate commands
                tsmiTable.Visible = IsParentTable();

                // display the text processing context menu
                cmsHtml.Show(wb, e.MousePosition);

                // cancel the standard menu and event bubbling
                e.BubbleEvent = false;
                e.ReturnValue = false;
            }

        } //DocumentContextMenu

        /// <summary>
        /// Method to perform the process of selection change
        /// </summary>
        private void DocumentSelectionChange(object sender, EventArgs e)
        {
            // if not in readonly mode process the selection change
            if (!_readOnly)
            {
                FormatSelectionChange();
            }

        } //DocumentSelectionChange
        
        /// <summary>
        /// Ensures the toolbar is correctly displaying state
        /// </summary>
        private void FormatSelectionChange()
        {
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
            tsbSubscript.Checked = IsSubscript();
            tsbSuperscript.Checked = IsSuperscript();
            tsbUndo.Enabled = IsUndo();
            tsbRedo.Enabled = IsRedo();
            tsbUnlink.Enabled = IsUnlink();
            UpdateFontComboBox();
            UpdateFontSizeComboBox();
        }

        /// <summary>
        /// Method to perform the process of key being pressed
        /// </summary>
        private void DocumentKeyPress(object sender, EventArgs e)
        {
            // define the event object being processes and review the key being pressed
            mshtmlEventObject eventObject = document.parentWindow.@event;
            if (eventObject.ctrlKey)
            {
                switch (eventObject.keyCode)
                {
                    case 65:
                        SelectAll();
                        eventObject.returnValue = true;
                        break;
                    case 72:
                        FindReplacePrompt(false);
                        eventObject.returnValue = false;
                        break;
                }
            }
            if ((eventObject.keyCode != 13)) return;
            if (EnterToBR)
            {
                var range = GetTextRange();
                Debug.Assert(range != null, "range != null");
                range.pasteHTML(!eventObject.shiftKey ? "<br>" : "<P>&nbsp;</P>");
                range.collapse();
                range.@select();
            }
            if (!eventObject.shiftKey)
            {
                eventObject.returnValue = SetupKeyListener();
            }

            eventObject.returnValue = true;

        } //DocumentKeyPress

        private void Selector_TableSizeSelected(object sender, TableSizeEventArgs e)
        {
            // if user has selected a table create a reference
            var table = GetFirstControl() as mshtmlTable;

            // define the base set of table properties
            HtmlTableProperty tableProperties = GetTableProperties(table);
            tableProperties.TableRows =(byte)e.SelectedSize.Height;
            tableProperties.TableColumns = (byte)e.SelectedSize.Width;
            if (table.IsNull()) TableInsert(tableProperties);
            else ProcessTable(table, tableProperties);
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
                    Text = Resources.strInsertTable,
                    Tag = "InsertTable"
                };
            tsmiInsertTable.Click += tsmiInsertTable_Click;
            tsddbInsertTable.DropDownItems.Add(tsmiInsertTable);

            string removeButton = ConfigurationManager.AppSettings["removeButtons"];
            if (!removeButton.IsNullOrEmpty())
            {
                var removeButtons = removeButton.Split(',');
                foreach (var button in removeButtons)
                {
                    foreach (var item in tsTopToolBar.Items)
                    {
                        if (item is ToolStripButton)
                        {
                            var tsb = item as ToolStripButton;
                            if (String.CompareOrdinal(tsb.Tag.ToString(), button) == 0)
                            {
                                tsb.Visible = false;
                                break;
                            }
                        }
                        else if (item is ToolStripDropDownButton)
                        {
                            var tsddb = item as ToolStripDropDownButton;
                            if (String.CompareOrdinal(tsddb.Tag.ToString(), button) == 0)
                            {
                                tsddb.Visible = false;
                                break;
                            }
                        }
                        else if (item is ToolStripFontComboBox)
                        {
                            var tsfcb = item as ToolStripFontComboBox;
                            if (String.CompareOrdinal(tsfcb.Tag.ToString(), button) == 0)
                            {
                                tsfcb.Visible = false;
                                break;
                            }
                        }
                        else if (item is ToolStripComboBox)
                        {
                            var tscb = item as ToolStripComboBox;
                            if (String.CompareOrdinal(tscb.Tag.ToString(), button) == 0)
                            {
                                tscb.Visible = false;
                                break;
                            }
                        }
                    }
                }
            }
            string removeMenu = ConfigurationManager.AppSettings["removeMenus"];
            if (!string.IsNullOrEmpty(removeMenu))
            {
                var removeMenus = removeMenu.Split(',');
                foreach (var menu in removeMenus)
                {
                    foreach (var item in cmsHtml.Items)
                    {
                        if (item is ToolStripMenuItem)
                        {
                            var tsmi = item as ToolStripMenuItem;
                            if (String.CompareOrdinal(tsmi.Tag.ToString(), menu) == 0)
                            {
                                tsmi.Visible = false;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void HtmlEditor_Load(object sender, EventArgs e)
        {
            tsTopToolBar.Dock = DockStyle.Top;
            InitUi();
            HTMLEditHelper.DOMDocument = _doc;
            Focus();
        }

#if VS2010
        public static string ClearWord(string sourceText, bool bIgnoreFont = true, bool bRemoveStyles = true, bool cleanWordKeepsStructure = true)
        {
            return ClearWordNoDefult(sourceText, bIgnoreFont, bRemoveStyles, cleanWordKeepsStructure);
        }
#endif

        private static String DeCompressWMZOREMZFile(String wmzoremzFile)
        {
            MemoryStream decompressStream = new MemoryStream(File.ReadAllBytes(wmzoremzFile));
            GZipStream gzipStream = new GZipStream(decompressStream, CompressionMode.Decompress);
            MemoryStream outStream = new MemoryStream();
            int readCount;
            byte[] data = new byte[2048];
            do
            {
                readCount = gzipStream.Read(data, 0, data.Length);
                outStream.Write(data, 0, readCount);
            } while (readCount == 2048);
            String imgFile = Path.GetDirectoryName(wmzoremzFile) + "\\" + Path.GetFileNameWithoutExtension(wmzoremzFile) +
                             (Path.GetExtension(wmzoremzFile) == ".wmz"
                                  ? ".wmf"
                                  : ".emf");
            File.WriteAllBytes(imgFile, outStream.GetBuffer());
            return imgFile;
        }

        public static string ReplaceWordImageFile(string sourceText)
        {
            var reg = new Regex(@"(?is)<v:imagedata[^>]*?src=(['""\s]?)((?:(?!topics)[^'""\s])*)\1[^>]*?>", RegexOptions.IgnoreCase);
            foreach (Match m in reg.Matches(sourceText))
            {
                switch (Path.GetExtension(m.Groups[2].Value))
                {
                    case ".wmz":
                    case ".emz":
                        {
                            string temp = m.Groups[0].Value.Replace(m.Groups[2].Value,
                                        DeCompressWMZOREMZFile(m.Groups[2].Value.StartsWith("file:///")
                                                                   ? m.Groups[2].Value.Substring(8)
                                                                   : m.Groups[2].Value));
                            sourceText = sourceText.Replace(m.Groups[0].Value, temp);
                        }
                        break;
                }
            }
            sourceText = Regex.Replace(sourceText, @"v:imagedata", "img");
            return sourceText;
        }

        public static string ClearWordNoDefult(string sourceText, bool bIgnoreFont, bool bRemoveStyles, bool cleanWordKeepsStructure)
        {
            sourceText = ReplaceWordImageFile(sourceText);
            sourceText = Regex.Replace(sourceText, @"<o:p>\s*<\/o:p>", "");
            sourceText = Regex.Replace(sourceText, @"<o:p>.*?<\/o:p>", " ");
            // Remove mso-xxx styles.
            sourceText = Regex.Replace(sourceText, @"\s*mso-[^:]+:[^;""'>]+;?", "", RegexOptions.IgnoreCase);
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

        /// <summary>
        /// Set editor's current selection to the value of the font size combo box.
        /// Ignore if the timer is currently updating the font size to synchronize 
        /// the font size combo box with the editor's current selection.
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">EventArgs</param>
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
                    Focus();
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
        private void tsfcbFontName_SelectedIndexChanged(object sender, EventArgs e)
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
            Focus();
        }

        #endregion

        #region Private variables and methods

        private void SynchFont(string sTagName)
        {
            //Times Roman New
            object obj = QueryCommandRange(HTML_COMMAND_FONTNAME);
            if (obj.IsNull())
                return;
            string fontname = obj.ToString();
            obj = QueryCommandRange(HTML_COMMAND_FONTSIZE);
            if (obj.IsNull())
                return;
            //Could indicate a headingxxx, P, or BODY
            _internalCall = true;
            if (obj.ToString().Length > 0)
                tscbFontSize.SelectedIndex = Convert.ToInt32(obj) - 1; //x (x - 1)
            else
                AdjustForHeading(sTagName);
            tsfcbFontName.Text = fontname;
        }

        private void AdjustForHeading(string sTag)
        {
            if (sTag.IsNullOrEmpty())
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
        /// Set up a key listener on the body once.
        /// The key listener checks for specific key strokes and takes 
        /// special action in certain cases.
        /// </summary>
        /// <returns></returns>
        private bool SetupKeyListener()
        {
            // handle enter code cancellation
            bool cancel = false;
            if (!EnterKeyEvent.IsNull())
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
            return ExecuteCommandQuery(HTML_COMMAND_JUSTIFY_LEFT);
        }

        /// <summary>
        /// Determine whether the current block is right justified.
        /// </summary>
        /// <returns>true if right justified, otherwise false</returns>
        public bool IsJustifyRight()
        {
            return ExecuteCommandQuery(HTML_COMMAND_JUSTIFY_RIGHT);
        }

        /// <summary>
        /// Determine whether the current block is center justified.
        /// </summary>
        /// <returns>true if center justified, false otherwise</returns>
        public bool IsJustifyCenter()
        {
            return ExecuteCommandQuery(HTML_COMMAND_JUSTIFY_CENTER);
        }

        /// <summary>
        /// Determine whether the current block is full justified.
        /// </summary>
        /// <returns>true if full justified, false otherwise</returns>
        public bool IsJustifyFull()
        {
            return ExecuteCommandQuery(HTML_COMMAND_JUSTIFY_FULL);
        }

        /// <summary>
        /// Determine whether the current selection is in Bold mode.
        /// </summary>
        /// <returns>whether or not the current selection is Bold</returns>
        public bool IsBold()
        {
            return ExecuteCommandQuery(HTML_COMMAND_BOLD);
        }

        /// <summary>
        /// Determine whether the current selection is in Italic mode.
        /// </summary>
        /// <returns>whether or not the current selection is Italicized</returns>
        public bool IsItalic()
        {
            return ExecuteCommandQuery(HTML_COMMAND_ITALIC);
        }

        /// <summary>
        /// Determine whether the current selection is in Underline mode.
        /// </summary>
        /// <returns>whether or not the current selection is Underlined</returns>
        public bool IsUnderline()
        {
            return ExecuteCommandQuery(HTML_COMMAND_UNDERLINE);
        }

        /// <summary>
        /// Determine whether the current selection is in StrikeThrough mode.
        /// </summary>
        /// <returns>whether or not the current selection is StrikeThrough</returns>
        public bool IsStrikeThrough()
        {
            return ExecuteCommandQuery(HTML_COMMAND_STRIKE_THROUGH);
        }

        /// <summary>
        /// Determine whether the current selection is in Subscript mode.
        /// </summary>
        /// <returns>whether or not the current selection is Subscript</returns>
        public bool IsSubscript()
        {
            return ExecuteCommandQuery(HTML_COMMAND_SUBSCRIPT);
        }

        /// <summary>
        /// Determine whether the current selection is in Superscript mode.
        /// </summary>
        /// <returns>whether or not the current selection is Superscript</returns>
        public bool IsSuperscript()
        {
            return ExecuteCommandQuery(HTML_COMMAND_SUPERSCRIPT);
        }

        /// <summary>
        /// Determine whether the current paragraph is an ordered list.
        /// </summary>
        /// <returns>true if current paragraph is ordered, false otherwise</returns>
        public bool IsOrderedList()
        {
            return ExecuteCommandQuery(HTML_COMMAND_INSERT_ORDERED_LIST);
        }

        /// <summary>
        /// Determine whether the current paragraph is an unordered list.
        /// </summary>
        /// <returns>true if current paragraph is ordered, false otherwise</returns>
        public bool IsUnorderedList()
        {
            return ExecuteCommandQuery(HTML_COMMAND_INSERT_UNORDERED_LIST);
        }

        /// <summary>
        /// Determine whether the current block can undo.
        /// </summary>
        /// <returns>true if current block can undo, false otherwise</returns>
        public bool IsUndo()
        {
            return _doc.queryCommandEnabled(HTML_COMMAND_UNDO);
        }

        /// <summary>
        /// Determine whether the current block can redo.
        /// </summary>
        /// <returns>true if current block can redo, false otherwise</returns>
        public bool IsRedo()
        {
            return _doc.queryCommandEnabled(HTML_COMMAND_REDO);
        }

        /// <summary>
        /// Determine whether the current block can unlink.
        /// </summary>
        /// <returns>true if current block can unlink, false otherwise</returns>
        public bool IsUnlink()
        {
            return ExecuteCommandQuery(HTML_COMMAND_UNLINK, true);
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
                if (!fam.IsNull())
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
                if (QueryCommandRange(HTML_COMMAND_FONTSIZE).IsNull())
                    return FontSize.NA;
                switch (QueryCommandRange(HTML_COMMAND_FONTSIZE).ToString())
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
                string text=string.Empty;
                switch (value)
                {
                    case FontSize.One:
                        sz = 1;
                        text = "1 (8 pt)";
                        break;
                    case FontSize.Two:
                        sz = 2;
                        text = "2 (10 pt)";
                        break;
                    case FontSize.Three:
                        sz = 3;
                        text = "3 (12 pt)";
                        break;
                    case FontSize.Four:
                        sz = 4;
                        text = "4 (14 pt)";
                        break;
                    case FontSize.Five:
                        sz = 5;
                        text = "5 (18 pt)";
                        break;
                    case FontSize.Six:
                        sz = 6;
                        text = "6 (24 pt)";
                        break;
                    case FontSize.Seven:
                        sz = 7;
                        text = "7 (36 pt)";
                        break;
                    default:
                        sz = 7;
                        text = "7 (36 pt)";
                        break;
                }
                if (!wb.Document.IsNull())
                {
                    wb.Document.ExecCommand(HTML_COMMAND_FONTSIZE, false, sz.ToString(CultureInfo.InvariantCulture));
                    this.tscbFontSize.Text = text;
                }
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
                var name = QueryCommandRange(HTML_COMMAND_FONTNAME) as string;
                if (name.IsNull()) return null;
                return new FontFamily(name);
            }
            set
            {
                if (!value.IsNull() && !wb.Document.IsNull())
                {
                    wb.Document.ExecCommand(HTML_COMMAND_FONTNAME, false, value.Name);
                    tsfcbFontName.Text = value.Name;
                }
            }
        }

        #endregion

        private void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string url = e.Url.ToString();
            if (!codeNavigate)
            {
                // call the appropriate event processing
                var navigateArgs = new HtmlNavigationEventArgs(url);
                OnHtmlNavigation(navigateArgs);

                // process the event based on the navigation option
                if (navigateArgs.Cancel)
                {
                    // cancel the navigation
                    e.Cancel = true;
                }
                else if (_navigateWindow == NavigateActionOption.NewWindow)
                {
                    // cancel the current navigation and load url into a new window
                    e.Cancel = true;
                    NavigateToUrl(url, true);
                }
                else
                {
                    // continue with current navigation
                    e.Cancel = false;
                }
            }
            else
            {
                // TODO Should ensure the following are no executed for the editor navigation
                //   Scripts
                //   Java
                //   ActiveX Controls
                //   Behaviors
                //   Dialogs

                // continue with current navigation
                e.Cancel = false;
            }
        }

        /// <summary>
        /// Processing for the HtmlNavigation event
        /// </summary>
        private void OnHtmlNavigation(HtmlNavigationEventArgs args)
        {
            if (!HtmlNavigation.IsNull())
            {
                HtmlNavigation(this, args);
            }

        } //OnHtmlNavigation

        #region MsHtml Command Processing

        /// <summary>
        /// Performs a query of the command state
        /// </summary>
        private bool ExecuteCommandQuery(string command, bool isEnabled=false)
        {
            // obtain the selected range object and query command
            mshtmlTextRange range = GetTextRange();
            return ExecuteCommandQuery(range, command, isEnabled);

        } //ExecuteCommandQuery

        /// <summary>
        /// Executes the queryCommandState on the selected range (given the range)
        /// </summary>
        private bool ExecuteCommandQuery(mshtmlTextRange range, string command, bool isEnabled = false)
        {
            // set the initial state as false
            bool retValue = false;

            try
            {
                if (!range.IsNull())
                {
                    // ensure command is a valid command and then enabled for the selection
                    if (range.queryCommandSupported(command))
                    {
                        if (isEnabled)
                        {
                            retValue = range.queryCommandEnabled(command);
                        }
                        else
                        {
                            if (range.queryCommandEnabled(command))
                            {
                                // mark the selection with the appropriate tag
                                retValue = range.queryCommandState(command);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Unknown error so inform user
                throw new HtmlEditorException("Unknown MSHTML Error.", command, ex);
            }

            // return the value
            return retValue;

        } // ExecuteCommandQuery

        /// <summary>
        /// Executes the execCommand on the selected range
        /// </summary>
        private void ExecuteCommandRange(string command, object data)
        {
            // obtain the selected range object and execute command
            mshtmlTextRange range = GetTextRange();
            ExecuteCommandRange(range, command, data);

        } // ExecuteCommandRange

        /// <summary>
        /// Executes the execCommand on the selected range (given the range)
        /// </summary>
        private void ExecuteCommandRange(mshtmlTextRange range, string command, object data)
        {
            try
            {
                if (!range.IsNull())
                {
                    // ensure command is a valid command and then enabled for the selection
                    if (range.queryCommandSupported(command))
                    {
                        if (range.queryCommandEnabled(command))
                        {
                            // mark the selection with the appropriate tag
                            range.execCommand(command, false, data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Unknown error so inform user
                throw new HtmlEditorException("Unknown MSHTML Error.", command, ex);
            }

        } // ExecuteCommandRange

        /// <summary>
        /// Executes the execCommand on the document with a system prompt
        /// </summary>
        private void ExecuteCommandDocumentPrompt(string command)
        {
            ExecuteCommandDocument(command, true);

        } // ExecuteCommandDocumentPrompt

        /// <summary>
        /// Executes the execCommand on the document with a system prompt
        /// </summary>
        private void ExecuteCommandDocument(string command, bool prompt = false)
        {
            try
            {
                // ensure command is a valid command and then enabled for the selection
                if (document.queryCommandSupported(command))
                {
                    // if (document.queryCommandEnabled(command)) {}
                    // Test fails with a COM exception if command is Print

                    // execute the given command
                    document.execCommand(command, prompt, null);
                }
            }
            catch (Exception ex)
            {
                // Unknown error so inform user
                throw new HtmlEditorException("Unknown MSHTML Error.", command, ex);
            }

        } // ExecuteCommandDocumentPrompt

        /// <summary>
        /// Determines the value of the command
        /// </summary>
        private object QueryCommandRange(string command)
        {
            // obtain the selected range object and execute command
            mshtmlTextRange range = GetTextRange();
            return !range.IsNull()? QueryCommandRange(range, command) : null;
        } // QueryCommandRange

        /// <summary>
        /// Determines the value of the command
        /// </summary>
        private object QueryCommandRange(mshtmlTextRange range, string command)
        {
            object retValue = null;
            if (!range.IsNull())
            {
                try
                {
                    // ensure command is a valid command and then enabled for the selection
                    if (range.queryCommandSupported(command))
                    {
                        if (range.queryCommandEnabled(command))
                        {
                            retValue = range.queryCommandValue(command);
                        }
                    }
                }
                catch (Exception)
                {
                    // have unknown error so set return to null
                    retValue = null;
                }
            }

            // return the obtained value
            return retValue;

        } //QueryCommandRange

        /// <summary>
        /// Gets the selected Html Range Element
        /// </summary>
        private mshtmlTextRange GetTextRange()
        {
            // define the selected range object
            mshtmlTextRange range = null;

            try
            {
                // calculate the text range based on user selection
                mshtmlSelection selection = document.selection;
                if (IsStringEqual(selection.type, SELECT_TYPE_TEXT) || IsStringEqual(selection.type, SELECT_TYPE_NONE))
                {
                    range = (mshtmlTextRange)selection.createRange();
                }
            }
            catch (Exception)
            {
                // have unknown error so set return to null
                range = null;
            }

            return range;

        } // GetTextRange

        /// <summary>
        /// Determines the color of the selected text range
        /// Returns default value if not text selected
        /// </summary>
        private Color GetFontColor()
        {
            object fontColor = QueryCommandRange(HTML_COMMAND_FORE_COLOR);
            return (fontColor is Int32) ? ColorTranslator.FromWin32((int)fontColor) : _bodyForeColor;

        } //GetFontColor

        /// <summary>
        /// Determines the color of the selected text range
        /// Returns default value if not text selected
        /// </summary>
        private Color GetBackColor()
        {
            object fontColor = QueryCommandRange(HTML_COMMAND_BACK_COLOR);
            return (fontColor is Int32) ? ColorTranslator.FromWin32((int)fontColor) : _bodyBackColor;

        } //GetFontColor

        /// <summary>
        /// Gets the first selected Html Control as an Element
        /// </summary>
        private mshtmlElement GetFirstControl()
        {
            // define the selected range object
            mshtmlElement control = null;

            try
            {
                // calculate the first control based on the user selection
                mshtmlSelection selection = document.selection;
                if (IsStringEqual(selection.type, SELECT_TYPE_CONTROL))
                {
                    var range = (mshtmlControlRange)selection.createRange();
                    if (range.length > 0) control = range.item(0);
                }
            }
            catch (Exception)
            {
                // have unknown error so set return to null
                control = null;
            }

            return control;

        } // GetFirstControl

        /// <summary>
        /// Gets all the selected Html Controls as a Control Range
        /// </summary>
        /// <returns></returns>
        private mshtmlControlRange GetAllControls()
        {
            // define the selected range object
            mshtmlControlRange range = null;

            try
            {
                // calculate the first control based on the user selection
                mshtmlSelection selection = document.selection;
                if (IsStringEqual(selection.type, SELECT_TYPE_CONTROL))
                {
                    range = (mshtmlControlRange)selection.createRange();
                }
            }
            catch (Exception)
            {
                // have unknow error so set return to null
                range = null;
            }

            return range;

        } //GetAllControls

        #endregion

        /// <summary>
        /// Method to process the toolbar command and handle error exception
        /// </summary>
        private void ProcessCommand(string command)
        {
            try
            {
                // Evaluate the Button property to determine which button was clicked.
                switch (command)
                {
                    case INTERNAL_COMMAND_NEW:
                        // New document
                        New();
                        break;
                    case INTERNAL_COMMAND_OPEN:
                        // Open a selected file
                        Open();
                        break;
                    case INTERNAL_COMMAND_PRINT:
                        // Print the current document
                        Print();
                        break;
                    case INTERNAL_COMMAND_SAVE:
                        // Saves the current document
                        Save();
                        break;
                    case INTERNAL_COMMAND_PREVIEW:
                        // Preview the html page
                        Preview();
                        break;
                    case INTERNAL_COMMAND_SHOWHTML:
                        // View the html contents
                        ShowHTML();
                        break;
                    case INTERNAL_COMMAND_COPY:
                        // Browser COPY command
                        Copy();
                        break;
                    case INTERNAL_COMMAND_CUT:
                        // Browser CUT command
                        Cut();
                        break;
                    case INTERNAL_COMMAND_PASTE:
                        // Browser PASTE command
                        Paste();
                        break;
                    case INTERNAL_COMMAND_DELETE:
                        // Browser DELETE command
                        Delete();
                        break;
                    case INTERNAL_COMMAND_UNDO:
                        // Undo the previous editing
                        Undo();
                        break;
                    case INTERNAL_COMMAND_REDO:
                        // Redo the previous undo
                        Redo();
                        break;
                    case INTERNAL_COMMAND_FIND:
                        //Find the string u input
                        Find();
                        break;
                    case INTERNAL_COMMAND_REMOVE_FORMAT:
                        // Browser REMOVEFORMAT command
                        RemoveFormat();
                        break;
                    case INTERNAL_COMMAND_JUSTIFYCENTER:
                        // Justify Center
                        JustifyCenter();
                        break;
                    case INTERNAL_COMMAND_JUSTIFYFULL:
                        // Justify Full
                        JustifyFull();
                        break;
                    case INTERNAL_COMMAND_JUSTIFYLEFT:
                        // Justify Left
                        JustifyLeft();
                        break;
                    case INTERNAL_COMMAND_JUSTIFYRIGHT:
                        // Justify Right
                        JustifyRight();
                        break;
                    case INTERNAL_COMMAND_UNDERLINE:
                        // Selection UNDERLINE command
                        Underline();
                        break;
                    case INTERNAL_COMMAND_ITALIC:
                        // Selection ITALIC command
                        Italic();
                        break;
                    case INTERNAL_COMMAND_BOLD:
                        // Selection BOLD command
                        Bold();
                        break;
                    case INTERNAL_COMMAND_FORECOLOR:
                        // FORECOLOR style creation
                        FormatFontColor(tscpForeColor.Color);
                        break;
                    case INTERNAL_COMMAND_BACKCOLOR:
                        // BACKCOLOR style creation
                        FormatBackColor(tscpBackColor.Color);
                        break;
                    case INTERNAL_COMMAND_STRIKETHROUGH:
                        // Selection STRIKETHROUGH command
                        StrikeThrough();
                        break;
                    case INTERNAL_COMMAND_CREATELINK:
                        // Selection CREATELINK command
                        CreateLink();
                        break;
                    case INTERNAL_COMMAND_UNLINK:
                        // Selection UNLINK command
                        UnLink();
                        break;
                    case INTERNAL_COMMAND_INSERTTABLE:
                        // Display a dialog to enable the user to insert a table
                        TableInsertPrompt();
                        break;
                    case INTERNAL_COMMAND_TABLEPROPERTIES:
                        // Display a dialog to enable the user to modify a table
                        TableModifyPrompt();
                        break;
                    case INTERNAL_COMMAND_TABLEINSERTROW:
                        // Display a dialog to enable the user to modify a table
                        TableInsertRow();
                        break;
                    case INTERNAL_COMMAND_TABLEDELETEROW:
                        // Display a dialog to enable the user to modify a table
                        TableDeleteRow();
                        break;
                    case INTERNAL_COMMAND_INSERTIMAGE:
                        // Display a dialog to enable the user to insert a image
                        InsertImage();
                        break;
                    case INTERNAL_COMMAND_INSERTHORIZONTALRULE:
                        // Horizontal Line
                        InsertLine();
                        break;
                    case INTERNAL_COMMAND_OUTDENT:
                        // Tab Left
                        Outdent();
                        break;
                    case INTERNAL_COMMAND_INDENT:
                        // Tab Right
                        Indent();
                        break;
                    case INTERNAL_COMMAND_INSERTUNORDEREDLIST:
                        // Unordered List
                        InsertUnorderedList();
                        break;
                    case INTERNAL_COMMAND_INSERTORDEREDLIST:
                        // Ordered List
                        InsertOrderedList();
                        break;
                    case INTERNAL_COMMAND_SUPERSCRIPT:
                        // Selection SUPERSCRIPT command
                        Superscript();
                        break;
                    case INTERNAL_COMMAND_SUBSCRIPT:
                        // Selection SUBSCRIPT command
                        Subscript();
                        break;
                    case INTERNAL_COMMAND_SELECTALL:
                        // Selects all document content
                        SelectAll();
                        break;
                    case INTERNAL_COMMAND_WORDCOUNT:
                        // Word count
                        int wordCount = WordCount();
                        tsslWordCount.Text = string.Format("字数：{0}", wordCount);
                        break;
                    case INTERNAL_COMMAND_INSERTDATE:
                        // Insert date
                        PasteIntoSelection(DateTime.Now.ToLongDateString());
                        break;
                    case INTERNAL_COMMAND_INSERTTIME:
                        // Insert time
                        PasteIntoSelection(DateTime.Now.ToLongTimeString());
                        break;
                    case INTERNAL_COMMAND_CLEARWORD:
                        // Clear word
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
                        break;
                    case INTERNAL_COMMAND_AUTOLAYOUT:
                        // Auto Layout
                        AutoLayout();
                        break;
                    case INTERNAL_COMMAND_ABOUT:
                        if (MessageBox.Show(Resources.AboutInfo, Resources.AboutText, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            Process.Start("http://tewuapple.github.com/WinHtmlEditor/");
                        }
                        break;
                    default:
                        throw new HtmlEditorException("Unknown Operation Being Performed.", command);
                }
            }
            catch (HtmlEditorException ex)
            {
                // process the html exception
                OnHtmlException(new HtmlExceptionEventArgs(ex.Operation, ex));
            }
            catch (Exception ex)
            {
                // process the exception
                OnHtmlException(new HtmlExceptionEventArgs(null, ex));
            }

            // ensure web browser has the focus after command execution
            Focus();

        } //ProcessCommand

        /// <summary>
        /// Method to raise an event if a delegeate is assigned for handling exceptions
        /// </summary>
        private void OnHtmlException(HtmlExceptionEventArgs args)
        {
            if (HtmlException.IsNull())
            {
                // obtain the message and operation
                // concatenate the message with any inner message
                string operation = args.Operation;
                Exception ex = args.ExceptionObject;
                string message = ex.Message;
                if (!ex.InnerException.IsNull())
                {
                    message = string.Format("{0}\n{1}", message, ex.InnerException.Message);
                }
                // define the title for the internal message box
                string title;
                if (operation.IsNullOrEmpty())
                {
                    title = "Unknown Error";
                }
                else
                {
                    title = operation + " Error";
                }
                // display the error message box
                MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                HtmlException(this, args);
            }
        }

        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // get access to the HTMLDocument
            var htmlDocument = wb.Document;
            if (!htmlDocument.IsNull())
            {
                document = (mshtmlDocument)htmlDocument.DomDocument;
                _doc = (mshtmlIHTMLDocument2)htmlDocument.DomDocument;

                // at this point the document and body has been loaded
                // so define the event handler for the context menu
                htmlDocument.ContextMenuShowing += DocumentContextMenu;
                htmlDocument.AttachEventHandler("onselectionchange", DocumentSelectionChange);
                htmlDocument.AttachEventHandler("onkeydown", DocumentKeyPress);
            }
            body = (mshtmlBody)document.body;

            // COM Interop Start
            // once browsing has completed there is the need to setup some options
            // need to ensure URLs are not modified when html is pasted
            int hResult;
            // try to obtain the command target for the web browser document
            try
            {
                // cast the document to a command target
                var target = (IOleCommandTarget)document;
                // set the appropriate no url fixups on paste
                hResult = target.Exec(ref CommandGroup.CGID_MSHTML, (int)CommandId.IDM_NOFIXUPURLSONPASTE, (int)CommandOption.OLECMDEXECOPT_DONTPROMPTUSER, ref EMPTY_PARAMETER, ref EMPTY_PARAMETER);
            }
            // catch any exception and map back to the HRESULT
            catch (Exception ex)
            {
                hResult = Marshal.GetHRForException(ex);
            }
            // test the HRESULT for a valid operation
            rebaseUrlsNeeded = hResult != HRESULT.S_OK;
            // COM Interop End

            // signalled complete
            codeNavigate = false;
            loading = false;

            // after navigation define the document Url
            string url = e.Url.ToString();
            _bodyUrl = IsStringEqual(url, BLANK_HTML_PAGE) ? string.Empty : url;
        }

        /// <summary>
        /// Called when a key is pressed on the font size combo box.
        /// The font size in the boxy box is set to the key press value.
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">KeyPressEventArgs</param>
        private void tscbFontSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;
                if (e.KeyChar <= '7' && e.KeyChar > '0')
                    tscbFontSize.SelectedIndex = GeneralUtil.ParseInt(e.KeyChar) - 1;
            }
            else if (!Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        
        /// <summary>
        /// General Context Meun processing method
        /// Calls the ProcessCommand with the selected command Tag Text
        /// </summary>
        private void ContextEditorClick(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var command = (string)menuItem.Tag;
            ProcessCommand(command);
        } //contextEditorClick

        private void tscpBackColor_SelectedColorChanged(object sender, EventArgs e)
        {
            var tscp = (ToolStripColorPicker)sender;
            var command = (string)tscp.Tag;
            ProcessCommand(command);
        }

        private void tscpForeColor_SelectedColorChanged(object sender, EventArgs e)
        {
            var tscp = (ToolStripColorPicker)sender;
            var command = (string)tscp.Tag;
            ProcessCommand(command);
        } 
        
    }
}
