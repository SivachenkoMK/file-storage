using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Storage.Domain.Storage;
using Profile.Storage.Persistence.Tools;

namespace Profile.Storage.Persistence.Mapping
{
    internal sealed class FileStorageMetadataMapping : IEntityMappingConfiguration<FileMetadata>
    {
        public void Map(EntityTypeBuilder<FileMetadata> builder)
        {
            builder.ToTable("FileMetadata");
            builder.HasKey(m => m.Id).HasName("Id");
            builder.Property(m => m.Name).HasColumnName("Name");
            builder.Property(m => m.ContentType).HasColumnName("ContentType");
            builder.Property(m => m.StorageType).HasColumnName("StorageType");
            builder.Property(m => m.DateLoaded).HasColumnName("DateLoaded");
            builder.Property(m => m.FileStatus).HasColumnName("FileStatus");
        }
    }
}