using RateThisStuff_Client.Controllers;
using System.Windows;

namespace RateThisStuff_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoginWindowController loginWindowController = new LoginWindowController();
            loginWindowController.Initialize();

            QuickConverter.EquationTokenizer.AddNamespace(typeof(object));
            QuickConverter.EquationTokenizer.AddNamespace(typeof(Mode));
        }
    }
}
