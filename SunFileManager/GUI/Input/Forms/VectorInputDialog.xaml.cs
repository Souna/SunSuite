using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmVectorInputBox : SunFileManager.GUI.SunFluentWindow
    {
        private string nameResult = null;
        private System.Drawing.Point? pointResult = null;

        public static bool Show(string title, out string name, out System.Drawing.Point? pointValue)
        {
            var form = new frmVectorInputBox(title);
            bool result = form.ShowDialog() == true;
            name = form.nameResult;
            pointValue = form.pointResult;
            return result;
        }

        public frmVectorInputBox(string title)
        {
            InitializeComponent();
            Title = title;
            Loaded += (_, __) => txtNameInput.Focus();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string nameText = txtNameInput.Text;
            if (string.IsNullOrEmpty(nameText) || nameText.StartsWith(" ") || nameText.EndsWith(" "))
            {
                txtNameInput.Focus();
                txtNameInput.SelectAll();
                return;
            }
            if (!int.TryParse(txtXValueInput.Text, out int x) || !int.TryParse(txtYValueInput.Text, out int y))
            {
                txtXValueInput.Focus();
                return;
            }
            nameResult = nameText;
            pointResult = new System.Drawing.Point(x, y);
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
