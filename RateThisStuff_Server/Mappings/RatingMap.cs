using FluentNHibernate.Mapping;
using SharedLibrary;

namespace RateThisStuff_Server.Mappings
{
    class RatingMap : ClassMap<Rating>
    {
        public RatingMap()
        {
            Table("Ratings");
            Id(x => x.Id).Column("Id").GeneratedBy.Native();
            Map(x => x.Score).Column("Score").Length(1).Not.Nullable();
            Map(x => x.Comment).Column("Comment").Length(100);
            OptimisticLock.Version();
            Version(x => x.Version).Column("Version").Not.Nullable();
            References(x => x.Item).Column("ItemId").Not.Nullable().Cascade.None().ForeignKey();
            References(x => x.User).Column("UserId").Not.Nullable().Cascade.None().ForeignKey();
        }
    }
}
