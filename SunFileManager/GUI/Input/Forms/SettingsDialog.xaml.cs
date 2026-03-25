using Microsoft.Win32;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Markup;

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
            {
                mainWindow?.ApplyTheme();
                ApplyDwmTheme();

                // Directly set the theme dictionary on this window's own resources so that
                // the modal dialog doesn't have to rely on app-level resource propagation,
                // which doesn't reliably reach a ShowDialog() nested dispatcher frame.
                // Also explicitly update the window background, since FluentWindow sets it
                // as a local value via WindowBackgroundManager (overriding DynamicResource).
                ApplicationTheme theme = Program.UserSettings.DarkMode ? ApplicationTheme.Dark : ApplicationTheme.Light;
                var dicts = Resources.MergedDictionaries;
                for (int i = dicts.Count - 1; i >= 0; i--)
                    if (dicts[i] is ThemesDictionary)
                        dicts.RemoveAt(i);
                dicts.Insert(0, new ThemesDictionary { Theme = theme });
                WindowBackgroundManager.UpdateBackground(this, theme, Wpf.Ui.Controls.WindowBackdropType.None);
            }
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
