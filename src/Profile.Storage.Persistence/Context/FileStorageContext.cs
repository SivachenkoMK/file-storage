using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Profile.Storage.Domain.Storage;
using Profile.Storage.Persistence.Configs;
using Profile.Storage.Persistence.Mapping;
using Profile.Storage.Persistence.Tools;

namespace Profile.Storage.Persistence.Context
{
    public sealed class FileStorageContext : DbContext
    {
        private readonly string _connection;

        public FileStorageContext(IOptions<DbConfig> config)
        {
            _connection = config.Value.DbConnection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connection, ServerVersion.AutoDetect(_connection));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.RegisterEntityMapping<FileMetadata, FileStorageMetadataMapping>();
        }
    }
}