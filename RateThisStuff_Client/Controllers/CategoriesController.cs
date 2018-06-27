using System.Windows;
using System.Windows.Controls;
using RateThisStuff_Client.RateThisStuffServiceReference;
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
            SessionProvider.Current.CanNew = true;
            SessionProvider.Current.CanEditAndDelete = false;
            SessionProvider.Current.CanSave = false;
            _viewModel.Categories = await SessionProvider.Current.Proxy.GetAllCategoriesAsync();
        }

        public void ExecuteDeleteCommand(object obj)
        {
            bool deleted = false;
            MessageBoxResult result = MessageBox.Show("Soll diese Kategorie wirklich gelöscht werden?\n Alle Items zu dieser Kategorie werden ebenfalls gelöscht", "Kategorie löschen",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                deleted = SessionProvider.Current.Proxy.DeleteCategoryAsync(_viewModel.SelectedCategory).Result;
            }

            if (deleted)
            {
                MessageBox.Show("Die Kategorie wurde erfolgreich gelöscht.", "Löschen erfolgreich");
                LoadData();
            }
            else
            {
                MessageBox.Show("Die Kategorie konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
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
            _viewModel.SelectedCategory = new Category();
            SessionProvider.Current.CanNew = false;
            SessionProvider.Current.CanEditAndDelete = false;
            SessionProvider.Current.CanSave = true;
        }

        public void ExecuteSaveCommand(object obj)
        {
            SessionProvider.Current.Proxy.SaveOrUpdateCategoryAsync(_viewModel.SelectedCategory);
            SessionProvider.Current.CanNew = true;
            SessionProvider.Current.CanSave = false;
            LoadData();
        }
    }
}
