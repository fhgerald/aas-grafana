using System.Runtime.Serialization;

namespace GrafanaConnector.Services;

/// <summary>
/// Exception for time series recording
/// </summary>
[Serializable]
public class RecordingStrategyException : Exception
{
    /// <inheritdoc />
    public RecordingStrategyException()
    {
    }

    /// <inheritdoc />
    public RecordingStrategyException(string message) : base(message)
    {
    }

    /// <inheritdoc />
    public RecordingStrategyException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <inheritdoc />
    protected RecordingStrategyException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
