using System;
using System.Collections.Generic;
using System.Text;

namespace Musala.Business.Exceptions
{
    public class ICustomException : Exception
    {
        public ICustomException(string message) : base(message)
        {
        }
    }
}
