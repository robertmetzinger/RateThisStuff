using System;
using System.Collections.Generic;
using System.IO;

namespace RateThisStuff_Server.Framework
{
    public class Repository<T> where T : class
    {
        public Repository(string databaseFile)
        {
            NHibernateHelper.DatabaseFile = databaseFile;
        }

        public Repository()
        {
            var path = Path.GetDirectoryName(Path.GetDirectoryName(
                Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)));
            var localPath = new Uri(path).LocalPath;
            NHibernateHelper.DatabaseFile = Path.Combine(localPath, @"Database\", "rateMe_filled.db3");
        }

        public List<T> GetAll()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var returnList = session.CreateCriteria<T>().List<T>();
                return returnList as List<T>;
            }
        }

        public T Get(int id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                T returnElement = session.Get<T>(id);
                return returnElement;
            }
        }

        public void Delete(T entity)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }

        public void SaveOrUpdate(T entity)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    transaction.Commit();
                }
            }
        }
    }
}
