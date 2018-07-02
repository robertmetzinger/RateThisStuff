using System;
using System.ServiceModel;
using RateThisStuff_Client.Framework;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;
using System.Windows;
using System.Windows.Controls;

namespace RateThisStuff_Client.Controllers
{
    class LoginWindowController
    {
        private LoginWindow _view;
        private LoginWindowViewModel _viewModel;

        public async void ExecuteLoginCommand(object obj)
        {
            try
            {
                var passwordBox =
                    (PasswordBox) _view.PasswordField.Template.FindName("InnerPasswordBox", _view.PasswordField);
                var passwordInput = passwordBox.Password;
                string message = (string) Application.Current.FindResource("LoginInProgessMessage");
                _viewModel.Message = message;
                var usernameInput = (string) _view.UsernameField.Content;
                if (usernameInput == null || usernameInput.Equals(String.Empty))
                {
                    MessageBox.Show("Gib einen Benutzernamen ein!");
                    return;
                }

                var user = await SessionProvider.Current.Proxy.LoginAsync(_viewModel.UsernameInput, passwordInput);
                if (user != null)
                {
                    SessionProvider.Current = new Session();
                    SessionProvider.Current.User = user;
                    MainWindowController mainWindowController = new MainWindowController();
                    mainWindowController.Initialize();
                    _view.Close();
                }
                else
                {
                    message = (string) Application.Current.FindResource("FalseAuthenticationMessage");
                    _viewModel.Message = message;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Login fehlgeschlagen.\n Überprüfe, ob alle erforderlichen Felder ausgefüllt sind.\n Überprüfe die Verbindung zum Service.",
                    "Fehler");
            }
        }

        public void Initialize()
        {
            _viewModel = new LoginWindowViewModel();
            _viewModel.LoginCommand = new RelayCommand(ExecuteLoginCommand);
            _view = new LoginWindow();
            _view.DataContext = _viewModel;
            _view.Show();
        }
    }
}
