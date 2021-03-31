using System;
using System.Windows.Forms;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmLongInputBox : Form
    {
        private string nameResult = null;
        private long? longResult = null;

        public static bool Show(string title, out string name, out long? longValue)
        {
            frmLongInputBox form = new frmLongInputBox(title);
            bool result = form.ShowDialog() == DialogResult.OK;
            name = form.nameResult;
            longValue = form.longResult;
            return result;
        }

        public frmLongInputBox(string title)
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
                longResult = txtIntValueInput.Value;
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

        private void txtIntValueInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnOk_Click(null, null);
            }
        }
    }
}