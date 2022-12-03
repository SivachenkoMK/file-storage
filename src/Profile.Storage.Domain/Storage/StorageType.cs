using System;

namespace Profile.Storage.Domain.Storage
{
    [Flags]
    public enum StorageType : byte
    {
        FileSystem = 1,
        AmazonS3 = 2,
        Memory = 4
    }
}