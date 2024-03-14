using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Service.Exceptions
{
    public class ForbiddenExceptions:Exception
    {
        public override string Message => $"Forbidden";

        public ForbiddenExceptions() 
        {

        }
    }
}
