namespace AppInsightsForWebApi.ServiceAndRepo;

public sealed class TestRepositoryException : Exception
{
    public TestRepositoryException()
    {
    }

    public TestRepositoryException(string message) : base(message)
    {
    }

    public TestRepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
