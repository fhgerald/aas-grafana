using GrafanaConnector.Models;
using GrafanaConnector.Repositories;

namespace GrafanaConnector.Services;

/// <summary>
/// Manages recoding strategies.  
/// </summary>
internal class RecodingStrategyService : IRecodingStrategyService
{
    private readonly Dictionary<Reference, ITimeSeriesRecorderStrategy<double>> _timeSeriesRecorderStrategies;

    /// <summary>
    /// Constructor
    /// </summary>
    public RecodingStrategyService()
    {
        _timeSeriesRecorderStrategies = new Dictionary<Reference, ITimeSeriesRecorderStrategy<double>>();
    }

    /// <summary>
    /// Returns repository of given reference
    /// </summary>
    /// <param name="reference"></param>
    /// <returns></returns>
    public IDataSeriesRepository<double> GetRepository(Reference reference)
    {
        return _timeSeriesRecorderStrategies[reference].Repository;
    }

    /// <summary>
    /// Returns all recorded sub model elements
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Reference> GetReferences()
    {
        return _timeSeriesRecorderStrategies.Select(x => x.Value.Repository.Reference);
    }

    /// <summary>
    /// Adds new recoding definition for interval based strategy.
    /// </summary>
    /// <param name="reference">Reference to sub model element</param>
    /// <param name="interval">Recoding time span</param>
    /// <param name="getValueFunc">Function the retrieve current value</param>
    /// <exception cref="RecordingStrategyException">Thrown, if a sub model element is already recorded.</exception>
    public void AddIntervalBasedStrategy(Reference reference, TimeSpan interval, Func<Reference, double?> getValueFunc)
    {
        if (_timeSeriesRecorderStrategies.ContainsKey(reference))
        {
            throw new RecordingStrategyException($"Reference '{reference.SubModelId}/{reference.SubModelElementIdPathList}' is already recorded.");
        }
        
        var repository = new InMemoryDataSeriesRepository<double>(reference);
        var timeSeriesGen = new IntervalBasedTimeSeriesRecorderStrategy<double>(interval, getValueFunc, repository);
        
        _timeSeriesRecorderStrategies.Add(reference, timeSeriesGen);
    }

    /// <summary>
    /// Disables all recording activities
    /// </summary>
    public void Disconnect()
    {
        foreach (var recorderStrategy in _timeSeriesRecorderStrategies.Values)
        {
            recorderStrategy.Dispose();
        }
    }
}