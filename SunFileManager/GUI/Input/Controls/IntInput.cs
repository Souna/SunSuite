/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System.Windows.Forms;

namespace SunFileManager.GUI.Input
{
    public partial class IntInput : TextBox
    {
        public IntInput()
        {
            KeyPress += new KeyPressEventHandler(HandleKeyPress);
        }

        private void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)))
                e.Handled = true;
        }

        public int Value
        {
            get
            {
                int result = 0;
                if (int.TryParse(this.Text, out result)) return result;
                else return 0;
            }
            set
            {
                Text = value.ToString();
            }
        }
    }
}