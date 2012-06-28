using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinHtmlEditor
{
    partial class frmTable : Form
    {
        public frmTable()
        {
            InitializeComponent();
            comboAlignment.SelectedIndex = 0; //Default
        }
        
        public DialogResult m_Result = DialogResult.Cancel;
        public int m_Rows = 1;
        public int m_Cols = 1;
        public string m_Alignment = string.Empty;
        public int m_BorderSize = 1;
        public int m_CellPadding = 1;
        public int m_CellSpacing = 2;
        public string m_WidthPercent = string.Empty;
        public int m_WidthPixels = 0;
        public bool m_WidthSpecified = false;

        private void FillinGlobals()
        {
            m_Result = DialogResult.OK;

            m_Rows = (int)UpDownNumberOfRows.Value;
            m_Cols = (int)UpDownNumberOfCols.Value;

            m_Alignment = string.Empty;
            if (comboAlignment.SelectedIndex > 0)
                m_Alignment = comboAlignment.Items[comboAlignment.SelectedIndex].ToString();
            m_BorderSize = (int)UpDownBorderSize.Value;
            m_CellPadding = (int)UpDownCellPadding.Value;
            m_CellSpacing = (int)UpDownCellSpacing.Value;

            m_WidthPercent = string.Empty;
            m_WidthPixels = 0;
            m_WidthSpecified = false;
            if( (chkSpecifyWidth.Checked) && (UpDownWidth.Text.Length > 0) )
            {
                m_WidthSpecified = true;
                if (radioBtnWidthInPercentage.Checked)
                    m_WidthPercent = UpDownWidth.Text + "%";
                else
                    m_WidthPixels = Convert.ToInt32(UpDownWidth.Text);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Hide();
            FillinGlobals();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            m_Result = DialogResult.Cancel;
        }

        private void frmTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
                m_Result = DialogResult.Cancel;
            }
        }
    }
}