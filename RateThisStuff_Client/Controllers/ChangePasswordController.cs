using System;
using System.Windows;
using System.Windows.Controls;
using RateThisStuff_Client.Framework;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;

namespace RateThisStuff_Client.Controllers
{
    class ChangePasswordController
    {
        private ChangePasswordWindow _view;
        private ChangePasswordViewModel _viewModel;

        public async void ExecuteChangePasswordCommand(object obj)
        {
            try
            {
                var oldPasswordBox = (PasswordBox)_view.OldPasswordField.Template.FindName("InnerPasswordBox", _view.OldPasswordField);
                var oldPasswordInput = oldPasswordBox.Password;
                var newPasswordBox = (PasswordBox)_view.NewPasswordField.Template.FindName("InnerPasswordBox", _view.NewPasswordField);
                var newPasswordInput = newPasswordBox.Password;
                var repeatPasswordBox = (PasswordBox)_view.RepeatNewPasswordField.Template.FindName("InnerPasswordBox", _view.RepeatNewPasswordField);
                var repeatPasswordInput = repeatPasswordBox.Password;

                if (!newPasswordInput.Equals(repeatPasswordInput))
                {
                    MessageBox.Show("Das wiederholte Passwort stimmt mit dem neuen Passwort nicht überein", "Falsche Eingabe");
                }
                else
                {
                    bool changed = await SessionProvider.Current.Proxy.ChangePasswordAsync(oldPasswordInput, newPasswordInput,
                        SessionProvider.Current.User);
                    if (changed == false) MessageBox.Show("Das alte Passwort ist falsch", "Falsche Eingabe");
                    else
                    {
                        MessageBox.Show("Das Passwort wurde erfolgreich geändert", " Änderung erfolgreich");
                        _view.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Das Passwort konnte nicht geändert werden.\n Überprüfe, ob alle erforderlichen Felder ausgefüllt sind",
                    "Passwort ändern fehlgeschlagen");
            }
        }

        public void Initialize()
        {
            _viewModel = new ChangePasswordViewModel();
            _view = new ChangePasswordWindow();
            _viewModel.ChangePasswordCommand = new RelayCommand(ExecuteChangePasswordCommand);
            _view.DataContext = _viewModel;
            _view.ShowDialog();
        }
    }
}
