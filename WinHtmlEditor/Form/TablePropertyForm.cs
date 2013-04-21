#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace WinHtmlEditor
{
    /// <summary>
    /// Form used to enter an Html Table structure
    /// Input based on the HtmlTableProperty struct
    /// </summary>
    internal partial class TablePropertyForm : Form
    {

        // private variable for the table properties
        private HtmlTableProperty tableProperties;

        // constants for the Maximum values
        private const byte MAXIMUM_CELL_ROW = 250;
        private const byte MAXIMUM_CELL_COL = 50;
        private const byte MAXIMUM_CELL_PAD = 25;
        private const byte MAXIMUM_BORDER = 25;
        private const ushort MAXIMUM_WIDTH_PERCENT = 100;
        private const ushort MAXIMUM_WIDTH_PIXEL = 2500;
        private const ushort WIDTH_INC_DIV = 20;


        /// <summary>
        /// Default form constructor
        /// </summary>
        public TablePropertyForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // define the dropdown list value
            this.listCaptionAlignment.Items.AddRange(Enum.GetNames(typeof(HorizontalAlignOption)));
            this.listCaptionLocation.Items.AddRange(Enum.GetNames(typeof(VerticalAlignOption)));
            this.listTextAlignment.Items.AddRange(Enum.GetNames(typeof(HorizontalAlignOption)));

            // ensure default values are listed in the drop down lists
            this.listCaptionAlignment.SelectedIndex = 0;
            this.listCaptionLocation.SelectedIndex = 0;
            this.listTextAlignment.SelectedIndex = 0;

            // define the new maximum values of the dialogs
            this.numericBorderSize.Maximum = MAXIMUM_BORDER;
            this.numericCellPadding.Maximum = MAXIMUM_CELL_PAD;
            this.numericCellSpacing.Maximum = MAXIMUM_CELL_PAD;
            this.numericRows.Maximum = MAXIMUM_CELL_ROW;
            this.numericColumns.Maximum = MAXIMUM_CELL_COL;
            this.numericTableWidth.Maximum = MAXIMUM_WIDTH_PIXEL;

            // define default values based on a new table
            this.TableProperties = new HtmlTableProperty(true);
            
        } //TablePropertyForm


        /// <summary>
        /// Property definition for the Table Properties
        /// Uses the HtmlTableProperty class definition
        /// </summary>
        /// <value></value>
        public HtmlTableProperty TableProperties
        {
            get
            {
                // define the appropriate table caption properties
                tableProperties.CaptionText = this.textTableCaption.Text;
                tableProperties.CaptionAlignment = (HorizontalAlignOption)this.listCaptionAlignment.SelectedIndex;
                tableProperties.CaptionLocation = (VerticalAlignOption)this.listCaptionLocation.SelectedIndex;
                // define the appropriate table specific properties
                tableProperties.BorderSize = (byte)Math.Min(this.numericBorderSize.Value, this.numericBorderSize.Maximum);
                tableProperties.TableAlignment = (HorizontalAlignOption)this.listTextAlignment.SelectedIndex;
                // define the appropriate table layout properties
                tableProperties.TableRows = (byte)Math.Min(this.numericRows.Value, this.numericRows.Maximum);
                tableProperties.TableColumns = (byte)Math.Min(this.numericColumns.Value, this.numericColumns.Maximum);
                tableProperties.CellPadding = (byte)Math.Min(this.numericCellPadding.Value, this.numericCellPadding.Maximum);
                tableProperties.CellSpacing = (byte)Math.Min(this.numericCellSpacing.Value, this.numericCellSpacing.Maximum);
                tableProperties.TableWidth = (ushort)Math.Min(this.numericTableWidth.Value, this.numericTableWidth.Maximum);
                tableProperties.TableWidthMeasurement = (this.radioWidthPercent.Checked) ? MeasurementOption.Percent : MeasurementOption.Pixel;
                // return the properties
                return tableProperties;
            }
            set
            {
                // persist the new values
                tableProperties = value;
                // define the dialog caption properties
                this.textTableCaption.Text = tableProperties.CaptionText;
                this.listCaptionAlignment.SelectedIndex = (int)tableProperties.CaptionAlignment;
                this.listCaptionLocation.SelectedIndex = (int)tableProperties.CaptionLocation;
                // define the dialog table specific properties
                this.numericBorderSize.Value = Math.Min(tableProperties.BorderSize, MAXIMUM_BORDER);
                this.listTextAlignment.SelectedIndex = (int)tableProperties.TableAlignment;
                // define the dialog table layout properties
                this.numericRows.Value = Math.Min(tableProperties.TableRows, MAXIMUM_CELL_ROW);
                this.numericColumns.Value = Math.Min(tableProperties.TableColumns, MAXIMUM_CELL_COL);
                this.numericCellPadding.Value = Math.Min(tableProperties.CellPadding, MAXIMUM_CELL_PAD);
                this.numericCellSpacing.Value = Math.Min(tableProperties.CellSpacing, MAXIMUM_CELL_PAD);
                this.radioWidthPercent.Checked = (tableProperties.TableWidthMeasurement == MeasurementOption.Percent);
                this.radioWidthPixel.Checked = (tableProperties.TableWidthMeasurement == MeasurementOption.Pixel);
                this.numericTableWidth.Value = Math.Min(tableProperties.TableWidth, this.numericTableWidth.Maximum);
            }

        } //TableProperties


        /// <summary>
        /// Property for the measurement values based on the selected mesaurment
        /// </summary>
        private void MeasurementOptionChanged(object sender, System.EventArgs e)
        {
            // define a dialog for a percentage change
            if (this.radioWidthPercent.Checked)
            {
                this.numericTableWidth.Maximum = MAXIMUM_WIDTH_PERCENT;
                this.numericTableWidth.Increment = MAXIMUM_WIDTH_PERCENT / WIDTH_INC_DIV;
            }
            // define a dialog for a pixel change
            if (this.radioWidthPixel.Checked)
            {
                this.numericTableWidth.Maximum = MAXIMUM_WIDTH_PIXEL;
                this.numericTableWidth.Increment = MAXIMUM_WIDTH_PIXEL / WIDTH_INC_DIV;
            }

        } //MeasurementOptionChanged

    }
}
