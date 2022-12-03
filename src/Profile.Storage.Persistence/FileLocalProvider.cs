using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Profile.Storage.Domain.Exceptions;
using Profile.Storage.Domain.Storage;
using Profile.Storage.Persistence.Configs;
using Profile.Storage.Persistence.Context;

namespace Profile.Storage.Persistence
{
    public class FileLocalProvider : IFileProvider
    {
        private readonly StorageConfig _config;
        
        public FileLocalProvider(IOptions<StorageConfig> config)
        {
            _config = config.Value;
        }
        
        public Task<Stream> GetFileAsync(string name, CancellationToken token)
        {
            var pathToFile = Path.Combine(_config.RootFolder, name);
            if (!File.Exists(pathToFile))
                throw new FileNotFoundByNameException("Incorrect name of file.");
            Stream file = File.OpenRead(pathToFile);
            return Task.FromResult(file);
        }

        public async Task SaveFileAsync(string name, Stream file, CancellationToken token)
        {
            if (!Directory.Exists(_config.RootFolder))
                Directory.CreateDirectory(_config.RootFolder);
            
            var fileStream = File.Create(Path.Combine(_config.RootFolder, name));
            await file.CopyToAsync(fileStream, token);
            await file.DisposeAsync();
            await fileStream.DisposeAsync();
        }

        public Task DeleteFilesAsync(IEnumerable<string> names, CancellationToken token)
        {
            foreach (var name in names)
            {
                var pathToFile = Path.Combine(_config.RootFolder, name);
                if (!File.Exists(pathToFile))
                    throw new FileNotFoundByNameException("No file to delete.");
                File.Delete(pathToFile);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}