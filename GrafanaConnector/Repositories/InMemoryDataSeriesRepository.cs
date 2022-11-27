using System.Collections.Immutable;
using GrafanaConnector.Models;
// ReSharper disable MemberCanBeProtected.Global

namespace GrafanaConnector.Repositories;

// TODO: Use the following class as template for a new persistance class which
//       persists all data to flat files (or optional database)
//       Then Grafana should be able to retrieve timeseries for all numeric values    

/// <summary>
/// Implements a memory-based data series repository, that means no data gets stored on hard disk.
/// </summary>
/// <typeparam name="T">Value type that is recorded</typeparam>
internal class InMemoryDataSeriesRepository<T> : IDataSeriesRepository<T>
 where T: struct
{
	private ImmutableSortedDictionary<DateTime, T?> _dataPoints;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="reference">Reference to sub model element</param>
	public InMemoryDataSeriesRepository(Reference reference)
	{
		_dataPoints = ImmutableSortedDictionary.Create<DateTime, T?>();
		Reference = reference;
	}

	/// <summary>
	/// Saves new value
	/// </summary>
	/// <param name="date"></param>
	/// <param name="value"></param>
	public void Add(DateTime date, T? value)
		=> Volatile.Write(ref _dataPoints, _dataPoints.Add(date, value));

	/// <summary>
	/// Gets data points based on the given time range.
	/// </summary>
	/// <param name="startDateTime"></param>
	/// <param name="endDateTime"></param>
	/// <returns></returns>
	public IEnumerable<DataPoint<T?>> GetDataPoints(DateTime startDateTime, DateTime endDateTime)
		=> Volatile.Read(ref _dataPoints)
			.Select(kvp => new DataPoint<T?>(kvp.Key, kvp.Value))
			.SkipWhile(p => p.DateTime < startDateTime)
			.TakeWhile(p => p.DateTime <= endDateTime);

	public void Dispose()
	{
		// No unmanaged resources needs to be freed in case of in-memory.
	}

	/// <inheritdoc />
	public Reference Reference { get; }
}
