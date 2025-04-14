using System;

namespace Hospital.Exceptions
{
    public class InputProfileException(string message) : Exception(message)
    {
    }
}
