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

        #region Table specific

        /// <summary>
        /// The currently selected text/controls will be replaced by the given HTML code.
        /// If nothing is selected, the HTML code is inserted at the cursor position
        /// </summary>
        /// <param name="sHtml"></param>
        /// <returns></returns>
        public static bool PasteIntoSelection(string sHtml)
        {
            if (_mPDoc2.IsNull())
                return false;
            var sel = _mPDoc2.selection as IHTMLSelectionObject;
            if (sel.IsNull())
                return false;
            var range = sel.createRange() as IHTMLTxtRange;
            if (range.IsNull())
                return false;
            //none
            //text
            //control
            if ((!sel.EventType.IsNullOrEmpty()) &&
                (sel.EventType == "control"))
            {
                var ctlrange = range as IHTMLControlRange;
                if (!ctlrange.IsNull()) //Control(s) selected
                {
                    int ctls = ctlrange.length;
                    for (int i = 0; i < ctls; i++)
                    {
                        //Remove all selected controls
                        IHTMLElement elem = ctlrange.item(i);
                        if (!elem.IsNull())
                        {
                            RemoveNode(elem, true);
                        }
                    }
                    // Now get the textrange after deleting all selected controls
                    range = sel.createRange() as IHTMLTxtRange;
                }
            }

            if (!range.IsNull())
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
            if (!node.IsNull())
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
