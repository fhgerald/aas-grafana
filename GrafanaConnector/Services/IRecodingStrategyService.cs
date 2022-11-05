using GrafanaConnector.Models;
using GrafanaConnector.Repositories;

namespace GrafanaConnector.Services;

public interface IRecodingStrategyService
{
    /// <summary>
    /// Returns repository of given reference
    /// </summary>
    /// <param name="reference"></param>
    /// <returns></returns>
    IDataSeriesRepository<double> GetRepository(Reference reference);

    /// <summary>
    /// Returns all recorded sub model elements
    /// </summary>
    /// <returns></returns>
    IEnumerable<Reference> GetReferences();

    /// <summary>
    /// Adds new recoding definition for interval based strategy.
    /// </summary>
    /// <param name="reference">Reference to sub model element</param>
    /// <param name="interval">Recoding time span</param>
    /// <param name="getValueFunc">Function the retrieve current value</param>
    /// <exception cref="RecordingStrategyException">Thrown, if a sub model element is already recorded.</exception>
    void AddIntervalBasedStrategy(Reference reference, TimeSpan interval, Func<Reference, double?> getValueFunc);

    /// <summary>
    /// Disables all recording activities
    /// </summary>
    void Disconnect();
}