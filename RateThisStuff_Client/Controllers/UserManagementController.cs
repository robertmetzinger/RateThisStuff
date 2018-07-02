using System;
using System.Windows;
using System.Windows.Controls;
using RateThisStuff_Client.RateThisStuffServiceReference;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;

namespace RateThisStuff_Client.Controllers
{
    class UserManagementController : IPageController
    {
        private UserManagementViewModel _viewModel;

        public Page Initialize()
        {
            Page page = new UserManagement();
            _viewModel = new UserManagementViewModel();
            LoadData();
            page.DataContext = _viewModel;
            return page;
        }

        public async void LoadData()
        {
            try
            {
                _viewModel.Users = await SessionProvider.Current.Proxy.GetAllUsersAsync();
                SessionProvider.Current.CanNew = true;
                SessionProvider.Current.CanEditAndDelete = false;
                SessionProvider.Current.CanSave = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Daten konnten nicht geladen werden.", "Fehler");
            }
        }

        public void ExecuteDeleteCommand(object obj)
        {
            try
            {
                bool selfdestruction = false;
                MessageBoxResult result = MessageBox.Show(
                    "Soll dieser Benutzer wirklich gelöscht werden?\n Alle Bewertungen des Benutzers werden ebenfalls gelöscht",
                    "Benutzer löschen",
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (_viewModel.SelectedUser.Id == SessionProvider.Current.User.Id)
                    {
                        MessageBoxResult result2 = MessageBox.Show(
                            "Das ist dein eigener Benutzer! Wenn du deinen eigenen Benutzer löschst, wirst du anschließend abgemeldet und kannst dich nicht mehr mit diesem Benutzer anmelden. " +
                            "\n Willst du wirklich deinen eigenen Benutzer löschen?", "Eigenen Benutzer löschen",
                            MessageBoxButton.YesNo);
                        if (result2 == MessageBoxResult.Yes) selfdestruction = true;
                        else return;
                    }

                    bool deleted = SessionProvider.Current.Proxy.DeleteUserAsync(_viewModel.SelectedUser).Result;

                    if (deleted)
                    {
                        MessageBox.Show("Der Benutzer wurde erfolgreich gelöscht.", "Löschen erfolgreich");
                        LoadData();
                        if (selfdestruction)
                        {
                            new LoginWindowController().Initialize();
                            Application.Current.Windows[0].Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Der Benutzer konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Der Benutzer konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
            }
        }

        public void ExecuteEditCommand(object obj)
        {
            SessionProvider.Current.CanNew = false;
            SessionProvider.Current.CanEditAndDelete = false;
            SessionProvider.Current.CanSave = true;
        }

        public void ExecuteNewCommand(object obj)
        {
            _viewModel.SelectedUser = new User();
            SessionProvider.Current.CanNew = false;
            SessionProvider.Current.CanEditAndDelete = false;
            SessionProvider.Current.CanSave = true;
        }

        public void ExecuteSaveCommand(object obj)
        {
            var usernameInput = _viewModel.SelectedUser.Username;
            if (usernameInput == null || usernameInput.Equals(String.Empty))
            {
                MessageBox.Show("Gib einen Benutzernamen ein!");
                return;
            }
            var lastnameInput = _viewModel.SelectedUser.Lastname;
            if (lastnameInput == null || lastnameInput.Equals(String.Empty))
            {
                MessageBox.Show("Gib einen Nachnamen ein!");
                return;
            }
            try
            {
                bool success = SessionProvider.Current.Proxy.SaveOrUpdateUserAsync(_viewModel.SelectedUser).Result;
                if (success) LoadData();
                else MessageBox.Show("Benutzername existiert schon.", "Speichern fehlgeschlagen");
            }
            catch (Exception e)
            {
                MessageBox.Show("Der Benutzer konnte nicht gespeichert werden.\n Überprüfe, ob du alle erforderlichen Felder ausgefüllt hast.", "Speichern fehlgeschlagen");
            }
        }
    }
}
