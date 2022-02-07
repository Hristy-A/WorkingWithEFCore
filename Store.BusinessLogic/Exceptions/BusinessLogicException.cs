using System;

namespace Store.BusinessLogic.Exceptions
{
    // TODO: создать дочерние типы LoginException, SignUpException и бросать их в нужных местах
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException()
        {
        }

        public BusinessLogicException(string message) : base(message)
        {
        }

        public BusinessLogicException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
