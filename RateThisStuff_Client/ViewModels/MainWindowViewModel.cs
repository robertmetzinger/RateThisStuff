using System.ComponentModel;
using RateThisStuff_Client.Framework;
using System.Windows.Controls;
using System.Windows.Input;

namespace RateThisStuff_Client.ViewModels
{
    class MainWindowViewModel:ViewModelBase
    {
        public string Username { get; set; } = SessionProvider.Current.User.Username;
        public bool IsAdmin { get; set; } = SessionProvider.Current.User.IsAdmin;
        public ICommand LogoutCommand { get; set; }

        private ListBoxItem _navigationItem;
        private ICommand _deleteCommand;
        private ICommand _saveCommand;
        private ICommand _editCommand;
        private ICommand _newCommand;

        public ICommand NewCommand
        {
            get => _newCommand;
            set
            {
                if (Equals(value, _newCommand)) return;
                _newCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditCommand
        {
            get => _editCommand;
            set
            {
                if (Equals(value, _editCommand)) return;
                _editCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand
        {
            get => _saveCommand;
            set
            {
                if (Equals(value, _saveCommand)) return;
                _saveCommand = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set
            {
                if (Equals(value, _deleteCommand)) return;
                _deleteCommand = value;
                OnPropertyChanged();
            }
        }


        public ListBoxItem NavigationItem
        {
            get => _navigationItem;
            set
            {
                if (Equals(value, _navigationItem)) return;
                _navigationItem = value;
                OnPropertyChanged();
                OnNavigationItemChanged();
            }
        }

        protected void OnNavigationItemChanged()
        {
            NavigationItemChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler NavigationItemChanged;

        public void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(e.RoutedEvent, e.RemovedItems, e.AddedItems));
        }

        public event SelectionChangedEventHandler SelectionChanged;
    }
}
