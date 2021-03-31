using SunFileManager.GUI.Input;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SunFileManager.GUI.Container
{
    public partial class Panning_PictureBox : UserControl
    {
        private Point startingPoint = new Point();
        private frmImageInputBox imageInputBox;

        public Panning_PictureBox()
        {
            InitializeComponent();
        }

        private void Panning_PictureBox_Load(object sender, EventArgs e)
        {
            picPan.SizeMode = PictureBoxSizeMode.AutoSize;
            imageInputBox = ParentForm as frmImageInputBox;
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
        public Image Image
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
            if (imageInputBox == null) return;
            if (e.Button == MouseButtons.Left)
            {
                imageInputBox.SelectImage();
            }
        }

        /// <summary>
        /// Draws the 1px border around the image in the preview.
        /// </summary>
        private void picPan_Paint(object sender, PaintEventArgs e)
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