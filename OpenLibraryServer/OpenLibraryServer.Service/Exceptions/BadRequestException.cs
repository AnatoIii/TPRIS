using System;

namespace OpenLibraryServer.Service.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        {
            
        }
    }
}