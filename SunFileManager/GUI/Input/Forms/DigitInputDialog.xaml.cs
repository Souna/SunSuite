using System.Windows;

namespace SunFileManager.GUI.Input
{
    public partial class frmDigitInputBox : Window
    {
        private string nameResult = null;
        private int? intResult = null;

        public static bool Show(string title, out string name, out int? intValue)
        {
            var form = new frmDigitInputBox(title);
            bool result = form.ShowDialog() == true;
            name = form.nameResult;
            intValue = form.intResult;
            return result;
        }

        public frmDigitInputBox(string title)
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
            if (!int.TryParse(txtIntValueInput.Text, out int val))
            {
                txtIntValueInput.Focus();
                txtIntValueInput.SelectAll();
                return;
            }
            nameResult = nameText;
            intResult = val;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
