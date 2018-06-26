using System.Windows.Controls;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;

namespace RateThisStuff_Client.Controllers
{
    class BestRatedController:IPageController
    {
        private BestRatedViewModel _viewModel;

        public Page Initialize()
        {
            var page = new BestRatedPage();
            _viewModel = new BestRatedViewModel();
            LoadData();
            page.DataContext = _viewModel;
            return page;
        }

        public async void LoadData()
        {
            _viewModel.Items = await SessionProvider.Current.Proxy.GetBestRatedItemsOfCategoryAsync(SessionProvider.Current.Category);
        }

        public void ExecuteNewCommand(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteEditCommand(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteSaveCommand(object obj)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteDeleteCommand(object obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
