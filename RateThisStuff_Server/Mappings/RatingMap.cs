using FluentNHibernate.Mapping;
using SharedLibrary;

namespace RateThisStuff_Server.Mappings
{
    class RatingMap : ClassMap<Rating>
    {
        public RatingMap()
        {
            Table("Ratings");
            Id(x => x.Id).Column("Id");
            Map(x => x.Score).Column("Score").Length(1).Not.Nullable();
            Map(x => x.Comment).Column("Comment").Length(100);
            Version(x => x.Version).Column("Version").Not.Nullable();
            References(x => x.User).Column("UserId");
            References(x => x.Item).Column("ItemId");
        }
    }
}
