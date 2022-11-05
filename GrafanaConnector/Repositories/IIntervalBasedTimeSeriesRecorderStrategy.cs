namespace GrafanaConnector.Repositories;

/// <summary>
/// Interface for time series data recording strategies
/// </summary>
/// <typeparam name="T">Value type that is recorded</typeparam>
internal interface ITimeSeriesRecorderStrategy<T> : IDisposable
 where T: struct
{
    /// <summary>
    /// Returns the corresponding data series repository
    /// </summary>
    IDataSeriesRepository<T> Repository { get; }
}