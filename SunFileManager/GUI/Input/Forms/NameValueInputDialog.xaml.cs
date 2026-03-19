using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmNameValueInputBox : SunFileManager.GUI.SunFluentWindow
    {
        private string nameResult = null;
        private string valueResult = null;

        public static bool Show(string title, out string name, out string value)
        {
            var form = new frmNameValueInputBox(title);
            bool result = form.ShowDialog() == true;
            name = form.nameResult;
            value = form.valueResult;
            return result;
        }

        public frmNameValueInputBox(string title)
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
            nameResult = nameText;
            valueResult = txtStringValueInput.Text;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
