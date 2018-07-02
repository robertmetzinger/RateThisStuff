using System;
using System.Windows;
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
            try
            {
                SessionProvider.Current.CanNew = false;
                SessionProvider.Current.CanEditAndDelete = false;
                SessionProvider.Current.CanSave = false;
                _viewModel.Items = await SessionProvider.Current.Proxy.GetMostRatedItemsOfCategoryAsync(SessionProvider.Current.Category);
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
    }
}
