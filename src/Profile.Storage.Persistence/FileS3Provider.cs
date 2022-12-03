using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using Profile.Storage.Domain.Storage;
using Profile.Storage.Persistence.Configs;

namespace Profile.Storage.Persistence
{
    public class FileS3Provider : IFileProvider
    {
        private readonly IOptions<S3Config> _config;

        private readonly AmazonS3Client _client;
        
        public FileS3Provider(IOptions<S3Config> config)
        {
            _config = config;
            var amazonConfig = new AmazonS3Config
            {
                ForcePathStyle = _config.Value.ForcePathStyle,
                AuthenticationRegion = RegionEndpoint.USEast1.SystemName,
                ServiceURL = _config.Value.EndpointUrl
            };
            _client = new AmazonS3Client(_config.Value.AccessKey, _config.Value.SecretKey, amazonConfig);
        }

        public async Task<Stream> GetFileAsync(string name, CancellationToken token)
        {
            using var transferUtility = new TransferUtility(_client);
            var stream = await transferUtility.OpenStreamAsync(_config.Value.BucketName, name, token);
            transferUtility.Dispose();
            return stream;
        }

        public async Task SaveFileAsync(string name, Stream file, CancellationToken token)
        {
            using var transferUtility = new TransferUtility(_client);
            await transferUtility.UploadAsync(file, _config.Value.BucketName, name, token);
        }
        
        public async Task DeleteFilesAsync(IEnumerable<string> names, CancellationToken token)
        {
            var keyList = new List<KeyVersion>();
            
            foreach (var name in names)
            {
                var key = new KeyVersion()
                {
                    Key = name
                };
                    
                keyList.Add(key);
            }

            var deleteRequest = new DeleteObjectsRequest()
            {
                BucketName = _config.Value.BucketName,
                Objects = keyList,
            };

            await _client.DeleteObjectsAsync(deleteRequest, token);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}