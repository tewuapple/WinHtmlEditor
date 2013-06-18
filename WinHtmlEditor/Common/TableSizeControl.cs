using System;
using System.Drawing;
using System.Windows.Forms;
namespace WinHtmlEditor
{
    public class TableSizeEventArgs : EventArgs
    {
        public TableSizeEventArgs(Size selectedSize)
        {
            this.selectedSize = selectedSize;
        }

        public Size SelectedSize
        {
            get { return this.selectedSize; }
        }

        private Size selectedSize;
    }

    public delegate void TableSizeSelectedEventHandler(object sender, TableSizeEventArgs e);

    class TableSizeControl : Control
    {
        public event TableSizeSelectedEventHandler TableSizeSelected;
        public event EventHandler SelectionCancelled;

        public TableSizeControl()
        {
            DoubleBuffered = true;

            SetStyle(ControlStyles.ResizeRedraw, true);
            UpdateLayout();
        }

        public int CellSpacing
        {
            get { return this.cellSpacing; }
            set
            {
                if (value <= 0 || value + 1 > CellSize)
                    throw new ArgumentOutOfRangeException();

                if (this.cellSpacing != value)
                {
                    this.cellSpacing = value;
                    UpdateLayout();
                }
            }
        }

        public int CellSize
        {
            get { return this.cellSize; }
            set
            {
                if (value <= 4)
                    throw new ArgumentOutOfRangeException();

                if (this.cellSize != value)
                {
                    this.cellSize = value;
                    UpdateLayout();
                }
            }
        }

        public Size MinimumRange
        {
            get { return this.minimumRange; }
            set
            {
                if ((value.Width < 0 || value.Height < 0) ||
                    (value.Width > 0 && value.Width >= this.maximumRange.Width) ||
                    (value.Height > 0 && value.Height >= this.maximumRange.Height))
                    throw new ArgumentOutOfRangeException();

                this.minimumRange = value;
                SelectedSize = ConstrainSizeToLimits(SelectedSize);
            }
        }

        public Size MaximumRange
        {
            get { return this.maximumRange; }
            set
            {
                if ((value.Width < 0 || value.Height < 0) ||
                    (value.Width > 0 && value.Width <= this.minimumRange.Width) ||
                    (value.Height > 0 && value.Height <= this.minimumRange.Height))
                    throw new ArgumentOutOfRangeException();

                this.maximumRange = value;
                SelectedSize = ConstrainSizeToLimits(SelectedSize);
                VisibleRange = ConstrainSizeToLimits(VisibleRange);
            }
        }

        public Size VisibleRange
        {
            get { return this.visibleRange; }
            set
            {
                Size size = ConstrainSizeToLimits(value);

                if (this.visibleRange != size)
                {
                    this.visibleRange = size;
                    UpdateLayout();
                }
            }
        }

        public Size SelectedSize
        {
            get { return this.checkedRange; }
            set
            {
                Size size = ConstrainSizeToLimits(value);

                if (this.checkedRange != size)
                {
                    this.checkedRange = size;

                    buttonText = (this.checkedRange.Width > 0 && this.checkedRange.Height > 0) ?
                        String.Format("{0} x {1} Table", this.checkedRange.Width, this.checkedRange.Height) :
                        "Cancel";

                    Invalidate();
                }

                size.Width = Math.Max(size.Width, VisibleRange.Width);
                size.Height = Math.Max(size.Height, VisibleRange.Height);
                VisibleRange = size;
            }
        }

        protected enum HitPart
        {
            Border,
            Pool,
            Button,
        }

        protected struct HitInfo
        {
            public HitPart part;
            public int col;
            public int row;
        }

        protected HitInfo QueryHit(Point pt)
        {
            HitInfo info = new HitInfo();

            Rectangle poolBounds = PoolBounds;

            if (poolBounds.Contains(pt))
            {
                info.part = HitPart.Pool;
                info.col = (pt.X - poolBounds.X) / CellSize;
                info.row = (pt.Y - poolBounds.Y) / CellSize;
            }
            else
            {
                info.part = (pt.Y > poolBounds.Bottom) ? HitPart.Button : HitPart.Border;
                info.col = -1;
                info.row = -1;
            }

            return info;
        }

        protected virtual void OnSelectionCancelled(EventArgs e)
        {
            if (!SelectionCancelled.IsNull())
                SelectionCancelled(this, e);
        }

