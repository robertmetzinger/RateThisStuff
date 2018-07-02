using System;
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
            try
            {
                _viewModel.Categories = await SessionProvider.Current.Proxy.GetAllCategoriesAsync();
                SessionProvider.Current.CanNew = true;
                SessionProvider.Current.CanEditAndDelete = false;
                SessionProvider.Current.CanSave = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Daten konnten nicht geladen werden", "Fehler");
            }
        }

        public void ExecuteDeleteCommand(object obj)
        {
            try
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
                    SessionProvider.Current.Category = null;
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Die Kategorie konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
                }
            }
            catch (Exception e)
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
            try
            {
                var nameInput = _viewModel.SelectedCategory.Name;
                if (nameInput == null || nameInput.Equals(String.Empty))
                {
                    MessageBox.Show("Gib einen Namen für die Kategorie ein!");
                    return;
                }
                bool success = SessionProvider.Current.Proxy.SaveOrUpdateCategoryAsync(_viewModel.SelectedCategory).Result;
                if (success)
                {
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Es existiert bereits eine Kategorie mit diesem Namen", "Speichern fehlgeschlagen");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Die Kategorie konnte nicht gespeichert werden.\n Überprüfe, ob alle erforderlichen Felder ausgefüllt sind", "Speichern fehlgeschlagen");
            }
        }
    }
}
