using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class AppValidationException : Exception
    {
        public AppValidationException() : base() { }
        public AppValidationException(string message) : base(message) { }
        public AppValidationException(string message, Exception innerException) : base(message, innerException) { }
    }

}
