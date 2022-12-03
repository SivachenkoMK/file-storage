using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Profile.Storage.Domain.Storage;

namespace Profile.Storage.UnitTesting.MockFileProvider
{
    public class MockFileMemoryProvider : IFileProvider
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> GetFileAsync(string name, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveFileAsync(string name, Stream file, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteFilesAsync(IEnumerable<string> names, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}