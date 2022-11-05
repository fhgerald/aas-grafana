namespace GrafanaConnector;

public class ConnectorOptions
{
    public ConnectorOptions()
    {
        RegistryUri = "http://localhost:4000/registry";
    }
    
    public string RegistryUri { get; set; }
}