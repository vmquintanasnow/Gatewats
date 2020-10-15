using System;
using System.Collections.Generic;
using System.Text;

namespace Musala.Business.Exceptions
{
    class GatewayDeviceLimitExceeded : ICustomException
    {
        public GatewayDeviceLimitExceeded(string message) : base(message) { }
    }
}

