using System;
using System.Runtime.Serialization;

namespace AppInsightsForWebApi.Repository
{
  public class LoggerRepositoryException : Exception
  {
    public LoggerRepositoryException()
    {
    }

    public LoggerRepositoryException(string message) : base(message)
    {
    }

    public LoggerRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected LoggerRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
