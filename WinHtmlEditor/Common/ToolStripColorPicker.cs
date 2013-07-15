using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace WinHtmlEditor
{
    /// <summary>
    /// Specifies the display options for the ToolStripColorPicker such as
    /// image, text and underline.
    /// </summary>
    public enum ToolStripColorPickerDisplayType
    {
        // <summary>
        /// Specifies that only a normal image is to be rendered for this ToolStripColorPicker
        /// </summary>   
        NormalImage,
        /// <summary>
        /// Specifies that both image and text are to be rendered for this ToolStripColorPicker
        /// </summary>
        NormalImageAndText,
        /// <summary>
        /// Specifies that both color under-line and image are to be rendered for this ToolStripColorPicker
        /// </summary>
        UnderLineAndImage,
        /// <summary>
        /// Specifies that both color under-line and text are to be rendered for this ToolStripColorPicker
        /// </summary>
        UnderLineAndText,
        /// <summary>
        /// Specifies that both color under-line, text and image are to be rendered for this ToolStripColorPicker
        /// </summary>
        UnderLineTextAndImage,
        /// <summary>
        /// Specifies that only a color under-line is to be rendered for this ToolStripColorPicker
        /// </summary>
        UnderLineOnly,
        /// <summary>
        /// Specified that neither image, text nor under-line is to be rendered for this ToolStripColorPicker
        /// </summary>
        None,
        /// <summary>
        /// Specifies that only text is to be rendered for this ToolStripColorPicker            
        /// </summary>
        Text
    }

    /// <summary>
    /// Represents a ToolStripButtonItem that contains Color Picker control.
    /// </summary>
    [DefaultEvent("SelectedColorChanged"), DefaultProperty("Color"),
    Description("ToolStripItem that allows selecting a color from a color picker control."),
    ToolboxItem(false),
    ToolboxBitmap(typeof(ToolStripColorPicker), "ToolStripColorPicker")]
    public class ToolStripColorPicker : ToolStripSplitButton
    {

        #region Events
        /// <summary>
        /// Occurs when the value of the Color property changes. 
        /// </summary>
        [Category("Behavior"), Description("Occurs when the value of the Color property changes.")]
        public event EventHandler SelectedColorChanged;
        /// <summary>
        /// Occurs when the button portion of the color is clicked. 
        /// </summary>
        [Category("Behavior"), Description("Occurs when the button portion of the color is clicked.")]
        public event EventHandler ButtonPortionClicked;
        #endregion

        #region Properties

        private ToolStripColorPickerDisplayType _buttonDisplayStyle = ToolStripColorPickerDisplayType.UnderLineAndImage;
        /// <summary>
        /// Gets or sets the ToolStripColorPickerDisplayType in order to
        /// specified the display style of the button - image, text, underline etc.
        /// </summary>
        [Category("Appearance"), Description("Specifies whether to display the image, text and underline on the button.")
        , DefaultValue(typeof(ToolStripColorPickerDisplayType), "ToolStripColorPickerDisplayType.UnderLineAndImage")]
        public ToolStripColorPickerDisplayType ButtonDisplayStyle
        {
            get { return _buttonDisplayStyle; }
            set
            {
                _buttonDisplayStyle = value;
                UpdateDisplayStyle();
            }
        }

        /// <summary>
        /// Overrides, Gets or sets the ToolStripItem.DisplayStyle property, use
        /// the ButtonDisplayStyle instead.
        /// </summary>
        [Browsable(false)]
        public override ToolStripItemDisplayStyle DisplayStyle
        {
            get { return base.DisplayStyle; }
            set { base.DisplayStyle = value; }
        }

        /// <summary>
        /// Gets or sets the color assign to the color picker control.
        /// </summary>
        [Category("Data"),
        Description("Gets or sets the color assign to the color picker control."),
        DefaultValue(typeof(Color), "Color.Black")]
        public Color Color
        {
            get { return _colorPicker.Color; }
            set
            {
                _colorPicker.Color = value;
                Refresh();
                OnSelectedColorChanged(EventArgs.Empty);
            }
        }

        private bool _addColorNameToToolTip = true;
        /// <summary>
        /// Gets or sets value indicating whether to render the color name to the tool tip text.
        /// </summary>
        [DefaultValue(true),
        Category("Behavior"), Description("Value indicating whether to render the color name to the tool tip text.")]
        public bool AddColorNameToToolTip
        {
            get { return _addColorNameToToolTip; }
            set { _addColorNameToToolTip = value; }
        }

        private string _originalToolTipText = "";
        /// <summary>
        /// Gets or sets the text that appears as a tooltip in the button.
        /// the color name will be rendered to the tooltip if the AddColorNameToolTip property set to true.
        /// </summary>
        [Category("Behavior"), Description("The text that appears as a tooltip (the color name will be render  automatically if defined to do so.")]
        new public string ToolTipText
        {
            get { return _originalToolTipText; }
            set
            {
                _originalToolTipText = value;
                if (_addColorNameToToolTip)
                {
                    base.ToolTipText = _originalToolTipText +
                        " (" + _colorPicker.ColorName + ")";
                }
                else
                {
                    base.ToolTipText = value;
                }
            }
        }

        #endregion

        #region Private Members
        /// <summary>
        /// The color picker control that opens when clicking on the button
        /// </summary>
        private OfficeColorPicker _colorPicker = new OfficeColorPicker();
        /// <summary>
        /// Default color rectangle (under line)
        /// </summary>
        private Rectangle _colorRectangle = new Rectangle(2, 17, 14, 4);
        /// <summary>
        /// The underline picture rectangle - stretch to 14X14
        /// </summary>
        private Rectangle _pictureRectangle = new Rectangle(2, 2, 14, 14);
        // Settings for the button painting
        private bool _showColorUnderLine = true;
        private bool _showUnderLineImage = true;
        private bool _showUnderLineText = false;

        private System.ComponentModel.IContainer components = null;


        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the ToolStripColorPicker that holds
        /// OfficeColorPicker control inside a ToolStripItem to add to ToolStrip containers.
        /// </summary>
        public ToolStripColorPicker()
            : base()
        {
            InitControl();
        }

        /// <summary>
        /// Initializes a new instance of the ToolStripColorPicker that holds
        /// OfficeColorPicker control inside a ToolStripItem to add to ToolStrip containers.
        /// </summary>
        /// <param name="startingColor">The color to assign to the color picker control</param>
        public ToolStripColorPicker(Color startingColor)
            : base()
        {
            Color = startingColor;
            InitControl();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                if (!_colorPicker.IsDisposed)
                    _colorPicker.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Inits
        /// <summary>
        /// Set starting properties for the control and register the needed events.
        /// </summary>
        private void InitControl()
        {
            _colorPicker.SelectedColorChanged += new EventHandler(HandleSelectedColorChanged);
            this.AutoSize = false;
            this.Width = 30;
            this.DropDownItems.Add("test");
        }
        /// <summary>
        /// Set the painting properties by the _buttonDisplayStyle property.
        /// </summary>
        private void UpdateDisplayStyle()
        {
            switch (_buttonDisplayStyle)
            {
                case ToolStripColorPickerDisplayType.NormalImage:
                    DisplayStyle = ToolStripItemDisplayStyle.Image;
                    _showColorUnderLine = false;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                case ToolStripColorPickerDisplayType.NormalImageAndText:
                    DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    _showColorUnderLine = false;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                case ToolStripColorPickerDisplayType.UnderLineAndImage:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = true;
                    _showUnderLineImage = true;
                    _showUnderLineText = false;
                    break;
                case ToolStripColorPickerDisplayType.UnderLineAndText:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = true;
                    _showUnderLineImage = false;
                    _showUnderLineText = true;
                    break;
                case ToolStripColorPickerDisplayType.UnderLineTextAndImage:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = true;
                    _showUnderLineImage = true;
                    _showUnderLineText = true;
                    break;
                case ToolStripColorPickerDisplayType.UnderLineOnly:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = true;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                case ToolStripColorPickerDisplayType.None:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = false;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                case ToolStripColorPickerDisplayType.Text:
                    DisplayStyle = ToolStripItemDisplayStyle.Text;
                    _showColorUnderLine = false;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                default:
                    break;
            }
            Refresh();
        }

        #endregion

        #region Overrides (event handlers)

        protected override void OnButtonClick(EventArgs e)
        {
            base.OnButtonClick(e);
            OnButtonPortionClicked(e);
        }
        protected override void OnDropDownOpened(EventArgs e)
        {
            Point startPoint = GetOpenPoint();
            _colorPicker.Show(startPoint);
        }


        /// <summary>
        /// Gets the button position by the parent ToolStrip
        /// </summary>
        /// <returns></returns>
        private Point GetOpenPoint()
        {
            if (this.Owner == null) return new Point(5, 5);
            int x = 0;
            foreach (ToolStripItem item in this.Parent.Items)
            {
                if (item == this) break;
                if (item.Visible) 
                    x += item.Width;
            }
            return this.Owner.PointToScreen(new Point(x, -4));
        }

        /// <summary>
        /// Fires the SelectedColorChanged event.
        /// </summary>
        /// <param name="e"></param>
        public void OnSelectedColorChanged(EventArgs e)
        {
            if (SelectedColorChanged != null)
                SelectedColorChanged(this, e);
        }
        /// <summary>
        /// Fires the ButtonPortionClicked event.
        /// </summary>
        /// <param name="e"></param>
        public void OnButtonPortionClicked(EventArgs e)
        {
            if (ButtonPortionClicked != null)
                ButtonPortionClicked(this, e);
        }
        /// <summary>
        /// Repaint the button with the new color 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSelectedColorChanged(object sender, EventArgs e)
        {
            Refresh();
            OnSelectedColorChanged(EventArgs.Empty);
        }
        /// <summary>
        /// Repaint the parent tool strip and the button tool tip
        /// </summary>
        private void Refresh()
        {
            // Call the tool tip set property to add color name to tool tip
            ToolTipText = _originalToolTipText;
            // Refresh the toolstrip that holds this button.
            if (this.Owner != null)
                this.Owner.Refresh();
        }
        #endregion

        #region Painting the Button
        /// <summary>
        /// Paints the underline rectangle.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        private void PaintUnderLine(Graphics g)
        {
            Color lineColor = Color;
            if (!Enabled)
                lineColor = Color.Gray;
            // Paint the line down on the button
            using (Brush brush = new SolidBrush(lineColor))
            {
                _colorRectangle = new Rectangle(2, this.Height - 6,
                    this.Width - 16, 4);                
                g.FillRectangle(brush, _colorRectangle);
            }
        }
        /// <summary>
        /// Paints the under line image
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private Size PaintUnderLineImage(Graphics g)
        {
            Bitmap bmp = this.Image as Bitmap;
            // Paints the image, if exists
            if (bmp != null)
            {
                bmp.MakeTransparent(this.ImageTransparentColor);
                g.DrawImage(bmp, _pictureRectangle,
                        0, 0, bmp.Width, bmp.Height,
                        GraphicsUnit.Pixel,
                        Enabled ? null : CustomColors.GrayScaleAttributes
                        );
                return bmp.Size;
            }
            else
            {
                return new Size(0, 0);
            }
        }

       
        /// <summary>
        /// Paints the underline text
        /// </summary>
        /// <param name="g"></param>
        /// <param name="imageSize"></param>
        /// <param name="bounds"></param>
        private void PaintUnderLineText(Graphics g, Size imageSize)
        {
            Color textColor = ForeColor;
            if (!Enabled)
                textColor = Color.Gray;
            using (Brush brush = new SolidBrush(textColor))
            {
                int x = imageSize.Width + 2;
                int y = 2;
                Rectangle stringRec = new Rectangle(x, y, this.Width - x, this.Height - y);
                g.DrawString(Text, this.Font, brush, stringRec);
            }
        }

        /// <summary>
        /// Overrides, Paint the image in the specified scale and the color line if defined.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_showColorUnderLine)
            {
                base.OnPaint(e);
                Size imageSize = new Size(0, 0);
                PaintUnderLine(e.Graphics);
                if (this.Image != null && _showUnderLineImage)
                {
                    imageSize = PaintUnderLineImage(e.Graphics);
                }
                if (_showUnderLineText)
                {
                    PaintUnderLineText(e.Graphics, imageSize);
                }
            }
            else
            {
                base.OnPaint(e);
            }
        }
        #endregion


    }
}