using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Profile.Storage.Domain.Storage
{
    public interface IFileStorageService
    {
        Task SaveFileAsync(FileMetadata metadata, Stream file, CancellationToken token);

        Task<(Stream file, FileMetadata metadata)> GetFileAsync(Guid id, CancellationToken token);

        Task DeleteFileByIdAsync(Guid id, CancellationToken token);

        Task DeleteFilesAsync(IEnumerable<FileMetadata> metadata, CancellationToken token);
    }
}