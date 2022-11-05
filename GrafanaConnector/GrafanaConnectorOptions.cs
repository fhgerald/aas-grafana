namespace GrafanaConnector;

public class ConnectorOptions
{
    public ConnectorOptions()
    {
        AasServerHost = "http://localhost:5080/aas";
    }
    
    public string AasServerHost { get; set; }
}