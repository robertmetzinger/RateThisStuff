using System;
using RateThisStuff_Client.Framework;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client.Controllers
{
    class ItemsController : IPageController
    {
        private ItemsViewModel _viewModel;

        public Page Initialize()
        {
            Page page = new ItemsPage();
            _viewModel = new ItemsViewModel();
            LoadData();
            _viewModel.SelectedItemChanged += SelectedItemChanged;
            _viewModel.RateCommand = new RelayCommand(ExecuteRateCommand);
            _viewModel.RemoveRatingCommand = new RelayCommand(ExecuteRemoveRatingCommand);
            page.DataContext = _viewModel;
            return page;
        }

        public async void LoadData()
        {
            try
            {
                SessionProvider.Current.CanNew = true;
                SessionProvider.Current.CanSave = false;
                _viewModel.Items = await SessionProvider.Current.Proxy.GetAllItemsByCategoryAsync(SessionProvider.Current.Category);
                SessionProvider.Current.CanEditAndDelete = false;
                _viewModel.CanRate = false;
                _viewModel.CanRemoveRating = false;
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
                MessageBoxResult result = MessageBox.Show("Soll dieses Item wirklich gelöscht werden?\n Alle Bewertungen zu dem Item werden ebenfalls gelöscht", "Item löschen",
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    bool deleted = SessionProvider.Current.Proxy.DeleteItemAsync(_viewModel.SelectedItem).Result;

                    if (deleted)
                    {
                        MessageBox.Show("Das Item wurde erfolgreich gelöscht.", "Löschen erfolgreich");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Das Item konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Das Item konnte nicht gelöscht werden.\n Überprüfe, ob alle erforderlichen Felder ausgefüllt sind",
                    "Löschen fehlgeschlagen");
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
            _viewModel.SelectedItem = new Item();
            _viewModel.SelectedItem.Category = SessionProvider.Current.Category;
            SessionProvider.Current.CanNew = false;
            SessionProvider.Current.CanEditAndDelete = false;
            SessionProvider.Current.CanSave = true;
            _viewModel.CanRate = false;
            _viewModel.CanRemoveRating = false;
        }

        public void ExecuteSaveCommand(object obj)
        {
            try
            {
                var nameInput = _viewModel.SelectedItem.Name;
                if (nameInput == null || nameInput.Equals(String.Empty))
                {
                    MessageBox.Show("Gib einen Namen für das Item ein!");
                    return;
                }
                bool success = SessionProvider.Current.Proxy.SaveOrUpdateItemAsync(_viewModel.SelectedItem).Result;
                if (success)
                {
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Es existiert bereits ein Item mit diesem Namen", "Speichern fehlgeschlagen");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Das Item konnte nicht gespeichert werden. Überprüfe, ob alle erforderlichen Felder ausgefüllt sind",
                    "Speichern fehlgeschlagen");
            }
        }

        public void ExecuteRateCommand(object obj)
        {
            RatingController ratingController = new RatingController();
            ratingController.Initialize(_viewModel.SelectedItem);
            SelectedItemChanged(null, null);
        }

        public async void ExecuteRemoveRatingCommand(object obj)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Soll deine Bewertung wirklich gelöscht werden?",
                    "Bewertung löschen", MessageBoxButton.YesNo);
                bool deleted = false;
                if (result == MessageBoxResult.Yes)
                {
                    deleted = await SessionProvider.Current.Proxy.DeleteRatingAsync(_viewModel.RatingForItem);
                }

                if (deleted)
                {
                    SelectedItemChanged(null, null);
                    MessageBox.Show("Die Bewertung wurde erfolgreich gelöscht", "Löschen erfolgreich");
                }
                else MessageBox.Show("Die Bewertung konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
            }
            catch (Exception e)
            {
                MessageBox.Show("Die Bewertung konnte nicht gelöscht werden", "Löschen fehlgeschlagen");
            }
        }


        private async void SelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                _viewModel.Ratings = await SessionProvider.Current.Proxy.GetAllRatingsForItemAsync(_viewModel.SelectedItem);
                _viewModel.RatingForItem = await SessionProvider.Current.Proxy.GetRatingFromUserForItemAsync(SessionProvider.Current.User, _viewModel.SelectedItem);
                if (!SessionProvider.Current.CanSave)
                {
                    if (_viewModel.RatingForItem == null)
                    {
                        _viewModel.CanRate = true;
                        _viewModel.CanRemoveRating = false;
                    }
                    else
                    {
                        _viewModel.CanRate = false;
                        _viewModel.CanRemoveRating = true;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Die Bewertungen zu dem Item konnten nicht geladen werden", "Fehler");
            }
        }
    }
}
