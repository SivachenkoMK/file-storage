using Profile.Storage.Domain.Storage;
using Profile.Storage.UnitTesting.MockFileProvider;
using Xunit;

namespace Profile.Storage.UnitTesting
{
    public class FileProviderFactoryTests
    {
        [Fact]
        public void IsMemoryProvider()
        {
            var factory = new MockProviderFactory();
                
            var memoryFactory = factory.Get(StorageType.Memory);
            
            Assert.IsType<MockFileMemoryProvider>(memoryFactory);
        }
        
        [Fact]
        public void IsLocalProvider()
        {
            var factory = new MockProviderFactory();
            
            var memoryFactory = factory.Get(StorageType.FileSystem);
            
            Assert.IsType<MockFileLocalProvider>(memoryFactory);
        }
        
        [Fact]
        public void IsS3Provider()
        {
            var factory = new MockProviderFactory();
            
            var memoryFactory = factory.Get(StorageType.AmazonS3);
            
            Assert.IsType<MockFileS3Provider>(memoryFactory);
        }
    }
}