using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmFloatInputBox : SunFileManager.GUI.SunFluentWindow
    {
        private string nameResult = null;
        private double? doubleResult = null;

        public static bool Show(string title, out string name, out double? value)
        {
            var form = new frmFloatInputBox(title);
            bool result = form.ShowDialog() == true;
            name = form.nameResult;
            value = form.doubleResult;
            return result;
        }

        public frmFloatInputBox(string title)
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
            if (!double.TryParse(txtFloatValueInput.Text, out double val))
            {
                txtFloatValueInput.Focus();
                txtFloatValueInput.SelectAll();
                return;
            }
            nameResult = nameText;
            doubleResult = val;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
