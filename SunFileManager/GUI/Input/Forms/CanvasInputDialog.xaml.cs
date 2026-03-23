using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SunFileManager.GUI.Input
{
    public partial class frmCanvasInputBox : SunFileManager.GUI.SunFluentWindow
    {
        private string nameResult = null;
        private List<string> selectedFiles = new List<string>();
        private List<Bitmap> bitmapResult = new List<Bitmap>();
        private List<int> gifFrameDelayResult = new List<int>();
        private List<Bitmap> gifResult = new List<Bitmap>();
        private bool createSubPropertyResult = false;

        public static bool Show(string title, out string name, out List<Bitmap> bitmaps,
            out List<int> gifFrameDelays, out List<Bitmap> gifs, out bool createSubProperty)
        {
            var form = new frmCanvasInputBox(title);
            bool result = form.ShowDialog() == true;
            name = form.nameResult;
            bitmaps = form.bitmapResult ?? new List<Bitmap>();
            gifFrameDelays = form.gifFrameDelayResult ?? new List<int>();
            gifs = form.gifResult ?? new List<Bitmap>();
            createSubProperty = form.createSubPropertyResult;
            return result;
        }

        public frmCanvasInputBox(string title)
        {
            InitializeComponent();
            Title = title;
            Loaded += (_, __) => txtNameInput.Focus();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Select Image File(s)",
                Filter = "Image Files|*.jpg;*.bmp;*.png;*.gif;*.tiff",
                Multiselect = true
            };
            if (ofd.ShowDialog() != true) return;

            selectedFiles.Clear();
            selectedFiles.AddRange(ofd.FileNames);
            bitmapResult.Clear();
            gifResult.Clear();
            gifFrameDelayResult.Clear();

            txtCanvasPath.Text = ofd.FileNames.Length == 1
                ? ofd.FileNames[0]
                : string.Join(" ", ofd.FileNames.Select(f => "\"" + Path.GetFileName(f) + "\""));

            // Auto-fill name from first file
            string firstName = Path.GetFileNameWithoutExtension(ofd.FileNames[0]);
            bool firstIsGif = ofd.FileNames[0].EndsWith(".gif", StringComparison.OrdinalIgnoreCase);
            if (firstIsGif)
            {
                string cap = char.ToUpper(firstName[0]) + firstName.Substring(1);
                txtNameInput.Text = cap;
            }
            else
            {
                txtNameInput.Text = "0";
            }

            // Load bitmaps/gifs
            try
            {
                foreach (string path in ofd.FileNames)
                {
                    using var img = Image.FromFile(path);
                    if (ImageAnimator.CanAnimate(img))
                    {
                        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                        var decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        foreach (BitmapSource frame in decoder.Frames)
                            gifResult.Add(CreateBitmapFromSource(frame));
                        gifFrameDelayResult = GetFrameDelays(img);
                    }
                    else
                    {
                        bitmapResult.Add(new Bitmap(img));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading image", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Show first bitmap as preview
            Bitmap preview = bitmapResult.Count > 0 ? bitmapResult[0]
                           : gifResult.Count > 0 ? gifResult[0]
                           : null;

            if (preview != null)
            {
                var workArea = SystemParameters.WorkArea;
                int maxViewerW = (int)(workArea.Width  * 0.65);
                int maxViewerH = (int)(workArea.Height * 0.55);
                int viewerW = Math.Clamp(preview.Width,        200, maxViewerW);
                int viewerH = Math.Clamp(preview.Height + 36,  150, maxViewerH); // +36 for zoom bar

                panningImageViewer.Width      = viewerW;
                panningImageViewer.Height     = viewerH;
                panningImageViewer.Canvas     = preview;
                panningImageViewer.ResetZoom();
                panningImageViewer.Visibility = Visibility.Visible;

                Width = Math.Max(400, viewerW + 24);

                string sizeStr = GetFileSizeString(new FileInfo(ofd.FileNames[0]).Length);
                lblInfo.Text = $"{preview.Width} x {preview.Height}  |  {sizeStr}  |  {Path.GetExtension(ofd.FileNames[0])}";
                fileInfoPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string nameText = txtNameInput.Text;
            if (string.IsNullOrEmpty(nameText) || nameText.StartsWith(" ") || nameText.EndsWith(" "))
            {
                txtNameInput.Focus();
                txtNameInput.SelectAll();
                return;
            }
            if (panningImageViewer.Canvas == null)
            {
                MessageBox.Show("Please select an image file.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            nameResult = nameText;
            createSubPropertyResult = chkCreateSub.IsChecked == true;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;

        private Bitmap CreateBitmapFromSource(BitmapSource source)
        {
            using var outStream = new MemoryStream();
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(source));
            enc.Save(outStream);
            return new Bitmap(outStream);
        }

        private List<int> GetFrameDelays(Image image)
        {
            var dimension = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCount = image.GetFrameCount(dimension);
            var delays = new List<int>();
            int index = 0;
            for (int i = 0; i < frameCount; i++)
            {
                image.SelectActiveFrame(dimension, i);
                int delay = BitConverter.ToInt32(image.GetPropertyItem(20736).Value, index) * 10;
                if (delay == 0) delay = 100;
                delays.Add(delay);
                index += 4;
            }
            return delays;
        }

        private static string GetFileSizeString(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (bytes >= 1024 && order + 1 < sizes.Length) { order++; bytes /= 1024; }
            return $"{bytes:0.##} {sizes[order]}";
        }
    }
}
