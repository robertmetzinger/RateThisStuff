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
            _viewModel.Users = await SessionProvider.Current.Proxy.GetAllUsersAsync();
        }

        public void ExecuteDeleteCommand(object obj)
        {
            bool deleted = false;
            MessageBoxResult result = MessageBox.Show("Soll dieser Benutzer wirklich gelöscht werden?", "Benutzer löschen",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                deleted = SessionProvider.Current.Proxy.DeleteUserAsync(_viewModel.SelectedUser).Result;
            }

            if (deleted)
            {
                MessageBox.Show("Der Benutzer wurde erfolgreich gelöscht.", "Löschen erfolgreich");
            }
            else
            {
                MessageBox.Show("Der Benutzer konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
            }
        }

        public void ExecuteEditCommand(object obj)
        {
            SessionProvider.Current.EditOrNewMode = true;
        }

        public void ExecuteNewCommand(object obj)
        {
            User newUser = new User();
            SessionProvider.Current.EditOrNewMode = true;
            _viewModel.SelectedUser = newUser;
        }

        public void ExecuteSaveCommand(object obj)
        {
            SessionProvider.Current.Proxy.SaveOrUpdateUserAsync(_viewModel.SelectedUser);
            SessionProvider.Current.EditOrNewMode = false;
        }
    }
}
