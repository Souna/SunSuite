/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System.Windows.Forms;

namespace SunFileManager.GUI.Input.Controls
{
    public partial class LongInput : TextBox
    {
        public LongInput()
        {
            KeyPress += new KeyPressEventHandler(HandleKeyPress);
        }

        private void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        public long Value
        {
            get
            {
                long result = 0;
                if (long.TryParse(this.Text, out result)) return result;
                else return 0;
            }
            set
            {
                Text = value.ToString();
            }
        }
    }
}