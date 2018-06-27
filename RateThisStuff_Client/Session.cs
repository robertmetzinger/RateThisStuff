using RateThisStuff_Client.Framework;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client
{
    public class Session:ViewModelBase
    {
        private Category _category;
        private bool _canNew;
        private bool _canSave;
        private bool _canEditAndDelete;
        public ClientServiceClient Proxy { get; set; } = new ClientServiceClient();
        public User User { get; set; }

        public Category Category
        {
            get => _category;
            set
            {
                if (Equals(value, _category)) return;
                _category = value;
                OnPropertyChanged();
            }
        }

        public bool CanNew
        {
            get => _canNew;
            set
            {
                if (value == _canNew) return;
                _canNew = value;
                OnPropertyChanged();
            }
        }

        public bool CanEditAndDelete
        {
            get => _canEditAndDelete;
            set
            {
                if (value == _canEditAndDelete) return;
                _canEditAndDelete = value;
                OnPropertyChanged();
            }
        }

        public bool CanSave
        {
            get => _canSave;
            set
            {
                if (value == _canSave) return;
                _canSave = value;
                OnPropertyChanged();
            }
        }
    }
}
