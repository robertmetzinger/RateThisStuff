using RateThisStuff_Client.Framework;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client
{
    public class Session:ViewModelBase
    {
        private Category _category;
        private bool _canNewEditDelete;
        private bool _canSave;
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

        public bool CanNewEditDelete
        {
            get => _canNewEditDelete;
            set
            {
                if (value == _canNewEditDelete) return;
                _canNewEditDelete = value;
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
