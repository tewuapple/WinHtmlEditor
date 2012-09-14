using System;
using System.Windows.Forms;

namespace WinHtmlEditor
{
    partial class FrmTable : Form
    {
        public FrmTable()
        {
            InitializeComponent();
            comboAlignment.SelectedIndex = 0; //Default
        }
        
        public DialogResult MResult = DialogResult.Cancel;
        public int MRows = 1;
        public int MCols = 1;
        public string MAlignment = string.Empty;
        public int MBorderSize = 1;
        public int MCellPadding = 1;
        public int MCellSpacing = 2;
        public string MWidthPercent = string.Empty;
        public int MWidthPixels;
        public bool MWidthSpecified;

        private void FillinGlobals()
        {
            MResult = DialogResult.OK;

            MRows = (int)UpDownNumberOfRows.Value;
            MCols = (int)UpDownNumberOfCols.Value;

            MAlignment = string.Empty;
            if (comboAlignment.SelectedIndex > 0)
                MAlignment = comboAlignment.Items[comboAlignment.SelectedIndex].ToString();
            MBorderSize = (int)UpDownBorderSize.Value;
            MCellPadding = (int)UpDownCellPadding.Value;
            MCellSpacing = (int)UpDownCellSpacing.Value;

            MWidthPercent = string.Empty;
            MWidthPixels = 0;
            MWidthSpecified = false;
            if( (chkSpecifyWidth.Checked) && (UpDownWidth.Text.Length > 0) )
            {
                MWidthSpecified = true;
                if (radioBtnWidthInPercentage.Checked)
                    MWidthPercent = UpDownWidth.Text + "%";
                else
                    MWidthPixels = Convert.ToInt32(UpDownWidth.Text);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Hide();
            FillinGlobals();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Hide();
            MResult = DialogResult.Cancel;
        }

        private void FrmTableForm_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
                MResult = DialogResult.Cancel;
            }
        }
    }
}