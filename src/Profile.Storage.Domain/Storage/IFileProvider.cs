using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Profile.Storage.Domain.Storage
{
    public interface IFileProvider : IDisposable
    {
        Task<Stream> GetFileAsync(string name, CancellationToken token);

        Task SaveFileAsync(string name, Stream file, CancellationToken token);
        
        Task DeleteFilesAsync(IEnumerable<string> names, CancellationToken token);
    }
}