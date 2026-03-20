using Microsoft.Win32;
using System.Windows;

namespace SunFileManager.GUI.Input
{
    public partial class frmSoundInputBox : SunFileManager.GUI.SunFluentWindow
    {
        private string nameResult = null;
        private string soundResult = null;

        public static bool Show(string title, out string name, out string path)
        {
            var form = new frmSoundInputBox(title);
            bool result = form.ShowDialog() == true;
            name = form.nameResult;
            path = form.soundResult;
            return result;
        }

        public frmSoundInputBox(string title)
        {
            InitializeComponent();
            Title = title;
            Loaded += (_, __) => txtNameInput.Focus();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Select Sound File",
                Filter = "MP3 Files|*.mp3"
            };
            if (ofd.ShowDialog() == true)
                txtSoundPath.Text = ofd.FileName;
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
            if (string.IsNullOrEmpty(txtSoundPath.Text))
            {
                MessageBox.Show("Please select a sound file.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            nameResult = nameText;
            soundResult = txtSoundPath.Text;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
