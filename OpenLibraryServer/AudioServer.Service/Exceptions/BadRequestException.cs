using System;

namespace AudioServer.Service.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string msg) : base(msg)
        { }
    }
}