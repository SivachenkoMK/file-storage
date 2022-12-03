using System;

namespace Profile.Storage.Domain.Exceptions
{
    public class FileMetadataNotFoundByIdException : Exception
    {
        public FileMetadataNotFoundByIdException()
        {
        }
        
        public FileMetadataNotFoundByIdException(string? message)
            : base(message)
        {
        }
    }
}