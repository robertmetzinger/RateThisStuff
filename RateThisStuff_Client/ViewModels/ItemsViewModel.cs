﻿using RateThisStuff_Client.Framework;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client.ViewModels
{
    class ItemsViewModel:ViewModelBase
    {
        public ICommand RateCommand { get; set; }
        public ICommand RemoveRatingCommand { get; set; }
        private Item _selectedItem;
        private ObservableCollection<Item> _items;
        private ObservableCollection<Rating> _ratings;
        private Rating _ratingForItem;
        private bool _canRate;
        private bool _canRemoveRating;

        public ObservableCollection<Item> Items
        {
            get => _items;
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Rating> Ratings
        {
            get => _ratings;
            set
            {
                if (Equals(value, _ratings)) return;
                _ratings = value;
                OnPropertyChanged();
            }
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged();
                SessionProvider.Current.CanEditAndDelete = true;
            }
        }

        public Rating RatingForItem
        {
            get => _ratingForItem;
            set
            {
                if (Equals(value, _ratingForItem)) return;
                _ratingForItem = value;
                OnPropertyChanged();
            }
        }

        public bool CanRate
        {
            get => _canRate;
            set
            {
                if (value == _canRate) return;
                _canRate = value;
                OnPropertyChanged();
            }
        }

        public bool CanRemoveRating
        {
            get => _canRemoveRating;
            set
            {
                if (value == _canRemoveRating) return;
                _canRemoveRating = value;
                OnPropertyChanged();
            }
        }

        protected void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler SelectedItemChanged;
    }
}
