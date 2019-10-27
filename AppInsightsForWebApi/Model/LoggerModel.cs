using AppInsightsForWebApi.Repository;

namespace AppInsightsForWebApi.Model
{
  public class LoggerModel
  {
    private readonly LoggerRepository _repository;

    public LoggerModel(LoggerRepository repository)
    {
      _repository = repository;
    }

    public void CallRepositoryInModel()
    {
      try
      {
        _repository.SaveEntity();
      }
      catch (LoggerRepositoryException ex)
      {
        throw new LoggerModelException("Failed to save the entity in the Model.", ex);
      }
    }
  }
}
