using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmSettings : SunFileManager.GUI.SunFluentWindow
    {
        private readonly GUI.MainWindow mainWindow;

        private bool _snapDarkMode, _snapAutoParseImages, _snapShowOriginCross;

        public frmSettings(GUI.MainWindow parent)
        {
            mainWindow = parent;
            Owner = parent;
            InitializeComponent();
            Loaded += (_, __) => LoadSettings();
        }

        private void LoadSettings()
        {
            _snapDarkMode        = Program.UserSettings.DarkMode;
            _snapAutoParseImages = Program.UserSettings.AutoParseImages;
            _snapShowOriginCross = Program.UserSettings.ShowOriginCross;

            chkDarkMode.IsChecked        = Program.UserSettings.DarkMode;
            chkAutoParseImages.IsChecked = Program.UserSettings.AutoParseImages;
            chkShowOriginCross.IsChecked = Program.UserSettings.ShowOriginCross;

            chkDarkMode.Checked           += OnSettingChanged;
            chkDarkMode.Unchecked         += OnSettingChanged;
            chkAutoParseImages.Checked    += OnSettingChanged;
            chkAutoParseImages.Unchecked  += OnSettingChanged;
            chkShowOriginCross.Checked    += OnSettingChanged;
            chkShowOriginCross.Unchecked  += OnSettingChanged;
        }

        private void OnSettingChanged(object sender, RoutedEventArgs e) => ApplyNow();

        private void ApplyNow()
        {
            bool darkModeChanged = (chkDarkMode.IsChecked == true) != Program.UserSettings.DarkMode;

            Program.UserSettings.DarkMode        = chkDarkMode.IsChecked == true;
            Program.UserSettings.AutoParseImages = chkAutoParseImages.IsChecked == true;
            Program.UserSettings.ShowOriginCross = chkShowOriginCross.IsChecked == true;
            Program.UserSettings.Save();

            if (darkModeChanged)
                mainWindow?.ApplyTheme();
            mainWindow?.ApplySettings();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) => Close();

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            bool darkModeChanged = Program.UserSettings.DarkMode != _snapDarkMode;

            Program.UserSettings.DarkMode        = _snapDarkMode;
            Program.UserSettings.AutoParseImages = _snapAutoParseImages;
            Program.UserSettings.ShowOriginCross = _snapShowOriginCross;
            Program.UserSettings.Save();

            if (darkModeChanged)
                mainWindow?.ApplyTheme();
            mainWindow?.ApplySettings();
            Close();
        }
    }
}
