using System;
using System.IO;
using System.Windows.Forms;

namespace SunFileManager.GUI.Input
{
    public partial class frmSoundInputBox : Form
    {
        // Returns
        private string nameResult = null;

        private string soundResult = null;
        private static frmSoundInputBox form = null;

        public static bool Show(string title, out string name, out string path)
        {
            form = new frmSoundInputBox(title);
            bool result = form.ShowDialog() == DialogResult.OK;
            name = form.nameResult;
            path = form.soundResult;
            return result;
        }

        public frmSoundInputBox(string title)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Text = title;
            txtNameInput.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
                && txtSoundPath.Text != string.Empty && txtSoundPath.Text != null || !File.Exists(txtSoundPath.Text))
            {
                if (txtNameInput.Text.StartsWith(" ") || txtNameInput.Text.EndsWith(" "))
                {
                    txtNameInput.Focus();
                    txtNameInput.SelectAll();
                    return;
                }
                nameResult = txtNameInput.Text;
                soundResult = txtSoundPath.Text;
                DialogResult = DialogResult.OK;
                CloseForm();
            }
            else return;
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

        private void txtSoundPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void txtSoundPath_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show(txtSoundPath.Text, txtSoundPath);
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

        private void frmSoundInputBox_KeyDown(object sender, KeyEventArgs e)
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

        // Browse for sound file
        private void txtSoundPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select Sound File",
                Filter = string.Format("{0}|*.mp3",
                Properties.Resources.Sound)
            };

            if (ofd.ShowDialog() == DialogResult.OK) txtSoundPath.Text = ofd.FileName;
        }
    }
}