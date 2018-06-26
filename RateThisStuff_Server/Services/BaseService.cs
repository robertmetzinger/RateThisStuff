using RateThisStuff_Server.Framework;
using System.Collections.Generic;

namespace RateThisStuff_Server.Services
{

    /**
     * provides basic CRUD functions for any Database object
     */
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private Repository<T> mRepository;

        public BaseService()
        {
            //mRepository = new Repository<T>(@"\Database\rateMe_filled.db3");
            mRepository = new Repository<T>();
        }

        public T GetById(int id)
        {
            return mRepository.Get(id);
        }

        public List<T> GetAll()
        {
            return mRepository.GetAll();
        }

        public bool SaveOrUpdate(T element)
        {
            mRepository.SaveOrUpdate(element);
            return true;
        }

        public bool Delete(T element)
        {
            mRepository.Delete(element);
            return true;
        }
    }
}
