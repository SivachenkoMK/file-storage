namespace Profile.Storage.Domain.Storage
{
    public interface IFileProviderFactory
    {
        IFileProvider Get(StorageType type);
    }
}