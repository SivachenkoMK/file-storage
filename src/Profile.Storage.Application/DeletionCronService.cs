using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Profile.Storage.Domain.Storage;
using Profile.Storage.Persistence.Configs;

namespace Profile.Storage.Application
{
    public class DeletionCronService : BackgroundService
    {
        private readonly IServiceProvider _factory;
        private readonly BackgroundDeletionConfig _config;
        
        public DeletionCronService(IServiceProvider factory, IOptions<BackgroundDeletionConfig> config)
        {
            _factory = factory;
            _config = config.Value;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(_config.DeletionIntervalFromSeconds), stoppingToken); 
                await DoWork(stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken token)
        {
            using var scope = _factory.CreateScope();
            var storageRepository = scope.ServiceProvider.GetRequiredService<IFileMetadataRepository>();

            // Get outdated files
            var expirationDate = DateTime.UtcNow - TimeSpan.FromDays(_config.FileExpiryTimeFromDays);
            
            var filesToDelete = await storageRepository.GetMetadataOfFiles(
                t => t.FileStatus == ExistenceStatus.Exists && t.DateLoaded > expirationDate);

            if (!filesToDelete.Any())
                return;

            var storageService = scope.ServiceProvider.GetRequiredService<IFileStorageService>();
            
            // Delete files
            await storageService.DeleteFilesAsync(filesToDelete, token);
        }
    }
}