namespace Infrastructure.Exceptions
{
    public class InvalidHandlingException : Exception
    {
        public InvalidHandlingException()
        { }

        public InvalidHandlingException(string message) : base (message)
        { }
    }
}
