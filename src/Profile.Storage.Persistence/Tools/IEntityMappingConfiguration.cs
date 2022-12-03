using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Profile.Storage.Persistence.Tools
{
    public interface IEntityMappingConfiguration<T>
        where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}