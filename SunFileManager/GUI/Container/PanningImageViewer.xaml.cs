using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SunFileManager.Converter;

namespace SunFileManager.GUI.Container
{
    public partial class PanningImageViewer : UserControl
    {
        private bool _isPanning;
        private System.Windows.Point _panStart;
        private double _scrollStartH;
        private double _scrollStartV;

        /// <summary>Raised when the user double-clicks the image.</summary>
        public event MouseButtonEventHandler ImageDoubleClicked;

        public PanningImageViewer()
        {
            InitializeComponent();
            zoomSlider.Loaded += (s, e) =>
            {
                var thumb = (zoomSlider.Template.FindName("PART_Track", zoomSlider) as Track)?.Thumb;
                if (thumb != null)
                    thumb.MouseDoubleClick += (_, __) => zoomSlider.Value = 1.0;
            };
        }

        /// <summary>Sets the displayed bitmap. Pass null to clear.</summary>
        public System.Drawing.Bitmap Canvas
        {
            get => _currentBitmap;
            set
            {
                _currentBitmap = value;
                displayImage.Source = value != null ? value.ToWpfBitmap() : null;
            }
        }

        private System.Drawing.Bitmap _currentBitmap;
        private System.Drawing.Point? _originPoint;
        private bool _showOriginCross = true;

        /// <summary>The origin point to mark with a cross, in image pixel coordinates. Null hides the cross.</summary>
        public System.Drawing.Point? OriginPoint
        {
            get => _originPoint;
            set { _originPoint = value; RefreshCross(); }
        }

        /// <summary>Controls whether the origin cross overlay is visible.</summary>
        public bool ShowOriginCross
        {
            get => _showOriginCross;
            set { _showOriginCross = value; RefreshCross(); }
        }

        private void RefreshCross()
        {
            originOverlay.Children.Clear();
            if (!_showOriginCross || !_originPoint.HasValue) return;

            double x = _originPoint.Value.X;
            double y = _originPoint.Value.Y;
            const double arm = 8;

            // White outline for contrast on any background
            originOverlay.Children.Add(new Line { X1 = x - arm, Y1 = y, X2 = x + arm, Y2 = y, Stroke = System.Windows.Media.Brushes.White, StrokeThickness = 3 });
            originOverlay.Children.Add(new Line { X1 = x, Y1 = y - arm, X2 = x, Y2 = y + arm, Stroke = System.Windows.Media.Brushes.White, StrokeThickness = 3 });
            // Red inner cross
            originOverlay.Children.Add(new Line { X1 = x - arm, Y1 = y, X2 = x + arm, Y2 = y, Stroke = System.Windows.Media.Brushes.Red, StrokeThickness = 1 });
            originOverlay.Children.Add(new Line { X1 = x, Y1 = y - arm, X2 = x, Y2 = y + arm, Stroke = System.Windows.Media.Brushes.Red, StrokeThickness = 1 });
        }

        // ── Zoom logic ────────────────────────────────────────────────────────────
        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (imageScale == null || zoomLabel == null) return;
            imageScale.ScaleX = e.NewValue;
            imageScale.ScaleY = e.NewValue;
            zoomLabel.Text = $"{(int)Math.Round(e.NewValue * 100)}%";
        }

        private void ZoomIn_Click(object sender, MouseButtonEventArgs e)
            => zoomSlider.Value = Math.Min(zoomSlider.Maximum, zoomSlider.Value + zoomSlider.SmallChange);

        private void ZoomOut_Click(object sender, MouseButtonEventArgs e)
            => zoomSlider.Value = Math.Max(zoomSlider.Minimum, zoomSlider.Value - zoomSlider.SmallChange);

        // ── Pan logic ─────────────────────────────────────────────────────────────
        private void displayImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ImageDoubleClicked?.Invoke(this, e);
                return;
            }
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
