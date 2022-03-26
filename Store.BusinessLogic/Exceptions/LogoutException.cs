using System;

namespace Store.BusinessLogic.Exceptions
{
    public class LogoutException : BusinessLogicException
    {
        public LogoutException() { }
        public LogoutException(string message) : base(message) { }
        public LogoutException(string message, Exception innerException) : base(message, innerException) { }
    }
}
