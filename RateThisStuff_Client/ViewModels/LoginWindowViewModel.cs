using System.Windows.Input;
using RateThisStuff_Client.Framework;

namespace RateThisStuff_Client.ViewModels
{
    class LoginWindowViewModel:ViewModelBase
    {
        private string _message;
        private string _usernameInput;

        public string Message
        {
            get => _message;
            set
            {
                if (value == _message) return;
                _message = value;
                OnPropertyChanged();
            }
        }

        public string UsernameInput
        {
            get => _usernameInput;
            set
            {
                if (value == _usernameInput) return;
                _usernameInput = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; set; }
    }
}
