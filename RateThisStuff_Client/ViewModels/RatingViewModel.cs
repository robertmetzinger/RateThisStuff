using System.Windows.Input;
using RateThisStuff_Client.Framework;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client.ViewModels
{
    class RatingViewModel:ViewModelBase
    {
        private Item _item;
        private int _score;
        private string _comment;
        public ICommand RateCommand { get; set; }

        public Item Item
        {
            get => _item;
            set
            {
                if (Equals(value, _item)) return;
                _item = value;
                OnPropertyChanged();
            }
        }

        public int Score
        {
            get => _score;
            set
            {
                if (value == _score) return;
                _score = value;
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                if (value == _comment) return;
                _comment = value;
                OnPropertyChanged();
            }
        }
    }
}
