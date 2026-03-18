using System.ComponentModel.Design;
using System.Windows;

namespace PolirubroWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            LoginWindow login = new LoginWindow();
            bool? resultado = login.ShowDialog();

            if (resultado == true)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow = mainWindow;
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                mainWindow.Show();
                mainWindow.Activate();
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}