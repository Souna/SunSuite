using System.Windows;
using Wpf.Ui.Appearance;

namespace SunFileManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Theme is applied from Program.cs after UserSettings are loaded.
        }
    }
}
