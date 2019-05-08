using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WinHtmlEditor.Common
{
    public class SubWebBroswer : System.Windows.Forms.WebBrowser
    {
        public SubWebBroswer()
            : base()
        {
        }


        public override bool PreProcessMessage(ref Message msg)
        {
            switch (msg.Msg)
            {
                case 0x100://WM_KEYDOWN
                    int vk = (int)(msg.WParam);
                    if (vk == 116) return false;    //钩掉f5
                    break;

            }
            return base.PreProcessMessage(ref msg);
        }
    }
}
