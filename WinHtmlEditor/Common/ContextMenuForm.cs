using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinHtmlEditor
{
    /// <summary>
    /// Provides a System.Windows.Forms.Form that have a ContextMenu behavior.
    /// Use this Form by extending it or by adding the control using the method:
    /// <code>SetContainingControl(Control control)</code>
    /// </summary>
    public partial class ContextMenuForm : Form
    {
        private bool _locked;

        /// <summary>
        /// Gets or sets a value indicating that the form is locked.
        /// The form should be locked when opening a Dialog on it.
        /// </summary>
        public bool Locked
        {
            get { return _locked; }
            set 
            {
                _locked = value; 
            }
        }

        private Control _parentControl = null;

        /// <summary>
        /// Initialize a new instace of the ContextMenuForm in order to hold a control that
        /// needes to have a ContextMenu behavior.
        /// </summary>
        public ContextMenuForm()
        {
            InitializeComponent();
        }
    
        /// <summary>
        /// Shows the form on the specifies parent in the specifies location.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="startLocation"></param>
        /// <param name="width"></param>
        public void Show(Control parent, Point startLocation, int width)
        {
            _parentControl = parent;
            Point location = parent.PointToScreen(startLocation);
            this.Location = location;            
            this.Width = width;
            this.Show();
        }
        /// <summary>
        /// Set the control that will populate the ContextMenuForm.
        /// <remarks>
        /// Any scrolling should be implemented in the control it self, the 
        /// ContextMenuForm will not support scrolling.
        /// </remarks>
        /// </summary>
        /// <param name="control"></param>
        public void SetContainingControl(Control control)
        {
            panelMain.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelMain.Controls.Add(control);
        }

        private void ContextMenuPanel_Deactivate(object sender, EventArgs e)
        {
            if (!Locked)
            {
                Hide();                
            }
        }

        private void ContextMenuPanel_Leave(object sender, EventArgs e)
        {
            if (!Locked)
            {
                Hide();
            }
        }

        new public void Hide()
        {
            base.Hide();
        }
    }
}