﻿using System.Runtime.Serialization;

namespace AppInsightsForWebApi.ModelAndRepo
{
    public class LoggerModelException : Exception
    {
        public LoggerModelException()
        {
        }

        public LoggerModelException(string message) : base(message)
        {
        }

        public LoggerModelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoggerModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
