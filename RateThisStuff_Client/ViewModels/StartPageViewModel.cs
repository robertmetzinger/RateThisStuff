using RateThisStuff_Client.Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client.ViewModels
{
    class StartPageViewModel:ViewModelBase
    {
        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                if (Equals(value, _categories)) return;
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangePasswordCommand { get; set; }
    }
}
