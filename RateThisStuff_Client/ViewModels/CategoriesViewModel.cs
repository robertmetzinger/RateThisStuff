using RateThisStuff_Client.Framework;
using System.Collections.ObjectModel;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client.ViewModels
{
    class CategoriesViewModel:ViewModelBase
    {
        private Category _selectedCategory;
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

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (Equals(value, _selectedCategory)) return;
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
    }
}
