using System;
using Profile.Storage.Domain.Storage;

namespace Profile.Storage.UnitTesting.MockFileProvider
{
    public class MockProviderFactory : IFileProviderFactory
    {
        public IFileProvider Get(StorageType type)
        {
            return type switch
            {
                StorageType.Memory => new MockFileMemoryProvider(),
                StorageType.FileSystem => new MockFileLocalProvider(),
                StorageType.AmazonS3 => new MockFileS3Provider(),
                _ => throw new NotImplementedException()
            };
        }
    }
}