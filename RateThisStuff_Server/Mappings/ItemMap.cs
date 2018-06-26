using FluentNHibernate.Mapping;
using SharedLibrary;

namespace RateThisStuff_Server.Mappings
{
    class ItemMap:ClassMap<Item>
    {
        public ItemMap()
        {
            Table("Items");
            Id(x => x.Id).Column("Id").GeneratedBy.Native();
            Map(x => x.Name).Column("Name").Length(20).Not.Nullable();
            OptimisticLock.Version();
            Version(x => x.Version).Column("Version").Not.Nullable();
            References(x => x.Category).Column("CategoryId").Not.Nullable().Cascade.None().ForeignKey();
        }
    }
}
