using System;

namespace AudioServer.Service.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message){}
    }
}