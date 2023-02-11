namespace AppInsightsForWebApi.ModelAndRepo;

public sealed class LoggerRepository
{
    public void SaveEntity()
    {
        int[] numbers = new int[] { 1, 2, 3 };

        try
        {
            numbers[100] = 0;
        }
        catch (IndexOutOfRangeException ex)
        {
            var lrException = new LoggerRepositoryException("Index was out of the range in the Repository", ex);
            lrException.Data.Add("RepositoryData1", "Just a value");

            throw lrException;
        }
    }
}
