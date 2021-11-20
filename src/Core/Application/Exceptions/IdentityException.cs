using System.Collections.Generic;
using System.Net;

namespace StoreKit.Application.Exceptions
{
    public class IdentityException : CustomException
    {
        public IdentityException(string message, List<string> errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message, errors, statusCode)
        {
        }
    }
}