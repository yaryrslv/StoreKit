using System.Net;

namespace StoreKit.Application.Exceptions
{
    public class InvalidTenantException : CustomException
    {
        public InvalidTenantException(string message)
        : base(message, null, HttpStatusCode.BadRequest)
        {
        }
    }
}