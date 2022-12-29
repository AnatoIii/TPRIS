using System;
using OpenLibraryServer.Service.Exceptions;

namespace OpenLibraryServer.Service.HelperFunctions
{
    public class EntityHelpers
    {
        public static T CheckEntityExists<T>(T entity, string errorMessage)
        {
            if (entity == null)
            {
                throw new NotFoundException(errorMessage);
            }

            return entity;
        }

        public static Guid TryParseGuid(string guidStr)
        {
            if (!Guid.TryParse(guidStr, out Guid guid))
            {
                throw new InvalidFormatException($"String {guidStr} is not a valid Guid");
            }
            return guid;
        }
    }
}