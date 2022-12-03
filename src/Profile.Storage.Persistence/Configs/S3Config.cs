using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Profile.Storage.Persistence.Configs
{
    public class S3Config
    {
        public string AccessKey { get; set; }
        
        public string SecretKey { get; set; }
        
        public string BucketName { get; set; }
        
        public string EndpointUrl { get; set; }
        
        public bool ForcePathStyle { get; set; }
    }
}