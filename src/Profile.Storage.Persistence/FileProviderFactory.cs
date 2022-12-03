using System;
using Microsoft.Extensions.Options;
using Profile.Storage.Domain.Storage;
using Profile.Storage.Persistence.Configs;
using Profile.Storage.Persistence.Context;

namespace Profile.Storage.Persistence
{
    public sealed class FileProviderFactory : IFileProviderFactory
    {
        private IOptions<StorageConfig> _storageConfig;

        private IOptions<S3Config> _s3Config;

        public FileProviderFactory(IOptions<StorageConfig> storageConfig, IOptions<S3Config> s3Config)
        {
            _storageConfig = storageConfig;
            _s3Config = s3Config;
        }
        
        public IFileProvider Get(StorageType type)
        {
            return type switch
            {
                StorageType.Memory => new FileMemoryProvider(),
                StorageType.FileSystem => new FileLocalProvider(_storageConfig),
                StorageType.AmazonS3 => new FileS3Provider(_s3Config),
                _ => throw new NotImplementedException()
            };
        }
    }
}