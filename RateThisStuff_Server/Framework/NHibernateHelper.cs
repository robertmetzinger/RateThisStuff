using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Reflection;

namespace RateThisStuff_Server.Framework
{
    public static class NHibernateHelper
    {
        private static ISessionFactory _mSessionFactory;
        public static string DatabaseFile { get; set; }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_mSessionFactory == null) InitializeSessionFactory();
                return _mSessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        private static void InitializeSessionFactory()
        {
            _mSessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(DatabaseFile).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never()))
                .BuildSessionFactory();
        }
    }
}
