using RateThisStuff_Client.Framework;
using System.Collections.ObjectModel;
using RateThisStuff_Client.RateThisStuffServiceReference;

namespace RateThisStuff_Client.ViewModels
{
    class UserManagementViewModel:ViewModelBase
    {
        private User _selectedUser;
        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                if (Equals(value, _users)) return;
                _users = value;
                OnPropertyChanged();
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (Equals(value, _selectedUser)) return;
                _selectedUser = value;
                OnPropertyChanged();
                SessionProvider.Current.CanEditAndDelete = true;
            }
        }
    }
}
