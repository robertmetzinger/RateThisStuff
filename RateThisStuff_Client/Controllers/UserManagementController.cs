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
            SessionProvider.Current.CanNew = true;
            SessionProvider.Current.CanEditAndDelete = false;
            SessionProvider.Current.CanSave = false;
            _viewModel.Users = await SessionProvider.Current.Proxy.GetAllUsersAsync();
        }

        public void ExecuteDeleteCommand(object obj)
        {
            MessageBoxResult result = MessageBox.Show("Soll dieser Benutzer wirklich gelöscht werden?\n Alle Bewertungen des Benutzers werden ebenfalls gelöscht", "Benutzer löschen",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                bool deleted = SessionProvider.Current.Proxy.DeleteUserAsync(_viewModel.SelectedUser).Result;

                if (deleted)
                {
                    MessageBox.Show("Der Benutzer wurde erfolgreich gelöscht.", "Löschen erfolgreich");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Der Benutzer konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
                }
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
            SessionProvider.Current.Proxy.SaveOrUpdateUserAsync(_viewModel.SelectedUser);
            SessionProvider.Current.CanNew = true;
            SessionProvider.Current.CanSave = false;
            LoadData();
        }
    }
}
