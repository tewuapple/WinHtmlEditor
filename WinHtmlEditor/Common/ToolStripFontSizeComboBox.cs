using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace WinHtmlEditor.Common
{
    public class ToolStripFontSizeComboBox : ToolStripComboBox
    {
        #region  Public Constructors

        public ToolStripFontSizeComboBox()
        {
            DrawMode = DrawMode.OwnerDrawVariable;
            if (!ComboBox.IsNull())
            {
                ComboBox.DrawItem += OnDrawItem;
                ComboBox.MeasureItem += OnMeasureItem;
            }
        }

        #endregion  Public Constructors

        #region  Public Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public DrawMode DrawMode
        {
            get
            {
                if (!ComboBox.IsNull()) return ComboBox.DrawMode;
                return DrawMode.Normal;
            }
            set { if (!ComboBox.IsNull()) ComboBox.DrawMode = value; }
        }

        #endregion  Public Properties

        #region  Protected Overridden Methods

        protected void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index <= -1 || e.Index >= Items.Count) return;
            e.DrawBackground();

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();

            using (var textBrush = new SolidBrush(e.ForeColor))
            {
                Font myFont = new Font(this.Font.FontFamily, GetFontSize(e.Index), FontStyle.Bold);
                e.Graphics.DrawString(Items[e.Index].ToString(), myFont, textBrush, e.Bounds);
            }
        }

        private float GetFontSize(int index)
        {
            float sz = 14;
            switch (index)
            {
                case 0:
                    sz = 8;
                    break;
                case 1:
                    sz = 10;
                    break;
                case 2:
                    sz = 12;
                    break;
                case 3:
                    sz = 14;
                    break;
                case 4:
                    sz = 18;
                    break;
                case 5:
                    sz = 24;
                    break;
                case 6:
                    sz = 36;
                    break;
                default:
                    sz = 14;
                    break;
            }
            return sz;
        }
        protected void OnMeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < Items.Count)
            {
                Font myFont = new Font(this.Font.FontFamily, GetFontSize(e.Index), FontStyle.Bold);
                e.ItemHeight = (int)e.Graphics.MeasureString(Items[e.Index].ToString(), myFont).Height;
                e.ItemWidth = (int)e.Graphics.MeasureString(Items[e.Index].ToString(), myFont).Width;
            }
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            AdjustComboBoxDropDownListWidth();  //调整comboBox的下拉列表的大小
        }

        private void AdjustComboBoxDropDownListWidth()
        {
            Graphics g = null;
            try
            {
                int width = Width;
                g = this.ComboBox.CreateGraphics();

                //checks if a scrollbar will be displayed.
                //If yes, then get its width to adjust the size of the drop down list.
                int vertScrollBarWidth =
                    (Items.Count > MaxDropDownItems)
                        ? SystemInformation.VerticalScrollBarWidth
                        : 0;
                Font myFont = new Font(this.Font.FontFamily, GetFontSize(this.Items.Count-1), FontStyle.Bold);
                //set the width of the drop down list to the width of the largest item.
                DropDownWidth = (int)g.MeasureString(this.Items[this.Items.Count-1].ToString().Trim(), myFont).Width+30
                           + vertScrollBarWidth;
            }
            catch
            { }
            finally
            {
                if (g != null)
                    g.Dispose();
            }
        }

        #endregion  Protected Overridden Methods


    }
}
