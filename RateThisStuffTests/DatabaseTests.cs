using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RateThisStuff_Server.Services;
using SharedLibrary;

namespace RateThisStuffTests
{
    //Mir sind nicht mehr sinnvolle Tests eingefallen

    [TestClass]
    public class DatabaseTests
    {
        private readonly ClientService _service = new ClientService();
        private User _adminUser;
        private User _normalUser;
        private User _invalidUser;
        private User _newUser;

        [TestInitialize]
        public void InitTest()
        {
            _adminUser = new User()
            {
                Username = "admin",
                Firstname = null,
                Lastname = "Administrator",
                Id = 1,
                IsAdmin = true,
                Password = "admin",
                Version = 1
            };
            _normalUser = new User()
            {
                Username = "frank",
                Firstname = "Frank",
                Lastname = "Meyer",
                Id = 3,
                IsAdmin = false,
                Password = "geheim",
                Version = 1
            };
            _invalidUser = new User()
            {
                Username = "invalid",
                Firstname = "Invalid",
                Lastname = "User",
                Id = 10000,
                IsAdmin = false,
                Password = "invalid",
                Version = 1
            };
            _newUser = new User()
            {
                Username = "new",
                Firstname = "New",
                Lastname = "User",
                IsAdmin = false
            };
        }

        //Login Tests

        [TestMethod]
        public void GetUserByUsername()
        {
            User user = _service.GetUserByUsername(_normalUser.Username);
            Assert.IsTrue(user.Username.Equals(_normalUser.Username));
            Assert.IsTrue(user.Firstname.Equals(_normalUser.Firstname));
            Assert.IsTrue(user.Lastname.Equals(_normalUser.Lastname));
            Assert.IsTrue(user.Id.Equals(_normalUser.Id));
            Assert.IsTrue(user.IsAdmin.Equals(_normalUser.IsAdmin));
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(_normalUser.Password, user.Password));
        }

