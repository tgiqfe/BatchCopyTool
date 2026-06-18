using System.Configuration;
using System.Data;
using System.Windows;

namespace BatchCopyTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Item.Setting = Setting.Load();
        }
        
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Item.Setting.Save();
        }
    }

}
