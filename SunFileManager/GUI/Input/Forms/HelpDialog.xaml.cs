using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmHelp : Window
    {
        public frmHelp()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) => Close();
    }
}
