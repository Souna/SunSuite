using Microsoft.Win32;
using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmSettings : SunFileManager.GUI.SunFluentWindow
    {
        private readonly GUI.MainWindow mainWindow;

        public frmSettings(GUI.MainWindow parent)
        {
            mainWindow = parent;
            Owner = parent;
            InitializeComponent();
            Loaded += (_, __) => LoadSettings();
        }

        private void LoadSettings()
        {
            chkDarkMode.IsChecked        = Program.UserSettings.DarkMode;
            chkAutoParseImages.IsChecked = Program.UserSettings.AutoParseImages;
            chkNodeWarnings.IsChecked    = Program.UserSettings.NodeWarnings;
            chkShowOriginCross.IsChecked = Program.UserSettings.ShowOriginCross;
            txtSunFilesPath.Text         = Program.UserSettings.SunFilesPath;

            chkDarkMode.Checked           += OnSettingChanged;
            chkDarkMode.Unchecked         += OnSettingChanged;
            chkAutoParseImages.Checked    += OnSettingChanged;
            chkAutoParseImages.Unchecked  += OnSettingChanged;
            chkNodeWarnings.Checked       += OnSettingChanged;
            chkNodeWarnings.Unchecked     += OnSettingChanged;
            chkShowOriginCross.Checked    += OnSettingChanged;
            chkShowOriginCross.Unchecked  += OnSettingChanged;
        }

        private void OnSettingChanged(object sender, RoutedEventArgs e) => ApplyNow();

        private void ApplyNow()
        {
            bool darkModeChanged = (chkDarkMode.IsChecked == true) != Program.UserSettings.DarkMode;

            Program.UserSettings.DarkMode        = chkDarkMode.IsChecked == true;
            Program.UserSettings.AutoParseImages = chkAutoParseImages.IsChecked == true;
            Program.UserSettings.NodeWarnings    = chkNodeWarnings.IsChecked == true;
            Program.UserSettings.ShowOriginCross = chkShowOriginCross.IsChecked == true;
            Program.UserSettings.SunFilesPath    = txtSunFilesPath.Text;
            Program.UserSettings.Save();

            if (darkModeChanged)
                mainWindow?.ApplyTheme();
            mainWindow?.ApplySettings();
        }

        private void btnBrowseSunPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog { Title = "Select Sun Files Folder" };
            if (!string.IsNullOrEmpty(txtSunFilesPath.Text))
                dialog.InitialDirectory = txtSunFilesPath.Text;
            if (dialog.ShowDialog() == true)
            {
                txtSunFilesPath.Text = dialog.FolderName;
                ApplyNow();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();
    }
}
