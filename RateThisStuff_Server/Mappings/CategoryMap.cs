using FluentNHibernate.Mapping;
using SharedLibrary;

namespace RateThisStuff_Server.Mappings
{
    class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Table("Categories");
            Id(x => x.Id).Column("Id");
            Map(x => x.Name).Column("Name").Length(20).Not.Nullable();
            Version(x => x.Version).Column("Version").Not.Nullable();
        }
    }
}
