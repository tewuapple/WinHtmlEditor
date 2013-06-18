using System.Drawing;

namespace WinHtmlEditor
{
    internal class SelectableColor
    {
        private Color _color;

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private bool _selected = false;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        private bool _hotTrack = false;

        public bool HotTrack
        {
            get { return _hotTrack; }
            set { _hotTrack = value; }
        }

        public SelectableColor(Color color)
        {
            _color = color;
        }
    }
}