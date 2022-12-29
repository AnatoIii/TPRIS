using System;

namespace OpenLibraryServer.Service.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message){}
    }
}