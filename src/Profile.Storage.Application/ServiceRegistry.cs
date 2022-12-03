using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Profile.Storage.Domain.Storage;
using Profile.Storage.Persistence;
using Profile.Storage.Persistence.Context;

namespace Profile.Storage.Application
{
    public static class ServiceRegistry
    {
        public static void RegisterStorageServices(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddScoped<IFileStorageService, FileStorageService>();
            serviceCollection.AddScoped<IFileProviderFactory, FileProviderFactory>();
            serviceCollection.AddScoped<IFileMetadataRepository, FileMetadataRepository>();
            serviceCollection.AddDbContext<FileStorageContext>(opt =>
            {
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                opt.EnableSensitiveDataLogging(true);
            });
            serviceCollection.AddHostedService<DeletionCronService>();
        }
    }
}