namespace AppInsightsForWebApi.ServiceAndRepo;

public sealed class TestServiceException : Exception
{
    public TestServiceException()
    {
    }

    public TestServiceException(string message) : base(message)
    {
    }

    public TestServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
