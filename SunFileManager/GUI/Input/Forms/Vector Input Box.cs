using System;
using System.Drawing;
using System.Windows.Forms;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmVectorInputBox : Form
    {
        private string nameResult = null;
        private Point? pointResult = null;

        public static bool Show(string title, out string name, out Point? pointValue)
        {
            frmVectorInputBox form = new frmVectorInputBox(title);
            bool result = form.ShowDialog() == DialogResult.OK;
            name = form.nameResult;
            pointValue = form.pointResult;
            return result;
        }

        public frmVectorInputBox(string title)
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
                int x = txtXValueInput.Value;
                int y = txtYValueInput.Value;
                pointResult = new Point(x, y);
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


        private void txtNameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }

        private void txtXValueInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }

        private void txtYValueInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }


        private void frmVectorInputBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}