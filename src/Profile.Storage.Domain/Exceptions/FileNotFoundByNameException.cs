using System;

namespace Profile.Storage.Domain.Exceptions
{
    public class FileNotFoundByNameException : Exception
    {
        public FileNotFoundByNameException()
        {
        }
        
        public FileNotFoundByNameException(string? message)
            : base(message)
        {
        }
    }
}