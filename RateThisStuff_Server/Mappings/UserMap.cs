using FluentNHibernate.Mapping;
using SharedLibrary;

namespace RateThisStuff_Server.Mappings
{
    class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(x => x.Id).Column("Id");
            Map(x => x.Username).Column("Username").Length(20).Not.Nullable();
            Map(x => x.Firstname).Column("Firstname").Length(50);
            Map(x => x.Lastname).Column("Lastname").Length(50).Not.Nullable();
            Map(x => x.Password).Column("Password").Length(60).Not.Nullable();
            Map(x => x.IsAdmin).Column("IsAdmin").Not.Nullable();
            Version(x => x.Version).Column("Version").Not.Nullable();
        }
    }
}
