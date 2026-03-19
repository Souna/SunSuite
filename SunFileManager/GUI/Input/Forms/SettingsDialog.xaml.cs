using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmSettings : SunFileManager.GUI.SunFluentWindow
    {
        private readonly GUI.MainWindow mainWindow;

        // Snapshot of settings at open time — restored on Cancel
        private bool _snapDarkMode, _snapAutoParseImages, _snapNodeWarnings,
                     _snapFileBoxes, _snapNodeLines, _snapHighlightLine;

        public frmSettings(GUI.MainWindow parent)
        {
            mainWindow = parent;
            Owner = parent;
            InitializeComponent();
            Loaded += (_, __) => LoadSettings();
        }

        private void LoadSettings()
        {
            // Snapshot for cancel revert
            _snapDarkMode        = Program.UserSettings.DarkMode;
            _snapAutoParseImages = Program.UserSettings.AutoParseImages;
            _snapNodeWarnings    = Program.UserSettings.NodeWarnings;
            _snapFileBoxes       = Program.UserSettings.FileBoxes;
            _snapNodeLines       = Program.UserSettings.NodeLines;
            _snapHighlightLine   = Program.UserSettings.HighlightLine;

            // Load current values (suppress immediate apply while loading)
            chkDarkMode.IsChecked        = Program.UserSettings.DarkMode;
            chkAutoParseImages.IsChecked = Program.UserSettings.AutoParseImages;
            chkNodeWarnings.IsChecked    = Program.UserSettings.NodeWarnings;
            chkFileBoxes.IsChecked       = Program.UserSettings.FileBoxes;
            radNodeLines.IsChecked       = Program.UserSettings.NodeLines;
            radHighlightLine.IsChecked   = Program.UserSettings.HighlightLine;

            // Wire immediate-apply after loading so initial assignment doesn't trigger it
            chkDarkMode.Checked            += OnSettingChanged;
            chkDarkMode.Unchecked          += OnSettingChanged;
            chkAutoParseImages.Checked     += OnSettingChanged;
            chkAutoParseImages.Unchecked   += OnSettingChanged;
            chkNodeWarnings.Checked        += OnSettingChanged;
            chkNodeWarnings.Unchecked      += OnSettingChanged;
            chkFileBoxes.Checked           += OnSettingChanged;
            chkFileBoxes.Unchecked         += OnSettingChanged;
            radNodeLines.Checked           += OnSettingChanged;
            radHighlightLine.Checked       += OnSettingChanged;
        }

        private void OnSettingChanged(object sender, RoutedEventArgs e) => ApplyNow();

        private void ApplyNow()
        {
            Program.UserSettings.DarkMode        = chkDarkMode.IsChecked == true;
            Program.UserSettings.AutoParseImages = chkAutoParseImages.IsChecked == true;
            Program.UserSettings.NodeWarnings    = chkNodeWarnings.IsChecked == true;
            Program.UserSettings.FileBoxes       = chkFileBoxes.IsChecked == true;
            Program.UserSettings.NodeLines       = radNodeLines.IsChecked == true;
            Program.UserSettings.HighlightLine   = radHighlightLine.IsChecked == true;
            Program.UserSettings.Save();
            mainWindow?.ApplySettings();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) => Close();

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Program.UserSettings.DarkMode        = _snapDarkMode;
            Program.UserSettings.AutoParseImages = _snapAutoParseImages;
            Program.UserSettings.NodeWarnings    = _snapNodeWarnings;
            Program.UserSettings.FileBoxes       = _snapFileBoxes;
            Program.UserSettings.NodeLines       = _snapNodeLines;
            Program.UserSettings.HighlightLine   = _snapHighlightLine;
            Program.UserSettings.Save();
            mainWindow?.ApplySettings();
            Close();
        }
    }
}
