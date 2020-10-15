using System;

namespace Musala.Business.Exceptions
{
    public class AlreadyExistException : ICustomException
    {
        public AlreadyExistException(string message) : base(message)
        {
        }
    }
}
