using BaSyx.AAS.Client.Http;
using BaSyx.Models.Connectivity.Descriptors;
using BaSyx.Models.Core.AssetAdministrationShell.Generics;
using BaSyx.Models.Core.AssetAdministrationShell.Implementations;
using BaSyx.Models.Core.Common;
using BaSyx.Registry.Client.Http;
using GrafanaConnector.Models;
using Microsoft.Extensions.Options;

namespace GrafanaConnector.Services;

/// <summary>
/// Service to access digital twins.
/// </summary>
internal class TwinClientService
{
    private readonly ILogger<TwinClientService> _logger;
    private readonly RegistryHttpClient _registryClient;

    public TwinClientService(ILogger<TwinClientService> logger, IOptions<ConnectorOptions> grafanaConnectorOptions)
    {
        _logger = logger;
        _registryClient = new RegistryHttpClient(new RegistryClientSettings
        {
            RegistryConfig = new RegistryClientSettings.RegistryConfiguration
            {
                RegistryUrl = grafanaConnectorOptions.Value.RegistryUri
            }
        });
    }

    /// <summary>
    /// Returns a list all properties that are numeric of the provided AAS server
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<Reference> GetReferences()
    {
        var references = new List<Reference>();

        GetAssets((aasClient, shellDescriptor) =>
        {
            var result = aasClient.RetrieveSubmodels();
            if (result.Success)
            {
                foreach (var subModel in result.Entity)
                {
                    var subModelElementList = new List<string>();
                    RetrieveElementReferences(subModel.SubmodelElements, shellDescriptor.Identification.Id,
                        subModel.IdShort, subModelElementList, references);
                }
            }
            else
            {
                _logger.LogWarning("Retrieving sub models of AAS \'{Id}\' failed: {Messages}", 
                    shellDescriptor.Identification.Id, string.Join("/", result.Messages.Select(x => x.Text)));
            }
        });

        return references;
    }


    /// <summary>
    /// Returns the value of a given reference
    /// </summary>
    /// <param name="reference"></param>
    /// <returns></returns>
    public object? GetValue(Reference reference)
    {
        var shellDescriptor = _registryClient.RetrieveAssetAdministrationShellRegistration(reference.AasId);
        if (shellDescriptor == null || !shellDescriptor.Success)
        {
            string? details = shellDescriptor != null ? string.Join("/", shellDescriptor.Messages.Select(x => x.Text)) : null;
            throw new TwinClientException("Asset id '{' cannot be loaded from registry:\n" + details);
        }

        var result = GetClientFromDescriptor(shellDescriptor.Entity)
            .RetrieveSubmodelElementValue(reference.SubModelId, reference.SubModelElementPath);
        if (result.Success && result.Entity != null)
        {
            return result.Entity.Value;
        }

        return null;
    }

    private void GetAssets(Action<AssetAdministrationShellHttpClient, IAssetAdministrationShellDescriptor> action,
        Predicate<IAssetAdministrationShellDescriptor>? predicate = null)
    {
        // TODO: Insert your filter here and make sure that only your assessments will  be shown in Grafana; 
        //       so all Festo and Bosch assessments shouldn't be shown in Grafana
        
        var retrievedShellDescriptors = predicate == null
            ? _registryClient.RetrieveAllAssetAdministrationShellRegistrations()
            : _registryClient.RetrieveAllAssetAdministrationShellRegistrations(predicate);

        if (retrievedShellDescriptors.Success && retrievedShellDescriptors.Entity != null)
        {
            var shellsDescriptors = retrievedShellDescriptors.Entity;

            foreach (var shellDescriptor in shellsDescriptors)
            {
                var client = GetClientFromDescriptor(shellDescriptor);
                action(client, shellDescriptor);
            }
        }
    }

    private void RetrieveElementReferences(IElementContainer<ISubmodelElement> subModelElements, string aasId, string subModelId,
        List<string> subModelElementIdList,
        List<Reference> referenceList)
    {
        foreach (var element in subModelElements)
        {
            if (element is ISubmodelElementCollection collection)
            {
                var childList = new List<string>(subModelElementIdList) { element.IdShort };
                RetrieveElementReferences(collection.Value, aasId, subModelId, childList, referenceList);
            }

            if (element is Property property && property.ValueType.SystemType.IsNumericType())
            {
                referenceList.Add(new Reference(aasId, subModelId, new[] { element.IdShort }));
            }
        }
    }

    private AssetAdministrationShellHttpClient GetClientFromDescriptor(IAssetAdministrationShellDescriptor administrationShellDescriptor)
    {
        var aasClient = new AssetAdministrationShellHttpClient(administrationShellDescriptor);
        return aasClient;
    }
}
