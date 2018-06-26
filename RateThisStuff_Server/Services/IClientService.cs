using SharedLibrary;
using System.Collections.Generic;
using System.ServiceModel;

namespace RateThisStuff_Server.Services
{
    [ServiceContract]
    public interface IClientService
    {

        #region User CRUD functions

        [OperationContract]
        User GetUser(int id);

        [OperationContract]
        User GetUserByUsername(string username);

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        bool SaveOrUpdateUser(User user);

        [OperationContract]
        bool DeleteUser(User user);

        #endregion

        #region Category CRUD functions

        [OperationContract]
        Category GetCategory(int id);

        [OperationContract]
        List<Category> GetAllCategories();

        [OperationContract]
        bool SaveOrUpdateCategory(Category category);

        [OperationContract]
        bool DeleteCategory(Category category);

        #endregion

        #region Item CRUD functions

        [OperationContract]
        Item GetItem(int id);

        [OperationContract]
        List<Item> GetAllItems();

        [OperationContract]
        List<Item> GetAllItemsByCategory(Category category);

        [OperationContract]
        List<Item> GetBestRatedItemsOfCategory(Category category);

        [OperationContract]
        List<Item> GetMostRatedItemsOfCategory(Category category);

        [OperationContract]
        bool SaveOrUpdateItem(Item item);

        [OperationContract]
        bool DeleteItem(Item item);

        #endregion

        #region Rating CRUD functions

        [OperationContract]
        Rating GetRating(int id);

        [OperationContract]
        List<Rating> GetAllRatings();

        [OperationContract]
        List<Rating> GetAllRatingsForItem(Item item);

        [OperationContract]
        bool SaveOrUpdateRating(Rating rating);

        [OperationContract]
        bool DeleteRating(Rating rating);

        #endregion

        #region other functions

        [OperationContract]
        User Login(string username, string password);

        [OperationContract]
        bool ChangePassword(string oldPassword, string newPassword, User user);

        [OperationContract]
        Rating GetRatingFromUserForItem(User user, Item item);

        #endregion
    }
}
