using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Profile.Storage.Domain.Storage
{
    public interface IFileMetadataRepository
    {
        Task SaveMetadataAsync(FileMetadata metadata, CancellationToken token);

        Task<IEnumerable<FileMetadata>> GetMetadataOfFilesAsync(Expression<Func<FileMetadata, bool>> lambda, CancellationToken token);

        Task MarkFilesAsDeletedAsync(IEnumerable<FileMetadata> metadata, CancellationToken token);

        public Task<List<FileMetadata>> GetMetadataOfFiles(Expression<Func<FileMetadata, bool>> lambda);
    }
}