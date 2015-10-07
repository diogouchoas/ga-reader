using System;

namespace ga_reader.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized")
        {
        }
    }
}