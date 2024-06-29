namespace AppInsightsForWebApi.ServiceAndRepo;

public sealed class TestService
{
    private readonly TestRepository _repository;

    public TestService(TestRepository repository)
    {
        _repository = repository;
    }

    public void CallRepositoryInModel()
    {
        try
        {
            _repository.SaveEntity();
        }
        catch (TestRepositoryException ex)
        {
            var exception = new TestServiceException("Failed to save the entity in the service.", ex);
            exception.Data.Add("ServiceData1", 5);
            exception.Data.Add("ServiceData2", true);

            throw exception;
        }
    }
}
