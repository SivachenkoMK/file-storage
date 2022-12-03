namespace Profile.Storage.Persistence.Configs
{
    public class BackgroundDeletionConfig
    {
        public int DeletionIntervalFromSeconds { get; set; }
        
        public int FileExpiryTimeFromDays { get; set; }
    }
}