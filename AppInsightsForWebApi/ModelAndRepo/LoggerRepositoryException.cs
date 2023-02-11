using System.Runtime.Serialization;

namespace AppInsightsForWebApi.ModelAndRepo;

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
