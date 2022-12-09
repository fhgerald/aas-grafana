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
    /// <param name="subModelElementIdPathList"></param>
    public Reference(string aasId, string subModelId, string[] subModelElementIdPathList)
    {
        AasId = aasId;
        SubModelId = subModelId;
        SubModelElementIdPathList = subModelElementIdPathList;
    }
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="referenceKey">A reference key for grafana Simple JSON in format AssetId/SubModelId/ElementId1/ElementId2</param>
    /// <exception cref="InvalidOperationException"></exception>
    public Reference(string referenceKey)
    {
        var elementPath = referenceKey.Split("]/[");
        if (elementPath.Length < 3)
        {
            throw new InvalidOperationException($"Invalid reference key: '{referenceKey}'.");
        }

        AasId = elementPath[0].Substring(1);
        SubModelId = elementPath[1];
        SubModelElementIdPathList = elementPath[2].Substring(0, elementPath[2].Length-1).Split('/').ToList();
    }
    
    /// <summary>
    /// Returns AAS Id
    /// </summary>
    public string AasId { get; set; }

    /// <summary>
    /// The id of sub model
    /// </summary>
    public string SubModelId { get; set; }
    
    /// <summary>
    /// Tjhe id of element in sub model
    /// </summary>
    public IReadOnlyList<string> SubModelElementIdPathList { get; set; }

    /// <summary>
    /// Returns unique key for the reference
    /// </summary>
    public string Key
    {
        get { return $"[{AasId}]/[{SubModelId}]/[{SubModelElementPath}]"; }
    }
    
    /// <summary>
    /// Returns unique key for the reference
    /// </summary>
    public string SubModelElementPath
    {
        get { return string.Join('/', SubModelElementIdPathList); }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        var reference = (Reference)obj;
        var equals = AasId == reference.AasId && SubModelId == reference.SubModelId &&
                SubModelElementIdPathList.Count == reference.SubModelElementIdPathList.Count;
        
        for (int i = 0; i < reference.SubModelElementIdPathList.Count; i++)
        {
            equals &= SubModelElementIdPathList[i] == reference.SubModelElementIdPathList[i];
        }

        return equals;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return SubModelId.GetHashCode() ^ SubModelElementPath.GetHashCode();
    }
}