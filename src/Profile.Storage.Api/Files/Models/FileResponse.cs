using System;
using System.Collections.Generic;

namespace Profile.Storage.Api.Files.Models
{
    public sealed class FileResponse
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public long SizeBytes { get; set; }
    }
}