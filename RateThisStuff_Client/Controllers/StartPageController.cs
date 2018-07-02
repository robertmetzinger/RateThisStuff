using System;
using System.Windows;
using RateThisStuff_Client.Framework;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;
using System.Windows.Controls;

namespace RateThisStuff_Client.Controllers
{
    class StartPageController:IPageController
    {
        private StartPageViewModel _viewModel;

        public Page Initialize()
        {
            Page page = new StartPage();
            _viewModel = new StartPageViewModel();
            LoadData();
            _viewModel.ChangePasswordCommand = new RelayCommand(ExecuteChangePasswordCommand);
            page.DataContext = _viewModel;
            return page;
        }

        public async void LoadData()
        {
            try
            {
                SessionProvider.Current.CanNew = false;
                SessionProvider.Current.CanEditAndDelete = false;
                SessionProvider.Current.CanSave = false;
                _viewModel.Categories = await SessionProvider.Current.Proxy.GetAllCategoriesAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show("Daten konnten nicht geladen werden", "Fehler");
            }
        }

        public void ExecuteNewCommand(object obj) { }

        public void ExecuteEditCommand(object obj) { }

        public void ExecuteSaveCommand(object obj) { }

        public void ExecuteDeleteCommand(object obj) { }

        public void ExecuteChangePasswordCommand(object obj)
        {
            ChangePasswordController changePasswordController = new ChangePasswordController();
            changePasswordController.Initialize();
        }
    }
}
