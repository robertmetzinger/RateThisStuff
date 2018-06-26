using RateThisStuff_Client.Framework;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RateThisStuff_Client.Controllers
{
    class ItemsController:IPageController
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
            _viewModel.Items = await SessionProvider.Current.Proxy.GetAllItemsByCategoryAsync(SessionProvider.Current.Category);
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

        public void ExecuteRateCommand(object obj)
        {
            RatingController ratingController = new RatingController();
            ratingController.Initialize(_viewModel.SelectedItem);
            SelectedItemChanged(null, null);
        }

        public async void ExecuteRemoveRatingCommand(object obj)
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
            else MessageBox.Show("Beim Löschen ist ein Fehler aufgetreten", "Löschen fehlgeschlagen");
        }

        
        private async void SelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            _viewModel.Ratings = await SessionProvider.Current.Proxy.GetAllRatingsForItemAsync(_viewModel.SelectedItem);
            _viewModel.RatingForItem = await SessionProvider.Current.Proxy.GetRatingFromUserForItemAsync(SessionProvider.Current.User, _viewModel.SelectedItem);
        }
    }
}
