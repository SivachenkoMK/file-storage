using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Profile.Storage.Domain.Exceptions;

namespace Profile.Storage.Domain.Storage
{
    public sealed class FileStorageService : IFileStorageService
    {
        private readonly IFileProviderFactory _factory;
        private readonly IFileMetadataRepository _repository;
        
        public FileStorageService(IFileProviderFactory factory, IFileMetadataRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }
        
        public async Task SaveFileAsync(FileMetadata metadata, Stream file, CancellationToken token)
        {
            using var provider = _factory.Get(metadata.StorageType);
            await _repository.SaveMetadataAsync(metadata, token);
            await provider.SaveFileAsync(metadata.Name, file, token);
        }

        public async Task<(Stream file, FileMetadata metadata)> GetFileAsync(Guid id, CancellationToken token)
        {
            var metadata = await _repository.GetMetadataOfFilesAsync(t => t.Id == id, token); 
            var metafiles = metadata.ToList();
            if (metafiles.Count == 0)
                throw new FileMetadataNotFoundByIdException($"File with id {id} not found.");
            
            var fileMetadata = metafiles[0];
            
            using var provider = _factory.Get(fileMetadata.StorageType);
            var file = await provider.GetFileAsync(fileMetadata.Name, token);

            if (!file.CanRead)
                throw new Exception("FileStorageService");
            return (file, fileMetadata);
        }

        public async Task DeleteFileByIdAsync(Guid id, CancellationToken token)
        {
            var metadatas = await _repository.GetMetadataOfFilesAsync(t => t.Id == id, token);
            await DeleteFilesAsync(metadatas, token);
        }

        public async Task DeleteFilesAsync(IEnumerable<FileMetadata> metadata, CancellationToken token)
        {
            var groupedMetadata = metadata.GroupBy(t => t.ContentType);
            foreach (var data in groupedMetadata)
            {
                var type = data.First().StorageType;
                List<FileMetadata> filesToDelete = data.ToList();

                await DeleteFilesOfStorageType(filesToDelete, type, token);
            }
        }

        private async Task DeleteFilesOfStorageType(IEnumerable<FileMetadata> metadata, StorageType type, CancellationToken token)
        {
            using var provider = _factory.Get(type);
            var listOfLocalFiles = metadata.Where(meta => meta.StorageType == type).Select(meta => meta.Name);
            await provider.DeleteFilesAsync(listOfLocalFiles, token);
            await _repository.MarkFilesAsDeletedAsync(metadata, token);
        }
    }
}