namespace GrafanaConnector.Models;

/// <summary>
/// Represents a reference to specific property
/// </summary>
public class Reference
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="subModelId"></param>
    /// <param name="subModelElementId"></param>
    public Reference(string subModelId, string subModelElementId)
    {
        SubModelId = subModelId;
        SubModelElementId = subModelElementId;
    }
    
    public Reference(string referenceKey)
    {
        var pos = referenceKey.IndexOf('/');
        if (pos == -1)
        {
            throw new InvalidOperationException($"Invalid reference key: '{referenceKey}'.");
        }

        SubModelId = referenceKey.Substring(0, pos);
        SubModelElementId = referenceKey.Substring(pos+1);
    }
    
    /// <summary>
    /// The id of sub model
    /// </summary>
    public string SubModelId { get; set; }
    
    /// <summary>
    /// Tjhe id of element in sub model
    /// </summary>
    public string SubModelElementId { get; set; }

    /// <summary>
    /// Returns unique key for the reference
    /// </summary>
    public string Key
    {
        get { return $"{SubModelId}/{SubModelElementId}"; }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        var reference = (Reference)obj;
        return SubModelId == reference.SubModelId && SubModelElementId == reference.SubModelElementId;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return SubModelId.GetHashCode() ^ SubModelElementId.GetHashCode();
    }
}