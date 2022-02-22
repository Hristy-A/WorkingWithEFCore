using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogic.Exceptions
{
    internal class LogOutException : BusinessLogicException
    {
        public LogOutException() { }
        public LogOutException(string message) : base(message) { }
        public LogOutException(string message, Exception innerException) : base(message, innerException) { }
    }
}
