using GrafanaConnector.Models;

namespace GrafanaConnector.Repositories;

/// <summary>
/// Interface for data series repositories for a specific tag
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDataSeriesRepository<T> : IDisposable
    where T: struct
{
    /// <summary>
    /// Saves new value
    /// </summary>
    /// <param name="date"></param>
    /// <param name="value"></param>
    public void Add(DateTime date, T? value);
        
    /// <summary>
    /// Gets data points based on the given time range.
    /// </summary>
    /// <param name="startDateTime"></param>
    /// <param name="endDateTime"></param>
    /// <returns></returns>
    IEnumerable<DataPoint<T?>> GetDataPoints(DateTime startDateTime, DateTime endDateTime);

    /// <summary>
    /// Returns the tag reference
    /// </summary>
    public Reference Reference { get; }
}