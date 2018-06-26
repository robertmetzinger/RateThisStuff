using System;
using FluentNHibernate.Conventions;
using SharedLibrary;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

namespace RateThisStuff_Server.Services
{
    public class ClientService : IClientService
    {
        private readonly BaseService<User> _userService = new BaseService<User>();
        private readonly BaseService<Category> _categoryService = new BaseService<Category>();
        private readonly BaseService<Item> _itemService = new BaseService<Item>();
        private readonly BaseService<Rating> _ratingService = new BaseService<Rating>();

        #region User CRUD functions

        public User GetUser(int id)
        {
            return _userService.GetById(id);
        }

        public User GetUserByUsername(string username)
        {
            return GetAllUsers().Find(x => x.Username == username);
        }

        public List<User> GetAllUsers()
        {
            return _userService.GetAll();
        }

        public bool SaveOrUpdateUser(User user)
        {
            //hash only if user does not already exist
            if (GetUser(user.Id) == null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword("geheim");
            }
            _userService.SaveOrUpdate(user);
            return true;
        }

        public bool DeleteUser(User user)
        {
            //check if user exists
            if (GetUser(user.Id) == null) return false;
            _userService.Delete(user);
            return true;
        }

        #endregion

        #region Category CRUD functions

        public Category GetCategory(int id)
        {
            return _categoryService.GetById(id);
        }

        public List<Category> GetAllCategories()
        {
            return _categoryService.GetAll();
        }

        public bool SaveOrUpdateCategory(Category category)
        {
            _categoryService.SaveOrUpdate(category);
            return true;
        }

        public bool DeleteCategory(Category category)
        {
            //check if category exists
            if (GetCategory(category.Id) == null) return false;
            _categoryService.Delete(category);
            return true;
        }

        #endregion

        #region Item CRUD functions

        public Item GetItem(int id)
        {
            return _itemService.GetById(id);
        }

        public List<Item> GetAllItems()
        {
            var items = _itemService.GetAll();
            foreach (Item item in items)
            {
                CalculateAverageRatingForItem(item);
                CountRatingsForItem(item);
            }
            return items;
        }

        public List<Item> GetAllItemsByCategory(Category category)
        {
            return GetAllItems().FindAll(x => x.Category.Equals(category));
        }

        public List<Item> GetBestRatedItemsOfCategory(Category category)
        {
            List<Item> allItems = GetAllItemsByCategory(category);
            return GetTenBestRatedItems(allItems);
        }

        public List<Item> GetMostRatedItemsOfCategory(Category category)
        {
            List<Item> items = GetAllItemsByCategory(category);
            return GetTenMostRatedItems(items);
        }

        public bool SaveOrUpdateItem(Item item)
        {
            _itemService.SaveOrUpdate(item);
            return true;
        }

        public bool DeleteItem(Item item)
        {
            //check if item exists
            if (GetItem(item.Id) == null) return false;
            _itemService.Delete(item);
            return true;
        }

        #endregion

        #region Rating CRUD functions

        public Rating GetRating(int id)
        {
            return _ratingService.GetById(id);
        }

        public List<Rating> GetAllRatings()
        {
            return _ratingService.GetAll();
        }

        public List<Rating> GetAllRatingsForItem(Item item)
        {
            return _ratingService.GetAll().FindAll(x => x.Item.Equals(item));
        }

        public bool SaveOrUpdateRating(Rating rating)
        {
            _ratingService.SaveOrUpdate(rating);
            return true;
        }

        public bool DeleteRating(Rating rating)
        {
            //check if rating exists
            if (GetRating(rating.Id) == null) return false;
            _ratingService.Delete(rating);
            return true;
        }

        #endregion

        #region other functions

        public User Login(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;
            return user;
        }

        public bool ChangePassword(string oldPassword, string newPassword, User user)
        {
            string newPasswordHash;
            try
            {
                newPasswordHash = BCrypt.Net.BCrypt.ValidateAndReplacePassword(oldPassword, user.Password, newPassword);
            }
            catch (Exception)
            {
                return false;
            }
            user.Password = newPasswordHash;
            return SaveOrUpdateUser(user);
        }

        #endregion

        #region private helper functions

        private void CalculateAverageRatingForItem(Item item)
        {
            List<Rating> ratings = GetAllRatingsForItem(item);
            var allScores = (from rating in ratings
                             select rating.Score).ToList();
            var average = 0.0;
            if (allScores.IsNotEmpty()) average = allScores.Average();
            item.AverageRating = average;
        }

        private void CountRatingsForItem(Item item)
        {
            List<Rating> ratings = GetAllRatingsForItem(item);
            item.RatingsCount = ratings.Count;
        }

        private List<Item> GetTenBestRatedItems(List<Item> items)
        {
            return (from item in items
                    orderby item.AverageRating descending, item.RatingsCount descending
                    select item).ToList().Take(10).ToList();
        }

        private List<Item> GetTenMostRatedItems(List<Item> items)
        {
            return (from item in items
                    orderby item.RatingsCount descending, item.AverageRating descending
                    select item).ToList().Take(10).ToList();
        }

        public Rating GetRatingFromUserForItem(User user, Item item)
        {
            var ratings = GetAllRatingsForItem(item);
            return (Rating) (from rating in ratings
                where rating.User.Equals(user)
                select rating).FirstOrNull();
        }

        #endregion

    }

}