        [TestMethod]
        public void LoginSuccessful()
        {
            User user = _service.Login(_adminUser.Username, _adminUser.Password);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void LoginSuccessfulUpperCase()
        {
            User user = _service.Login(_adminUser.Username.ToUpper(), _adminUser.Password);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void LoginWrongPassword()
        {
            User user = _service.Login(_adminUser.Username, "wrongpassword");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void LoginInvalidUser()
        {
            User user = _service.Login(_invalidUser.Username, _invalidUser.Password);
            Assert.IsNull(user);
        }

        // Start Page Tests

        [TestMethod]
        public void GetAllCategories()
        {
            var categories = _service.GetAllCategories();
            Assert.AreEqual(categories.Count, 5);
        }

        //Change Password Tests

        [TestMethod]
        public void ChangePasswordSuccesful()
        {
            User user = _service.GetUser(3);
            bool changed = _service.ChangePassword("geheim", "neu", user);
            Assert.IsTrue(changed);
            _service.ChangePassword("neu", "geheim", user);
        }

        [TestMethod]
        public void ChangePasswordWrongPassword()
        {
            bool changed = _service.ChangePassword("falsch", "neu", _normalUser);
            Assert.IsFalse(changed);
        }

        //User Management Tests

        [TestMethod]
        public void SaveUser()
        {
            _service.SaveOrUpdateUser(_newUser);
            User savedUser = _service.GetUserByUsername(_newUser.Username);
            _service.DeleteUser(savedUser);
            Assert.IsNotNull(savedUser);
        }

        [TestMethod]
        public void SaveUserWithAlreadyExistingUsername()
        {
            User user = new User()
            {
                Username = "Frank",
                Firstname = "neuer",
                Lastname = "Frank"
            };
            var saved = _service.SaveOrUpdateUser(user);
            Assert.IsFalse(saved);
        }

        [TestMethod]
        public void SaveUserAndCheckInitPassword()
        {
            _service.SaveOrUpdateUser(_newUser);
            User savedUser = _service.GetUserByUsername(_newUser.Username);
            _service.DeleteUser(savedUser);
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("geheim", savedUser.Password));
        }

        [TestMethod]
        public void UpdateUser()
        {
            User user = _service.GetUserByUsername(_normalUser.Username);
            user.Firstname = "updateMe";
            _service.SaveOrUpdateUser(user);
            User updatedUser = _service.GetUserByUsername(user.Username);
            user.Firstname = _normalUser.Firstname;
            _service.SaveOrUpdateUser(user);
            Assert.AreEqual(updatedUser.Firstname, "updateMe");
        }

        [TestMethod]
        public void DeleteUser()
        {
            _service.SaveOrUpdateUser(_newUser);
            User user = _service.GetUserByUsername(_newUser.Username);
            _service.DeleteUser(user);
            User deletedUser = _service.GetUserByUsername(user.Username);
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public void DeleteUserAndCheckIfRatingsDeleted()
        {
            _service.SaveOrUpdateUser(_newUser);
            User user = _service.GetUserByUsername(_newUser.Username);
            Item item = _service.GetItem(1);
            Rating rating = new Rating()
            {
                Score = 5,
                Comment = "This is a comment",
                Item = item,
                User = _newUser
            };
            _service.DeleteUser(user);
            var deletedRatings = _service.GetAllRatings().FindAll(x => x.User.Equals(user));
            Assert.AreEqual(deletedRatings.Count, 0);
        }

        //Category Management Tests

        [TestMethod]
        public void SaveCategory()
        {
            Category newCategory = new Category()
            {
                Name = "newCategory"
            };
            _service.SaveOrUpdateCategory(newCategory);
            Category savedCategory = _service.GetAllCategories()[5];
            Assert.IsNotNull(savedCategory);
            _service.DeleteCategory(savedCategory);
        }

        [TestMethod]
        public void UpdateCategory()
        {
            Category category = _service.GetCategory(1);
            var originalName = category.Name;
            category.Name = "updateMe";
            _service.SaveOrUpdateCategory(category);
            Category updatedCategory = _service.GetCategory(1);
            Assert.AreEqual(updatedCategory.Name, "updateMe");
            category.Name = originalName;
            _service.SaveOrUpdateCategory(category);
        }

        [TestMethod]
        public void DeleteCategory()
        {
            Category category = new Category()
            {
                Name = "newCategory"
            };
            _service.SaveOrUpdateCategory(category);
            Category savedCategory = _service.GetAllCategories().Last();
            _service.DeleteCategory(savedCategory);
            Category deletedCategory = _service.GetCategory(savedCategory.Id);
            Assert.IsNull(deletedCategory);
        }

        [TestMethod]
        public void DeleteCategoryAndCheckIfItemsDeleted()
        {
            Category category = new Category()
            {
                Name = "newCategory"
            };
            Item item = new Item()
            {
                Category = category,
                Name = "newItem"
            };
            _service.SaveOrUpdateCategory(category);
            _service.SaveOrUpdateItem(item);
            Category savedCategory = _service.GetAllCategories().Last();
            Item savedItem = _service.GetAllItems().Last();
            _service.DeleteCategory(savedCategory);
            Item deletedItem = _service.GetItem(savedItem.Id);
            Assert.IsNull(deletedItem);
        }

        //Item tests

        [TestMethod]
        public void GetAllItems()
        {
            var items = _service.GetAllItems();
            Assert.AreEqual(items.Count, 50);
        }

        [TestMethod]
        public void GetAllRatingsForItem()
        {
            Item item = _service.GetItem(1);
            var ratings = _service.GetAllRatingsForItem(item);
            Assert.AreEqual(ratings.Count, 5);
        }

        [TestMethod]
        public void SaveItem()
        {
            Item item = new Item()
            {
                Category = _service.GetCategory(1),
                Name = "new Item"
            };
            _service.SaveOrUpdateItem(item);
            Item savedItem = _service.GetAllItems().Last();
            Assert.AreEqual(savedItem.Name, "new Item");
            _service.DeleteItem(savedItem);
        }

        [TestMethod]
        public void UpdateItem()
        {
            Item item = _service.GetItem(1);
            var originalName = item.Name;
            item.Name = "updateMe";
            _service.SaveOrUpdateItem(item);
            Item updatedItem = _service.GetItem(1);
            Assert.AreEqual(updatedItem.Name, "updateMe");
            item.Name = originalName;
            _service.SaveOrUpdateItem(item);
        }

        [TestMethod]
        public void DeleteItem()
        {
            Item item = new Item()
            {
                Category = _service.GetCategory(1),
                Name = "new Item"
            };
            _service.SaveOrUpdateItem(item);
            Item savedItem = _service.GetAllItems().Last();
            _service.DeleteItem(savedItem);
            Item deletedItem = _service.GetItem(savedItem.Id);
            Assert.IsNull(deletedItem);
        }

        //Rating methods

        [TestMethod]
        public void SaveRating()
        {
            var ratings = _service.GetAllRatings();
            Item item = _service.GetItem(1);
            Rating rating = new Rating()
            {
                Comment = "This is a comment",
                Item = item,
                Score = 5,
                User = _service.GetUser(1)
            };
            _service.SaveOrUpdateRating(rating);
            var ratingsAfterSave = _service.GetAllRatings();
            Assert.IsTrue(ratingsAfterSave.Count == ratings.Count + 1);
            _service.DeleteRating(rating);
        }

        [TestMethod]
        public void GetRatingsForItems()
        {
            Item item = _service.GetItem(1);
            var ratings = _service.GetAllRatingsForItem(item);
            Assert.AreEqual(ratings.Count, 5);
        }

        [TestMethod]
        public void DeleteRating()
        {
            Item item = _service.GetItem(1);
            Rating rating = new Rating()
            {
                Comment = "This is a comment",
                Item = item,
                Score = 5,
                User = _service.GetUser(1)
            };
            _service.SaveOrUpdateRating(rating);
            var ratings = _service.GetAllRatings();
            _service.DeleteRating(rating);
            var ratingsAfterDelete = _service.GetAllRatings();
            Assert.IsTrue(ratingsAfterDelete.Count == ratings.Count - 1);
        }

        [TestMethod]
        public void GetRatingFromUserForItem()
        {
            Item item = _service.GetItem(1);
            User user = _service.GetUser(1);
            Rating rating = new Rating()
            {
                Comment = "This is a comment",
                Item = item,
                Score = 5,
                User = user
            };
            _service.SaveOrUpdateRating(rating);
            Rating ratingFromUser = _service.GetRatingFromUserForItem(user, item);
            Assert.AreEqual(ratingFromUser.Comment, "This is a comment");
            _service.DeleteRating(ratingFromUser);
        }

        //Best rated methods

        [TestMethod]
        public void AreBestRatedItemsSortedCorrectly()
        {
            var items = _service.GetBestRatedItemsOfCategory(_service.GetCategory(1));
            var sortedCorrectly = true;
            Item current = items[0];
            for (int i = 1; i < items.Count; i++)
            {
                Item item = items[i];
                if (item.AverageRating > current.AverageRating) sortedCorrectly = false;
                current = item;
            }
            Assert.IsTrue(sortedCorrectly);
        }

        [TestMethod]
        public void DoGetBestRatedItemsReturn10Elements()
        {
            var items = _service.GetBestRatedItemsOfCategory(_service.GetCategory(1));
            Assert.IsTrue(items.Count <= 10);
        }

        //Most rated methods

        [TestMethod]
        public void AreMostRatedItemsSortedCorrectly()
        {
            var items = _service.GetMostRatedItemsOfCategory(_service.GetCategory(1));
            var sortedCorrectly = true;
            Item current = items[0];
            for (int i = 1; i < items.Count; i++)
            {
                Item item = items[i];
                if (item.RatingsCount > current.RatingsCount) sortedCorrectly = false;
                current = item;
            }
            Assert.IsTrue(sortedCorrectly);
        }

        [TestMethod]
        public void DoGetMostRatedItemsReturn10Elements()
        {
            var items = _service.GetMostRatedItemsOfCategory(_service.GetCategory(1));
            Assert.IsTrue(items.Count <= 10);
        }
    }
}