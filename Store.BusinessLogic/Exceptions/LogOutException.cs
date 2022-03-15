using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogic.Exceptions
{
    internal class LogoutException : BusinessLogicException
    {
        public LogoutException() { }
        public LogoutException(string message) : base(message) { }
        public LogoutException(string message, Exception innerException) : base(message, innerException) { }
    }
}
