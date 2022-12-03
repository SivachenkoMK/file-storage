using System;

namespace Profile.Storage.Domain.Storage
{
    public sealed class FileMetadata
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string ContentType { get; set; } = default!;
        
        public StorageType StorageType { get; set; }

        public DateTime DateLoaded { get; set; }
        
        public ExistenceStatus FileStatus { get; set; }
    }
}