using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogic.Exceptions
{
    internal class DisableException : BusinessLogicException
    {
        public DisableException() { }
        public DisableException(string message) : base(message) { }
        public DisableException(string message, Exception innerException) : base(message, innerException) { }
    }
}
