using System;

namespace HotelSupervisorService.Exceptions
{
    public class ExceptionPlus : Exception
    {
        public ExceptionPlus(string message, Exception exception)
            : base(message)
        {
            this.exception = exception;
        }

        private Exception exception = null;

        public Exception Exception
        {
            get
            {
                return exception;
            }
            set
            {
                exception = value;
            }
        }
    }
}
