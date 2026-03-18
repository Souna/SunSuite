using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SunFileManager.Converter;

namespace SunFileManager.GUI.Container
{
    public partial class PanningImageViewer : UserControl
    {
        private bool _isPanning;
        private System.Windows.Point _panStart;
        private double _scrollStartH;
        private double _scrollStartV;

        public PanningImageViewer()
        {
            InitializeComponent();
        }

        /// <summary>Sets the displayed bitmap. Pass null to clear.</summary>
        public Bitmap Canvas
        {
            get => _currentBitmap;
            set
            {
                _currentBitmap = value;
                displayImage.Source = value != null ? value.ToWpfBitmap() : null;
            }
        }

        private Bitmap _currentBitmap;

        // ── Pan logic ─────────────────────────────────────────────────────────────
        private void displayImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            displayImage.CaptureMouse();
            _isPanning = true;
            _panStart = e.GetPosition(scrollViewer);
            _scrollStartH = scrollViewer.HorizontalOffset;
            _scrollStartV = scrollViewer.VerticalOffset;
        }

        private void displayImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isPanning) return;
            System.Windows.Point current = e.GetPosition(scrollViewer);
            double dx = _panStart.X - current.X;
            double dy = _panStart.Y - current.Y;
            scrollViewer.ScrollToHorizontalOffset(_scrollStartH + dx);
            scrollViewer.ScrollToVerticalOffset(_scrollStartV + dy);
        }

        private void displayImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isPanning = false;
            displayImage.ReleaseMouseCapture();
        }
    }
}
