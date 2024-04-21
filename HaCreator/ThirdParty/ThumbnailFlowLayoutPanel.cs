/*
  koolk's Map Editor

  Copyright (c) 2009-2013 koolk

  This software is provided 'as-is', without any express or implied
  warranty. In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

     1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.

     2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.

     3. This notice may not be removed or altered from any source
     distribution.
*/

using SunFileManager.Converter;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HaCreator.ThirdParty
{
    public class ThumbnailFlowLayoutPanel : FlowLayoutPanel
    {
        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }

        public ImageViewer Add(Bitmap bitmap, String name, bool Text, Color bgColor)
        {
            ImageViewer imageViewer = new ImageViewer();
            imageViewer.Dock = DockStyle.Bottom;
            bitmap = SetBackgroundColor(bitmap, bgColor);
            imageViewer.Image = new Bitmap(bitmap); // Copying the bitmap for thread safety
            imageViewer.IsText = Text;
            imageViewer.Width = bitmap.Width + 12;
            imageViewer.Height = bitmap.Height + 8 + ((Text) ? 16 : 0);
            imageViewer.Name = name;
            imageViewer.IsThumbnail = false;

            Controls.Add(imageViewer);

            return imageViewer;
        }

        public ImageViewer Add(Bitmap bitmap, String name, bool Text)
        {
            ImageViewer imageViewer = new ImageViewer();
            imageViewer.Dock = DockStyle.Bottom;
            imageViewer.Image = new Bitmap(bitmap); // Copying the bitmap for thread safety
            imageViewer.IsText = Text;
            imageViewer.Width = bitmap.Width + 12;
            imageViewer.Height = bitmap.Height + 8 + ((Text) ? 16 : 0);
            imageViewer.Name = name;
            imageViewer.IsThumbnail = false;

            Controls.Add(imageViewer);

            return imageViewer;
        }

        public Bitmap SetBackgroundColor(Bitmap bmp1, Color targetColor)
        {
            Bitmap bmp2 = new Bitmap(bmp1.Width, bmp1.Height);
            Rectangle rect = new Rectangle(Point.Empty, bmp1.Size);
            using (Graphics G = Graphics.FromImage(bmp2))
            {
                G.Clear(targetColor);
                G.DrawImageUnscaledAndClipped(bmp1, rect);
            }
            return bmp2;
        }
    }
}