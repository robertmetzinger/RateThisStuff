using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using RateThisStuff_Client.Framework;
using RateThisStuff_Client.ViewModels;
using RateThisStuff_Client.Views;

namespace RateThisStuff_Client.Controllers
{
    public class MainWindowController
    {
        private MainWindowViewModel _viewModel;

        private Page _page;
        private StartPage _startPage;
        private ItemsPage _itemsPage;
        private BestRatedPage _bestRatedPage;
        private MostRatedPage _mostRatedPage;
        private UserManagement _userManagement;
        private CategoriesPage _categoriesPage;

        private IPageController _pageController;
        private StartPageController _startController;
        private ItemsController _itemsController;
        private BestRatedController _bestRatedController;
        private MostRatedController _mostRatedController;
        private UserManagementController _userManagementController;
        private CategoriesController _categoriesController;

        private Frame _frame;

        public void Initialize()
        {
            MainWindow view = new MainWindow();
            _viewModel = new MainWindowViewModel();
            InitializePages();
            _frame = view.Frame;
            _frame.Navigated += FrameOnNavigated;
            _viewModel.NavigationItemChanged += OnNavigationItemChanged;
            view.DataContext = _viewModel;
            view.Show();
        }

        private void InitializePages()
        {
            _startController = new StartPageController();
            _startPage = _startController.Initialize() as StartPage;

            _itemsController = new ItemsController();
            _itemsPage = _itemsController.Initialize() as ItemsPage;

            _bestRatedController = new BestRatedController();
            _bestRatedPage = _bestRatedController.Initialize() as BestRatedPage;

            _mostRatedController = new MostRatedController();
            _mostRatedPage = _mostRatedController.Initialize() as MostRatedPage;

            _userManagementController = new UserManagementController();
            _userManagement = _userManagementController.Initialize() as UserManagement;

            _categoriesController = new CategoriesController();
            _categoriesPage = _categoriesController.Initialize() as CategoriesPage;
        }

        private void OnNavigationItemChanged(object sender, PropertyChangedEventArgs e)
        {
            ListBoxItem navItem = _viewModel.NavigationItem;
            if (navItem != null)
            {
                var page = navItem.Content.ToString();
                NavigateTo(page);
            }
        }

        public void NavigateTo(string pageName)
        {
            string startPageName = (string)Application.Current.FindResource("StartPageName");
            string itemsPageName = (string)Application.Current.FindResource("ItemsPageName");
            string bestRatedPageName = (string)Application.Current.FindResource("BestRatedPageName");
            string mostRatedPageName = (string)Application.Current.FindResource("MostRatedPageName");
            string usersPageName = (string)Application.Current.FindResource("UserManagementPageName");
            string categoriesPageName = (string)Application.Current.FindResource("CategoriesPageName");

            if (pageName.Equals(startPageName))
            {
                _page = _startPage;
                _pageController = _startController;
            }
            else if (pageName.Equals(itemsPageName))
            {
                _page = _itemsPage;
                _pageController = _itemsController;
            }
            else if (pageName.Equals(bestRatedPageName))
            {
                _page = _bestRatedPage;
                _pageController = _bestRatedController;
            }
            else if (pageName.Equals(mostRatedPageName))
            {
                _page = _mostRatedPage;
                _pageController = _mostRatedController;
            }
            else if (pageName.Equals(usersPageName))
            {
                _page = _userManagement;
                _pageController = _userManagementController;
            }
            else if (pageName.Equals(categoriesPageName))
            {
                _page = _categoriesPage;
                _pageController = _categoriesController;
            }

            _frame.Navigate(_page);
        }


        private void FrameOnNavigated(object sender, NavigationEventArgs e)
        {
            _pageController.LoadData();
            _viewModel.NewCommand = new RelayCommand(_pageController.ExecuteNewCommand);
            _viewModel.EditCommand = new RelayCommand(_pageController.ExecuteEditCommand);
            _viewModel.SaveCommand = new RelayCommand(_pageController.ExecuteSaveCommand);
            _viewModel.DeleteCommand = new RelayCommand(_pageController.ExecuteDeleteCommand);
        }
    }
}
