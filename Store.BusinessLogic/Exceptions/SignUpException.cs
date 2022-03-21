using System;

namespace Store.BusinessLogic.Exceptions
{
    public class SignupException : BusinessLogicException
    {
        public SignupException() { }
        public SignupException(string message) : base(message) { }
        public SignupException(string message, Exception innerException) : base(message, innerException) { }
    }
}
