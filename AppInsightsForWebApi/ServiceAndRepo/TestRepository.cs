namespace AppInsightsForWebApi.ServiceAndRepo;

public sealed class TestRepository
{
    public void SaveEntity()
    {
        int[] numbers = [1, 2, 3];

        try
        {
            numbers[100] = 0;
        }
        catch (IndexOutOfRangeException ex)
        {
            var exception = new TestRepositoryException("Index was out of the range in the Repository", ex);
            exception.Data.Add("RepositoryData1", "Just a value");

            throw exception;
        }
    }
}
