using System.Windows;

namespace SunFileManager.GUI
{
    public partial class frmNameInputBox : SunFileManager.GUI.SunFluentWindow
    {
        private string nameResult = null;

        public static bool Show(string title, out string name, string highlighted = "")
        {
            var form = new frmNameInputBox(title, highlighted);
            bool result = form.ShowDialog() == true;
            name = form.nameResult;
            return result;
        }

        public frmNameInputBox(string title, string highlighted = "")
        {
            InitializeComponent();
            Title = title;
            if (highlighted.EndsWith(".sun"))
                highlighted = highlighted.Remove(highlighted.IndexOf(".sun"));
            txtNameInput.Text = highlighted;
            Loaded += (_, __) => { txtNameInput.Focus(); txtNameInput.SelectAll(); };
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string text = txtNameInput.Text;
            if (string.IsNullOrEmpty(text) || text.StartsWith(" ") || text.EndsWith(" "))
            {
                txtNameInput.Focus();
                txtNameInput.SelectAll();
                return;
            }
            nameResult = text;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
