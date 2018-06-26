using RateThisStuff_Client.Framework;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client
{
    public class Session:ViewModelBase
    {
        private Category _category;
        private bool _editOrNewMode;
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

        public bool EditOrNewMode
        {
            get => _editOrNewMode;
            set
            {
                if (value == _editOrNewMode) return;
                _editOrNewMode = value;
                OnPropertyChanged();
            }
        }
    }
}
