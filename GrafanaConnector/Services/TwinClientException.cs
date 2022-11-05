using System.Runtime.Serialization;

namespace GrafanaConnector.Services;

[Serializable]
public class TwinClientException : Exception
{
    public TwinClientException()
    {
    }

    public TwinClientException(string message) : base(message)
    {
    }

    public TwinClientException(string message, Exception inner) : base(message, inner)
    {
    }

    protected TwinClientException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
