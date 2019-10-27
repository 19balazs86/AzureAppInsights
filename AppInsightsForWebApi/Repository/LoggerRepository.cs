using System;

namespace AppInsightsForWebApi.Repository
{
  public class LoggerRepository
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
        throw new LoggerRepositoryException("Index was out of the range in the Repository", ex);
      }
    }
  }
}