        protected virtual void OnTableSizeSelected(TableSizeEventArgs e)
        {
            if (!TableSizeSelected.IsNull())
                TableSizeSelected(this, e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
                this.selectUsingMouse = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Brush brush = new SolidBrush(this.selectedColor);

            using (Pen pen = new Pen(this.borderColor))
            {
                int extent = CellSize - CellSpacing - 1;

                Rectangle cellRect = new Rectangle(0, PoolBounds.Top, extent, extent);

                for (int row = 0; row < visibleRange.Height; ++row, cellRect.Y += CellSize)
                {
                    cellRect.X = PoolBounds.Left;

                    for (int col = 0; col < visibleRange.Width; ++col, cellRect.X += CellSize)
                    {
                        if (col < checkedRange.Width && row < checkedRange.Height)
                            e.Graphics.FillRectangle(brush, cellRect);

                        e.Graphics.DrawRectangle(pen, cellRect);
                    }
                }
            }

            brush.Dispose();

            using (Brush textBrush = new SolidBrush(this.textColor))
            {
                Rectangle bounds = ClientRectangle;
                bounds.Y = PoolBounds.Bottom + CellSpacing;
                bounds.Height = Font.Height;

                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;

                e.Graphics.DrawString(buttonText, Font, textBrush, bounds, format);
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            bool eatKey = true;

            Size newCheckedRange = SelectedSize;

            switch (keyData)
            {
                case Keys.Up:
                    {
                        this.selectUsingMouse = false;

                        if (newCheckedRange.Height > 0)
                            --newCheckedRange.Height;
                        break;
                    }

                case Keys.Down:
                    {
                        this.selectUsingMouse = false;

                        ++newCheckedRange.Height;
                        newCheckedRange.Width = Math.Max(1, newCheckedRange.Width);
                        break;
                    }

                case Keys.Left:
                    {
                        this.selectUsingMouse = false;

                        if (newCheckedRange.Width > 0)
                            --newCheckedRange.Width;
                        break;
                    }

                case Keys.Right:
                    {
                        this.selectUsingMouse = false;

                        ++newCheckedRange.Width;
                        newCheckedRange.Height = Math.Max(1, newCheckedRange.Height);
                        break;
                    }

                case Keys.Cancel:
                    {
                        OnSelectionCancelled(new EventArgs());
                        break;
                    }

                case Keys.Enter:
                    {
                        EndSelection();
                        break;
                    }

                default:
                    eatKey = false;
                    break;
            }

            SetSelection(newCheckedRange);

            return eatKey || base.ProcessDialogKey(keyData);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            HitInfo hit = QueryHit(e.Location);

            if (!this.selectUsingMouse || hit.part == HitPart.Button)
            {
                Capture = false;
                EndSelection();
            }
            else if (hit.part == HitPart.Pool)
            {
                if (this.selectUsingMouse)
                    ExpandPoolToPt(e.Location);

                Capture = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (Capture)
            {
                Capture = false;
                EndSelection();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.selectUsingMouse)
            {
                HitInfo hit = QueryHit(e.Location);

                if (Capture)
                    ExpandPoolToPt(e.Location);

                SelectedSize = (hit.part == HitPart.Pool) ?
                    new Size(hit.col + 1, hit.row + 1) :
                    new Size(0, 0);
            }
        }

        #region Implementation

        private Rectangle PoolBounds
        {
            get
            {
                int cx = CellSize * VisibleRange.Width;
                int cy = CellSize * VisibleRange.Height;

                return new Rectangle(CellSpacing * 2, CellSpacing, cx, cy);
            }
        }

        private Size LayoutSize
        {
            get { return PoolBounds.Size + new Size(CellSpacing * 3, CellSpacing * 2 + Font.Height); }
        }

        private void UpdateLayout()
        {
            this.Size = LayoutSize;
        }

        private void SetSelection(Size size)
        {
            if (SelectedSize != ConstrainSizeToLimits(size))
            {
                SelectedSize = size;

                ExpandPool(
                    new Size(
                        Math.Max(0, SelectedSize.Width - VisibleRange.Width),
                        Math.Max(0, SelectedSize.Height - VisibleRange.Height)));
            }
        }

        private void EndSelection()
        {
            if (SelectedSize.Width > 0 && SelectedSize.Height > 0)
                OnTableSizeSelected(new TableSizeEventArgs(SelectedSize));
            else
                OnSelectionCancelled(new EventArgs());
        }

        private Size ConstrainSizeToLimits(Size size)
        {
            if (MinimumRange.Width > 0)
                size.Width = Math.Max(MinimumRange.Width, size.Width);

            if (MaximumRange.Width > 0)
                size.Width = Math.Min(MaximumRange.Width, size.Width);

            if (MinimumRange.Height > 0)
                size.Height = Math.Max(MinimumRange.Height, size.Height);

            if (MaximumRange.Height > 0)
                size.Height = Math.Min(MaximumRange.Height, size.Height);

            return size;
        }

        private void ExpandPoolToPt(Point pt)
        {
            Rectangle bounds = PoolBounds;

            Size growSize = new Size(0, 0);

            if (pt.X > bounds.Right - CellSpacing)
                growSize.Width = (pt.X - bounds.Right) / CellSize + 1;

            if (pt.Y > bounds.Bottom - CellSpacing)
                growSize.Height = (pt.Y - bounds.Bottom) / CellSize + 1;

            ExpandPool(growSize);
        }

        private void ExpandPool(Size sizeIncrease)
        {
            Size newSize = VisibleRange;

            if (sizeIncrease.Width > 0)
                if (MaximumRange.Width <= 0 || VisibleRange.Width + sizeIncrease.Width <= MaximumRange.Width)
                    newSize.Width += sizeIncrease.Width;

            if (sizeIncrease.Height > 0)
                if (MaximumRange.Height <= 0 || VisibleRange.Height + sizeIncrease.Height <= MaximumRange.Height)
                    newSize.Height += sizeIncrease.Height;

            VisibleRange = newSize;
        }
        #endregion

        // Model state
        private Size minimumRange = new Size(0, 0);
        private Size maximumRange = new Size(10, 10);
        private Size visibleRange = new Size(5, 4);
        private Size checkedRange = new Size(0, 0);

        private string buttonText = "Cancel";

        // Presentation
        private int cellSize = 24;
        private int cellSpacing = 2;

        private Color selectedColor = SystemColors.Highlight;
        private Color borderColor = SystemColors.WindowText;
        private Color textColor = SystemColors.WindowText;

        // Interaction state
        private bool selectUsingMouse = true;
    }
}
