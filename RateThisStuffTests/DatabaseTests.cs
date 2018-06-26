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
                Password = "$2a$10$Kcj5sQps/Kt86dQ/L4dFiOZHLXFpC9tk.uJiEQxtFHLDKyDEdSj8q",
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

        [TestMethod]
        public void LoginTestSuccessful()
        {
            User user = _service.Login(_adminUser.Username, _adminUser.Password);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void LoginTestWrongPassword()
        {
            User user = _service.Login(_adminUser.Username, "wrongpassword");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void LoginTestInvalidUser()
        {
            User user = _service.Login(_invalidUser.Username, _invalidUser.Password);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserByUsernameTest()
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
        public void ChangePasswordSuccesful()
        {
            bool changed = _service.ChangePassword("geheim", "neu", _normalUser);
            Assert.IsTrue(changed);
        }

        [TestMethod]
        public void ChangePasswordWrongPassword()
        {
            bool changed = _service.ChangePassword("falsch", "neu", _normalUser);
            Assert.IsFalse(changed);
        }
    }
}
