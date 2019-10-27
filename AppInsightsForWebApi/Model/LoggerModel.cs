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
        var lmException = new LoggerModelException("Failed to save the entity in the Model.", ex);
        lmException.Data.Add("ModelData1", 5);
        lmException.Data.Add("ModelData2", true);

        throw lmException;
      }
    }
  }
}
