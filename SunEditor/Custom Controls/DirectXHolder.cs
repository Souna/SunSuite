using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunEditor.Custom_Controls
{
    public partial class DirectXHolder : UserControl
    {
        public DirectXHolder()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            base.OnKeyDown(new KeyEventArgs(keyData));
            return true;
        }
    }
}
