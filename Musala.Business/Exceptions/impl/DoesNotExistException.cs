using System;
using System.Collections.Generic;
using System.Text;

namespace Musala.Business.Exceptions
{
    public class DoesNotExistException : ICustomException
    {
        public DoesNotExistException(string message) : base(message) { }
    }
}
