using System;

namespace AudioServer.Service.Exceptions
{
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException(string message) : base(message)
        {
            
        }
    }
}