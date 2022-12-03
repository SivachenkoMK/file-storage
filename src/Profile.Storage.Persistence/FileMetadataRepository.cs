using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Profile.Storage.Domain.Storage;
using Profile.Storage.Persistence.Context;

namespace Profile.Storage.Persistence
{
    public sealed class FileMetadataRepository : IFileMetadataRepository
    {
        private readonly FileStorageContext _dbContext;

        public FileMetadataRepository(FileStorageContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        private DbSet<FileMetadata> FileMetadataDbSet => _dbContext.Set<FileMetadata>();

        public async Task SaveMetadataAsync(FileMetadata metadata, CancellationToken token)
        {
            await FileMetadataDbSet.AddAsync(metadata, token);
            await _dbContext.SaveChangesAsync(token);
        }
        
        // Looks like this method can't get the metadata, when requested by DeleteCronService.
        public async Task<IEnumerable<FileMetadata>> GetMetadataOfFilesAsync(Expression<Func<FileMetadata, bool>> lambda, CancellationToken token)
        {
            var dbSet = FileMetadataDbSet;
            var result = await dbSet.Where(lambda).ToListAsync(token);
            return result;
        }

        public async Task MarkFilesAsDeletedAsync(IEnumerable<FileMetadata> metadata, CancellationToken token)
        {
            foreach (var meta in metadata)
                FileMetadataDbSet.Update(meta).Entity.FileStatus = ExistenceStatus.Deleted;
            await _dbContext.SaveChangesAsync(token);
        }

        public Task<List<FileMetadata>> GetMetadataOfFiles(Expression<Func<FileMetadata, bool>> lambda)
        {
            var dbSet = FileMetadataDbSet;
            var result = dbSet.Where(lambda).ToList();
            return Task.FromResult(result);
        }
    }
}