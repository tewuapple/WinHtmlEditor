using System;
using System.Globalization;
using System.Runtime.InteropServices;
using mshtml;

namespace WinHtmlEditor
{
    #region HTMLEditHelper class
    public static class HTMLEditHelper
    {
        private static IHTMLDocument2 _mPDoc2;

        public static IHTMLDocument2 DOMDocument
        {
            get { return _mPDoc2; }
            set { _mPDoc2 = value; }
        }

        public static string HtmlSpace = "&nbsp;";  // space

        #region Table specific

        //bordersize 2 or "2"
        public static bool AppendTable(int colnum, int rownum, int bordersize, string alignment, int cellpadding, int cellspacing, string widthpercentage, int widthpixel)
        {
            if (_mPDoc2 == null)
                return false;
            var t = _mPDoc2.createElement("table") as IHTMLTable;
            //set the cols
            if (t != null)
            {
                t.cols = colnum;
                t.border = bordersize;

                if (!string.IsNullOrEmpty(alignment))
                    t.align = alignment; //"center"
                t.cellPadding = cellpadding; //1
                t.cellSpacing = cellspacing; //2

                if (!string.IsNullOrEmpty(widthpercentage))
                    t.width = widthpercentage; //"50%"; 
                else if (widthpixel > 0)
                    t.width = widthpixel; //80;
            }
            //Insert rows and fill them with space
            int cells = colnum - 1;
            int rows = rownum - 1;

            CalculateCellWidths(colnum);
            for (int i = 0; i <= rows; i++)
            {
                if (t != null)
                {
                    var tr = t.insertRow() as IHTMLTableRow;
                    for (int j = 0; j <= cells; j++)
                    {
                        if (tr != null)
                        {
                            var c = tr.insertCell() as IHTMLElement;
                            if (c != null)
                            {
                                c.innerHTML = HtmlSpace;
                                var tcell = c as IHTMLTableCell;
                                if (tcell != null)
                                {
                                    //set width so as user enters text
                                    //the cell width would not adjust
                                    tcell.width = j == cells ? _mLastcellwidth : _mCellwidth;
                                }
                            }
                        }
                    }
                }
            }
            //Append to body DOM collection
            var htmlElement = t as IHTMLElement;
            return htmlElement != null && PasteIntoSelection(htmlElement.outerHTML);
        }

        private static string _mCellwidth = string.Empty;
        private static string _mLastcellwidth = string.Empty;

        private static void CalculateCellWidths(int numberofcols)
        {
            //Even numbers. for 2 cols; 50%, 50%
            //Odd numbers.  for 3 cols; 33%, 33%, 34%
            var cellwidth = 100 / numberofcols;
            _mCellwidth = cellwidth.ToString(CultureInfo.InvariantCulture) + "%";
            //modulus operator (%).
            //http://msdn2.microsoft.com/en-us/library/0w4e0fzs.aspx
            //http://msdn2.microsoft.com/en-us/library/6a71f45d.aspx
            cellwidth += 100 % numberofcols;
            _mLastcellwidth = cellwidth.ToString(CultureInfo.InvariantCulture) + "%";
        }

        /// <summary>
        /// The currently selected text/controls will be replaced by the given HTML code.
        /// If nothing is selected, the HTML code is inserted at the cursor position
        /// </summary>
        /// <param name="sHtml"></param>
        /// <returns></returns>
        public static bool PasteIntoSelection(string sHtml)
        {
            if (_mPDoc2 == null)
                return false;
            var sel = _mPDoc2.selection as IHTMLSelectionObject;
            if (sel == null)
                return false;
            var range = sel.createRange() as IHTMLTxtRange;
            if (range == null)
                return false;
            //none
            //text
            //control
            if ((!String.IsNullOrEmpty(sel.EventType)) &&
                (sel.EventType == "control"))
            {
                var ctlrange = range as IHTMLControlRange;
                if (ctlrange != null) //Control(s) selected
                {
                    int ctls = ctlrange.length;
                    for (int i = 0; i < ctls; i++)
                    {
                        //Remove all selected controls
                        IHTMLElement elem = ctlrange.item(i);
                        if (elem != null)
                        {
                            RemoveNode(elem, true);
                        }
                    }
                    // Now get the textrange after deleting all selected controls
                    range = sel.createRange() as IHTMLTxtRange;
                }
            }

            if (range != null)
            {
                // range will be valid if nothing is selected or if text is selected
                // or if multiple elements are seleted
                range.pasteHTML(sHtml);
                range.collapse(false);
                range.select();
            }
            return true;
        }

        /// <summary>
        /// Removes node element
        /// If RemoveAllChildren == true, Removes this element and all it's children from the document
        /// else it just strips this element but does not remove its children
        /// E.g.  "<BIG><b>Hello World</b></BIG>"  ---> strip BIG tag --> "<b>Hello World</b>"
        /// </summary>
        /// <param name="elem">element to remove</param>
        /// <param name="removeAllChildren"></param>
        public static IHTMLDOMNode RemoveNode(IHTMLElement elem, bool removeAllChildren)
        {
            var node = elem as IHTMLDOMNode;
            if (node != null)
                return node.removeNode(removeAllChildren);
            return null;
        }

        #endregion

    }
    #endregion

    [ComVisible(true), ComImport]
    [TypeLibType((short)4160)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3050f25A-98b5-11cf-bb82-00aa00bdce0b")]
    interface IHTMLSelectionObject
    {
        [return: MarshalAs(UnmanagedType.IDispatch)]
        [DispId(HTMLDispIDs.DispidIhtmlselectionobjectCreaterange)]
        object createRange();
        [DispId(HTMLDispIDs.DispidIhtmlselectionobjectEmpty)]
        void empty();
        [DispId(HTMLDispIDs.DispidIhtmlselectionobjectClear)]
        void clear();
        [DispId(HTMLDispIDs.DispidIhtmlselectionobjectType)]
        string EventType { [return: MarshalAs(UnmanagedType.BStr)] get; }
    }

    /// <summary>
    /// Dispids taken from MsHtmdid.h
    /// </summary>
    sealed class HTMLDispIDs
    {
        //    DISPIDs for interface IHTMLSelectionObject
        public const int DispidNormalFirst = 1000;
        public const int DispidSelectobj = DispidNormalFirst;

        public const int DispidIhtmlselectionobjectCreaterange = DispidSelectobj + 1;
        public const int DispidIhtmlselectionobjectEmpty = DispidSelectobj + 2;
        public const int DispidIhtmlselectionobjectClear = DispidSelectobj + 3;
        public const int DispidIhtmlselectionobjectType = DispidSelectobj + 4;
    }
}
