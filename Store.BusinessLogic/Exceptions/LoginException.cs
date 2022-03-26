using System;

namespace Store.BusinessLogic.Exceptions
{
    public class LoginException : BusinessLogicException
    {
        public LoginException() { }
        public LoginException(string message) : base(message) { }
        public LoginException(string message, Exception innerException) : base(message, innerException) { }
    }
}
