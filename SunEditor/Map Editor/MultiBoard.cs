using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SunEditor.Map_Editor
{
    public partial class MultiBoard : UserControl
    {
        private GraphicsDevice graphicsDevice;
        SpriteBatch sprite;

        public event EventHandler<bool> MiniMapStateChanged;

        public MultiBoard()
        {
            InitializeComponent();
        }

        #region Methods
        public void OnMiniMapStateChanged(Board board, bool hasMiniMap)
        {
            if (MiniMapStateChanged != null)
            {
                MiniMapStateChanged.Invoke(board, hasMiniMap);
            }
        }

        public void SetHorizontalScrollbarValue(int value)
        {
            lock (this)
            {
                horizontalScrollBar.Value = value;
            }
        }

        public void SetVerticalScrollbarValue(int value)
        {
            lock (this)
            {
                verticalScrollBar.Value = value;
            }
        }

        #endregion Methods
    }
}
