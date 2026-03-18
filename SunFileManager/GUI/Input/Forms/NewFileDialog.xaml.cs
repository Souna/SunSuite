using SunLibrary.SunFileLib.Structure;
using System.IO;
using System.Windows;

namespace SunFileManager.GUI
{
    public partial class frmNewFile : Window
    {
        private GUI.MainWindow mainWindow = null;

        public frmNewFile(GUI.MainWindow parent)
        {
            mainWindow = parent;
            Owner = parent;
            InitializeComponent();
            Loaded += (_, __) => txtFileName.Focus();
        }

        public void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string name = txtFileName.Text;
            if (string.IsNullOrEmpty(name)) return;

            if (name.StartsWith(" ") || name.EndsWith(" "))
            {
                txtFileName.Focus();
                txtFileName.SelectAll();
                return;
            }

            if (File.Exists(Path.Combine(GUI.MainWindow.DefaultPath, name + ".sun")))
            {
                MessageBox.Show(name + ".sun is an already existing file.", "File Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                MessageBox.Show("Invalid file name.", "File Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!name.EndsWith(".sun"))
                name += ".sun";

            var fullPath = Path.Combine(GUI.MainWindow.DefaultPath, name);
            var file = new SunFile(name, fullPath);

            mainWindow.TreeNodes.Add(new SunNode(file));
            GUI.MainWindow.manager.sunFiles.Add(file);
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}
