using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogic.Exceptions
{
    internal class SignUpException : BusinessLogicException
    {
        public SignUpException() { }
        public SignUpException(string message) : base(message) { }
        public SignUpException(string message, Exception innerException) : base(message, innerException) { }
    }
}
