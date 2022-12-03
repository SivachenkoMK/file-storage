using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Profile.Storage.Domain.Exceptions;
using Profile.Storage.Domain.Storage;

namespace Profile.Storage.Persistence
{
    public class FileMemoryProvider : IFileProvider
    {
        private static readonly IDictionary<string, byte[]> _files = new Dictionary<string, byte[]>();
        
        public async Task<Stream> GetFileAsync(string name, CancellationToken token)
        {
            _files.TryGetValue(name, out var fileBody);
            if (fileBody == null)
                throw new FileNotFoundByNameException("Incorrect name of file.");
            var file = new MemoryStream(fileBody);
            return await Task.FromResult(file);
        }

        public async Task SaveFileAsync(string name, Stream file, CancellationToken token) 
        {
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms, token);
            var fileBody = ms.ToArray();
            _files.Add(name, fileBody);
            await file.DisposeAsync();
        }

        public Task DeleteFilesAsync(IEnumerable<string> names, CancellationToken token)
        {
            foreach (var name in names)
            {
                if (!_files.ContainsKey(name))
                    throw new FileNotFoundByNameException($"No file with name {name} to delete.");
                _files.Remove(name);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}