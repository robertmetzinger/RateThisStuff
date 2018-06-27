using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RateThisStuff_Server.Services;
using SharedLibrary;

namespace RateThisStuffTests
{
    [TestClass]
    public class DatabaseTests
    {
        private readonly ClientService _service = new ClientService();
        private User _adminUser;
        private User _normalUser;
        private User _invalidUser;

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
                Id = 1000,
                IsAdmin = false,
                Password = "invalid",
                Version = 1
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
            User newUser = new User()
            {
                Username = "newUser",
                Firstname = "new",
                Lastname = "User"
            };
            _service.SaveOrUpdateUser(newUser);
            User savedUser = _service.GetUserByUsername(newUser.Username);
            _service.DeleteUser(savedUser);
            Assert.IsNotNull(savedUser);
        }

        [TestMethod]
        public void SaveUserCheckInitPassword()
        {
            User newUser = new User()
            {
                Username = "newUser",
                Firstname = "new",
                Lastname = "User"
            };
            _service.SaveOrUpdateUser(newUser);
            User savedUser = _service.GetUserByUsername(newUser.Username);
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
            User user = _service.GetUserByUsername(_normalUser.Username);
            _service.DeleteUser(user);
            User deletedUser = _service.GetUserByUsername(user.Username);
            _service.SaveOrUpdateUser(user);
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public void DeleteUserCheckIfRatingsDeleted()
        {
            User user = _service.GetUserByUsername(_normalUser.Username);
            var ratingsOfUser = _service.GetAllRatings().FindAll(x => x.User.Equals(user));
            _service.DeleteUser(user);
            var deletedRatings = _service.GetAllRatings().FindAll(x => x.User.Equals(user));
            _service.SaveOrUpdateUser(user);
            foreach (var rating in ratingsOfUser)
            {
                _service.SaveOrUpdateRating(rating);
            }
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
            Category category = _service.GetAllCategories()[5];
            var originalName = category.Name;
            category.Name = "updateMe";
            _service.SaveOrUpdateCategory(category);
            Category updatedCategory= _service.GetAllCategories()[5];
            category.Name = originalName;
            _service.SaveOrUpdateCategory(category);
            Assert.AreEqual(updatedCategory.Name, "updateMe");
        }
    }
}
