using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogic.Exceptions
{
    internal class SignupException : BusinessLogicException
    {
        public SignupException() { }
        public SignupException(string message) : base(message) { }
        public SignupException(string message, Exception innerException) : base(message, innerException) { }
    }
}
