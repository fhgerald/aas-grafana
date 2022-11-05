using GrafanaConnector.Models;

namespace GrafanaConnector.Repositories;

/// <summary>
/// Implements interval based time series recoding
/// </summary>
/// <typeparam name="T">Value type that is recorded</typeparam>
internal sealed class IntervalBasedTimeSeriesRecorderStrategy<T> : ITimeSeriesRecorderStrategy<T>
	where T : struct
{
	private readonly Timer _timer;
	private readonly TimeSpan _period;
	private readonly Func<Reference, T?> _getValueFunc;
	private DateTime _nextDateTime;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="period">Recoding period</param>
	/// <param name="getValueFunc">Function to retrieve the current value.</param>
	/// <param name="dataSeriesRepository">The data series repository responsible to store data</param>
	public IntervalBasedTimeSeriesRecorderStrategy(TimeSpan period, Func<Reference, T?> getValueFunc, IDataSeriesRepository<T> dataSeriesRepository)
	{
		Repository = dataSeriesRepository;
		_period = period;
		_getValueFunc = getValueFunc;

		_nextDateTime = DateTime.UtcNow;
		_timer = new Timer(Tick, null, 0, Timeout.Infinite);
	}

	/// <inheritdoc />
	public IDataSeriesRepository<T> Repository { get; }

	/// <inheritdoc />
	public void Dispose()
	{
		_timer.Dispose();
	}

	private void Tick(object? state)
	{
		var now = DateTime.UtcNow;

		while (_nextDateTime < now)
		{
			AddValue();
		}

		_timer.Change(_period, Timeout.InfiniteTimeSpan);
	}

	private void AddValue()
	{
		AddValue(_nextDateTime);
		_nextDateTime += _period;
	}

	private void AddValue(DateTime dateTime)
	{
		Repository.Add(dateTime, _getValueFunc(Repository.Reference));
	}
}