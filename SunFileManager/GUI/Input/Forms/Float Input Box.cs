using System;
using System.Windows.Forms;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmFloatInputBox : Form
    {
        private string nameResult = null;
        private double? doubleResult = null;

        public static bool Show(string title, out string name, out double? value)
        {
            frmFloatInputBox form = new frmFloatInputBox(title);
            bool result = form.ShowDialog() == DialogResult.OK;
            name = form.nameResult;
            value = form.doubleResult;
            return result;
        }

        public frmFloatInputBox(string title)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            Text = title;
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
                doubleResult = txtFloatValueInput.Value;
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
            if (e.KeyChar == (char)13)
            {
                btnOk_Click(null, null);
            }
        }

        private void txtFloatValueInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnOk_Click(null, null);
            }
        }
    }
}