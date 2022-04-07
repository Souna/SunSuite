using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace SunFileManager.GUI.Input
{
    public partial class frmCanvasInputBox : Form
    {
        private string nameResult = null, defaultText = null;
        private Image image;
        private List<Bitmap> bitmapResult = new List<Bitmap>(); // List for ability to accommodate a gif's frames.
        private bool gifListResult = false; // Bool for whether or not the submission is a gif
        private static frmCanvasInputBox form = null;

        public static bool Show(string title, out string name, out List<Bitmap> bitmap, out bool isGif)
        {
            form = new frmCanvasInputBox(title);
            bool result = form.ShowDialog() == DialogResult.OK;
            name = form.nameResult;
            bitmap = form.bitmapResult;
            isGif = form.gifListResult;
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
            if (image != null)
            {
                image.Dispose();
                image = null;
                panning_PictureBox.Canvas.Dispose();
                panning_PictureBox.Canvas = null;
                panning_PictureBox.Dispose();
                form.Dispose();
                Dispose();
                DialogResult = DialogResult.Cancel;
            }
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
                Close();
            }
            else return;
        }

        // When you change the canvas.
        private void txtCanvasPath_TextChanged(object sender, EventArgs e)
        {
            // This block is for resetting different properties back to their default values.
            {
                Size = form.MinimumSize;
                Text = defaultText;
                txtCanvasPath.Size = txtCanvasPath.MinimumSize;
                txtNameInput.Size = txtNameInput.MinimumSize;
            }
            if (panning_PictureBox.Canvas != null)
            {
                panning_PictureBox.Canvas.Dispose();
                panning_PictureBox.Canvas = null;
            }

            try
            {
                bitmapResult.Clear();
                string path = txtCanvasPath.Text;
                image = Image.FromFile(path);
                Text += " (" + Path.GetFileName(path) + ")";

                //  Capitalizes the first letter of the image/gif name when automatically deriving it from the original filename.
                string capitalizedName = Path.GetFileNameWithoutExtension(path);
                capitalizedName = capitalizedName.First().ToString().ToUpper() + capitalizedName.Substring(1);
                txtNameInput.Text = capitalizedName;
                txtNameInput.SelectionStart = txtNameInput.Text.Length;
                txtNameInput.SelectAll();
                txtNameInput.Focus();

                //  Reveal labels.
                lbltxtDimensions.Visible = true;
                lblCanvasDimensions.Text = image.Width.ToString() + " x " + image.Height.ToString();
                lbltxtSize.Visible = true;
                lblCanvasSize.Text = GetFileSizeString(new FileInfo(path).Length);
                lbltxtType.Visible = true;
                lblCanvasType.Text = Path.GetExtension(path);

                //  Display picturebox, resize it, and display the image.
                panning_PictureBox.Visible = true;
                if ((image.Width > form.MaximumSize.Width) && (image.Height > form.MaximumSize.Height))
                    panning_PictureBox.Size = panning_PictureBox.MaximumSize;
                else panning_PictureBox.Size = image.Size;
                form.Size = form.PreferredSize;
                panning_PictureBox.Canvas = image;

                //CenterFormOnImageChange();
                CenterToScreen();

                if (PathIsGif(path))
                {
                    gifListResult = true;
                    //  Decode gif into individual frames.
                    Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    GifBitmapDecoder decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    foreach (BitmapSource bmp in decoder.Frames)
                        bitmapResult.Add(CreateBitmapFromSource(bmp));
                }
                else
                {
                    bitmapResult.Add((Bitmap)image);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                SupportMultiDottedExtensions = false
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtCanvasPath.Text = ofd.FileName;
            }
        }

        private bool PathIsGif(string path)
        {
            if (path.EndsWith("gif")) return true;
            return false;
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