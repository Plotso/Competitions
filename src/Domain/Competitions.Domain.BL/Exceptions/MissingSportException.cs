namespace Competitions.Domain.BL.Exceptions
{
    using System;

    public class MissingSportException : Exception
    {
        public MissingSportException(string errorMessage) : base(errorMessage)
        {
        }
    }
}