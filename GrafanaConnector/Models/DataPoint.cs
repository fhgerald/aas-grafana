namespace GrafanaConnector.Models;

/// <summary>
/// Represents a data point based on timestamp and value.
/// </summary>
/// <typeparam name="T">Type of timestamp</typeparam>
public struct DataPoint<T>
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="dateTime"></param>
	/// <param name="value"></param>
	public DataPoint(DateTime dateTime, T value)
	{
		DateTime = dateTime;
		Value = value;
	}

	/// <summary>
	/// Returns the time stamp
	/// </summary>
	public DateTime DateTime { get; }
	
	/// <summary>
	/// Returns value at the given date time
	/// </summary>
	public T Value { get; }
}