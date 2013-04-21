using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinHtmlEditor
{
    /// <summary>
    /// Provides color picker control that could be used in a model or non-model form.
    /// </summary
    [DefaultEvent("SelectedColorChanged"), DefaultProperty("Color"), 
    ToolboxItem(true) ,
   ToolboxBitmap(typeof(OfficeColorPicker), "OfficeColorPicker"),
    Description("Provides color picker control that could be used in a model or non-model form.")]       
    public partial class OfficeColorPicker : UserControl
    {
        #region Static Methods
        /// <summary>
        /// The preferred height to span the control to
        /// </summary>
        public static readonly int PreferredHeight = 120;
        /// <summary>
        /// The preferred width to span the control to
        /// </summary>
        public static readonly int PreferredWidth = 146;

        #endregion

        #region Events
        /// <summary>
        /// Occurs when the value of the Color property changes. 
        /// </summary>
        [Category("Behavior"), Description("Occurs when the value of the Color property changes.")]
        public event EventHandler SelectedColorChanged;
        #endregion

        #region Properties
        private Color _color = Color.Black;
        /// <summary>
        /// Gets or sets the selected color from the OfficeColorPicker
        /// </summary>
        [Category("Data"), Description("The color selected in the dialog"),
       DefaultValue(typeof(Color), "System.Drawing.Color.Black")]
        public Color Color
        {
            get { return _color; }
            set 
            { 
                _color = value;
                // Set Selected Color in the GUI
                SetColor(value);
                // Fires the SelectedColorChanged event.
                OnSelectedColorChanged(EventArgs.Empty);                
            }
        }
        /// <summary>
        /// Gets the selected color name, or 'Custom' if it is not one
        /// of the Selectable colors.
        /// </summary>
        [Browsable(false)]
        public string ColorName
        {
            get
            {
                string colorName = "Custom";
                if (_currentSelected > -1 &&
                    _currentSelected < CustomColors.SelectableColorsNames.Length)
                {
                    colorName =
                        CustomColors.SelectableColorsNames[_currentSelected];
                }
                return colorName;

            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Parent form when this control is inside a context menu form
        /// </summary>
        private ContextMenuForm _contextForm;
        /// <summary>
        /// Parent control, when on of the Show(Control parent ...) is called.
        /// </summary>
        private Control _parentControl;
        /// <summary>
        /// Known colors list that user may select from 
        /// </summary>
        private SelectableColor[] colors = new SelectableColor[40];
        /// <summary>
        /// Buttons rectangle definitions.
        /// </summary>
        private Rectangle[] buttons = new Rectangle[41];
        /// <summary>
        /// Hot Track index to paint its button with HotTrack color
        /// </summary>
        private int _currentHotTrack = -1;
        /// <summary>
        /// Last value of the hottrack to use to restore the location after key up\down from "more colors"
        /// </summary>
        private int _lastHotTrack = -1;
        /// <summary>
        /// Current selected index to paint its button with Selected color
        /// </summary>
        private int _currentSelected = -1;
        #endregion

        #region Ctor

        /// <summary>
        /// Initialized a new instance of the OfficeColorPicker in order to provide 
        /// color picker control that could be used in a model or non-model form.
        /// </summary>
        public OfficeColorPicker()
        {
            InitializeComponent();
            SetColorsObjects();
            // Set painting style for better performance.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// Initialized a new instance of the OfficeColorPicker in order to provide 
        /// color picker control that could be used in a model or non-model form.   
        /// </summary>
        /// <param name="startingColor">Starting color to the OfficeColorPicker control</param>
        public OfficeColorPicker(Color startingColor)
            : this()
        {
            Color = startingColor;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Opens the control inside a context menu in the specified location
        /// relative to the specified control.
        /// </summary>
        /// <param name="left">Parent control coordinates left location of the control</param>
        /// <param name="top">Parent control coordinates top location of the control</param>
        /// <param name="parent">Parent control to place the control at</param>
        public void Show(Control parent, int left, int top)
        {            
            Show(parent, new Point(left, top));
        }
        /// <summary>
        /// Opens the control inside a context menu in the specified location
        /// </summary>
        /// <param name="left">Screen coordinates left location of the control</param>
        /// <param name="top">Screen coordinates top location of the control</param>
        public void Show(int left, int top)
        {
            Show(new Point(left, top));
        }
        /// <summary>
        /// Opens the control inside a context menu in the specified location
        /// </summary>
        /// <param name="startLocation">Screen coordinates location of the control</param>
        public void Show(Point startLocation)
        {
            // Creates new contextmenu form, adds the control to it, display it.
            _contextForm = new ContextMenuForm();
            _contextForm.SetContainingControl(this);
            _contextForm.Height = OfficeColorPicker.PreferredHeight;
            _contextForm.Show(this, startLocation, OfficeColorPicker.PreferredWidth);
        }
        /// <summary>
        /// Opens the control inside a context menu in the specified location
        /// </summary>
        /// <param name="startLocation">Screen coordinates location of the control</param>
        /// <param name="parent">Parent control to place the control at</param> 
        public void Show(Control parent, Point startLocation)
        {
            _parentControl = parent;
            // Creates new contextmenu form, adds the control to it, display it.      
            ContextMenuForm frm = new ContextMenuForm();
            frm.SetContainingControl(this);
            frm.Height = OfficeColorPicker.PreferredHeight;
            _contextForm = frm;
            frm.Show(parent, startLocation, OfficeColorPicker.PreferredWidth);
        }
        /// <summary>
        /// Fires the OfficeColorPicker.SelectedColorChanged event
        /// </summary>
        /// <param name="e"></param>
        public void OnSelectedColorChanged(EventArgs e)
        {
            Refresh();
            if (SelectedColorChanged != null)
                SelectedColorChanged(this, e);
        }

        #endregion

        #region Private and protected methods

        /// <summary>
        /// Creates the custom colors buttons
        /// </summary>
        private void SetColorsObjects()
        {
            for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
            {
                colors[colorIndex] = new SelectableColor(CustomColors.SelectableColors[colorIndex]);
            }
        }
        /// <summary>
        /// Set color to the specified one
        /// </summary>
        /// <param name="color"></param>
        private void SetColor(Color color)
        {
            _currentHotTrack = -1;
            _currentSelected = -1;
            // Search the color on the known color list
            for (int colorIndex = 0; colorIndex < CustomColors.SelectableColors.Length; colorIndex++)
            {
                if (CustomColors.ColorEquals(CustomColors.SelectableColors[colorIndex], color))
                {
                    _currentSelected = colorIndex;
                    _currentHotTrack = -1;
                }
            }
            this.Refresh();
        }

        /// <summary>
        /// Overrides, when mouse move - allow the hot-track look-and-feel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            // Check cursor, if on one of the buttons - hot track it
            for (int recIndex = 0; recIndex < buttons.Length; recIndex++)
            {
                // Check that current mouse position is in one of the rectangle
                // to have HotTrack effect.
                if (buttons[recIndex].Contains(e.Location))
                {
                    _currentHotTrack = recIndex;
                    colorToolTip.SetToolTip(this, CustomColors.SelectableColorsNames[recIndex]);
                }
            }
            this.Refresh();
        }
        /// <summary>
        /// Overrides, when click on, handles color selection.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            // Check cursor, if on one of the buttons - hot track it
            for (int recIndex = 0; recIndex < buttons.Length; recIndex++)
            {
                if (buttons[recIndex].Contains(e.Location))
                {
                    _currentSelected = recIndex;
                    // More colors - open dialog
                    if (_currentSelected == 40)
                    {
                        Color = OpenMoreColorsDialog();
                    }
                    else
                    {
                        Color = CustomColors.SelectableColors[recIndex];
                        colorToolTip.SetToolTip(this, CustomColors.SelectableColorsNames[recIndex]);
                    }
                    if (_contextForm != null)
                        _contextForm.Hide();
                    _contextForm = null;                   
                }
            }
            this.Refresh();
        }
        /// <summary>
        /// Open the 'More Color' dialog, that is, a normal ColorDialog control.
        /// </summary>
        /// <returns></returns>
        private Color OpenMoreColorsDialog()
        {
            colorDialog.Color = Color;
           
            Form parentForm = this.FindForm();
            ContextMenuForm contextFrm = parentForm as ContextMenuForm;
            if (contextFrm != null)
            {
                // Ignore lost focus events on owner context menu form
                contextFrm.Locked = true;
                colorDialog.ShowDialog(contextFrm);
                // Give the focus back to owner form
                if (_parentControl != null)
                {
                    _parentControl.FindForm().BringToFront();
                }               
                // Active lost focus events on owner context menu form
                contextFrm.Locked = false;
            }
            else
            {
                colorDialog.ShowDialog(this); // (parentForm);            
            }
            return colorDialog.Color;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _currentHotTrack = -1;
            Refresh();
        }
        /// <summary>
        /// Override, paint background to white
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            using(Brush brush = new SolidBrush(CustomColors.ColorPickerBackgroundDocked))
            {
                pevent.Graphics.FillRectangle(brush, pevent.ClipRectangle);
            }
        }
        
        /// <summary>
        /// Overrides, paint all buttons
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            int x = 0, y = 0;
            int recWidth = 18, recHeight = 18;
            // Go over all colors buttons, paint them
            for (int colorIndex = 0; colorIndex < colors.Length; colorIndex++)
            {
                // Check if current button is selected and/or hottrack
                bool hotTrack = colorIndex == _currentHotTrack;
                bool selected = colorIndex == _currentSelected;
                // Paints the color button itself, saving the rectangle of the
                // button.
                buttons[colorIndex] =
                    PaintColor(e.Graphics, colors[colorIndex].Color, hotTrack, selected, x, y);
                x += recWidth;
                // Each row has 8 buttons, so move y down when its in the end of the line
                if (x > 7 * recWidth)
                {
                    x = 0;
                    y += recHeight;
                }
            }
            y += 4;
            PaintMoreColorsButton(e.Graphics, x, y);
        }
        /// <summary>
        /// Paints the more colors button
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void PaintMoreColorsButton(Graphics graphics, int x, int y)
        {
            // The rectangle for the 'More Colors' button
            Rectangle buttonRec = new Rectangle(x, y, 8 * 18 - 1, 22);
            // The text will be displayed in the center of the rectangle
            // using format string.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Font buttonFont = new Font("Arial", 8);
            bool selected = _currentSelected == 40;
            bool hotTrack = _currentHotTrack == 40;
           
            // Paints the button with the selected, hot track settings if needed
            using (Brush hotTrackBrush = new SolidBrush(CustomColors.ButtonHoverLight))
            using (Brush selectedBrush = new SolidBrush(CustomColors.ButtonHoverDark))
            using (Pen selectedPen = new Pen(CustomColors.SelectedBorder))
            using (Pen borderPen = new Pen(CustomColors.ButtonBorder))
            {
                if (hotTrack)
                {
                    graphics.FillRectangle(hotTrackBrush, buttonRec);
                    graphics.DrawRectangle(selectedPen, buttonRec);
                }
                else if (selected)
                {
                    graphics.FillRectangle(selectedBrush, buttonRec);
                    graphics.DrawRectangle(selectedPen, buttonRec);
                }
            }
            //  Draw the string itself using the rectangle, font and format specified.
            graphics.DrawString("More Colors...", buttonFont, Brushes.Black, buttonRec, format);
            format.Dispose();
            buttonFont.Dispose();
            buttons[40] = buttonRec;         
        }
        /// <summary>
        /// Paints one color button
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="color"></param>
        /// <param name="hotTrack"></param>
        /// <param name="selected"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Rectangle PaintColor(Graphics graphics, Color color, bool hotTrack, bool selected, int x, int y)
        {            
            // Button inside rectangle
            Rectangle mainRec = new Rectangle(x + 3, y + 3, 11, 11);
            // Button border rectangle 
            Rectangle borderRec = new Rectangle(x, y, 17, 17);
            // Check if the button is selected and HotTrack ( no the same color)
            bool selectedAndHotTrack = selected && hotTrack;
            // Paints the button using the brushes needed
            using (Brush brush = new SolidBrush(color))
            using (Brush hotTrackBrush = new SolidBrush(CustomColors.ButtonHoverLight))
            using (Brush selectedBrush = new SolidBrush(CustomColors.ButtonHoverDark))
            using (Brush selectedHotTrackBrush = new SolidBrush(CustomColors.SelectedAndHover))
            using (Pen selectedPen = new Pen(CustomColors.SelectedBorder))
            using (Pen borderPen = new Pen(CustomColors.ButtonBorder))
            {
                // Paints the rectangle with the Track/Selected color
                // if this color is selected/hottrack
                if (selectedAndHotTrack)
                {
                    graphics.FillRectangle(selectedHotTrackBrush, borderRec);
                    graphics.DrawRectangle(selectedPen, borderRec);
                }
                else if (hotTrack)
                {
                    graphics.FillRectangle(hotTrackBrush, borderRec);
                    graphics.DrawRectangle(selectedPen, borderRec);
                }
                else if (selected)
                {
                    graphics.FillRectangle(selectedBrush, borderRec);
                    graphics.DrawRectangle(selectedPen, borderRec);
                }
                // Fills the rectangle with the current color, paints
                // the background.               
                graphics.FillRectangle(brush, mainRec);
                graphics.DrawRectangle(borderPen, mainRec);
            }
            return borderRec;
        }
        #endregion
    }
  }
