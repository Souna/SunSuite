using System.Windows;

namespace SunFileManager.GUI.Input.Forms
{
    public partial class frmPeterAlert : SunFileManager.GUI.SunFluentWindow
    {
        public frmPeterAlert()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) => Close();
    }
}
