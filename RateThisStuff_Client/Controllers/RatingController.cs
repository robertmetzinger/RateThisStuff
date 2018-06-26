using System.Windows;
using RateThisStuff_Client.Framework;
using RateThisStuff_Client.RateThisStuffServiceReference;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;

namespace RateThisStuff_Client.Controllers
{
    class RatingController
    {
        private RatingWindow _view;
        private RatingViewModel _viewModel;

        public async void ExecuteRateCommand(object obj)
        {
            Rating rating = new Rating();
            rating.User = SessionProvider.Current.User;
            rating.Item = _viewModel.Item;
            rating.Score = _viewModel.Score;
            rating.Comment = _viewModel.Comment;
            bool created = await SessionProvider.Current.Proxy.SaveOrUpdateRatingAsync(rating);
            if (!created)
                MessageBox.Show("Beim Erstellen der Bewertung ist ein Fehler aufgetreten", "Bewerten fehlgeschlagen");
            _view.Close();
        }

        public void Initialize(Item item)
        {
            _view = new RatingWindow();
            _viewModel = new RatingViewModel();
            _viewModel.RateCommand = new RelayCommand(ExecuteRateCommand);
            _viewModel.Item = item;
            _view.DataContext = _viewModel;
            _view.ShowDialog();
        }
    }
}
