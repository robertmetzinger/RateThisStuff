using FluentNHibernate.Mapping;
using SharedLibrary;

namespace RateThisStuff_Server.Mappings
{
    class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Table("Categories");
            Id(x => x.Id).Column("Id").GeneratedBy.Native();
            Map(x => x.Name).Column("Name").Length(20).Not.Nullable();
            OptimisticLock.Version();
            Version(x => x.Version).Column("Version").Not.Nullable();
        }
    }
}
