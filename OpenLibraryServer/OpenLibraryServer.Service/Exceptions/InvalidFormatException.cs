using System;

namespace OpenLibraryServer.Service.Exceptions
{
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException(string message) : base(message)
        {
            
        }
    }
}