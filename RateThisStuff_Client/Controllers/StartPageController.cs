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
            _viewModel.Categories = await SessionProvider.Current.Proxy.GetAllCategoriesAsync();
        }

        public void ExecuteDeleteCommand(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteEditCommand(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteNewCommand(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteSaveCommand(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteChangePasswordCommand(object obj)
        {
            ChangePasswordController changePasswordController = new ChangePasswordController();
            changePasswordController.Initialize();
        }
    }
}
