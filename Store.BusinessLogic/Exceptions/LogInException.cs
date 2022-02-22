using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogic.Exceptions
{
    internal class LogInException : BusinessLogicException
    {
        public LogInException() { }
        public LogInException(string message) : base(message) { }
        public LogInException(string message, Exception innerException) : base(message, innerException) { }
    }
}
