using System;

namespace Hospital.Exceptions
{
    class AuthenticationException(string message) : Exception(message)
    {
    }
}
