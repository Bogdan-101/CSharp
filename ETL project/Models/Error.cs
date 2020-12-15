using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Error
    {
        public string Exception { get; private set; }

        public string Message { get; private set; }

        public DateTime? Time { get; private set; }

        public Error(string exception, string message, DateTime time)
        {
            Exception = exception;
            Message = message;
            Time = time;
        }
    }
}
