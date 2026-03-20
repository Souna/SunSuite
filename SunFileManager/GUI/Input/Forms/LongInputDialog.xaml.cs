using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmLongInputBox : SunFileManager.GUI.SunFluentWindow
    {
        private string nameResult = null;
        private long? longResult = null;

        public static bool Show(string title, out string name, out long? longValue)
        {
            var form = new frmLongInputBox(title);
            bool result = form.ShowDialog() == true;
            name = form.nameResult;
            longValue = form.longResult;
            return result;
        }

        public frmLongInputBox(string title)
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
            if (!long.TryParse(txtLongValueInput.Text, out long val))
            {
                txtLongValueInput.Focus();
                txtLongValueInput.SelectAll();
                return;
            }
            nameResult = nameText;
            longResult = val;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
