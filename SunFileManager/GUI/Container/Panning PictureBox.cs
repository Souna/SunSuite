using SunFileManager.GUI.Input;
using SunLibrary.SunFileLib.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SunFileManager.GUI.Container
{
    public partial class Panning_PictureBox : UserControl
    {
        private Point startingPoint = new Point();
        private frmCanvasInputBox canvasInputBox;
        private frmFileManager mainForm = null;

        public Panning_PictureBox()
        {
            InitializeComponent();
        }

        private void Panning_PictureBox_Load(object sender, EventArgs e)
        {
            picPan.SizeMode = PictureBoxSizeMode.AutoSize;

            if (ParentForm is frmFileManager)
            {
                mainForm = (frmFileManager)ParentForm;
            }
            else if (ParentForm is frmCanvasInputBox)
            {
                canvasInputBox = (frmCanvasInputBox)ParentForm;
            }
        }

        private void picPan_MouseDown(object sender, MouseEventArgs e)
        {
            startingPoint = new Point(e.X, e.Y);
        }

        /// <summary>
        /// Logic for dragging the image around with the left mouse button.
        /// </summary>
        private void picPan_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!pnlPan.AutoScroll) return;

                int X = startingPoint.X - e.X;
                int Y = startingPoint.Y - e.Y;

                pnlPan.AutoScrollPosition = new Point(X - pnlPan.AutoScrollPosition.X,
                    Y - pnlPan.AutoScrollPosition.Y);
            }
        }

        /// <summary>
        /// The image contained in the PictureBox.
        /// </summary>
        public Image Canvas
        {
            get { if (picPan.Image != null) return picPan.Image; return null; }
            set
            {
                picPan.Image = null;
                picPan.Image = value;
                if (picPan.Image != null)
                {
                    pnlPan.AutoScroll = false;
                    if (picPan.Image.Width > pnlPan.Width || picPan.Image.Height > pnlPan.Height)
                    {
                        pnlPan.AutoScroll = true;
                    }
                }
            }
        }

        private void picPan_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (canvasInputBox != null)
                    canvasInputBox.SelectCanvas();
                else
                {
                    OpenFileDialog dialog = new OpenFileDialog()
                    {
                        Title = "Replace Image",
                        Filter = "Image File|*.jpg;*.bmp;*.png;*.gif;*.tiff"
                    };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        SunNode targetNode = (SunNode)mainForm.sunTreeView.SelectedNode;
                        if (targetNode.Tag is SunCanvasProperty canvas)
                        {
                            canvas.PNG.SetPNG((Bitmap)Image.FromFile(dialog.FileName));

                            // Refresh picturebox to show new image
                            mainForm.sunTreeView.SelectedNode = null;
                            mainForm.sunTreeView.SelectedNode = targetNode;
                        }
                    }
                }
            }
        }

        private void picPan_Paint(object sender, PaintEventArgs e)
        {
            DrawBorder(sender, e);
        }

        /// <summary>
        /// Draws the 1px border around the image in the preview.
        /// </summary>
        private void DrawBorder(object sender, PaintEventArgs e)
        {
            if (picPan.Image == null) return;
            ControlPaint.DrawBorder(e.Graphics, new Rectangle(pnlPan.Location.X, pnlPan.Location.Y, picPan.Image.Width, picPan.Image.Height),
                // Left
                Color.Red, 1, ButtonBorderStyle.Solid,
                // Top
                Color.Red, 1, ButtonBorderStyle.Solid,
                // Right
                Color.Red, 1, ButtonBorderStyle.Solid,
                // Bottom
                Color.Red, 1, ButtonBorderStyle.Solid);
        }

        private void Panning_PictureBox_Resize(object sender, EventArgs e)
        {
            if (picPan.Image != null)
            {
                pnlPan.AutoScroll = false;
                if (picPan.Image.Width > pnlPan.Width || picPan.Image.Height > pnlPan.Height)
                {
                    pnlPan.AutoScroll = true;
                }
            }
        }
    }
}