using BaSyx.AAS.Client.Http;
using BaSyx.Models.Core.AssetAdministrationShell.Implementations;
using GrafanaConnector.Models;
using Microsoft.Extensions.Options;

namespace GrafanaConnector.Services;

/// <summary>
/// Service to access digital twins.
/// </summary>
internal class TwinClientService
{
    private readonly AssetAdministrationShellHttpClient _client;

    public TwinClientService(IOptions<ConnectorOptions> grafanaConnectorOptions)
    {
        _client = new AssetAdministrationShellHttpClient(new Uri(grafanaConnectorOptions.Value.AasServerHost));
    }

    /// <summary>
    /// Returns a list all properties that are numeric of the provided AAS server
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<Reference> GetKeys()
    {
        var keyList = new List<Reference>();
        
        var result = _client.RetrieveSubmodels();
        if (result.Success)
        {
            foreach (var subModel in result.Entity)
            {
                foreach (var element in subModel.SubmodelElements)
                {
                    if (element is Property property && property.ValueType.SystemType.IsNumericType())
                    {
                        keyList.Add(new Reference(subModel.IdShort, element.IdShort));
                    }
                }
            }
        }
        else
        {
            throw new Exception(string.Join("/", result.Messages.Select(x=> x.Text)));
        }

        return keyList;
    }


    /// <summary>
    /// Returns the value of a given reference
    /// </summary>
    /// <param name="reference"></param>
    /// <returns></returns>
    public object? GetValue(Reference reference)
    {
        var result = _client.RetrieveSubmodelElementValue(reference.SubModelId, reference.SubModelElementId);

        if (result.Success && result.Entity != null)
        {
            return result.Entity.Value;
        }

        return null;
    }
}