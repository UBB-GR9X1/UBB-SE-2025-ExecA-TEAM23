using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    class InputProfileException : Exception
    {
        public InputProfileException(string message) : base(message)
        {

        }
    }
}
