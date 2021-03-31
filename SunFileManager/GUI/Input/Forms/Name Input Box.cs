using System;
using System.Windows.Forms;

namespace SunFileManager.GUI
{
    /// <summary>
    /// The form for entering name input.
    /// </summary>
    public partial class frmNameInputBox : Form
    {
        private string nameResult = null;

        public static bool Show(string title, out string name, string highlighted = "")
        {
            frmNameInputBox form = new frmNameInputBox(title, highlighted);
            bool result = form.ShowDialog() == DialogResult.OK;
            name = form.nameResult;
            return result;
        }

        public frmNameInputBox(string title, string highlighted)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Text = title;
            if (highlighted.EndsWith(".sun")) highlighted = highlighted.Remove(highlighted.IndexOf(".sun"));
            txtNameInput.Text = highlighted;
            txtNameInput.SelectAll();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtNameInput.Text != string.Empty && txtNameInput.Text != null)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
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
    }
}