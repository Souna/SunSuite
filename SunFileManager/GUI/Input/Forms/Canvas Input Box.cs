using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace SunFileManager.GUI.Input
{
    public partial class frmCanvasInputBox : Form
    {
        // Returns
        private string nameResult = null, defaultText = null;

        private List<Bitmap> bitmapResult = new List<Bitmap>(); // List for ability to accommodate a gif's frames.
        private List<int> gifFrameDelayResult = new List<int>();
        private bool gifResult = false; // Bool for whether or not the submission is a gif
        private bool createSubPropertyResult = false;

        private Image canvas;
        private static frmCanvasInputBox form = null;

        public static bool Show(string title, out string name, out List<Bitmap> bitmaps, out List<int> gifFrameDelays, out bool isGif, out bool createSubProperty)
        {
            form = new frmCanvasInputBox(title);
            bool result = form.ShowDialog() == DialogResult.OK;
            name = form.nameResult;
            bitmaps = form.bitmapResult;
            gifFrameDelays = form.gifFrameDelayResult;
            isGif = form.gifResult;
            createSubProperty = form.createSubPropertyResult;
            return result;
        }

        public frmCanvasInputBox(string title)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Text = title;
            defaultText = title;
            txtNameInput.Focus();
            lbltxtType.Visible = false;
            lbltxtDimensions.Visible = false;
            lbltxtSize.Visible = false;
            panning_PictureBox.Visible = false;
        }

        private void txtCanvasPath_Click(object sender, EventArgs e)
        {
            SelectCanvas();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (canvas != null)
            {
                canvas.Dispose();
                canvas = null;
                bitmapResult = null;
                nameResult = null;
                panning_PictureBox.Canvas.Dispose();
                panning_PictureBox.Canvas = null;
                panning_PictureBox.Dispose();
                form.Dispose();
                Dispose();
                DialogResult = DialogResult.Cancel;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            CloseForm();
        }

        public void CloseForm()
        {
            // Explicitly calling this is bad but I wanted it to collect immediately when the form closed
            GC.Collect();
            Dispose();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtNameInput.Text != string.Empty && txtNameInput.Text != null
                && txtCanvasPath.Text != string.Empty && txtCanvasPath.Text != null
                && panning_PictureBox.Canvas != null)
            {
                if (txtNameInput.Text.StartsWith(" ") || txtNameInput.Text.EndsWith(" "))
                {
                    txtNameInput.Focus();
                    txtNameInput.SelectAll();
                    return;
                }
                nameResult = txtNameInput.Text;
                DialogResult = DialogResult.OK;
                createSubPropertyResult = chkCreateSub.Checked;
                CloseForm();
            }
            else return;
        }

        private void ResetDefaultControlSettings()
        {
            Size = form.MinimumSize;
            Text = defaultText;
            txtCanvasPath.Size = txtCanvasPath.MinimumSize;
            txtNameInput.Size = txtNameInput.MinimumSize;
        }

        private void UpdateFormControls(string path)
        {
            Text += " (" + Path.GetFileName(path) + ")";

            //  Capitalizes the first letter of the image/gif name when automatically deriving it from the original filename.
            string capitalizedName = Path.GetFileNameWithoutExtension(path);
            capitalizedName = capitalizedName.First().ToString().ToUpper() + capitalizedName.Substring(1);
            if (PathIsGif(path))
            {
                txtNameInput.Text = capitalizedName;
            }
            else
            {
                txtNameInput.Text = "0";
            }
            txtNameInput.SelectionStart = txtNameInput.Text.Length;
            txtNameInput.SelectAll();
            txtNameInput.Focus();

            //  Reveal labels.
            lbltxtDimensions.Visible = true;
            lblCanvasDimensions.Text = canvas.Width.ToString() + " x " + canvas.Height.ToString();
            lbltxtSize.Visible = true;
            lblCanvasSize.Text = GetFileSizeString(new FileInfo(path).Length);
            lbltxtType.Visible = true;
            lblCanvasType.Text = Path.GetExtension(path);

            //  Display picturebox, resize it, and display the canvas.
            panning_PictureBox.Visible = true;
            if ((canvas.Width > form.MaximumSize.Width) && (canvas.Height > form.MaximumSize.Height))
                panning_PictureBox.Size = panning_PictureBox.MaximumSize;
            else panning_PictureBox.Size = canvas.Size;
            form.Size = form.PreferredSize;
            panning_PictureBox.Canvas = canvas;
        }

        // When you change the canvas.
        private void txtCanvasPath_TextChanged(object sender, EventArgs e)
        {
            ResetDefaultControlSettings();
            if (panning_PictureBox.Canvas != null)
            {
                panning_PictureBox.Canvas.Dispose();
                panning_PictureBox.Canvas = null;
            }

            try
            {
                if (bitmapResult != null)
                    bitmapResult.Clear();
                string path = txtCanvasPath.Text;
                canvas = Image.FromFile(path);

                UpdateFormControls(path);

                //CenterFormOnImageChange();
                CenterToScreen();

                if (ImageAnimator.CanAnimate(canvas))
                {
                    //  Decode the gif into individual frames.
                    Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    GifBitmapDecoder decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    foreach (BitmapSource bmp in decoder.Frames)
                    {
                        bitmapResult.Add(CreateBitmapFromSource(bmp));
                    }
                    gifFrameDelayResult = GetFrameDelays(canvas);
                    gifResult = true;
                }
                else
                {
                    bitmapResult.Add((Bitmap)canvas);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<int> GetFrameDelays(Image image)
        {
            var dimension = new FrameDimension(image.FrameDimensionsList[0]);
            int frameCount = image.GetFrameCount(dimension);
            List<int> frameDelayList = new List<int>();
            int index = 0;

            for (int i = 0; i < frameCount; i++)
            {
                image.SelectActiveFrame(dimension, i);
                int delay = BitConverter.ToInt32(image.GetPropertyItem(20736).Value, index) * 10;
                if (delay == 0) delay = 100;
                frameDelayList.Add(delay);

                index += 4;
            }
            return frameDelayList;
        }

        /// <summary>
        /// Constructs a Bitmap from a BitmapSource.
        /// </summary>
        private Bitmap CreateBitmapFromSource(BitmapSource source)
        {
            using var outStream = new MemoryStream();
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(source));
            pngEncoder.Save(outStream);
            return new Bitmap(outStream);
        }

        public void SelectCanvas()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select Canvas File",
                Filter = "Image File|*.jpg;*.bmp;*.png;*.gif;*.tiff",
                //SupportMultiDottedExtensions = false
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtCanvasPath.Text = ofd.FileName;
            }
        }

        private bool PathIsGif(string path)
        {
            return (path.EndsWith("gif"));
        }

        /// <summary>
        /// Keeps the form in the center of the screen when changing the canvas.
        /// </summary>
        private void CenterFormOnCanvasChange()
        {
            //  Get screen the form is on.
            Screen screen = Screen.FromControl(this);
            Rectangle area = screen.WorkingArea;
            Top = (area.Height - Height) / 2;
            Left = (area.Width - Width) / 2;
        }

        private void txtNameInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter button
            if (e.KeyChar == (char)13)
            {
                btnOk_Click(null, null);
            }
        }

        private void txtNameInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void txtCanvasPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void txtCanvasPath_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(txtCanvasPath.Text, txtCanvasPath);
        }

        private void txtNameInput_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(txtNameInput.Text, txtNameInput);
        }

        private void txtNameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }

        private void frmCanvasInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }

        /// <summary>
        /// Returns the file size in an easy-to-read string.
        /// </summary>
        private string GetFileSizeString(long fileLength)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (fileLength >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                fileLength /= 1024;
            }
            string result = string.Format("{0:0.##} {1}", fileLength, sizes[order]);
            return result;
        }
    }
}