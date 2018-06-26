using System.Windows.Controls;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;

namespace RateThisStuff_Client.Controllers
{
    class MostRatedController:IPageController
    {
        private MostRatedViewModel _viewModel;

        public Page Initialize()
        {
            Page page = new MostRatedPage();
            _viewModel = new MostRatedViewModel();
            LoadData();
            page.DataContext = _viewModel;
            return page;
        }

        public async void LoadData()
        {
            _viewModel.Items = await SessionProvider.Current.Proxy.GetMostRatedItemsOfCategoryAsync(SessionProvider.Current.Category);
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
