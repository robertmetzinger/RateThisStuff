using System.Windows.Controls;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;

namespace RateThisStuff_Client.Controllers
{
    class CategoriesController:IPageController
    {
        private CategoriesViewModel _viewModel;

        public Page Initialize()
        {
            Page page = new CategoriesPage();
            _viewModel = new CategoriesViewModel();
            LoadData();
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
    }
}
