using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using mshtml;

namespace WinHtmlEditor
{
    #region HTMLEditHelper class
    public class HTMLEditHelper
    {
        private IHTMLDocument2 m_pDoc2 = null;

        public IHTMLDocument2 DOMDocument
        {
            get { return m_pDoc2; }
            set { m_pDoc2 = value; }
        }

        public string HtmlSpace = "&nbsp;";  // space

        public HTMLEditHelper()
        {

        }

        #region Table specific

        //bordersize 2 or "2"
        public bool AppendTable(int colnum, int rownum, int bordersize, string alignment, int cellpadding, int cellspacing, string widthpercentage, int widthpixel)
        {
            if (m_pDoc2 == null)
                return false;
            IHTMLTable t = (IHTMLTable)m_pDoc2.createElement("table");
            //set the cols
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
            //Insert rows and fill them with space
            int cells = colnum - 1;
            int rows = rownum - 1;

            CalculateCellWidths(colnum);
            for (int i = 0; i <= rows; i++)
            {
                IHTMLTableRow tr = (IHTMLTableRow)t.insertRow(-1);
                for (int j = 0; j <= cells; j++)
                {
                    IHTMLElement c = tr.insertCell(-1) as IHTMLElement;
                    if (c != null)
                    {
                        c.innerHTML = HtmlSpace;
                        IHTMLTableCell tcell = c as IHTMLTableCell;
                        if (tcell != null)
                        {
                            //set width so as user enters text
                            //the cell width would not adjust
                            if (j == cells) //last cell
                                tcell.width = m_lastcellwidth;
                            else
                                tcell.width = m_cellwidth;
                        }
                    }
                }
            }
            //Append to body DOM collection
            return PasteIntoSelection((t as IHTMLElement).outerHTML);
        }

        private string m_cellwidth = string.Empty;
        private string m_lastcellwidth = string.Empty;

        private void CalculateCellWidths(int numberofcols)
        {
            //Even numbers. for 2 cols; 50%, 50%
            //Odd numbers.  for 3 cols; 33%, 33%, 34%
            int cellwidth = (int)(100 / numberofcols);
            m_cellwidth = cellwidth.ToString() + "%";
            //modulus operator (%).
            //http://msdn2.microsoft.com/en-us/library/0w4e0fzs.aspx
            //http://msdn2.microsoft.com/en-us/library/6a71f45d.aspx
            cellwidth += 100 % numberofcols;
            m_lastcellwidth = cellwidth.ToString() + "%";
        }

        /// <summary>
        /// The currently selected text/controls will be replaced by the given HTML code.
        /// If nothing is selected, the HTML code is inserted at the cursor position
        /// </summary>
        /// <param name="s_Html"></param>
        /// <returns></returns>
        public bool PasteIntoSelection(string s_Html)
        {
            if (m_pDoc2 == null)
                return false;
            IHTMLSelectionObject sel = m_pDoc2.selection as IHTMLSelectionObject;
            if (sel == null)
                return false;
            IHTMLTxtRange range = sel.createRange() as IHTMLTxtRange;
            if (range == null)
                return false;
            //none
            //text
            //control
            if ((!String.IsNullOrEmpty(sel.EventType)) &&
                (sel.EventType == "control"))
            {
                IHTMLControlRange ctlrange = range as IHTMLControlRange;
                if (ctlrange != null) //Control(s) selected
                {
                    IHTMLElement elem = null;
                    int ctls = ctlrange.length;
                    for (int i = 0; i < ctls; i++)
                    {
                        //Remove all selected controls
                        elem = ctlrange.item(i);
                        if (elem != null)
                        {
                            RemoveNode(elem, true);
                        }
                    }
                    // Now get the textrange after deleting all selected controls
                    range = null;
                    range = sel.createRange() as IHTMLTxtRange;
                }
            }

            if (range != null)
            {
                // range will be valid if nothing is selected or if text is selected
                // or if multiple elements are seleted
                range.pasteHTML(s_Html);
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
        /// <param name="node">element to remove</param>
        /// <param name="RemoveAllChildren"></param>
        public IHTMLDOMNode RemoveNode(IHTMLElement elem, bool RemoveAllChildren)
        {
            IHTMLDOMNode node = elem as IHTMLDOMNode;
            if (node != null)
                return node.removeNode(RemoveAllChildren);
            else
                return null;
        }

        #endregion

    }
    #endregion

    [ComVisible(true), ComImport()]
    [TypeLibType((short)4160)]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3050f25A-98b5-11cf-bb82-00aa00bdce0b")]
    interface IHTMLSelectionObject
    {
        [return: MarshalAs(UnmanagedType.IDispatch)]
        [DispId(HTMLDispIDs.DISPID_IHTMLSELECTIONOBJECT_CREATERANGE)]
        object createRange();
        [DispId(HTMLDispIDs.DISPID_IHTMLSELECTIONOBJECT_EMPTY)]
        void empty();
        [DispId(HTMLDispIDs.DISPID_IHTMLSELECTIONOBJECT_CLEAR)]
        void clear();
        [DispId(HTMLDispIDs.DISPID_IHTMLSELECTIONOBJECT_TYPE)]
        string EventType { [return: MarshalAs(UnmanagedType.BStr)] get; }
    }

    /// <summary>
    /// Dispids taken from MsHtmdid.h
    /// </summary>
    sealed class HTMLDispIDs
    {
        //    DISPIDs for interface IHTMLSelectionObject
        public const int DISPID_NORMAL_FIRST = 1000;
        public const int DISPID_SELECTOBJ = DISPID_NORMAL_FIRST;

        public const int DISPID_IHTMLSELECTIONOBJECT_CREATERANGE = DISPID_SELECTOBJ + 1;
        public const int DISPID_IHTMLSELECTIONOBJECT_EMPTY = DISPID_SELECTOBJ + 2;
        public const int DISPID_IHTMLSELECTIONOBJECT_CLEAR = DISPID_SELECTOBJ + 3;
        public const int DISPID_IHTMLSELECTIONOBJECT_TYPE = DISPID_SELECTOBJ + 4;
    }
}
