using System;
using System.Windows;
using System.Windows.Controls;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;

namespace RateThisStuff_Client.Controllers
{
    class BestRatedController : IPageController
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
            try
            {
                _viewModel.Items =
                    await SessionProvider.Current.Proxy.GetBestRatedItemsOfCategoryAsync(SessionProvider.Current
                        .Category);
                SessionProvider.Current.CanNew = false;
                SessionProvider.Current.CanEditAndDelete = false;
                SessionProvider.Current.CanSave = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Daten konnten nicht geladen werden", "Fehler");
            }

            ;
        }

        public void ExecuteNewCommand(object obj) { }

        public void ExecuteEditCommand(object obj) { }

        public void ExecuteSaveCommand(object obj) { }

        public void ExecuteDeleteCommand(object obj) { }
    }
}
