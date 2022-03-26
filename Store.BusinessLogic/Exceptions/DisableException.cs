using System;

namespace Store.BusinessLogic.Exceptions
{
    public class DisableException : BusinessLogicException
    {
        public DisableException() { }
        public DisableException(string message) : base(message) { }
        public DisableException(string message, Exception innerException) : base(message, innerException) { }
    }
}
