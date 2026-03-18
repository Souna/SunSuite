using System.Windows;
using System.Windows.Controls;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmSettings : Window
    {
        private GUI.MainWindow mainWindow = null;

        public frmSettings(GUI.MainWindow parent)
        {
            mainWindow = parent;
            Owner = parent;
            InitializeComponent();
            Loaded += (_, __) => LoadSettings();

            // Enable Apply when anything changes
            foreach (CheckBox cb in new[] { chkDarkMode, chkAutoParseImages, chkNodeWarnings, chkFileBoxes })
                cb.Checked += (__, ___) => btnApply.IsEnabled = true;
            foreach (CheckBox cb in new[] { chkDarkMode, chkAutoParseImages, chkNodeWarnings, chkFileBoxes })
                cb.Unchecked += (__, ___) => btnApply.IsEnabled = true;
            radNodeLines.Checked += (__, ___) => btnApply.IsEnabled = true;
            radHighlightLine.Checked += (__, ___) => btnApply.IsEnabled = true;
        }

        private void LoadSettings()
        {
            chkDarkMode.IsChecked = Program.UserSettings.DarkMode;
            chkAutoParseImages.IsChecked = Program.UserSettings.AutoParseImages;
            chkNodeWarnings.IsChecked = Program.UserSettings.NodeWarnings;
            chkFileBoxes.IsChecked = Program.UserSettings.FileBoxes;
            radNodeLines.IsChecked = Program.UserSettings.NodeLines;
            radHighlightLine.IsChecked = Program.UserSettings.HighlightLine;
            btnApply.IsEnabled = false;
        }

        private void GatherSettings()
        {
            Program.UserSettings.DarkMode = chkDarkMode.IsChecked == true;
            Program.UserSettings.AutoParseImages = chkAutoParseImages.IsChecked == true;
            Program.UserSettings.NodeWarnings = chkNodeWarnings.IsChecked == true;
            Program.UserSettings.FileBoxes = chkFileBoxes.IsChecked == true;
            Program.UserSettings.NodeLines = radNodeLines.IsChecked == true;
            Program.UserSettings.HighlightLine = radHighlightLine.IsChecked == true;
            Program.UserSettings.Save();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            GatherSettings();
            mainWindow?.ApplySettings();
            Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            GatherSettings();
            mainWindow?.ApplySettings();
            btnApply.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
