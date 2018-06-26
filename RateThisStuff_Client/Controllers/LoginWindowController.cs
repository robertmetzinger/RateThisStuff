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
            var passwordBox = (PasswordBox) _view.PasswordField.Template.FindName("InnerPasswordBox", _view.PasswordField);
            var passwordInput = passwordBox.Password;
            string message = (string)Application.Current.FindResource("LoginInProgessMessage");
            _viewModel.Message = message;
            var user = await SessionProvider.Current.Proxy.LoginAsync(_viewModel.UsernameInput, passwordInput);
            if (user != null)
            {
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
